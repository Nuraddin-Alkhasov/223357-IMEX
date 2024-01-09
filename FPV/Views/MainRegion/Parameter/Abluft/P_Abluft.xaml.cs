using HMI.Views.Parameter.ALO;
using HMI.Views.Parameter.LGO;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace HMI.Views.Parameter.Abluft
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("P_Abluft")]
    public partial class P_Abluft : View
    {
		
        public P_Abluft()
        {
            this.InitializeComponent();
            
        }

        private void btn1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
             Reg.Content = new P_Abluft1();
        }

        private void btn2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_Abluft2();
        }


        private void btn1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    btn1.IsChecked = true;
                });
            });
        }


    }
}