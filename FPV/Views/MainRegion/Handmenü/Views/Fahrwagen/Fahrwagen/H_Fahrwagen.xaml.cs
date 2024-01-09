using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;

namespace HMI.Handmenu.Fahrwagen
{
    /// <summary>
    /// Interaction logic for ButtonsView.xaml
    /// </summary>
    [ExportView("H_Fahrwagen")]
 
    public partial class H_Fahrwagen : View
    {
        private readonly VisiWin.Logging.ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();
        public H_Fahrwagen()
        {
            this.InitializeComponent();
           
        }

        private void ComboBox_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                string txt = "";
                switch ((short)e.Value)
                {
                    case 0: txt = textService.GetText("@Logging.Service.Text21"); break;
                    case 1: txt = textService.GetText("@Logging.Service.Text20"); break;
                }

                this.loggingService.Log("Service", "New Reference", txt, FastDateTime.Now);
            }
        }

        private void Key_Click(object sender, System.Windows.RoutedEventArgs e)
        {
 			string txt = textService.GetText("@Logging.Service.Text22");
            this.loggingService.Log("Service", "CarriageSpecialRight", txt + " : " + ((VisiWin.Controls.Key)sender).Tag.ToString(), FastDateTime.Now);
        }
    }
}