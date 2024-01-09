using HMI.UserControls;
using HMI.Views.DialogRegion;
using HMI.Views.MessageBoxRegion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.DataAccess;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_MaschineView")]
    public partial class MO_MaschineView : VisiWin.Controls.View
    {
        public MO_MaschineView()
        {
            InitializeComponent();
        }

        double Oldpos = 0;
        private void TStatus_Change(object sender, VariableEventArgs e)
        {
            double pos = Math.Round(((int)e.Value) / 3.2615);

            if (Oldpos != pos)
            {
                FW.Margin = new Thickness(0, 375, pos + 50, 0);
                Rack1.Margin = new Thickness(0, 400, pos + 236, 0);
                Rack2.Margin = new Thickness(0, 400, pos + 89, 0);
                Oldpos = pos;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_Transporter");
        }

        private void LoadALO7()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(3000);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO7.Content = new MV_O()
                    {

                        OvenType = "ALO",
                        OvenId = "ALO7",
                        Base = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.19 ALO 7.Türe.DB ALO 7 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text37",
                        OvenOrder = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadALO6()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(2700);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO6.Content = new MV_O()
                    {
                        OvenType = "ALO",
                        OvenId = "ALO6",
                        Base = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.18 ALO 6.Türe.DB ALO 6 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text36",
                        OvenOrder = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"

                    };
                });
            });
        }
        private void LoadALO5()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(2400);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO5.Content = new MV_O()
                    {

                        OvenType = "ALO", 
                        OvenId = "ALO5",
                        Base = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.17 ALO 5.Türe.DB ALO 5 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text35",
                        OvenOrder = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadLGO3()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(2100);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    LGO3.Content = new MV_O()
                    {
                        OvenType = "LGO",
                        OvenId = "LGO3",
                        Base = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.12 LGO 3.Türe.DB LGO 3 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text29",
                        OvenOrder = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"

                    };
                });
            });
        }
        private void LoadALO4()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(1800);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO4.Content = new MV_O()
                    {
                        OvenType = "ALO",
                        OvenId = "ALO4",
                        Base = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.16 ALO 4.Türe.DB ALO 4 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text34",
                        OvenOrder = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadALO3()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(1500);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO3.Content = new MV_O()
                    {
                        OvenType = "ALO",
                        OvenId = "ALO3",
                        Base = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.15 ALO 3.Türe.DB ALO 3 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text32",
                        OvenOrder = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadLGO2()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(1200);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    LGO2.Content = new MV_O()
                    {

                        OvenType = "LGO",
                        OvenId = "LGO2",
                        Base = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.11 LGO 2.Türe.DB LGO 2 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text24",
                        OvenOrder = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadALO2()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(900);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO2.Content = new MV_O()
                    {
                        OvenType = "ALO",
                        OvenId = "ALO2",
                        Base = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.14 ALO 2.Türe.DB ALO 2 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text31",
                        OvenOrder = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"

                    };
                });
            });
        }
        private void LoadALO1()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(600);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ALO1.Content = new MV_O()
                    {

                        OvenType = "ALO",
                        OvenId = "ALO1"  ,
                        Base = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.13 ALO 1.Türe.DB ALO 1 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text30",
                        OvenOrder = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"
                    };
                });
            });
        }
        private void LoadLGO1()
        {
            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(300);
                await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    LGO1.Content = new MV_O()
                    {
                        OvenType = "LGO",
                        OvenId = "LGO1",
                        Base = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Status_Platz.belegt",
                        Rack1Visibility = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Status_Platz.Rack1_vorhanden",
                        Rack2Visibility = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Status_Platz.Rack2_vorhanden",
                        RackStatus = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Allgemein HMI.Actual value.Rack Status Textliste",
                        DoorPosition = "IMEX.PLC.Blocks.10 LGO 1.Türe.DB LGO 1 Türe HMI.Actual value.Türe.Aktuelle Position in mm",
                        OvenStatus = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.Actual value.an HMI control.Ofen Ein / Aus",
                        NachlaufStatus = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Status_Bearbeitung.Nacharbeit_laeuft",
                        OvenName = "@MainOverview.Text23",
                        OvenOrder = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Maschinenprogramm#STRING20",
                        OvenSetTemp = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti Status.Temperatur.Sollwert für Prozess",
                        OvenSetH = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Gesamtzeit.Std",
                        OvenSetM = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Gesamtzeit.Min",
                        OvenActualTemp = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Istwerte.Temperatur",
                        OvenActualH = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Restzeit.Std",
                        OvenActualM = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Restzeit.Min",
                        OvenSimulation = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Status an BP.Simulation Ein"

                    };
                });
            });
        }

        private void LayoutRoot_Initialized(object sender, EventArgs e)
        {
            LoadALO7();
            LoadALO6();
            LoadALO5();
            LoadLGO3();
            LoadALO4();
            LoadALO3();
            LoadLGO2();
            LoadALO2();
            LoadALO1();
            LoadLGO1();
        }
    }
}



