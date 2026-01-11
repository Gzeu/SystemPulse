using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SystemPulse.App.ViewModels;
using SystemPulse.App.Views;

namespace SystemPulse.App;

public sealed partial class MainWindow : Window
{
    private ShellViewModel _viewModel;
    private DispatcherTimer _updateTimer;

    public MainWindow()
    {
        InitializeComponent();

        var app = (App)Application.Current;
        _viewModel = app.Services.GetService(typeof(ShellViewModel)) as ShellViewModel;
        DataContext = _viewModel;

        SetupTitleBar();
        MainNavView.SelectionChanged += MainNavView_SelectionChanged;
        StartStatusBarUpdates();
        NavigateToPage("overview");
    }

    private void SetupTitleBar()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
    }

    private void MainNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer is NavigationViewItem item && item.Tag is string tag)
        {
            NavigateToPage(tag);
        }
    }

    private void NavigateToPage(string tag)
    {
        Type pageType = tag switch
        {
            "overview" => typeof(OverviewPage),
            "processes" => typeof(ProcessesPage),
            "performance" => typeof(PerformancePage),
            "startup" => typeof(StartupPage),
            "services" => typeof(ServicesPage),
            "users" => typeof(UsersPage),
            "details" => typeof(DetailsPage),
            "settings" => typeof(SettingsPage),
            "about" => typeof(AboutPage),
            _ => typeof(OverviewPage)
        };

        ContentFrame.Navigate(pageType, _viewModel);
    }

    private void StartStatusBarUpdates()
    {
        _updateTimer = new DispatcherTimer();
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += (s, e) => UpdateStatusBar();
        _updateTimer.Start();
    }

    private void UpdateStatusBar()
    {
        if (_viewModel?.SystemMetrics != null)
        {
            CPUText.Text = $"CPU: {_viewModel.SystemMetrics.CPUUsage:F1}%";
            RAMText.Text = $"RAM: {_viewModel.SystemMetrics.RAMUsagePercent:F1}%";
            GPUText.Text = $"GPU: {_viewModel.SystemMetrics.GPUUsage:F1}%";
        }
    }
}
