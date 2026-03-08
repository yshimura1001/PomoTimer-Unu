using PomoTimer.Domain.Service;
using PomoTimer.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PomoTimer
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindowVM vm { get; set; }
        public ConfigWindow(TimeTableServiceBase timeTableService)
        {
            InitializeComponent();
            vm = new ConfigWindowVM(timeTableService);
            DataContext = vm;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            // DataGridの編集中のセル及び行をコミット
            TimeTableGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            TimeTableGrid.CommitEdit(DataGridEditingUnit.Row, true);

            vm.Dispose();
            base.OnClosed(e);
        }
    }
}
