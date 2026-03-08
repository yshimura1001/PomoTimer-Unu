using PomoTimer.AppServices;
using System.Windows.Threading;

namespace PomoTimer.Services
{
    public class TimerService : TimerServiceBase
    {
        public TimerService(Dispatcher? dispatcher = null)
        {
            _interval = TimeSpan.FromSeconds(1);
            _dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
            _timer = new PeriodicTimer(_interval);
        }
    }
}