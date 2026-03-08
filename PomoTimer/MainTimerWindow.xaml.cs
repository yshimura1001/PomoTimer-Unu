using PomoTimer.ViewModels;
using System.Windows;

namespace PomoTimer;
public partial class MainTimerWindow : Window
{
    public MainTimerWindowVMBase VM { get; private set; }
    public MainTimerWindow(MainTimerWindowVMBase vm)
    {
        VM = vm;
        InitializeComponent();
        if (VM is VariableMainTimerWindowVM)
            Buttons.Content = new VariableTimerButtons();
        else
            Buttons.Content = new TimeTableTimerButtons();
        VM.TopMostAction = () =>
        {
            Topmost = !Topmost;
        };
        VM.DarkModeAction = () =>
        {
            #pragma warning disable WPF0001
            if (this.ThemeMode == ThemeMode.Dark)
                this.ThemeMode = ThemeMode.Light;
            else
                this.ThemeMode = ThemeMode.Dark;
            #pragma warning restore WPF0001
        };
        VM.CloseAction = () => { this.Close(); };
        DataContext = VM;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
    }
}