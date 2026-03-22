
namespace Test
{
    [Collection("Global test collection")]
    public class MainTimerWindowVMTest
    {
        /*
        private List<MainTimerWindowVMBase> viewModels;
        private readonly TimerService _timerService;
        public MainTimerWindowVMTest()
        {
            viewModels = new List<MainTimerWindowVMBase>();
            _timerService = Ioc.Default.GetRequiredService<FakeTimerService>();
            bool isTest = true;
            TimeTableServiceBase timeTableService = new FakeTimeTableService();
            TimerModelBase timerModel = new TimeTableTimerModel(timeTableService);
            viewModels.Add(new TimeTableMainTimerWindowVM(isTest, timerModel));
            timeTableService = new FakeFixedTimeTableService();
            timerModel = new TimeTableTimerModel(timeTableService);
            viewModels.Add(new TimeTableMainTimerWindowVM(isTest, timerModel));
            //timerModel = new VariableTimerModel();
            //viewModels.Add(new TimeTableMainTimerWindowVM(isTest, timerModel));
            _timerService.SetLocalTimeZone();
        } */
        /*
                    // 通知フラグがONかつ、開始時間と一致した場合、休憩画面を表示
            if (IsShowBreakTimerWindow(_now)) ShowBreakTimerWindow();
            // 通知フラグがOFFかつ、開始時間と一致した場合、休憩画面を表示しない(Excelへの記録は行う)
            if (IsSkipBreakTime(_now)) SkipBreakTime();
            // 次の時間枠の通知フラグがONの場合、休憩の開始時間の3分前に休憩準備のメッセージを出す
            if (IsShowNotifitonToast(_now, CurrentTimeFrameStr, _model.GetDateTimeAddTime(_endTime, 0, -3, 0)))
                ShowNotifitonToast();
            _model.OnTick(_now);
            // 17時になったら、アプリを終了する
            if (IsNow17pm()) AppShutdown();
            // 21時になったら、自動でPCごとシャットダウンする
            if (IsNow21pm()) SystemShutdown();
        */

        /*
        [Fact]
        public void Test1()
        {
            // このテストを書いた日を設定
            DateTimeOffset dateTimeOffset = new(2026, 3, 9, 10, 49, 59, TimeSpan.FromHours(9));
            _timerService.SetTime(dateTimeOffset);
            TickAndUpdate();
            _timerService.AdvanceTime(TimeSpan.FromSeconds(1));
            TestIsShowBreakTimerWindow();
        }
        private void TickAndUpdate()
        {
            viewModels.ForEach(vm => {
                vm.Update();
                vm.OnTickByModel();
            });
        }
        private void TestIsShowBreakTimerWindow()
        {
            bool isOk;
            viewModels.ForEach(vm => {
                vm.Update();
                isOk = vm.IsShowBreakTimerWindow();
                Assert.True(true);
                Assert.True(vm.IsShowBreakTimerWindow());
            });
        }
        */
    }
}
