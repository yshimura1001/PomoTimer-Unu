using CommunityToolkit.Mvvm.Input;
using PomoTimer.Models;

namespace PomoTimer.ViewModels;

public partial class TimeTableMainTimerWindowVM : MainTimerWindowVMBase
{

    public TimeTableMainTimerWindowVM(string modeStr, TimerModelBase model) : base(modeStr, model)
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
    public void ShowConfigWindow() => _model.ShowConfigWindow();
}