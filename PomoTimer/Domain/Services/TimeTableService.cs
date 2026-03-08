using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.Infrastructure;

namespace PomoTimer.Domain.Service
{
    public class TimeTableService : TimeTableServiceBase
    {
        private ITimeTableRepository _repository { get; set; }
        public TimeTableService()
        {
            _repository = Ioc.Default.GetRequiredService<TimeTableCSVRepository>();
            //_repository = Ioc.Default.GetRequiredService<TimeTableInMemoryRepository>();
            _timeTableRowsFull = _repository.GetTimeTable();
            _timeTableRows = CreateTimeTable();
        }
    }
}