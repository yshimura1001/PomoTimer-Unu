using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PomoTimer.AppServices;
using PomoTimer.Models;
using PomoTimer.Services;
using System.Windows;

namespace PomoTimer.ViewModels
{
    public partial class BreakTimerWindowVM : ObservableObject
    {
        private readonly BreakTimerWindowModel _model;
        private readonly TimerService _timerService;
        private readonly LoggerService _loggerService;
        private readonly ExcelService _excelService;
        private readonly MessageBoxService _messageBoxService;
        [ObservableProperty]
        private string timeFrameStr;
        [ObservableProperty]
        private string restTimeStr;
        [ObservableProperty]
        private string nowTimeStr;

        private DateTime _now;

        public Action? CloseAction { get; set; }
        public Action? ExitAction { get; set; }
        public Action? OnClosed { get; set; }

        public BreakTimerWindowVM()
        {
            _model = Ioc.Default.GetRequiredService<BreakTimerWindowModel>();
            _timerService = Ioc.Default.GetRequiredService<TimerService>();
            _loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            _excelService = Ioc.Default.GetRequiredService<ExcelService>();
            _messageBoxService = Ioc.Default.GetRequiredService<MessageBoxService>();
            _now = _timerService.Now();
            // 初期値
            TimeFrameStr = _model.TimeFrameStr;
            RestTimeStr = "";
            NowTimeStr = _timerService.GetNowStrByHHmm(_now);
            _timerService.Tick += OnTick;
            _timerService.Start();
        }
        private void OnTick(object? sender, EventArgs e)
        {
            //Debug.WriteLine("Tick.");
            _now = _timerService.Now();
            // Tick は UI スレッドで発火するため直接更新OK
            TimeFrameStr = _model.TimeFrameStr;
            TimeSpan intervel = _model.EndTime - _now;
            RestTimeStr = _timerService.GetRestTimeStrBymmss(_now, _model.EndTime);
            // デバッグ用
            //RestTimeStr += $" [{_model.EndTimeSeconds}]";
            NowTimeStr = _timerService.GetNowStrByHHmm(_now);
            //if (_model.EndTimeSeconds <= 0)
            if(_now >= _model.EndTime)
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
            DateTime now = _timerService.Now();
            _model.SetValues(now, timeFrameStr, breakMinutes);
            _model.CountDown();
        }
        [RelayCommand]
        public void CloseWithoutSave()
        {
            if (_messageBoxService.ShowQuestion("保存しないで、この画面を閉じますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit Break Time without save.");
                CloseAction?.Invoke();
            }
        }
        [RelayCommand]
        public void Close()
        {
            if (_messageBoxService.ShowQuestion("この画面を閉じますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit Break Time with save.");
                _excelService.WriteRow(_now, _model.StartTime, _model.EndTime, TimeFrameStr, "いいえ", "BreakTimer");
                CloseAction?.Invoke();
            }
        }
        [RelayCommand]
        public void ForceQuit()
        {
            if (_messageBoxService.ShowQuestion("本当に終了しますか？") == MessageBoxResult.Yes)
            {
                _loggerService.Info("Quit App with save.");
                _excelService.WriteRow(_now, _model.StartTime, _model.EndTime, TimeFrameStr, "いいえ", "BreakTimer");
                ExitAction?.Invoke();
            }
        }
        public void StopTimer() => _timerService.Tick -= OnTick;
    }
}
