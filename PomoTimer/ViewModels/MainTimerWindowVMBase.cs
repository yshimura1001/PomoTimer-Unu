using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.AppServices;
using PomoTimer.Models;
using PomoTimer.Services;
using System.Windows;
using System.Windows.Documents;

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
        public readonly ShutdownService _shutdownService;
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

        protected DateTime _now;
        protected DateTime _startTime;
        protected DateTime _endTime;

        public Action? TopMostAction { get; set; }
        public Action? DarkModeAction { get; set; }
        public Action? CloseAction { get; set; }

        public MainTimerWindowVMBase(string modeStr, TimerModelBase model)
        {
            _model = model;
            _appSettingService = Ioc.Default.GetRequiredService<AppSettingService>();
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            _toastService = Ioc.Default.GetRequiredService<ToastService>();
            _excelService = Ioc.Default.GetRequiredService<ExcelService>();
            _shutdownService = Ioc.Default.GetRequiredService<ShutdownService>();

            appTitleStr = _appSettingService.GetAppTitleShort() + " - " + modeStr;
            _now = _timerService.Now();
            // 初期値
            CurrentTimeFrameStr = "null<Base>";
            CurrentTimeFrameRangeStr = "null<Base>";
            RestTimeStr = "null<Base>";
            NowTimeStr = _timerService.GetNowStrByHHmm(_now);
        }
        protected void Update()
        {
            // モデルのデータを更新
            _model.Update();
            // 表示用変数の元になる変数などを更新
            _now = _timerService.Now();
            _startTime = _model.GetCurrentStartTime();
            _endTime = _model.GetCurrentEndTime();
            // 表示用変数を更新
            CurrentTimeFrameStr = _model.CurrentTimeFrameStr;
            CurrentTimeFrameRangeStr = _model.CurrentTimeFrameRangeStr;
            RestTimeStr = (_model.CurrentTimeFrameRangeStr != "なし") ?
                _timerService.GetRestTimeStrByhhmm(_now, _model.EndTime) : "なし";
            NowTimeStr = _timerService.GetNowStrByHHmm(_now);
        }
        protected void OnTick(object? sender, EventArgs e)
        {
            // Tick は UI スレッドで発火するため直接更新OK
            Update();
            //_loggerService.Info("Tick.");
            // 通知フラグがONかつ、開始時間と一致した場合
            if(_model.IsShowBreakTimerWindow(_now))
            {
                _loggerService.Info("Show BreakTimerWindow.");
                new BreakTimerWindow(_model.CurrentTimeFrameStr, _model.BreakMinutes).Show();
            }
            // 通知フラグがOFFかつ、開始時間と一致した場合
            if(_model.IsSkipBreakTime(_now))
            {
                _loggerService.Info("Skip Break Time.");
                _excelService.WriteRow(_now, _startTime, _endTime, CurrentTimeFrameStr, "いいえ", "TimeTableTimer");
            }
            // 次の時間枠の通知フラグがONの場合、休憩の開始時間の3分前に休憩準備のメッセージを出す
            if(_model.IsShowNotifitonToast(_now, CurrentTimeFrameStr, _model.GetDateTimeAddTime(_endTime, 0, -3, 0)))
            {
                _loggerService.Info("Show Toast Break Before 3 minutes.");
                _toastService.ShowToast("休憩時間の3分前です。休憩の準備をしてください。");
            }
            // 17時になったら、アプリを終了する
            AppShutdownAt17pm();
            // 21時になったら、自動でPCごとシャットダウンする
            SystemShutdownAt21pm();
        }
        protected void AppShutdownAt17pm()
        {
            if (_now == _timerService.CreateDateTime(_now, 17, 0, 0))
            {
                _loggerService.Info("App Auto Exit.(17:00)");
                _toastService.ShowToast("17時になりましたので、自動終了しました。");
                Dispose();
                Application.Current.Shutdown();
            }
        }
        public void SystemShutdownAt21pm()
        {
            if (_now == _timerService.CreateDateTime(_now, 21, 0, 0))
            {
                _loggerService.Info("System Auto Shutdown.(21:00)");
                _shutdownService.Shutdown();
            }
        }
        protected void Dispose()
        {
            _timerService.Tick -= OnTick;
            _timerService.Dispose();
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
        public void ShowBreakTimerWindow() => new BreakTimerWindow("休憩中 [手動]", 10).Show();
    }
}
