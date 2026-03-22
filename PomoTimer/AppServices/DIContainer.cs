using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using PomoTimer.Domain.Service;
using PomoTimer.Infrastructure;
using PomoTimer.Models;
using PomoTimer.ViewModels;

namespace PomoTimer.AppServices
{
    public static class DIContainer
    {
        public static void Bulid()
        {
            ServiceCollection services = new();
            // 共通サービス
            services.AddSingleton<AppSettingService>();
            services.AddTransient<TimerService>();
            services.AddSingleton<TimeProviderService>();
            services.AddSingleton<Func<string, TimeProviderService>>(sp => key =>
            {
                return key switch
                {
                    "Test" => new TimeProviderService(true),
                    "Release" => new TimeProviderService(false),
                    _ => throw new ArgumentException("Unknown key")
                };
            });
            services.AddSingleton<Func<string, NowTimeProviderService>>(sp => key =>
            {
                return key switch
                {
                    "Test" => new NowTimeProviderService(true),
                    "Release" => new NowTimeProviderService(false),
                    _ => throw new ArgumentException("Unknown key")
                };
            });
            services.AddSingleton<Func<string, RestTimeProviderService>>(sp => key =>
            {
                return key switch
                {
                    "Test" => new RestTimeProviderService(true),
                    "Release" => new RestTimeProviderService(false),
                    _ => throw new ArgumentException("Unknown key")
                };
            });
            services.AddSingleton<LoggerService>();
            services.AddSingleton<ToastService>();
            services.AddSingleton<ExcelService>();
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

            // テスト用
            services.AddSingleton<FakeTimeProvider>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
        public static TimeProviderService GetTimeProviderService(bool isTest)
        {
            string str = (isTest) ? "Test" : "Release";
            return Ioc.Default.GetRequiredService<Func<string, TimeProviderService>>().Invoke(str);
        }
        public static NowTimeProviderService GetNowTimeProviderService(bool isTest)
        {
            string str = (isTest) ? "Test" : "Release";
            return Ioc.Default.GetRequiredService<Func<string, NowTimeProviderService>>().Invoke(str);
        }
        public static RestTimeProviderService GetRestTimeProviderService(bool isTest)
        {
            string str = (isTest) ? "Test" : "Release";
            return Ioc.Default.GetRequiredService<Func<string, RestTimeProviderService>>().Invoke(str);
        }
    }
}
