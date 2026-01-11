using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class ServicesPage : Page
{
    public ServicesViewModel ViewModel { get; private set; }

    public ServicesPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(ServicesViewModel)) as ServicesViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("ServicesViewModel could not be resolved from DI container");
        }

        Bindings.Update();
        ViewModel.LoadServices();
    }
}
