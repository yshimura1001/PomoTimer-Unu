using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.AppServices;
using PomoTimer.Domain;
using PomoTimer.Domain.Service;
using PomoTimer.Models;
using System.Windows;

namespace PomoTimer.ViewModels
{
    public abstract partial class MainTimerWindowVMBase : ObservableObject
    {
        protected readonly TimerModelBase _model;
        protected readonly AppSettingService _appSettingService;
        protected readonly TimerService _timerService;
        protected readonly LoggerService _loggerService;
        protected readonly ToastService _toastService;
        protected readonly ExcelService _excelService;
        protected readonly ShutdownService _shutdownService;
        protected TimeProviderService _timeService;
        [ObservableProperty]
        private string appTitleStr;
        [ObservableProperty]
        private string currentTimeFrameStr;
        [ObservableProperty]
        private string currentTimeFrameRangeStr;
        [ObservableProperty]
        private string restTimeStr;
        [ObservableProperty]
        private string nowTimeStr;

        protected Time _now;
        protected Time _startTime;
        protected Time _endTime;

        public Action? TopMostAction { get; set; }
        public Action? DarkModeAction { get; set; }
        public Action? CloseAction { get; set; }

        public MainTimerWindowVMBase(bool isTest, TimerModelBase model)
        {
            _model = model;
            _appSettingService = Ioc.Default.GetRequiredService<AppSettingService>();
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            _toastService = Ioc.Default.GetRequiredService<ToastService>();
            _excelService = Ioc.Default.GetRequiredService<ExcelService>();
            _shutdownService = Ioc.Default.GetRequiredService<ShutdownService>();
            _timeService = DIContainer.GetTimeProviderService(false);
            string modeStr = GetModeStr(model);
            appTitleStr = _appSettingService.GetAppTitleShort() + " - " + modeStr;
            _startTime = Time.MinValue;
            _endTime = Time.MinValue;
            _now = _timeService.Now.ToTime();
            // 初期値
            CurrentTimeFrameStr = "null<Base>";
            CurrentTimeFrameRangeStr = "null<Base>";
            RestTimeStr = "null<Base>";
            NowTimeStr = _timeService.ToString(_timeService.Now.ToTime(), TimeProviderService.HHMM);
        }
        private string GetModeStr(TimerModelBase model)
        {
            string modeStr = "";
            switch (model.GetTimeTableService())
            {
                case TimeTableService:
                    modeStr = "時刻表";
                    break;
                case FixedTimeTableService:
                    modeStr = "固定";
                    break;
                case null:
                    modeStr = "変動";
                    break;
            }
            return modeStr;
        }
        public void Update()
        {
            // モデルのデータを更新
            _model.Update();
            // 表示用変数の元になる変数などを更新
            _now = _timeService.Now.ToTime();
            _startTime = _model.GetCurrentStartTime();
            _endTime = _model.GetCurrentEndTime();
            // 表示用変数を更新
            CurrentTimeFrameStr = _model.CurrentTimeFrameStr;
            CurrentTimeFrameRangeStr = _model.CurrentTimeFrameRangeStr;
            RestTimeStr = (_model.CurrentTimeFrameRangeStr != "なし") ?
                RestTimeStr = _timeService.ToString(_timeService.Rest.ToTimeSpan(_now, _model.EndTime), TimeProviderService.HHMM) : "なし";
            NowTimeStr = _timeService.ToString(_now, TimeProviderService.HHMM);
        }
        protected void OnTick(object? sender, EventArgs e)
        {
            // Tick は UI スレッドで発火するため直接更新OK
            Update();
            //_loggerService.Info("Tick.");
            // 通知フラグがONかつ、開始時間と一致した場合、休憩画面を表示
            if (IsShowBreakTimerWindow()) ShowBreakTimerWindow();
            // 通知フラグがOFFかつ、開始時間と一致した場合、休憩画面を表示しない(Excelへの記録は行う)
            if (IsSkipBreakTime()) SkipBreakTime();
            // 次の時間枠の通知フラグがONの場合、休憩の開始時間の3分前に休憩準備のメッセージを出す
            if (IsShowNotifitonToast())
                ShowNotifitonToast();
            OnTickByModel();
            // 17時になったら、アプリを終了する
            if (IsNow17pm()) AppShutdown();
            // 21時になったら、自動でPCごとシャットダウンする
            if (IsNow21pm()) SystemShutdown();
        }
        public void OnTickByModel() => _model.OnTick(_now);
        public virtual void RecreateTimeTable() { /* 何もしない */ } 
        public bool IsShowBreakTimerWindow() => _model.IsShowBreakTimerWindow();
        protected void ShowBreakTimerWindow()
        {
            _loggerService.Info("Show BreakTimerWindow.");
            new BreakTimerWindow(_model.CurrentTimeFrameStr, _model.BreakMinutes).Show();
        }
        public bool IsSkipBreakTime() => _model.IsSkipBreakTime();
        protected void SkipBreakTime()
        {
            _loggerService.Info("Skip Break Time.");
            _excelService.WriteRow(_now, _startTime, _endTime, CurrentTimeFrameStr, "いいえ", "MainTimerWindow");
        }
        public bool IsShowNotifitonToast()
        {
            return _model.IsShowNotifitonToast(CurrentTimeFrameStr, _endTime.Add(0, -3, 0));
        }
        public void ShowNotifitonToast()
        {
            _loggerService.Info("Show Toast Break Before 3 minutes.");
            _toastService.ShowToast("休憩時間の3分前です。休憩の準備をしてください。");
        }
        public bool IsNow17pm() => (_now == Time.Create(17, 0, 0));
        protected void AppShutdown()
        {
            _loggerService.Info("App Auto Exit.(17:00)");
            _toastService.ShowToast("17時になりましたので、自動終了しました。");
            Dispose();
            Application.Current.Shutdown();
        }
        public bool IsNow21pm() => (_now == Time.Create(21, 0, 0));
        protected void SystemShutdown()
        {
            _loggerService.Info("System Auto Shutdown.(21:00)");
            _shutdownService.Shutdown();
        }
        [RelayCommand]
        public void ChangeTopMost() => TopMostAction?.Invoke();
        [RelayCommand]
        public void DarkMode() => DarkModeAction?.Invoke();
        [RelayCommand]
        public void ModeChange()
        {
            new ComfirmWindow().Show();
            CloseAction?.Invoke();
        }
        [RelayCommand]
        public void ShowBreakTimerWindowMaual() => new BreakTimerWindow("休憩中 [手動]", 10).Show();
        protected void Dispose()
        {
            _timerService.Tick -= OnTick;
            _timerService.Dispose();
        }
    }
}
