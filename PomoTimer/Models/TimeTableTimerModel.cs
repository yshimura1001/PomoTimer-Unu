using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Domain;
using PomoTimer.Domain.Service;

namespace PomoTimer.Models
{
    public class TimeTableTimerModel : TimerModelBase
    {
        private TimeTableRow _currentRow;
        private TimeTableRow _nextRow;
        private readonly TimeTableServiceBase _timeTableService;

        public TimeTableTimerModel(TimeTableServiceBase timeTableService)
        {
            _timeTableService = timeTableService;
            Now = _timeService.Now.ToTime();
            _currentRow = _timeTableService.GetCurrentRow();
            _nextRow = _timeTableService.GetNextRow();
            CurrentTimeFrameStr = _currentRow.TimeFrameStr;
            CurrentTimeFrameRangeStr = 
                (_currentRow.StartTimeStr != "") ? 
                $"{_currentRow.StartTimeStr}～{_currentRow.EndTimeStr}" : "なし";
            StartTime = _currentRow.StartTime;
            EndTime = _currentRow.EndTime;
            BreakMinutes = _currentRow.BreakTimeMinutes;
        }
        public override void Update()
        {
            Now = _timeService.Now.ToTime();
            _currentRow = _timeTableService.GetCurrentRow();
            _nextRow = _timeTableService.GetNextRow();
            StartTime = _currentRow.StartTime;
            EndTime = _currentRow.EndTime;
            if (Now >= StartTime)
            {
                CurrentTimeFrameStr = _currentRow.TimeFrameStr;
                switch(_currentRow.StartTimeStr)
                {
                    case "":
                    case "なし":
                        CurrentTimeFrameRangeStr = "なし";
                        break;
                    default:
                        CurrentTimeFrameRangeStr = $"{_currentRow.StartTimeStr}～{_currentRow.EndTimeStr}";
                        break;
                }
                BreakMinutes = _currentRow.BreakTimeMinutes;
            }
            else
            {
                CurrentTimeFrameStr = "始業前";
                CurrentTimeFrameRangeStr = $"なし";
                BreakMinutes = 0;
            }
        }
        public override bool IsShowBreakTimerWindow() => 
            (GetCurrentIsOn()) && (Now == StartTime);
        public override bool IsSkipBreakTime() =>
            (!GetCurrentIsOn()) && (Now == StartTime);
        public override bool IsShowNotifitonToast(string timeFrameStr, Time showTime)
        {
            //_loggerService.Info($"now:{now}, time:{GetDateTimeAddTime(EndTime, 0, -3, 0)}, NextRow.IsOn:{GetNextIsOn()}");
            return (GetNextIsOn()) && (Now == showTime);
        }
        public override void ShowConfigWindow() => new ConfigWindow(GetTimeTableService()).Show();
        public override void OnTick(Time now)
        {
            // 現在の時刻表の削除：開始時間と終了時間が被っているため、終了時間の1秒前に実行する
            if (now == EndTime.Add( 0, 0, -1))
            {
                _loggerService.Info("Delete Now TimeTable.");
                DeleteNowRow();
            }
        }
        public bool GetCurrentIsOn() => _timeTableService.GetCurrentIsOn();
        public bool GetNextIsOn() => _timeTableService.GetNextIsOn();
        public void DeleteNowRow() => _timeTableService.DeleteNowRow();
        public override TimeTableServiceBase GetTimeTableService() => _timeTableService;
        public override void RecteateTimeTable() => _timeTableService.RecreateTimeTable();
    }
}
