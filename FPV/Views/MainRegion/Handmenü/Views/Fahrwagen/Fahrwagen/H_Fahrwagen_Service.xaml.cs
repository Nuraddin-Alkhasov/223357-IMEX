using HMI.Views.MessageBoxRegion;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;

namespace HMI.Handmenu.Fahrwagen
{
    /// <summary>
    /// Interaction logic for KeyAndSwitchView.xaml
    /// </summary>
    [ExportView("H_Fahrwagen_Service")]

    public partial class H_Fahrwagen_Service : View
    {
        private readonly VisiWin.Logging.ILoggingService loggingService;
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();
        public H_Fahrwagen_Service()
        {
            this.InitializeComponent();
            this.loggingService = ApplicationService.GetService<ILoggingService>();
        }

        private void Key_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBoxView.Show("@HandMenu.Text1", "@HandMenu.Text2", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.Yes)
            {
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.FA.DB FW FA HMI.PC.Fahrantrieb.Vorwahl Referenz", 1);
                

                string txt = textService.GetText("@Logging.Service.Text11");
                this.loggingService.Log("Service", "New Reference", txt, FastDateTime.Now);
            }
        }

        private void Key_Click_1(object sender, RoutedEventArgs e)
        {
            string txt = textService.GetText("@Logging.Service.Text22");
            this.loggingService.Log("Service", "CarriageSpecialRight", txt + " : " + ((VisiWin.Controls.Key)sender).Tag.ToString(), FastDateTime.Now);
        }
    }
}