using VisiWin.ApplicationFramework;
using VisiWin.Language;
using VisiWin.Logging;


namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_StatusPD")]
	public partial class MO_StatusPD : VisiWin.Controls.View
	{
        private readonly ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();

        public MO_StatusPD()
		{
			this.InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }
    }
}