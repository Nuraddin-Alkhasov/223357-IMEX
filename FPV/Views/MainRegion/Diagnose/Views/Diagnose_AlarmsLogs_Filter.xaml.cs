using VisiWin.ApplicationFramework;

namespace HMI.Diagnose
{
	/// <summary>
	/// Interaction logic for AlarmHistoryFilterView.xaml
	/// </summary>
    [ExportView("Diagnose_AlarmsLogs_Filter")]
	public partial class Diagnose_AlarmsLogs_Filter : VisiWin.Controls.View
	{
		public Diagnose_AlarmsLogs_Filter()
		{

			this.InitializeComponent();
		}

        private void Btn5_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn5.IsSelected = true;
        }

        private void Btn4_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
           btn4.IsSelected = true;
        }

        private void Btn2_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn2.IsSelected = true;
        }

        private void Btn1_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn1.IsSelected = true;
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                HistoricalAlarmFilterAdapter a = (HistoricalAlarmFilterAdapter)this.DataContext;
                int temp = a.SelectedTimeSpanFilterTypeIndex;
                a.TimeSpanFilterTypes = null;
                a.SetTimeSpan(a.TimeSpanFilterTypes[temp].FilterType);
                a.SelectedTimeSpanFilterTypeIndex = temp;
            }
        }
    }
}