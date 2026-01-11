using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SystemPulse.App.ViewModels;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPulse.App.Views;

public sealed partial class ProcessesPage : Page
{
    public ProcessesViewModel ViewModel { get; private set; }

    public ProcessesPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Get ViewModel from DI
        var app = Application.Current as App;
        ViewModel = app?.Services.GetService(typeof(ProcessesViewModel)) as ProcessesViewModel;

        if (ViewModel == null)
        {
            throw new InvalidOperationException("ProcessesViewModel could not be resolved from DI container");
        }

        // Update bindings
        Bindings.Update();

        // Load processes
        ViewModel.LoadProcessesCommand.Execute(null);

        // Start auto-refresh
        ViewModel.StartMonitoring();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        // Stop auto-refresh when leaving page
        ViewModel?.StopMonitoring();
    }

    private async void Priority_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string priorityString)
        {
            if (Enum.TryParse<ProcessPriorityClass>(priorityString, out var priority))
            {
                await ViewModel.SetProcessPriorityAsync(priority);
            }
        }
    }
}
