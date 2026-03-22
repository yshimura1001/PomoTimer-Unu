using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.AppServices;
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
        LoggerService loggerService = Ioc.Default.GetRequiredService<LoggerService>();

        int miliseconds = 1000 - DateTime.Now.Microsecond;
        Task.Delay(miliseconds).Wait();
        loggerService.Info($"{miliseconds} miliseconds wait.(Near 000 milisecond start)");
        loggerService.DeletePastLogs();  // 直近7日間より古いログを削除
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
    }
}