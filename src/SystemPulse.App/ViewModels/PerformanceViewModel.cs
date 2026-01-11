using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemPulse.App.Models;
using SystemPulse.App.Services;
using SystemPulse.App.Helpers;
using Microsoft.UI.Xaml;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.ObjectModel;

namespace SystemPulse.App.ViewModels;

public partial class PerformanceViewModel : ObservableObject
{
    private readonly ISystemMonitorService _monitorService;
    private readonly ILoggingService _logger;
    private readonly ChartDataHelper _chartHelper;
    private readonly ExportHelper _exportHelper;
    private DispatcherTimer _updateTimer;
    private readonly int _maxDataPoints = 300; // 5 minutes at 1s interval

    // Time range properties
    [ObservableProperty]
    private bool isOneMinute = true;

    [ObservableProperty]
    private bool isFiveMinutes;

    [ObservableProperty]
    private bool isFifteenMinutes;

    [ObservableProperty]
    private bool isThirtyMinutes;

    [ObservableProperty]
    private bool isOneHour;

    [ObservableProperty]
    private int updateIntervalIndex = 1; // Default 2 seconds

    // Plot Models
    [ObservableProperty]
    private PlotModel cpuPlotModel;

    [ObservableProperty]
    private PlotModel ramPlotModel;

    [ObservableProperty]
    private PlotModel gpuPlotModel;

    [ObservableProperty]
    private PlotModel networkPlotModel;

    [ObservableProperty]
    private PlotModel diskPlotModel;

    // Statistics - CPU
    [ObservableProperty]
    private float cpuCurrent;

    [ObservableProperty]
    private float cpuAverage;

    [ObservableProperty]
    private float cpuMin;

    [ObservableProperty]
    private float cpuMax;

    // Statistics - RAM
    [ObservableProperty]
    private float ramCurrent;

    [ObservableProperty]
    private float ramAverage;

    [ObservableProperty]
    private float ramMin;

    [ObservableProperty]
    private float ramMax;

    // Statistics - GPU
    [ObservableProperty]
    private float gpuCurrent;

    [ObservableProperty]
    private float gpuAverage;

    [ObservableProperty]
    private float gpuMin;

    [ObservableProperty]
    private float gpuMax;

    // Statistics - Network
    [ObservableProperty]
    private float networkCurrent;

    [ObservableProperty]
    private float networkAverage;

    [ObservableProperty]
    private float networkMax;

    // Statistics - Disk
    [ObservableProperty]
    private float diskCurrent;

    [ObservableProperty]
    private float diskAverage;

    [ObservableProperty]
    private float diskMax;

    // Data collections
    private readonly ObservableCollection<float> _cpuHistory = new();
    private readonly ObservableCollection<float> _ramHistory = new();
    private readonly ObservableCollection<float> _gpuHistory = new();
    private readonly ObservableCollection<float> _networkHistory = new();
    private readonly ObservableCollection<float> _diskHistory = new();

    public PerformanceViewModel(
        ISystemMonitorService monitorService,
        ILoggingService logger,
        ChartDataHelper chartHelper,
        ExportHelper exportHelper)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _chartHelper = chartHelper ?? throw new ArgumentNullException(nameof(chartHelper));
        _exportHelper = exportHelper ?? throw new ArgumentNullException(nameof(exportHelper));

        InitializePlotModels();
    }

    private void InitializePlotModels()
    {
        CpuPlotModel = CreatePlotModel("CPU Usage", "Time", "Usage (%)", "#00D4FF");
        RamPlotModel = CreatePlotModel("Memory Usage", "Time", "Usage (%)", "#10B981");
        GpuPlotModel = CreatePlotModel("GPU Usage", "Time", "Usage (%)", "#7C3AED");
        NetworkPlotModel = CreatePlotModel("Network Activity", "Time", "Speed (Mbps)", "#F59E0B");
        DiskPlotModel = CreatePlotModel("Disk Activity", "Time", "Activity (%)", "#EF4444");
    }

    private PlotModel CreatePlotModel(string title, string xAxisTitle, string yAxisTitle, string color)
    {
        var model = new PlotModel
        {
            Title = title,
            Background = OxyColors.Transparent,
            PlotAreaBorderColor = OxyColor.FromRgb(200, 200, 200),
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        // X-Axis (Time)
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = xAxisTitle,
            Minimum = 0,
            Maximum = 60,
            MajorGridlineStyle = LineStyle.Solid,
            MajorGridlineColor = OxyColor.FromRgb(240, 240, 240),
            MinorGridlineStyle = LineStyle.Dot,
            MinorGridlineColor = OxyColor.FromRgb(250, 250, 250)
        });

        // Y-Axis (Value)
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = yAxisTitle,
            Minimum = 0,
            Maximum = 100,
            MajorGridlineStyle = LineStyle.Solid,
            MajorGridlineColor = OxyColor.FromRgb(240, 240, 240)
        });

        // Line Series
        var series = new LineSeries
        {
            Color = OxyColor.Parse(color),
            StrokeThickness = 2,
            MarkerType = MarkerType.None,
            LineStyle = LineStyle.Solid
        };

        model.Series.Add(series);
        return model;
    }

    public void StartMonitoring()
    {
        if (_updateTimer != null)
            return;

        var interval = UpdateIntervalIndex switch
        {
            0 => 1,
            1 => 2,
            2 => 5,
            _ => 2
        };

        _updateTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(interval)
        };
        _updateTimer.Tick += (s, e) => UpdateCharts();
        _updateTimer.Start();

        _logger.LogInfo("Performance monitoring started");
    }

    public void StopMonitoring()
    {
        _updateTimer?.Stop();
        _updateTimer = null;
        _logger.LogInfo("Performance monitoring stopped");
    }

    partial void OnUpdateIntervalIndexChanged(int value)
    {
        if (_updateTimer != null)
        {
            StopMonitoring();
            StartMonitoring();
        }
    }

    partial void OnIsOneMinuteChanged(bool value)
    {
        if (value) { IsFiveMinutes = IsFifteenMinutes = IsThirtyMinutes = IsOneHour = false; UpdateTimeRange(60); }
    }

    partial void OnIsFiveMinutesChanged(bool value)
    {
        if (value) { IsOneMinute = IsFifteenMinutes = IsThirtyMinutes = IsOneHour = false; UpdateTimeRange(300); }
    }

    partial void OnIsFifteenMinutesChanged(bool value)
    {
        if (value) { IsOneMinute = IsFiveMinutes = IsThirtyMinutes = IsOneHour = false; UpdateTimeRange(900); }
    }

    partial void OnIsThirtyMinutesChanged(bool value)
    {
        if (value) { IsOneMinute = IsFiveMinutes = IsFifteenMinutes = IsOneHour = false; UpdateTimeRange(1800); }
    }

    partial void OnIsOneHourChanged(bool value)
    {
        if (value) { IsOneMinute = IsFiveMinutes = IsFifteenMinutes = IsThirtyMinutes = false; UpdateTimeRange(3600); }
    }

    private void UpdateTimeRange(int seconds)
    {
        var xAxis = CpuPlotModel.Axes[0] as LinearAxis;
        if (xAxis != null)
        {
            xAxis.Maximum = seconds;
            CpuPlotModel.InvalidatePlot(true);
        }

        // Update all chart X-axes
        foreach (var model in new[] { RamPlotModel, GpuPlotModel, NetworkPlotModel, DiskPlotModel })
        {
            var axis = model.Axes[0] as LinearAxis;
            if (axis != null)
            {
                axis.Maximum = seconds;
                model.InvalidatePlot(true);
            }
        }
    }

    private void UpdateCharts()
    {
        try
        {
            var metrics = _monitorService.GetMetrics();
            if (metrics == null)
                return;

            // Add to history
            AddToHistory(_cpuHistory, metrics.CPUUsage);
            AddToHistory(_ramHistory, metrics.RAMUsagePercent);
            AddToHistory(_gpuHistory, metrics.GPUUsage);
            AddToHistory(_networkHistory, metrics.NetworkUsageMbps);
            AddToHistory(_diskHistory, metrics.DiskUsagePercent);

            // Update charts
            UpdateChartSeries(CpuPlotModel, _cpuHistory);
            UpdateChartSeries(RamPlotModel, _ramHistory);
            UpdateChartSeries(GpuPlotModel, _gpuHistory);
            UpdateChartSeries(NetworkPlotModel, _networkHistory);
            UpdateChartSeries(DiskPlotModel, _diskHistory);

            // Update statistics
            UpdateStatistics();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update performance charts", ex);
        }
    }

    private void AddToHistory(ObservableCollection<float> history, float value)
    {
        history.Add(value);
        if (history.Count > _maxDataPoints)
        {
            history.RemoveAt(0);
        }
    }

    private void UpdateChartSeries(PlotModel model, ObservableCollection<float> data)
    {
        var series = model.Series[0] as LineSeries;
        if (series == null)
            return;

        series.Points.Clear();
        for (int i = 0; i < data.Count; i++)
        {
            series.Points.Add(new DataPoint(i, data[i]));
        }

        model.InvalidatePlot(true);
    }

    private void UpdateStatistics()
    {
        // CPU
        if (_cpuHistory.Any())
        {
            CpuCurrent = _cpuHistory.Last();
            CpuAverage = _cpuHistory.Average();
            CpuMin = _cpuHistory.Min();
            CpuMax = _cpuHistory.Max();
        }

        // RAM
        if (_ramHistory.Any())
        {
            RamCurrent = _ramHistory.Last();
            RamAverage = _ramHistory.Average();
            RamMin = _ramHistory.Min();
            RamMax = _ramHistory.Max();
        }

        // GPU
        if (_gpuHistory.Any())
        {
            GpuCurrent = _gpuHistory.Last();
            GpuAverage = _gpuHistory.Average();
            GpuMin = _gpuHistory.Min();
            GpuMax = _gpuHistory.Max();
        }

        // Network
        if (_networkHistory.Any())
        {
            NetworkCurrent = _networkHistory.Last();
            NetworkAverage = _networkHistory.Average();
            NetworkMax = _networkHistory.Max();
        }

        // Disk
        if (_diskHistory.Any())
        {
            DiskCurrent = _diskHistory.Last();
            DiskAverage = _diskHistory.Average();
            DiskMax = _diskHistory.Max();
        }
    }

    [RelayCommand]
    public async Task ExportDataAsync()
    {
        try
        {
            var data = new Dictionary<string, List<float>>
            {
                { "CPU", _cpuHistory.ToList() },
                { "RAM", _ramHistory.ToList() },
                { "GPU", _gpuHistory.ToList() },
                { "Network", _networkHistory.ToList() },
                { "Disk", _diskHistory.ToList() }
            };

            await _exportHelper.ExportPerformanceDataAsync(data);
            _logger.LogInfo("Performance data exported successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to export performance data", ex);
        }
    }

    [RelayCommand]
    public void ClearHistory()
    {
        _cpuHistory.Clear();
        _ramHistory.Clear();
        _gpuHistory.Clear();
        _networkHistory.Clear();
        _diskHistory.Clear();

        // Clear chart series
        foreach (var model in new[] { CpuPlotModel, RamPlotModel, GpuPlotModel, NetworkPlotModel, DiskPlotModel })
        {
            if (model.Series[0] is LineSeries series)
            {
                series.Points.Clear();
                model.InvalidatePlot(true);
            }
        }

        // Reset statistics
        CpuCurrent = CpuAverage = CpuMin = CpuMax = 0;
        RamCurrent = RamAverage = RamMin = RamMax = 0;
        GpuCurrent = GpuAverage = GpuMin = GpuMax = 0;
        NetworkCurrent = NetworkAverage = NetworkMax = 0;
        DiskCurrent = DiskAverage = DiskMax = 0;

        _logger.LogInfo("Performance history cleared");
    }
}
