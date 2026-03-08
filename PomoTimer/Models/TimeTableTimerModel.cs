using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using PomoTimer.Domain;
using PomoTimer.Domain.Service;
using PomoTimer.Services;

namespace PomoTimer.Models
{
    public class TimeTableTimerModel : TimerModelBase
    {
        private TimeTableRow _currentRow;

        protected TimeTableServiceBase _timeTableService;

        public TimeTableTimerModel(TimeTableServiceBase timeTableService)
        {
            _timeTableService = timeTableService;
            _currentRow = _timeTableService.GetCurrentRow();
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
            DateTime now = _timerService.Now();
            _currentRow = _timeTableService.GetCurrentRow();
            StartTime = _currentRow.StartTime;
            EndTime = _currentRow.EndTime;
            if (now >= StartTime)
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
        public override bool IsShowBreakTimerWindow(DateTime now) => 
            (GetCurrentIsOn()) && (now == StartTime);
        public override bool IsSkipBreakTime(DateTime now) =>
            (!GetCurrentIsOn()) && (now == StartTime);
        public override bool IsShowNotifitonToast(DateTime now, string timeFrameStr, DateTime showTime)
        {
            //_loggerService.Info($"now:{now}, time:{GetDateTimeAddTime(EndTime, 0, -3, 0)}, NextRow.IsOn:{GetNextIsOn()}");
            return (GetNextIsOn()) && (now == showTime);
        }
        public override void ShowConfigWindow() => new ConfigWindow(GetTimeTableService()).Show();
        public override void OnTick(DateTime now)
        {
            // 現在の時刻表の削除：開始時間と終了時間が被っているため、終了時間の1秒前に実行する
            if (now == GetDateTimeAddTime(EndTime, 0, 0, -1))
            {
                _loggerService.Info("Delete Now TimeTable.");
                DeleteNowRow();
            }
        }
        public bool GetCurrentIsOn() => _timeTableService.GetCurrentIsOn();
        public bool GetNextIsOn() => _timeTableService.GetNextIsOn();
        public void DeleteNowRow() => _timeTableService.DeleteNowRow();
        public TimeTableServiceBase GetTimeTableService() => _timeTableService;
    }
}
