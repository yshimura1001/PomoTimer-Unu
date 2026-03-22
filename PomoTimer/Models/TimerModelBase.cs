using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.AppServices;
using PomoTimer.Domain;
using PomoTimer.Domain.Service;

namespace PomoTimer.Models
{
    public abstract class TimerModelBase
    {
        public string CurrentTimeFrameStr { get; protected set; }
        public string CurrentTimeFrameRangeStr { get; protected set; }
        public Time Now { get; protected set; }
        public Time StartTime { get; protected set; }
        public Time EndTime { get; protected set; }
        public int BreakMinutes { get; protected set; }

        protected readonly TimerService _timerService;
        protected readonly TimeProviderService _timeService;
        protected readonly LoggerService _loggerService;
        public TimerModelBase()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _timeService = DIContainer.GetTimeProviderService(false);
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            CurrentTimeFrameStr = "なし";
            CurrentTimeFrameRangeStr = "なし";
            Now = _timeService.Now.ToTime();
            StartTime = Time.MinValue;
            EndTime = Time.MinValue;
            BreakMinutes = -1;
        }
        public Time GetCurrentStartTime() => StartTime;
        public Time GetCurrentEndTime() => EndTime;
        public abstract void Update();
        public abstract void OnTick(Time now);
        public abstract bool IsShowBreakTimerWindow();
        public abstract bool IsSkipBreakTime();
        public abstract bool IsShowNotifitonToast(string timeFrameStr, Time showTime);

        public virtual TimeTableServiceBase? GetTimeTableService() => null;
        public virtual void RecteateTimeTable() { /* 何もしない */ }

        // 時刻表・固定のみ
        public virtual void ShowConfigWindow() { /* 何もしない */ }

        // 変動のみ
        public virtual void Start() { /* 何もしない */ }
    }
}
