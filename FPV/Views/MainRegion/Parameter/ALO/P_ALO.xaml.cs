using HMI.Views.Parameter.ALO;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace HMI.Parameter.ALO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("P_ALO")]
    public partial class P_ALO : View
    {
		
        public P_ALO()
        {
            this.InitializeComponent();
            
        }

        private void btnALO1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO1_P();
        }

        private void btnALO2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO2_P();
        }

        private void btnALO3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO3_P();
        }

        private void btnALO4_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO4_P();
        }

        private void btnALO5_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO5_P();
        }

        private void btnALO6_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO6_P();
        }

        private void btnALO7_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_ALO7_P();
        }


        private void btnALO1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    btnALO1.IsChecked = true;
                });
            });
        }


    }
}