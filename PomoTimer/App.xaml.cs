using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PomoTimer.AppServices;

namespace PomoTimer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DIContainer.Bulid();
        }
    }
}