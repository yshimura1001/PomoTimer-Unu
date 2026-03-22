using CommunityToolkit.Mvvm.ComponentModel;

namespace PomoTimer.Domain
{
    public partial class TimeTableRow : ObservableObject
    {
        [ObservableProperty]
        private bool isOn;
        [ObservableProperty]
        private string startTimeStr = "";
        public required Time StartTime { get; set; }
        [ObservableProperty]
        private string endTimeStr = "";
        public required Time EndTime { get; set; }
        [ObservableProperty]
        private string timeFrameStr = "";
        [ObservableProperty]
        private string notificationEndStr = "";
        [ObservableProperty]
        private int breakTimeMinutes;
        
        public TimeTableRow Clone()
        {
            return new TimeTableRow
            {
                IsOn = IsOn,
                StartTimeStr = StartTimeStr,
                StartTime = StartTime,
                EndTimeStr = EndTimeStr,
                EndTime = EndTime,
                TimeFrameStr = TimeFrameStr,
                NotificationEndStr = NotificationEndStr,
                BreakTimeMinutes = BreakTimeMinutes
            };
        }
    }
}