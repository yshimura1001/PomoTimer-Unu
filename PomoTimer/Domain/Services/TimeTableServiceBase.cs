using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.AppServices;
using System.Collections.ObjectModel;

namespace PomoTimer.Domain.Service
{
    public abstract class TimeTableServiceBase
    {
        public ObservableCollection<TimeTableRow> _timeTableRowsFull { get; protected set; }
        public List<TimeTableRow> _timeTableRows { get; protected set; }
        protected TimeTableRow nullRow { get; private set; } // nullの場合これをCloneして返す
        protected readonly TimerService _timerService;
        protected TimeProviderService _timeService;

        public TimeTableServiceBase()
        {
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _timeService = DIContainer.GetTimeProviderService(false);
            nullRow = new TimeTableRow
            {
                IsOn = false,
                StartTimeStr = "",
                StartTime = Time.MinValue,
                EndTimeStr = "",
                EndTime = Time.MinValue,
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
            Time now = _timeService.Now.ToTime();
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
            Time now = _timeService.Now.ToTime();
            TimeTableRow? row = _timeTableRows.FirstOrDefault();
            if (row != null)
            {
                if (row.StartTime > now)
                {
                    return new TimeTableRow
                    {
                        IsOn = false,
                        StartTimeStr = "なし",
                        StartTime = Time.MinValue,
                        EndTimeStr = "なし",
                        EndTime = Time.MinValue,
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
        public Time GetCurrentStartTime() => GetCurrentRow().StartTime;
        public Time GetCurrnetEndTime() => GetCurrentRow().EndTime;
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
