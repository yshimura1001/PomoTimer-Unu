using System.Diagnostics;
using System.Windows.Threading;

namespace PomoTimer.AppServices
{
    public class TimerService : IDisposable
    {
        protected TimeSpan _interval;
        protected PeriodicTimer _timer;
        protected Dispatcher _dispatcher;
        protected CancellationTokenSource? _cts;
        protected Task? _loopTask;
        public event EventHandler? Tick;

        public TimerService(Dispatcher? dispatcher = null)
        {
            _interval = TimeSpan.FromSeconds(1);
            _dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
            _timer = new PeriodicTimer(_interval);
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

        public void Dispose() => Stop();
    }
}
