using PomoTimer.Domain;
using System.Collections.ObjectModel;

namespace PomoTimer.Infrastructure
{
    public interface ITimeTableRepository
    {
        public ObservableCollection<TimeTableRow> GetTimeTable();
    }
}
