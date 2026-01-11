using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; private set; }

    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Get ViewModel from DI
        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(SettingsViewModel)) as SettingsViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("SettingsViewModel could not be resolved from DI container");
        }

        // Update bindings
        Bindings.Update();

        // Load current settings
        ViewModel.LoadSettings();
    }
}
