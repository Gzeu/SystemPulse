using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class StartupPage : Page
{
    public StartupViewModel ViewModel { get; private set; }

    public StartupPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(StartupViewModel)) as StartupViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("StartupViewModel could not be resolved from DI container");
        }

        Bindings.Update();
        ViewModel.LoadStartupApps();
    }
}
