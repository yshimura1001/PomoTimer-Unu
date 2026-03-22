using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.AppServices;
using PomoTimer.Domain;
using System.Runtime.CompilerServices;

namespace PomoTimer.Models
{
    public class BreakTimerWindowModel
    {
        public string TimeFrameStr { get; protected set; }
        public Time StartTime { get; protected set; }
        public Time EndTime { get; protected set; }
        public int EndTimeSeconds { get; protected set; }

        private readonly TimerService _timerService;
        private readonly TimeProviderService _timeService;
        public BreakTimerWindowModel()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _timeService = DIContainer.GetTimeProviderService(false);
            TimeFrameStr = "";
            StartTime = Time.MinValue; // 仮の値(使用しない)
            EndTime = Time.MinValue; // 仮の値(使用しない)
        }
        public void SetValues(Time startTime, string timeFrameStr, int breakMinutes)
        {
            TimeFrameStr = timeFrameStr;
            StartTime = startTime;
            EndTime = _timeService.Now.ToTime().Add(0, breakMinutes, 0);
            EndTimeSeconds = breakMinutes * 60;
        }
        public void CountDown()
        {
            EndTimeSeconds--;
        }
    }
}
