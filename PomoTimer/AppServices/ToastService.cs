using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PomoTimer.AppServices
{
    public class ToastService
    {
        private readonly AppSettingService _appSettingService;
        public ToastService()
        {
            _appSettingService = Ioc.Default.GetRequiredService<AppSettingService>();
        }
        public void ShowToast(string message)
        {
            new ToastContentBuilder()
                .AddText(_appSettingService.GetAppName())
                .AddText(message)
                .Show();
        }
    }
}
