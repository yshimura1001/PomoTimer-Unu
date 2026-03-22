using PomoTimer.AppServices;
using PomoTimer.Domain;

namespace PomoTimer.Models
{
    public class VariableTimerModel : TimerModelBase
    {
        public VariableTimerModel()
        {
            CurrentTimeFrameStr = "なし";
            CurrentTimeFrameRangeStr = "なし";
            StartTime = Time.MinValue;
            Now = _timeService.Now.ToTime();
            EndTime = Time.MinValue;
            BreakMinutes = -1;
        }
        public override void Update()
        {
            Now = _timeService.Now.ToTime();
            string[] kindStrs = ["作業", "休憩"];
            int breakMinutes;
            foreach (string kindStr in kindStrs)
            {
                if ((Now == EndTime) && (CurrentTimeFrameStr == kindStr))
                {
                    StartTime = Now;
                    breakMinutes = (kindStr == "作業") ? 10 : 50;
                    EndTime = Now.Add(0, breakMinutes, 0);
                    CurrentTimeFrameStr = (kindStr == "作業") ? "休憩" : "作業";
                    string startTimeStr = _timeService.ToString(StartTime, TimeProviderService.HHMM);
                    string endTimeStr = _timeService.ToString(EndTime, TimeProviderService.HHMM);
                    CurrentTimeFrameRangeStr = $"{startTimeStr}～{endTimeStr}";
                    BreakMinutes = 10;
                }
            }
        }
        public override void OnTick(Time now)
        {
            // 何もしない
        }
        public override bool IsShowBreakTimerWindow() => ((Now == StartTime) && (CurrentTimeFrameStr == "休憩"));
        public override bool IsSkipBreakTime() => false;
        public override bool IsShowNotifitonToast(string timeFrameStr, Time showTime) =>
            (timeFrameStr == "作業") && (Now == showTime);
        public override void Start()
        {
            Time now = _timeService.Now.ToTime();
            StartTime = now;
            EndTime = StartTime.Add(0, 50, 0);
            CurrentTimeFrameStr = "作業";
            string startTimeStr = _timeService.ToString(StartTime, TimeProviderService.HHMM);
            string endTimeStr = _timeService.ToString(EndTime, TimeProviderService.HHMM);
            CurrentTimeFrameRangeStr = $"{startTimeStr}～{endTimeStr}";
            BreakMinutes = -1;
        }
    }
}
