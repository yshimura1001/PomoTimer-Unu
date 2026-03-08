using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.Models;

namespace PomoTimer.ViewModels;

public partial class VariableMainTimerWindowVM : MainTimerWindowVMBase
{
    [ObservableProperty]
    private bool canExecute = true;
    public VariableMainTimerWindowVM(string modeStr, TimerModelBase model) : base(modeStr, model)
    {
        _now = _timerService.Now();
        _startTime = _model.GetCurrentStartTime();
        _endTime = _model.GetCurrentEndTime();
        // 初期値
        CurrentTimeFrameStr = _model.CurrentTimeFrameStr;
        CurrentTimeFrameRangeStr = _model.CurrentTimeFrameRangeStr;
        RestTimeStr = (_model.CurrentTimeFrameRangeStr != "なし") ?
            _timerService.GetRestTimeStrByhhmm(_now, _model.EndTime) : "なし";
        NowTimeStr = _timerService.GetNowStrByHHmm(_now);

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