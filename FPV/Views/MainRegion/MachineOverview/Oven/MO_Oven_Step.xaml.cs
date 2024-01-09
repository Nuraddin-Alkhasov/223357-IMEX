using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Language;

namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_Oven_Step")]
	public partial class MO_Oven_Step : VisiWin.Controls.View
	{
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();
        string OS;
        IVariableService VS;
        IVariable DataToPC;

        public MO_Oven_Step(string _O, string _OS)
		{
			this.InitializeComponent();

            switch (_OS)
            {
                case "1": header.LocalizableLabelText = "@MainOverview.Text3"; break;
                case "2": header.LocalizableLabelText = "@MainOverview.Text5"; break;
                case "3": header.LocalizableLabelText = "@MainOverview.Text6"; break;
                case "4": header.LocalizableLabelText = "@MainOverview.Text7"; break;
            }

            OS = _OS;

            T_soll.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte." + _O + ".Heizen_" + _OS + ".Temperatur";
            A_soll_S.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte." + _O + ".Heizen_" + _OS + ".Aufheizzeit.Std";
            A_soll_M.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte." + _O + ".Heizen_" + _OS + ".Aufheizzeit.Min";
            H_soll_S.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte." + _O + ".Heizen_" + _OS + ".Haltezeit.Std";
            H_soll_M.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Sollwerte." + _O + ".Heizen_" + _OS + ".Haltezeit.Min";

            T_ist.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Istwerte.Temperatur";
            A_ist_S.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Istwerte.Heizen_" + _OS + ".Aufheizzeit.Std";
            A_ist_M.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Istwerte.Heizen_" + _OS + ".Aufheizzeit.Min";
            H_ist_S.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Istwerte.Heizen_" + _OS + ".Haltezeit.Std";
            H_ist_M.VariableName = "IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Prozessdaten.Istwerte.Heizen_" + _OS + ".Haltezeit.Min";


            VS = ApplicationService.GetService<IVariableService>();
            DataToPC = VS.GetVariable("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Heizen Status");
            DataToPC.Change += DataToPC_Change;
        }

        private void DataToPC_Change(object sender, VariableEventArgs e)
        {
            if (e.Value.ToString() == OS)
            {
                header.IsBlinkEnabled = true;
            }
            else
            {
                header.IsBlinkEnabled = false;
            }
        }
    }
}