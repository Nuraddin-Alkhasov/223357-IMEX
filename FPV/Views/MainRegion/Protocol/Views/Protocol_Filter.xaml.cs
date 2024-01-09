using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{
	/// <summary>
	/// Interaction logic for LoggingFilterView.xaml
	/// </summary>
    [ExportView("Protocol_Filter")]
	public partial class Protocol_Filter : VisiWin.Controls.View
	{
		public Protocol_Filter()
		{
			this.InitializeComponent();
        }

        private void Btn1_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn1.IsSelected = true;
        }

        private void Btn2_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn2.IsSelected = true;
        }

        private void Btn3_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn3.IsSelected = true;
        }

        private void Btn4_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn4.IsSelected = true;
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                ProtocolAdapter a = (ProtocolAdapter)this.DataContext;
                int temp = a.SelectedTimeSpanFilterTypeIndex;
                a.TimeSpanFilterTypes = null;
                a.SetTimeSpan(a.TimeSpanFilterTypes[temp].FilterType);
                a.SelectedTimeSpanFilterTypeIndex = temp;
            }
        }
    }
}