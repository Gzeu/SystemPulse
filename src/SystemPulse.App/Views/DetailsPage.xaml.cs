using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using SystemPulse.App.Models;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class DetailsPage : Page
{
    public DetailsViewModel ViewModel { get; private set; }

    public DetailsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(DetailsViewModel)) as DetailsViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("DetailsViewModel could not be resolved from DI container");
        }

        Bindings.Update();

        // Load process details if passed as parameter
        if (e.Parameter is ProcessInfo process)
        {
            ViewModel.LoadProcessDetails(process);
        }
    }
}
