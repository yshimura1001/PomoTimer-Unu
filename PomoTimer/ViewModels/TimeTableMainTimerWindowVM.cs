using CommunityToolkit.Mvvm.Input;
using PomoTimer.AppServices;
using PomoTimer.Models;

namespace PomoTimer.ViewModels;

public partial class TimeTableMainTimerWindowVM : MainTimerWindowVMBase
{
    public TimeTableMainTimerWindowVM(bool isTest, TimerModelBase model) : base(isTest, model)
    {
        _now = _timeService.Now.ToTime();
        _startTime = _model.GetCurrentStartTime();
        _endTime = _model.GetCurrentEndTime();
        // 初期値
        CurrentTimeFrameStr = _model.CurrentTimeFrameStr;
        CurrentTimeFrameRangeStr = _model.CurrentTimeFrameRangeStr;
        RestTimeStr = (_model.CurrentTimeFrameRangeStr != "なし") ?
            _timeService.ToString(_timeService.Rest.ToTimeSpan(_now, _model.EndTime), TimeProviderService.HHMM) : "なし";
        NowTimeStr = _timeService.ToString(_now, TimeProviderService.HHMM);

        _timerService.Tick += OnTick;
        _timerService.Start();
    }

    public override void RecreateTimeTable() => _model.RecteateTimeTable();

    [RelayCommand]
    public void ShowConfigWindow() => _model.ShowConfigWindow();
}