using System.Windows;
using System.Windows.Controls;
using VisiWin.ApplicationFramework;

namespace HMI
{
    /// <summary>
    /// Interaction logic for AlarmHistoryView.xaml
    /// </summary>
    [ExportView("LoggingView")]
    public partial class LoggingView : VisiWin.Controls.View
    {
        public LoggingView()
        {
            this.InitializeComponent();
        }
        private void _PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
        }
    }
}