using HMI.Module;
using HMI.Views.DialogRegion;
using HMI.Views.MessageBoxRegion;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_StatusIn")]
	public partial class MO_StatusIn : VisiWin.Controls.View
	{
        private readonly ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();
        IVariableService VS;
        IVariable newDataV;

        public MO_StatusIn()
		{
			this.InitializeComponent();
        }

        private void NewDataV_Change(object sender, VariableEventArgs e)
        {
            if (data2.IsVisible && e.Value.ToString() != "")
            {
                if (ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() != "")
                {
                    user.Value = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();
                    data2.Value = e.Value.ToString();
                    ApplicationService.SetVariableValue("DataPicker.DatafromScanner", "");
                    LoadMR(e.Value.ToString());
                }
                else
                {
                    new MessageBoxTask("@Datenauswahl.Text13", "@Datenauswahl.Text15", MessageBoxIcon.Warning);
                }
            }  
        }

        private void _IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                new SP("3", "1");
                data1.Value = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Charge#STRING20").ToString();
                data2.Value = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Barcode#STRING20").ToString();
                mr.Value = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Maschinenprogramm#STRING20").ToString();
                user.Value = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.User#STRING20").ToString();
                LoadDescription(mr.Value);
            }
            else
            {
                new SP();
            }
        }

        private void Mr_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if ((bool)ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data to PC.MR loading enable"))
            {
                ApplicationService.SetView("MessageBoxRegion", "Recipe_Selector","L");
            }
        }

        private void Mr_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((bool)ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data to PC.MR loading enable"))
            {
                ApplicationService.SetView("MessageBoxRegion", "Recipe_Selector", "L");
            }
        }

        private void LoadMR(string a)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    DataTable temp = (new localDBAdapter("SELECT MachineRecipes.MR FROM BarcodeToMR INNER JOIN MachineRecipes ON BarcodeToMR.MR_Id = MachineRecipes.ID  WHERE Barcode ='" + a + "' ; ")).DB_Output();
                    if (temp.Rows.Count == 0)
                    {
                        mr.Value = "";
                        rd.Value = "";
                        new MessageBoxTask("@Datapicker.Text16", "@Datapicker.Text15", MessageBoxIcon.Warning);
                    }
                    else
                    {

                        mr.Value = temp.Rows[0][0].ToString();
                        LoadDescription(temp.Rows[0][0].ToString());
                        user.Value = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();
                    }
                });
            });
        }

        private void LoadDescription(string a)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
                    if (T.IsExistingRecipeFile(a))
                    {
                        rd.Value = T.GetRecipeFile(a).Description;
                    }
                    else
                    {
                        rd.Value = "";
                    }
                });
            });
        }

        private void Data1_Loaded(object sender, RoutedEventArgs e)
        {
            VS = ApplicationService.GetService<IVariableService>();
            newDataV = VS.GetVariable("DataPicker.DatafromScanner");
            newDataV.Change += NewDataV_Change;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_Status_Recipe", mr.Value);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_Remark","R");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBoxView.Show("@StatusView.Text25", "@Tasten.Entfernen", MessageBoxButton.YesNo, MessageBoxResult.No, MessageBoxIcon.Question) == MessageBoxResult.Yes)
            {
                
                string txt = textService.GetText("@StatusView.Text32") + " -> " + textService.GetText("@Logging.Service.Text14");
                this.loggingService.Log("Service", "ChargeDataChanged", txt, FastDateTime.Now);

                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Platz löschen", true);

                Task.Run(() =>
                {
                    Thread.Sleep(250);
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Platz löschen", false);
                });

                ApplicationService.SetView("DialogRegion", "EmptyView");
            }
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_Remark", "W");

            //ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Einschleussen Entnahme", true);

            //Task.Run(() =>
            //{
            //    Thread.Sleep(250);
            //    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Einschleussen Entnahme", false);


            //    ILanguageService textService = ApplicationService.GetService<ILanguageService>();

            //    string txt = textService.GetText("@Logging.Service.Text17");
            //    this.loggingService.Log("Service", "Withdrawal", txt, FastDateTime.Now);
            //});
        }

        private void Data2_WriteValueCompleted(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible && data2.Value.ToString() != "")
            {
                if (ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() != "")
                {
                    user.Value = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();
                    LoadMR(data2.Value.ToString());
                }
                else
                {
                    new MessageBoxTask("@Datenauswahl.Text13", "@Datenauswahl.Text15", MessageBoxIcon.Warning);
                } 
            }
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data to PC.MR loading enable"))
            {
               
                if (data1.Value.ToString().Length < 2)
                {
                    new MessageBoxTask("@Datapicker.Text9", "@Datapicker.Text7", MessageBoxIcon.Exclamation);
                    return;
                }

                if (data2.Value.ToString().Length < 2)
                {
                    new MessageBoxTask("@Datapicker.Text10", "@Datapicker.Text7", MessageBoxIcon.Exclamation);
                    return;
                }

                if (mr.Value.ToString().Length < 2)
                {
                    new MessageBoxTask("@Datapicker.Text11", "@Datapicker.Text7", MessageBoxIcon.Exclamation);
                    return;
                }

                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.MR.Header.Charge#STRING20", data1.Value.ToString());
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.MR.Header.Barcode#STRING20", data2.Value.ToString());
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.MR.Header.Maschinenprogramm#STRING20", mr.Value.ToString());
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.MR.Header.User#STRING20", user.Value.ToString());

                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.MR requested", true);

                ApplicationService.SetView("DialogRegion", "EmptyView");
            }
            else
            {
                new MessageBoxTask("@Datapicker.Text6", "@Datapicker.Text7", MessageBoxIcon.Exclamation);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DialogView.Show("MO_PD_Test", "PD DATA", DialogButton.Close);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("DialogRegion", "EmptyView");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_StatusPD");
        }

        private void Data2_ValueChanged(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                data1.Value = DateTime.Now.ToString("yyyy_MM_dd_HHmm");
            }
           
        }
    }
}