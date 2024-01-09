using HMI.Views.DialogRegion;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.MachineOverview
{
    class SP
    {

        public SP()
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", 0);
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", 0);
        }

        public SP(string _Module, string _Station)
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", _Module);
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", _Station);
        }

        public SP(string _Text, string _Module, string _Station)
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Plant Module Number", _Module);
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data From Panel.Station Number", _Station);

            DialogView.Show("MO_Status", "@StatusView." + _Text, DialogButton.Close, DialogResult.Cancel);
        }

    }
}
