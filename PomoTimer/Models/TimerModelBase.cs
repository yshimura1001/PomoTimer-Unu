using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Services;

namespace PomoTimer.Models
{
    public abstract class TimerModelBase
    {
        public string CurrentTimeFrameStr { get; protected set; }
        public string CurrentTimeFrameRangeStr { get; protected set; }
        public DateTime StartTime { get; protected set; }
        public DateTime EndTime { get; protected set; }
        public int BreakMinutes { get; protected set; }

        protected TimerService _timerService;
        protected LoggerService _loggerService;
        public TimerModelBase()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            CurrentTimeFrameStr = "なし";
            CurrentTimeFrameRangeStr = "なし";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            BreakMinutes = -1;
        }
        public DateTime GetCurrentStartTime() => StartTime;
        public DateTime GetCurrentEndTime() => EndTime;
        public DateTime GetDateTimeAddTime(DateTime value, int hours, int minutes, int seconds)
        {
            if (value == DateTime.MinValue) return DateTime.MinValue;

            value = (hours != 0) ? value.AddHours(hours) : value;
            value = (minutes != 0) ? value.AddMinutes(minutes) : value;
            return (seconds != 0) ? value.AddSeconds(seconds) : value;
        }
        public abstract void Update();
        public abstract void OnTick(DateTime now);
        public abstract bool IsShowBreakTimerWindow(DateTime now);
        public abstract bool IsSkipBreakTime(DateTime now);
        public abstract bool IsShowNotifitonToast(DateTime now, string timeFrameStr, DateTime showTime);

        // 時刻表・固定のみ
        public virtual void ShowConfigWindow() { /* 何もしない */ }

        // 変動のみ
        public virtual void Start() { /* 何もしない */ }
    }
}
