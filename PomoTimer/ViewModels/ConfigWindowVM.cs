using CommunityToolkit.Mvvm.ComponentModel;
using PomoTimer.Domain;
using PomoTimer.Domain.Service;
using System.Collections.ObjectModel;

namespace PomoTimer.ViewModels
{
    public class ConfigWindowVM : ObservableObject
    {
        private readonly TimeTableServiceBase _timeTableService;
        public ObservableCollection<TimeTableRow> TimeTableRows { get; set; }
        public ConfigWindowVM(TimeTableServiceBase timeTableService)
        {
            _timeTableService = timeTableService;
            TimeTableRows = _timeTableService.GetFullTimeTable();
        }
        public void Dispose()
        {
            _timeTableService.RecreateTimeTable();
        }
    }
}