using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PomoTimer.Domain.Service;
using PomoTimer.Infrastructure;
using PomoTimer.Models;
using PomoTimer.ViewModels;
using PomoTimer.Services;
using PomoTimer.AppServices;
using Windows.Media.Core;

namespace PomoTimer
{
    public partial class App : Application
    {
        public App() { }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceCollection services = new();

            // 共通サービス
            services.AddSingleton<AppSettingService>();
            services.AddSingleton<TimerService>();
            services.AddSingleton<LoggerService>();
            services.AddSingleton<ToastService>();
            services.AddSingleton<ExcelService>();
            services.AddSingleton<MessageBoxService>();
            services.AddSingleton<ShutdownService>();

            // ComfirmWindow用
            services.AddTransient<ComfirmWindowVM>();
            services.AddTransient<ComfirmWindow>();

            // MainTimerModel用
            services.AddSingleton<TimeTableService>();
            services.AddSingleton<FixedTimeTableService>();
            services.AddSingleton<TimeTableTimerModel>();
            services.AddSingleton<VariableTimerModel>();

            // 時刻表タイマー用
            services.AddSingleton<TimeTableCSVRepository>();
            services.AddSingleton<TimeTableInMemoryRepository>();

            // BreakTimerWindow用
            services.AddTransient<BreakTimerWindowModel>();
            services.AddTransient<BreakTimerWindowVM>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());

            LoggerService loggerService = Ioc.Default.GetRequiredService<LoggerService>();
            
            int miliseconds = 1000 - DateTime.Now.Microsecond;
            Task.Delay(miliseconds).Wait();
            loggerService.Info($"{miliseconds} miliseconds wait.(Near 000 milisecond start)");
            loggerService.DeletePastLogs();  // 直近7日間より古いログを削除
        }
    }
}