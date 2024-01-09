using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.MachineOverview
{

	[ExportView("MO_AutoONOFF")]
	public partial class MO_AutoONOFF : VisiWin.Controls.View
	{

        public MO_AutoONOFF()
		{
			this.InitializeComponent();
          
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetView("DialogRegion", "EmptyView");
        }

        private void LeftButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Zeitschaltuhr.isOn",true);
        }

        private void RightButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB PC Status Platz / HMI.Data To Panel.Zeitschaltuhr.isOn", false);
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsVisible)
            {
               
                Task obTask = Task.Run(() =>
                {
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Zeitschaltuhr speichern", true);
                    Thread.Sleep(300);
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Zeitschaltuhr speichern", false);
                });
            }
        }
    }
}