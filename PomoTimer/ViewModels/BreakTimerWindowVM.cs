using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.Models;
using PomoTimer.AppServices;
using System.Windows;
using PomoTimer.Domain;

namespace PomoTimer.ViewModels
{
    public partial class BreakTimerWindowVM : ObservableObject
    {
        private readonly BreakTimerWindowModel _model;
        private readonly TimerService _timerService;
        private readonly LoggerService _loggerService;
        private readonly ExcelService _excelService;
        private readonly TimeProviderService _timeService;
        [ObservableProperty]
        private string timeFrameStr;
        [ObservableProperty]
        private string restTimeStr;
        [ObservableProperty]
        private string nowTimeStr;

        private Time _now;

        public Action? CloseAction { get; set; }
        public Action? ExitAction { get; set; }
        public Action? OnClosed { get; set; }

        public BreakTimerWindowVM()
        {
            _model = Ioc.Default.GetRequiredService<BreakTimerWindowModel>();
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            _excelService = Ioc.Default.GetRequiredService<ExcelService>();
            _timeService = DIContainer.GetTimeProviderService(false);
            _now = _timeService.Now.ToTime();
            // 初期値
            TimeFrameStr = _model.TimeFrameStr;
            RestTimeStr = "";
            NowTimeStr = _timeService.ToString(_timeService.Now.ToTime(), TimeProviderService.HHMM);
            _timerService.Tick += OnTick;
            _timerService.Start();
        }
        private void OnTick(object? sender, EventArgs e)
        {
            //Debug.WriteLine("Tick.");
            _now = _timeService.Now.ToTime();
            // Tick は UI スレッドで発火するため直接更新OK
            TimeFrameStr = _model.TimeFrameStr;
            TimeSpan intervel = _model.EndTime.TimeOnly - _now.TimeOnly;
            RestTimeStr = _timeService.ToString(_timeService.Rest.ToTimeSpan(_now, _model.EndTime), TimeProviderService.MMSS);
            // デバッグ用
            //RestTimeStr += $" [{_model.EndTimeSeconds}]";
            NowTimeStr = _timeService.ToString(_timeService.Now.ToTime(), TimeProviderService.HHMM);
            if (_model.EndTimeSeconds <= 0)
            {
                _loggerService.Info("Finish Break Time.");
                _excelService.WriteRow(_now, _model.StartTime, _model.EndTime, TimeFrameStr, "はい", "BreakTimer");
                StopTimer();
                CloseAction?.Invoke();
                return;
            }
            _model.CountDown();
        }
        public void SetValues(string timeFrameStr, int breakMinutes)
        {
            Time now = _timeService.Now.ToTime();
            _model.SetValues(now, timeFrameStr, breakMinutes);
            _model.CountDown();
        }
        [RelayCommand]
        public void CloseWithoutSave()
        {
            if (ShowMessageBox("保存しないで、この画面を閉じますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit Break Time without save.");
                CloseAction?.Invoke();
            }
        }
        [RelayCommand]
        public void Close()
        {
            if (ShowMessageBox("この画面を閉じますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit Break Time with save.");
                _excelService.WriteRow(_now, _model.StartTime, _model.EndTime, TimeFrameStr, "いいえ", "BreakTimer");
                CloseAction?.Invoke();
            }
        }
        [RelayCommand]
        public void ForceQuit()
        {
            if (ShowMessageBox("本当に終了しますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit App with save.");
                _excelService.WriteRow(_now, _model.StartTime, _model.EndTime, TimeFrameStr, "いいえ", "BreakTimer");
                ExitAction?.Invoke();
            }
        }
        private MessageBoxResult ShowMessageBox(string message) => 
            MessageBox.Show(message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
        public void StopTimer() => _timerService.Tick -= OnTick;
    }
}
