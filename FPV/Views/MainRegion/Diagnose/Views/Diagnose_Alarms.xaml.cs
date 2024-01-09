using HMI.Views.DialogRegion;
using HMI.Views.MessageBoxRegion;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisiWin.ApplicationFramework;
using VisiWin.UserManagement;

namespace HMI.Diagnose
{
	/// <summary>
	/// Interaction logic for View1.xaml
	/// </summary>
	[ExportView("Diagnose_Alarms")]
	public partial class Diagnose_Alarms : VisiWin.Controls.View
	{

        private readonly Stopwatch _doubleTapStopwatch = new Stopwatch();
        private Point _lastTapLocation;

        public Diagnose_Alarms()
		{
			this.InitializeComponent();
		}

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;

        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;

        }


        private bool IsDoubleTap(TouchEventArgs e)
        {
            Point currentTapPosition = e.GetTouchPoint(this).Position;
            bool tapsAreCloseInDistance = currentTapPosition.X - _lastTapLocation.X < 40;
            _lastTapLocation = currentTapPosition;

            TimeSpan elapsed = _doubleTapStopwatch.Elapsed;
            _doubleTapStopwatch.Restart();
            bool tapsAreCloseInTime = (elapsed != TimeSpan.Zero && elapsed < TimeSpan.FromSeconds(0.2));

            return tapsAreCloseInDistance && tapsAreCloseInTime;
        }

        private void OnPreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (IsDoubleTap(e))
            {
                IUserManagementService userService = ApplicationService.GetService<IUserManagementService>();
                if (userService.CurrentUser != null && userService.CurrentUser.RightNames.Contains("Alarms"))
                {
                    Task taskA = new Task(async () => 
                    {
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren", true);
                        await Task.Delay(500);
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren", false);
                    });
                    taskA.Start();
                }
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IUserManagementService userService = ApplicationService.GetService<IUserManagementService>();
            if (userService.CurrentUser!=null && userService.CurrentUser.RightNames.Contains("Alarms"))
            {
                Task taskA = new Task(async () =>
                {
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren", true);
                    await Task.Delay(250);
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren", false);
                });
                taskA.Start();
            }
        }

        private void Button_Click_TO(object sender, RoutedEventArgs e)
        {
            if (MessageBoxView.Show("@HistoricalAlarms.Text9", "@HistoricalAlarms.Text5", MessageBoxButton.YesNo, MessageBoxResult.No, MessageBoxIcon.Question) == MessageBoxResult.Yes)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.WindowStyle = ProcessWindowStyle.Hidden;
                proc.FileName = "cmd";
                proc.Arguments = "/C shutdown -f -r -t 0";
                Process.Start(proc);
            }
            
        }

        private void Button_Click_JS(object sender, RoutedEventArgs e)
        {
            DialogView.Show("JumoStatus", "@Jumo.Text15", DialogButton.Close);
        }

        private void Button_Click_BC(object sender, RoutedEventArgs e)
        {
            DialogView.Show("Barcode", "Barcode", DialogButton.Close, DialogResult.OK);
        }

        private void Button_Click_JC(object sender, RoutedEventArgs e)
        {
            DialogView.Show("ServiceChecker", "Jumo Checker", DialogButton.Close, DialogResult.OK);
        }


        private void Button_Click_B(object sender, RoutedEventArgs e)
        {
            DialogView.Show("Backup", "Backup", DialogButton.Close, DialogResult.OK);
        }

        private void Button_Click_EKS(object sender, RoutedEventArgs e)
        {
            DialogView.Show("EKS", "EKS System", DialogButton.Close, DialogResult.OK);
        }

        private void Button_Click_CS(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("TouchpadRegion", "WaitScreen", 2);
            
        }
    }
}