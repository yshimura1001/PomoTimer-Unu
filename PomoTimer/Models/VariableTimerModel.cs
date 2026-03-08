
namespace PomoTimer.Models
{
    public class VariableTimerModel : TimerModelBase
    {
        public VariableTimerModel()
        {
            CurrentTimeFrameStr = "なし";
            CurrentTimeFrameRangeStr = "なし";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            BreakMinutes = -1;
        }
        public override void Update()
        {
            DateTime now = _timerService.Now();
            string[] kindStrs = ["作業", "休憩"];
            int breakMinutes;
            foreach (string kindStr in kindStrs)
            {
                if ((now == EndTime) && (CurrentTimeFrameStr == kindStr))
                {
                    StartTime = now;
                    breakMinutes = (kindStr == "作業") ? 10 : 50;
                    EndTime = GetDateTimeAddTime(now, 0, breakMinutes, 0);
                    CurrentTimeFrameStr = (kindStr == "作業") ? "休憩" : "作業";
                    string startTimeStr = _timerService.GetTimeStr(StartTime);
                    string endTimeStr = _timerService.GetTimeStr(EndTime);
                    CurrentTimeFrameRangeStr = $"{startTimeStr}～{endTimeStr}";
                    BreakMinutes = 10;
                }
            }
        }
        public override void OnTick(DateTime now)
        {
            // 何もしない
        }
        public override bool IsShowBreakTimerWindow(DateTime now) => 
            ((now == StartTime) && (CurrentTimeFrameStr == "休憩"));
        public override bool IsSkipBreakTime(DateTime now) => false;
        public override bool IsShowNotifitonToast(DateTime now, string timeFrameStr, DateTime showTime) =>
            (timeFrameStr == "作業") && (now == showTime);
        public override void Start()
        {
            DateTime now = _timerService.Now();
            StartTime = now;
            EndTime = GetDateTimeAddTime(now, 0, 50, 0);
            CurrentTimeFrameStr = "作業";
            string startTimeStr = _timerService.GetTimeStr(StartTime);
            string endTimeStr = _timerService.GetTimeStr(EndTime);
            CurrentTimeFrameRangeStr = $"{startTimeStr}～{endTimeStr}";
            BreakMinutes = -1;
        }
    }
}
