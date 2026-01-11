using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class PerformancePage : Page
{
    public PerformanceViewModel ViewModel { get; private set; }

    public PerformancePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Get ViewModel from DI
        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(PerformanceViewModel)) as PerformanceViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("PerformanceViewModel could not be resolved from DI container");
        }

        // Update bindings
        Bindings.Update();

        // Start monitoring
        ViewModel.StartMonitoring();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        // Stop monitoring when leaving page
        ViewModel?.StopMonitoring();
    }
}
