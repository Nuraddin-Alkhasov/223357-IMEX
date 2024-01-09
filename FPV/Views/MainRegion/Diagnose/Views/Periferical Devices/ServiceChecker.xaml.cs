using System;
using System.Windows.Forms;
using HMI.Interfaces;
using HMI.Module;
using HMI.Services;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.User
{
	/// <summary>
	/// Interaction logic for UserAdd.xaml
	/// </summary>
    [ExportView("ServiceChecker")]
	public partial class ServiceChecker : VisiWin.Controls.View, IView
	{
      

        public ServiceChecker()
		{
			this.InitializeComponent();
        }

   
        private void CheckNow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new CheckJumo();
        }

        private void Browse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path.Text = folderBrowserDialog1.SelectedPath;
                bool b = (new localDBAdapter("UPDATE Config SET Value = '" + folderBrowserDialog1.SelectedPath + "' WHERE Variable = 'JumoPath' ;").DB_Input());
            }
        }

        private void Start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           // JumoCheckService.Start();
            start.IsEnabled = false;
            stop.IsEnabled = true;
        }

        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //JumoCheckService.Stop();
            start.IsEnabled = true;
            stop.IsEnabled = false;
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            //if (this.IsVisible)
            //{
            //    tbTime.Value = new DateTime(2000,01,01, JumoCheckService.Time.Hours, JumoCheckService.Time.Minutes, JumoCheckService.Time.Seconds);
            //    path.Text = (new localDBAdapter("SELECT Value FROM Config WHERE Variable = 'JumoPath';")).DB_Output().Rows[0][0].ToString();
            //    if (JumoCheckService.isRunning)
            //    {
            //        start.IsEnabled = false;
            //        stop.IsEnabled = true;
            //    }
            //    else
            //    {
            //        start.IsEnabled = true;
            //        stop.IsEnabled = false;
            //    }
            //}
        }

        private void TbTime_ValueChanged(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                Stop_Click(this, null);
                bool b = (new localDBAdapter("UPDATE Config SET Value = '" + GetDataTimeToFormat() + "' WHERE Variable = 'JumoCheck' ;").DB_Input());

                //JumoCheckService.Time = new TimeSpan(tbTime.Value.Hour, tbTime.Value.Minute, tbTime.Value.Second);
                //Start_Click(this, null);
            }
            
        }
        private string GetDataTimeToFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + " " + tbTime.Value.ToLongTimeString();
        }
    }
}