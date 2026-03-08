using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.Domain.Service;
using PomoTimer.Models;
using PomoTimer.Services;
namespace PomoTimer.ViewModels;

public partial class ComfirmWindowVM : ObservableObject
{
    private readonly AppSettingService _appSettingService;

    [ObservableProperty]
    private string appTitleStr;

    public Action? CloseAction { get; set; }

    public ComfirmWindowVM()
    {
        _appSettingService = Ioc.Default.GetRequiredService<AppSettingService>();
        AppTitleStr = _appSettingService.GetAppTitleLong() + " - 確認";
    }
    [RelayCommand]
    public void ShowTimeTableTimerWindow()
    {
        TimeTableServiceBase timeTableService = Ioc.Default.GetRequiredService<TimeTableService>();
        new MainTimerWindow(new TimeTableMainTimerWindowVM("時刻表", new TimeTableTimerModel(timeTableService))).Show();
        CloseAction?.Invoke();

    }
    [RelayCommand]
    public void ShowVariableTimerWindow()
    {
        VariableTimerModel timerModel = Ioc.Default.GetRequiredService<VariableTimerModel>();
        new MainTimerWindow(new VariableMainTimerWindowVM("変動", timerModel)).Show();
        CloseAction?.Invoke();
    }
    [RelayCommand]
    public void ShowFixedTimerWindow()
    {
        TimeTableServiceBase timeTableService = new FixedTimeTableService();
        new MainTimerWindow(new TimeTableMainTimerWindowVM("固定", new TimeTableTimerModel(timeTableService))).Show();
        CloseAction?.Invoke();
    }

    [RelayCommand]
    public void ForceQuit() => System.Windows.Application.Current.Shutdown();
}