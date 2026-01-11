using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;

namespace SystemPulse.App.Views;

public sealed partial class OverviewPage : Page
{
    public ShellViewModel ViewModel { get; private set; }

    public OverviewPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Get ViewModel from navigation parameter or DI
        if (e.Parameter is ShellViewModel shellViewModel)
        {
            ViewModel = shellViewModel;
        }
        else
        {
            // Fallback: get from App services
            var app = Application.Current as App;
            ViewModel = app?.Services.GetService(typeof(ShellViewModel)) as ShellViewModel;
        }

        // Update bindings
        Bindings.Update();
    }
}
