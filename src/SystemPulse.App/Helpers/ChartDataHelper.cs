using System.Collections.ObjectModel;
using SystemPulse.App.Models;

namespace SystemPulse.App.Helpers;

public static class ChartDataHelper
{
    /// <summary>
    /// Calculates statistics from a collection of values
    /// </summary>
    public static (float current, float min, float max, float average) CalculateStatistics(
        ObservableCollection<float> data)
    {
        if (data.Count == 0)
            return (0, 0, 0, 0);

        var current = data[data.Count - 1];
        var min = data.Min();
        var max = data.Max();
        var average = data.Average();

        return (current, min, max, average);
    }

    /// <summary>
    /// Limits the history collection to a maximum size
    /// </summary>
    public static void TrimHistory(ObservableCollection<float> history, int maxSize)
    {
        while (history.Count > maxSize)
        {
            history.RemoveAt(0);
        }
    }

    /// <summary>
    /// Adds data to history and trims if necessary
    /// </summary>
    public static void AddToHistory(ObservableCollection<float> history, float value, int maxSize = 300)
    {
        history.Add(value);
        if (history.Count > maxSize)
        {
            history.RemoveAt(0);
        }
    }

    /// <summary>
    /// Generates time labels for chart X-axis (MM:SS format)
    /// </summary>
    public static List<string> GenerateTimeLabels(int dataPointCount, int intervalSeconds = 1)
    {
        var labels = new List<string>();
        var totalSeconds = (dataPointCount - 1) * intervalSeconds;

        for (int i = 0; i < dataPointCount; i++)
        {
            var seconds = i * intervalSeconds;
            var elapsed = totalSeconds - seconds;
            var minutes = elapsed / 60;
            var secs = elapsed % 60;
            labels.Add($"{minutes}:{secs:D2}");
        }

        return labels;
    }

    /// <summary>
    /// Interpolates between data points for smoother rendering
    /// </summary>
    public static ObservableCollection<float> InterpolateData(
        ObservableCollection<float> original,
        int interpolationFactor = 2)
    {
        var interpolated = new ObservableCollection<float>();

        for (int i = 0; i < original.Count - 1; i++)
        {
            var current = original[i];
            var next = original[i + 1];
            interpolated.Add(current);

            // Add intermediate points
            for (int j = 1; j < interpolationFactor; j++)
            {
                var ratio = (float)j / interpolationFactor;
                var interpolated_value = current + (next - current) * ratio;
                interpolated.Add(interpolated_value);
            }
        }

        // Add last point
        if (original.Count > 0)
        {
            interpolated.Add(original[original.Count - 1]);
        }

        return interpolated;
    }

    /// <summary>
    /// Smooths data using moving average
    /// </summary>
    public static ObservableCollection<float> SmoothData(
        ObservableCollection<float> data,
        int windowSize = 5)
    {
        var smoothed = new ObservableCollection<float>();

        for (int i = 0; i < data.Count; i++)
        {
            var start = Math.Max(0, i - windowSize / 2);
            var end = Math.Min(data.Count, i + windowSize / 2 + 1);
            var average = data.Skip(start).Take(end - start).Average();
            smoothed.Add(average);
        }

        return smoothed;
    }
}
