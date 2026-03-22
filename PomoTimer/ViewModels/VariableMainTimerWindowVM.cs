using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.AppServices;
using PomoTimer.Models;

namespace PomoTimer.ViewModels;

public partial class VariableMainTimerWindowVM : MainTimerWindowVMBase
{
    [ObservableProperty]
    private bool canExecute = true;
    public VariableMainTimerWindowVM(bool isTest, TimerModelBase model) : base(isTest, model)
    {
        _now = _timeService.Now.ToTime();
        _startTime = _model.GetCurrentStartTime();
        _endTime = _model.GetCurrentEndTime();
        // 初期値
        CurrentTimeFrameStr = _model.CurrentTimeFrameStr;
        CurrentTimeFrameRangeStr = _model.CurrentTimeFrameRangeStr;
        RestTimeStr = (_model.CurrentTimeFrameRangeStr != "なし") ?
            RestTimeStr = _timeService.ToString(_timeService.Rest.ToTimeSpan(_now, _model.EndTime), TimeProviderService.HHMM) : "なし";
        NowTimeStr = _timeService.ToString(_now, TimeProviderService.HHMM);

        _timerService.Tick += OnTick;
        _timerService.Start();
    }
    [RelayCommand]
    public void StartPomo()
    {
        CanExecute = false;
        _model.Start();
    }
    [RelayCommand]
    public void StartBreak()
    {
        CanExecute = false;
        new BreakTimerWindow("休憩中 [手動]", 10).Show();
    }
}