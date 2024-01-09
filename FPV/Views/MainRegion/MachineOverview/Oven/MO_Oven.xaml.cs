using HMI.Views.DialogRegion;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_Oven")]
    public partial class MO_Oven : VisiWin.Controls.View
    {
        string ovenID;
     
        public MO_Oven()
        {
            InitializeComponent();          
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                ovenID = ApplicationService.ObjectStore.GetValue("MO_Oven_KEY").ToString();
                switch (ovenID)
                {
                    case "LGO1":
                        header.LocalizableText = "@Parameter.Text16";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 1);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 1);
                        break;
                    case "LGO2":
                        header.LocalizableText = "@Parameter.Text17";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 1);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 2);
                        break;
                    case "LGO3":
                        header.LocalizableText = "@Parameter.Text18";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 1);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 3);
                        break;
                    case "ALO1":
                        header.LocalizableText = "@Parameter.Text8";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 1);
                        break;
                    case "ALO2":
                        header.LocalizableText = "@Parameter.Text9";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 2);
                        break;
                    case "ALO3":
                        header.LocalizableText = "@Parameter.Text10";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 3);
                        break;
                    case "ALO4":
                        header.LocalizableText = "@Parameter.Text11";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 4);
                        break;
                    case "ALO5":
                        header.LocalizableText = "@Parameter.Text12";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 5);
                        break;
                    case "ALO6":
                        header.LocalizableText = "@Parameter.Text13";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 6);
                        break;
                    case "ALO7":
                        header.LocalizableText = "@Parameter.Text14";
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 2);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 7);
                        break;
                }
                if (ovenID.Contains("ALO"))
                {
                   O.OvenType="ALO";
                   CheckSteps("ALO");
                }
                else
                {
                    O.OvenType = "LGO";
                    CheckSteps("LGO");
                }
            }
            else
            {
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 0);
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 0);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "TrendChartView",
              new TrendData()
              {
                  ArchiveName = ovenID,
                  TrendName_1 = "Istwert",
                  CurveTag_1 = "@TrendSystem.Text2",
                  TrendName_2 = "Sollwert",
                  CurveTag_2 = "@TrendSystem.Text3",
                  Header = GetHeader(ovenID),
                  Min = 0,
                  Max = ovenID.Contains("LGO") ? 550 : 420,
                  BackViewName = "MO_Oven"
              }
              );
        }
        private string GetHeader(string _oId) 
        {
            string ret_val = "";
            switch (_oId) 
            {
                case "LGO1": ret_val = "@TrendSystem.Text6"; break;
                case "LGO2": ret_val = "@TrendSystem.Text7"; break;
                case "LGO3": ret_val = "@TrendSystem.Text8"; break;
                case "ALO1": ret_val = "@TrendSystem.Text9"; break;
                case "ALO2": ret_val = "@TrendSystem.Text10"; break;
                case "ALO3": ret_val = "@TrendSystem.Text11"; break;
                case "ALO4": ret_val = "@TrendSystem.Text12"; break;
                case "ALO5": ret_val = "@TrendSystem.Text13"; break;
                case "ALO6": ret_val = "@TrendSystem.Text14"; break;
                case "ALO7": ret_val = "@TrendSystem.Text15"; break;
                default:break;
            }
            return ret_val;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_MainView");
        }

        private void Mr_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
                    if (T.IsExistingRecipeFile(mr.Value.ToString()))
                    {
                        descr.Value = T.GetRecipeFile(mr.Value.ToString()).Description;
                    }
                    else
                    {
                        descr.Value = "";
                    }
                });
            });
        }

        private void Button_Click_AutoOnOff(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("DialogRegion", "MO_AutoONOFF");
        }

        private void CheckSteps(string _O)
        {
            tStack.Items.Clear();
            Task obTask = Task.Run(() =>
            {
                Thread.Sleep(500);
                switch (_O)
                {
                    case "ALO":
                        if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.ALO.Heizen_1.Temperatur").ToString()) > 0)
                        {
                            if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.ALO.Heizen_2.Temperatur").ToString()) > 0)
                            {
                                if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.ALO.Heizen_3.Temperatur").ToString()) > 0)
                                {
                                    if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.ALO.Heizen_4.Temperatur").ToString()) > 0)
                                    {
                                        Generate_Steps("ALO", 4);
                                    }
                                    else
                                    {
                                        Generate_Steps("ALO", 3);
                                    }
                                }
                                else
                                {
                                    Generate_Steps("ALO", 2);
                                }
                            }
                            else
                            {
                                Generate_Steps("ALO", 1);
                            }
                        }
                        else
                        {
                            Generate_Steps("ALO", 0);
                        }
                        break;
                    case "LGO":
                        if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.LGO.Heizen_1.Temperatur").ToString()) > 0)
                        {
                            if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.LGO.Heizen_2.Temperatur").ToString()) > 0)
                            {
                                if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.LGO.Heizen_3.Temperatur").ToString()) > 0)
                                {
                                    if (int.Parse(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte.LGO.Heizen_4.Temperatur").ToString()) > 0)
                                    {
                                        Generate_Steps("LGO", 4);
                                    }
                                    else
                                    {
                                        Generate_Steps("LGO", 3);
                                    }
                                }
                                else
                                {
                                    Generate_Steps("LGO", 2);
                                }
                            }
                            else
                            {
                                Generate_Steps("LGO", 1);
                            }
                        }
                        else
                        {
                            Generate_Steps("ALO", 0);
                        }
                        break;
                    }
            });
        }

        private void Generate_Steps(string _O, int _OS)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                   
                    for (int i = 0; i < _OS; i++)
                    {
                        tStack.Items.Add(new Region());
                        tStack.Items[i] = new MO_Oven_Step(_O, (i+1).ToString());
                    }
                });
            });
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_Status_Recipe", mr.Value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_StatusPD");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DialogView.Show("MO_O_Umluft", "@MainOverview.Text46", DialogButton.Close);
        }
    }
}



