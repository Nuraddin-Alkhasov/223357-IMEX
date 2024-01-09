using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Parameter.Fahrwagen
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("P_Fahrwagen")]
    public partial class P_Fahrwagen : View
    {
		
        public P_Fahrwagen()
        {
            this.InitializeComponent();
            
        }

        private void BtnIn_Loaded(object sender, RoutedEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    btnIn.IsChecked = true;
                });
            });
        }

        private void BtnIn_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_In_P();
        }

        private void BtnLGO1_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_LGO1_P();
        }

        private void BtnALO1_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO1_P();
        }

        private void BtnALO2_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO2_P();
        }

        private void BtnLGO2_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_LGO2_P();
        }

        private void BtnALO3_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO3_P();
        }

        private void BtnALO4_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO4_P();
        }

        private void BtnLGO3_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_LGO3_P();
        }

        private void BtnALO5_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO5_P();
        }

        private void BtnALO6_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO6_P();
        }

        private void BtnALO7_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_ALO7_P();
        }

        private void BtnOut_Checked(object sender, RoutedEventArgs e)
        {
            Reg.Content = new P_Fahrwagen_Out_P();
        }
    }
}