using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Services;
using System.Collections.ObjectModel;

namespace PomoTimer.Domain.Service
{
    public abstract class TimeTableServiceBase
    {
        protected ObservableCollection<TimeTableRow> _timeTableRowsFull { get; set; }
        protected List<TimeTableRow> _timeTableRows { get; set; }
        protected TimeTableRow nullRow { get; private set; } // nullの場合これをCloneして返す
        protected TimerService _timerService { get; set; }

        public TimeTableServiceBase()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            nullRow = new TimeTableRow
            {
                IsOn = false,
                StartTimeStr = "",
                StartTime = DateTime.MinValue,
                EndTimeStr = "",
                EndTime = DateTime.MinValue,
                TimeFrameStr = "<null>",
                NotificationEndStr = "",
                BreakTimeMinutes = 0
            };
            _timeTableRowsFull = new ObservableCollection<TimeTableRow>();
            _timeTableRows = new List<TimeTableRow>();
        }
        protected List<TimeTableRow> CloneTimeTable(ObservableCollection<TimeTableRow> timeTableRows)
        {
            var list = new List<TimeTableRow>();
            foreach (var row in timeTableRows.ToList())
                list.Add(row.Clone());
            return list;
        }
        protected List<TimeTableRow> CreateTimeTable()
        {
            List<TimeTableRow> list = CloneTimeTable(_timeTableRowsFull);
            DateTime now = _timerService.Now();
            //now = new DateTime(2026, 2, 20, 8, 0, 0);
            foreach (TimeTableRow row in list.ToList()) // 別のリストでないとエラーになるため、ToList()を呼び出す。
            {
                if (now >= row.EndTime)
                    list.RemoveAt(0); // 先頭の要素を削除
                else
                    break;
            }
            return list;
        }

        public ObservableCollection<TimeTableRow> GetFullTimeTable() => _timeTableRowsFull;
        public List<TimeTableRow> GetTimeTable() => _timeTableRows;
        public TimeTableRow GetCurrentRow()
        {
            DateTime now = _timerService.Now();
            TimeTableRow? row = _timeTableRows.FirstOrDefault();
            if (row != null)
            {
                if (row.StartTime > now)
                {
                    return new TimeTableRow
                    {
                        IsOn = false,
                        StartTimeStr = "なし",
                        StartTime = DateTime.MinValue,
                        EndTimeStr = "なし",
                        EndTime = DateTime.MinValue,
                        TimeFrameStr = "始業前",
                        NotificationEndStr = "",
                        BreakTimeMinutes = 0
                    };
                } 
                else if(row.EndTime <= now) 
                {
                    return GetNextRow();
                }
                return row;
            }
            return nullRow.Clone();
        }
        public bool GetCurrentIsOn() => GetCurrentRow().IsOn;
        public bool GetNextIsOn() => GetNextRow().IsOn;
        public DateTime GetCurrentStartTime() => GetCurrentRow().StartTime;
        public DateTime GetCurrnetEndTime() => GetCurrentRow().EndTime;
        public void DeleteNowRow()
        {
            TimeTableRow row = GetCurrentRow();
            if (row.StartTimeStr != "")
            {
                _timeTableRows.RemoveAt(0);
            }
        }
        public TimeTableRow GetNextRow() =>
            _timeTableRows[1] != null ? _timeTableRows[1] : nullRow.Clone();

        public void RecreateTimeTable() => _timeTableRows = CreateTimeTable(); // 内部でディープコピー
    }
}
