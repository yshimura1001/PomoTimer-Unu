using PomoTimer.Domain;
using System.Collections.ObjectModel;

namespace PomoTimer.Infrastructure
{
    public class TimeTableInMemoryRepository : ITimeTableRepository
    {
        public ObservableCollection<TimeTableRow> _timeTableRows;

        public TimeTableInMemoryRepository() 
        {
            _timeTableRows = [
                new TimeTableRow{ IsOn=false, StartTimeStr="08:30", EndTimeStr="08:50",
                    StartTime=Time.FromString("08:30"), EndTime=Time.FromString("08:50"),
                    TimeFrameStr="メールチェック等", NotificationEndStr="開始", BreakTimeMinutes=0},
                new TimeTableRow { IsOn=true, StartTimeStr= "08:50", EndTimeStr="09:00",
                    StartTime=Time.FromString("08:50"), EndTime=Time.FromString("09:00"),
                    TimeFrameStr="小休憩(1)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="09:00", EndTimeStr="09:50",
                    StartTime=Time.FromString("09:00"), EndTime=Time.FromString("09:50"),
                    TimeFrameStr="作業(1)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="09:50", EndTimeStr="10:00",
                    StartTime=Time.FromString("09:50"), EndTime=Time.FromString("10:00"),
                    TimeFrameStr="小休憩(2)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="10:00", EndTimeStr="10:50",
                    StartTime=Time.FromString("10:00"), EndTime=Time.FromString("10:50"),
                    TimeFrameStr = "作業(2)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="10:50", EndTimeStr="11:00",
                    StartTime=Time.FromString("10:50"), EndTime=Time.FromString("11:00"),
                    TimeFrameStr="小休憩(3)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="11:00", EndTimeStr="11:50",
                    StartTime=Time.FromString("11:00"), EndTime=Time.FromString("11:50"),
                    TimeFrameStr="作業(3)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow{ IsOn=true, StartTimeStr="11:50", EndTimeStr="12:00",
                    StartTime=Time.FromString("11:50"), EndTime=Time.FromString("12:00"),
                    TimeFrameStr="小休憩(4)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="12:00", EndTimeStr="13:00",
                    StartTime=Time.FromString("12:00"), EndTime=Time.FromString("13:00"),
                    TimeFrameStr="昼休憩", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=false, StartTimeStr="13:00", EndTimeStr="13:50",
                    StartTime=Time.FromString("13:00"), EndTime=Time.FromString("13:50"),
                    TimeFrameStr="作業(4)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="13:50", EndTimeStr="14:00",
                    StartTime=Time.FromString("13:50"), EndTime=Time.FromString("14:00"),
                    TimeFrameStr="小休憩(5)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="14:00", EndTimeStr="14:50",
                    StartTime=Time.FromString("14:00"), EndTime=Time.FromString("14:50"),
                    TimeFrameStr="作業(5)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="14:50", EndTimeStr="15:00",
                    StartTime=Time.FromString("14:50"), EndTime=Time.FromString("15:00"),
                    TimeFrameStr="小休憩(6)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="15:00", EndTimeStr="15:50",
                    StartTime=Time.FromString("15:00"), EndTime=Time.FromString("15:50"),
                    TimeFrameStr="作業(6)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="15:50", EndTimeStr="16:00",
                    StartTime=Time.FromString("15:50"), EndTime=Time.FromString("16:00"),
                    TimeFrameStr="小休憩(7)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="16:00", EndTimeStr="16:50",
                    StartTime=Time.FromString("16:00"), EndTime=Time.FromString("16:50"),
                    TimeFrameStr="作業(7)", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow { IsOn=true, StartTimeStr="16:50", EndTimeStr="17:00",
                    StartTime=Time.FromString("16:50"), EndTime=Time.FromString("17:00"),
                    TimeFrameStr="小休憩(8)", NotificationEndStr="開始", BreakTimeMinutes=10 },
                new TimeTableRow { IsOn=false, StartTimeStr="17:00", EndTimeStr="17:15",
                    StartTime=Time.FromString("17:00"), EndTime=Time.FromString("17:15"),
                    TimeFrameStr="明日の準備等", NotificationEndStr="開始", BreakTimeMinutes=0 },
                new TimeTableRow{ IsOn=false, StartTimeStr="17:15", EndTimeStr="17:15",
                    StartTime=Time.FromString("17:15"), EndTime=Time.FromString("17:15"),
                    TimeFrameStr="終業", NotificationEndStr="開始", BreakTimeMinutes=0 },
            ];
        }
        public ObservableCollection<TimeTableRow> GetTimeTable() => _timeTableRows;

        public List<TimeTableRow> CloneTimeTable(ObservableCollection<TimeTableRow> timeTableRows)
        {
            var list = new List<TimeTableRow>();
            foreach (var row in timeTableRows.ToList())
            {
                list.Add(row.Clone());
            }
            return list;
        }
    }
}