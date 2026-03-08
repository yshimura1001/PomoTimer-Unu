using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Services;

namespace PomoTimer.Models
{
    public class BreakTimerWindowModel
    {
        public string TimeFrameStr { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int EndTimeSeconds { get; private set; }

        private TimerService _timerService;
        public BreakTimerWindowModel()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            TimeFrameStr = "";
            EndTime = DateTime.Now; // 仮の値(使用しない)
        }
        public void SetValues(DateTime startTime, string timeFrameStr, int breakMinutes)
        {
            TimeFrameStr = timeFrameStr;
            StartTime = startTime;
            EndTime = _timerService.Now().AddMinutes(breakMinutes);
            EndTimeSeconds = breakMinutes * 60;
        }
        public void CountDown()
        {
            EndTimeSeconds--;
        }
    }
}
