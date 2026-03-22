
namespace PomoTimer.AppServices
{
    public class ShutdownService
    {
        public void Shutdown()
        {
            // 実行中のアプリを強制終了して、シャットダウンする
            System.Diagnostics.Process.Start("shutdown.exe", "/s /f /t 0");
        }
    }
}
