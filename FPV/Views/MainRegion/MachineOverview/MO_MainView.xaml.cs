using HMI.Views.DialogRegion;
using HMI.Views.MessageBoxRegion;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;


namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_MainView")]
    public partial class MO_MainView : VisiWin.Controls.View
    {
        IVariableService VS;
        private readonly IVariable status;


        private readonly ILoggingService loggingService;
        ILanguageService textService;
        public MO_MainView()
        {
            InitializeComponent();
            VS = ApplicationService.GetService<IVariableService>();
            status = VS.GetVariable("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Steuerspannung Status Taster");
           
            loggingService = ApplicationService.GetService<ILoggingService>();
            textService = ApplicationService.GetService<ILanguageService>();
            status.Change += Status_Change;

        }

        private void Status_Change(object sender, VariableEventArgs e)
        {
            string txt = "";

            switch ((short)e.Value)
            {
                case 2:
                    ONOFF.VariableName = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Steuerspannung Aus";
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Steuerspannung Ein", 0);
                    txt = textService.GetText("@Logging.Service.Text18");
                    powerOFF.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    powerOFF.Visibility = Visibility.Visible; break;
                default:
                    powerOFF.Visibility = Visibility.Hidden;
                    ONOFF.VariableName = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Steuerspannung Ein";
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Steuerspannung Aus", 0);
                    txt = textService.GetText("@Logging.Service.Text19");
                    break;
            }

            this.loggingService.Log("Service", "Anlage Ein/Aus", txt, FastDateTime.Now);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogView.Show("MO_Status", "Status", DialogButton.Close, DialogResult.Cancel);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_Transporter");
        }

        private void Reg_Loaded(object sender, RoutedEventArgs e)
        {
            if (Reg.Content == null)
            {
                   Task obTask = Task.Run(() =>
                    {
                        Application.Current.Dispatcher.InvokeAsync((Action)delegate
                        {
                            Reg.Content = new MO_MaschineView();
                       
                        });
                    });
            }
        }
       


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("DialogRegion", "MO_Barcode");

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
          //  ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.MR requested", true);
        }
    }
}



