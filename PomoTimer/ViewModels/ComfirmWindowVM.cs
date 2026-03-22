using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.Domain.Service;
using PomoTimer.Models;
using PomoTimer.AppServices;
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
        bool isTest = false;
        TimeTableServiceBase timeTableService = Ioc.Default.GetRequiredService<TimeTableService>();
        new MainTimerWindow(
            new TimeTableMainTimerWindowVM(isTest, new TimeTableTimerModel(timeTableService))).Show();
        CloseAction?.Invoke();

    }
    [RelayCommand]
    public void ShowVariableTimerWindow()
    {
        bool isTest = false;
        VariableTimerModel timerModel = Ioc.Default.GetRequiredService<VariableTimerModel>();
        new MainTimerWindow(
            new VariableMainTimerWindowVM(isTest, timerModel)).Show();
        CloseAction?.Invoke();
    }
    [RelayCommand]
    public void ShowFixedTimerWindow()
    {
        bool isTest = false;
        TimeTableServiceBase timeTableService = new FixedTimeTableService();
        new MainTimerWindow(
            new TimeTableMainTimerWindowVM(isTest, new TimeTableTimerModel(timeTableService))).Show();
        CloseAction?.Invoke();
    }

    [RelayCommand]
    public void ForceQuit() => System.Windows.Application.Current.Shutdown();
}