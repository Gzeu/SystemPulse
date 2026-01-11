using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class UsersPage : Page
{
    public UsersViewModel ViewModel { get; private set; }

    public UsersPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(UsersViewModel)) as UsersViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("UsersViewModel could not be resolved from DI container");
        }

        Bindings.Update();
        ViewModel.LoadSessions();
    }
}
