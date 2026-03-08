using Microsoft.Extensions.Time.Testing;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;

namespace PomoTimer.AppServices
{
    public abstract class TimerServiceBase : IDisposable
    {
        protected TimeSpan _interval;
        protected PeriodicTimer _timer;
        protected Dispatcher _dispatcher;
        protected CancellationTokenSource? _cts;
        protected Task? _loopTask;
        public event EventHandler? Tick;
        public FakeTimeProvider FakeTimeProvider;

        public TimerServiceBase(Dispatcher? dispatcher = null)
        {
            _interval = TimeSpan.FromSeconds(1);
            _dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
            _timer = new PeriodicTimer(_interval);
            FakeTimeProvider = new FakeTimeProvider();
        }
        public TimeSpan Interval
        {
            get => _interval;
            set => _interval = value; // 再起動で反映
        }

        public bool IsEnabled => _loopTask is { IsCompleted: false };

        public void Start()
        {
            if (IsEnabled) return;

            _cts = new CancellationTokenSource();
            _loopTask = RunLoopAsync(_cts.Token);
        }
        public void Stop()
        {
            if (_cts == null) return;

            try { _cts.Cancel(); } catch { /* ignore */ }

            try { _loopTask?.Wait(100); } catch { /* ignore */ }

            _cts.Dispose();
            _cts = null;
            _loopTask = null;
        }

        private async Task RunLoopAsync(CancellationToken ct)
        {
            //using var timer = new PeriodicTimer(_interval);
            try
            {
                while (await _timer.WaitForNextTickAsync(ct).ConfigureAwait(false))
                {
                    var handler = Tick;
                    if (handler != null)
                    {
                        try
                        {
                            await _dispatcher.InvokeAsync(() =>
                            {
                                try
                                {
                                    handler(this, EventArgs.Empty);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.ToString());
                                }
                            }, DispatcherPriority.Normal);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Stop・Dispose によるキャンセル
                Debug.WriteLine(ex.ToString());
            }
        }
        public DateTime Now()
        {
            TimeProvider timeProvider = (this is FakeTimerService) ? FakeTimeProvider : TimeProvider.System;
            DateTimeOffset now = timeProvider.GetLocalNow();
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
        }
        public string GetTimeStr(DateTime dateTime) => dateTime.ToString("HH:mm");
        public string GetRestTimeStrByhhmm(DateTime now, DateTime endTime) => GetRestTimeStr(now, endTime, @"hh\:mm");

        public string GetRestTimeStrByhhmm(DateTime now, string endTimeStr) =>
            GetRestTimeStrByhhmm(now, Convert(endTimeStr));

        public string GetRestTimeStrBymmss(DateTime now, DateTime endTime) => GetRestTimeStr(now, endTime, @"mm\:ss");

        public string GetRestTimeStrBymmss(DateTime now, string endTimeStr) =>
            GetRestTimeStrBymmss(now, Convert(endTimeStr));

        protected string GetRestTimeStr(DateTime now, DateTime endTime, string format)
        {
            TimeSpan interval = endTime - now;
            return ChangeTimeSeparateChar(interval, format);
        }
        public string GetNowStrByHHmm(DateTime now) => ChangeTimeSeparateChar(now, now.ToString("HH:mm"));

        protected string ChangeTimeSeparateChar(DateTime now, string timeStr) =>
            (now.Second % 2 == 0) ? timeStr : timeStr.Replace(":", " ");
        protected string ChangeTimeSeparateChar(TimeSpan span, string format)
        {
            string timeStr = span.ToString(format);
            return (span.Seconds % 2 == 0) ? timeStr : timeStr.Replace(":", " ");
        }
        public DateTime Convert(string timeStr)
        {
            DateTime now = Now();
            string[] formats = { "HH:mm", "H:mm" };
            var culture = CultureInfo.GetCultureInfo("ja-JP"); // 日本
            var styles = DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces;
            if (DateTime.TryParseExact(timeStr, formats, culture, styles, out DateTime dt))
            {
                string[] strs = timeStr.Split(':');
                return CreateDateTime(now, System.Convert.ToInt32(strs[0]), System.Convert.ToInt32(strs[1]), 0, 0);
            }
            else
                return dt; // DateTime.MinValue
        }
        public DateTime CreateDateTime(DateTime now, int hours, int minutes, int seconds)
        {
            return new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds, 0);
        }
        protected DateTime CreateDateTime(DateTime now, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds, milliseconds);
        }
        public void Dispose() => Stop();
    }
}
