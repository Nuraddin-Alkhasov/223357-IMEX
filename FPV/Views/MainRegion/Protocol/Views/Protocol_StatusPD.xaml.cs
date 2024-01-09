using VisiWin.ApplicationFramework;
using VisiWin.Language;
using VisiWin.Logging;


namespace HMI.Views.MainRegion.Protocol
{

    [ExportView("Protocol_StatusPD")]
	public partial class Protocol_StatusPD : VisiWin.Controls.View
	{
        public Protocol_StatusPD()
		{
			this.InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }
    }
}