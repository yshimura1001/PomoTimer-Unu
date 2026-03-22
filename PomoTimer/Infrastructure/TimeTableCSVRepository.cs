using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Domain;
using PomoTimer.AppServices;
using System.Collections.ObjectModel;
using System.IO;

namespace PomoTimer.Infrastructure
{
    public class TimeTableCSVRepository : ITimeTableRepository
    {
        private ObservableCollection<TimeTableRow> _timeTableRows;
        private TimeProviderService _TimeService;
        private string csvFileName { get; } = "time_table.csv";

        public TimeTableCSVRepository()
        {
            _TimeService = DIContainer.GetTimeProviderService(false);
            _timeTableRows = new ObservableCollection<TimeTableRow>();
            using (StreamReader sr = new StreamReader($@"{Environment.CurrentDirectory}\{csvFileName}"))
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    // 行が空ではないかつ、1(0)行目ではない場合
                    if ((line != null) && (i > 0))
                    {
                        string[] values = line.Split(",");
                        bool isOn = bool.Parse(values[0]);
                        string startTimeStr = values[1];
                        Time startTime = Time.FromString(startTimeStr);
                        string endTimeStr = values[2];
                        Time endTime = Time.FromString(endTimeStr);
                        string timeFrameStr = values[3];
                        string notificationEndStr = values[4];
                        int breakTimeMinutes = int.Parse(values[5]);
                        var vo = new TimeTableRow {
                            IsOn = isOn, 
                            StartTimeStr = startTimeStr, 
                            StartTime = startTime,
                            EndTimeStr = endTimeStr, 
                            EndTime = endTime,
                            TimeFrameStr = timeFrameStr, NotificationEndStr = notificationEndStr, 
                            BreakTimeMinutes = breakTimeMinutes
                        };
                        _timeTableRows.Add(vo);
                    }
                    i++;
                }
            }
        }
        public ObservableCollection<TimeTableRow> GetTimeTable() => _timeTableRows;
    }
}