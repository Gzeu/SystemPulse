using Microsoft.UI.Xaml.Controls;

namespace SystemPulse.App.Helpers;

public static class DialogHelper
{
    /// <summary>
    /// Shows an error dialog
    /// </summary>
    public static async Task<ContentDialogResult> ShowErrorDialogAsync(
        string title,
        string message,
        string primaryButtonText = "OK")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = primaryButtonText,
            DefaultButton = ContentDialogButton.Primary
        };

        return await dialog.ShowAsync();
    }

    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    public static async Task<ContentDialogResult> ShowConfirmationDialogAsync(
        string title,
        string message,
        string primaryButtonText = "Yes",
        string secondaryButtonText = "No")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            DefaultButton = ContentDialogButton.Secondary
        };

        return await dialog.ShowAsync();
    }

    /// <summary>
    /// Shows an information dialog
    /// </summary>
    public static async Task<ContentDialogResult> ShowInfoDialogAsync(
        string title,
        string message,
        string primaryButtonText = "OK")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = primaryButtonText,
            DefaultButton = ContentDialogButton.Primary
        };

        return await dialog.ShowAsync();
    }

    /// <summary>
    /// Shows a warning dialog
    /// </summary>
    public static async Task<ContentDialogResult> ShowWarningDialogAsync(
        string title,
        string message,
        string primaryButtonText = "OK")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = primaryButtonText,
            DefaultButton = ContentDialogButton.Primary
        };

        return await dialog.ShowAsync();
    }
}
