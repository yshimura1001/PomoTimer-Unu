using CommunityToolkit.Mvvm.DependencyInjection;
using PomoTimer.ViewModels;
using System.Windows;

namespace PomoTimer
{
    public partial class ComfirmWindow : Window
    {
        public ComfirmWindow()
        {
            InitializeComponent();
            ComfirmWindowVM vm = Ioc.Default.GetRequiredService<ComfirmWindowVM>();
            vm.CloseAction = () => { this.Close(); };
            DataContext = vm;
        }
    }
}
