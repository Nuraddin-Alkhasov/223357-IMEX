using HMI.Views.Parameter.ALO;
using HMI.Views.Parameter.LGO;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace HMI.Parameter.LGO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("P_LGO")]
    public partial class P_LGO : View
    {
		
        public P_LGO()
        {
            this.InitializeComponent();
            
        }

        private void btnLGO1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_LGO1_P();
        }

        private void btnLGO2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_LGO2_P();
        }

        private void btnLGO3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Reg.Content = new P_LGO3_P();
        }

        private void btnLGO1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    btnLGO1.IsChecked = true;
                });
            });
        }


    }
}