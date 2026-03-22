using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.ViewModels;
using System.Windows;

namespace PomoTimer;

public partial class BreakTimerWindow : Window
{
    public BreakTimerWindow(string timeFrameStr, int breakMinutes)
    {
        InitializeComponent();
        BreakTimerWindowVM vm = Ioc.Default.GetRequiredService<BreakTimerWindowVM>();
        vm.CloseAction = () => this.Close();
        vm.OnClosed = () => vm.StopTimer();
        vm.ExitAction = () => Application.Current.Shutdown();
        vm.SetValues(timeFrameStr, breakMinutes);
        DataContext = vm;
    }
}
