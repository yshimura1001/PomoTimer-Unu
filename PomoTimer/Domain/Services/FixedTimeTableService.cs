using System.Collections.ObjectModel;

namespace PomoTimer.Domain.Service
{
    public class FixedTimeTableService : TimeTableServiceBase
    {
        public FixedTimeTableService()
        {
            _timeTableRowsFull = CreateTimeTableFull();
            _timeTableRows = CreateTimeTable();
        }

        private ObservableCollection<TimeTableRow> CreateTimeTableFull()
        {
            ObservableCollection<TimeTableRow> rows = new();
            string startTimeStr;
            string endTimeStr;
            int num;
            for (int i = 1; i < 23; i++)  // 1～22時までの間で作成する(TimeOnly型で"24:00"がエラーになる対策。)
            {
                num = i;
                startTimeStr = $"{num.ToString("00")}:00";
                endTimeStr = $"{num.ToString("00")}:50";
                rows.Add(new TimeTableRow
                {
                    IsOn = false,
                    StartTimeStr = startTimeStr,
                    StartTime = Time.FromString(startTimeStr),
                    EndTimeStr = endTimeStr,
                    EndTime = Time.FromString(endTimeStr),
                    TimeFrameStr = "作業",
                    NotificationEndStr = "開始",
                    BreakTimeMinutes = 0
                });
                startTimeStr = $"{num.ToString("00")}:50";
                num++;
                endTimeStr = $"{num.ToString("00")}:00";
                rows.Add(new TimeTableRow
                {
                    IsOn = true,
                    StartTimeStr = startTimeStr,
                    StartTime = Time.FromString(startTimeStr),
                    EndTimeStr = endTimeStr,
                    EndTime = Time.FromString(endTimeStr),
                    TimeFrameStr = "休憩",
                    NotificationEndStr = "開始",
                    BreakTimeMinutes = 10
                });
            }
            return rows;
        }
    }
}
