using System.Windows;

namespace PomoTimer.AppServices
{
    public class MessageBoxService
    {
        public MessageBoxResult ShowQuestion(string message) =>
            MessageBox.Show(message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
    }
}
