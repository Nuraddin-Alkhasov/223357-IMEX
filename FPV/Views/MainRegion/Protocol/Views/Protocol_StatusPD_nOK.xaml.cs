using VisiWin.ApplicationFramework;
using VisiWin.Language;
using VisiWin.Logging;


namespace HMI.Views.MainRegion.Protocol
{

    [ExportView("Protocol_StatusPD_nOK")]
	public partial class Protocol_StatusPD_nOK : VisiWin.Controls.View
	{
        public Protocol_StatusPD_nOK()
		{
			this.InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }
    }
}