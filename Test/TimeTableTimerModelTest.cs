
using PomoTimer.Domain.Service;
using PomoTimer.Models;

namespace Test
{
    [Collection("Global test collection")]
    public class TimeTableTimerModelTest
    {
        protected TimeTableServiceBase timeTableService;
        protected TimerModelBase timerModel;
        public TimeTableTimerModelTest()
        {
            timeTableService = new TimeTableService();
            timerModel = new TimeTableTimerModel(timeTableService);
        }
        [Fact]
        public void Test1()
        {

        }
    }
}
