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
    [ExportView("P_HubtischL")]
    public partial class P_HubtischL : View
    {
		
        public P_HubtischL()
        {
            this.InitializeComponent();
            
        }

        private void btnH_A_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_HubtischL_A();
        }

        private void btnH_P_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_HubtischL_P();
        }

        private void btnH_B_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_HubtischL_B();
        }


        private void btnH_A_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    btnH_A.IsChecked = true;
                });
            });
        }
    }
}