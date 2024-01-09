using HMI.Views.DialogRegion;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace HMI.Views.MainRegion.Betriebstunden
{
    /// <summary>
    /// Interaction logic for ButtonsView.xaml
    /// </summary>
    [ExportView("Betriebstunden_DB")]
   
    public partial class Betriebstunden_DB : View
    {
       
        public string ovenID;
        public Betriebstunden_DB()
        {
            this.InitializeComponent();
        }

        private void NavigationButton_O(object sender, System.Windows.RoutedEventArgs e)
        {
            ovenID = ((NavigationButton)e.OriginalSource).Tag.ToString();
            switch (ovenID)
            {
                case "LGO1": DialogView.Show("BS_Oven_LGO1", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "LGO2": DialogView.Show("BS_Oven_LGO2", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "LGO3": DialogView.Show("BS_Oven_LGO3", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO1": DialogView.Show("BS_Oven_ALO1", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO2": DialogView.Show("BS_Oven_ALO2", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO3": DialogView.Show("BS_Oven_ALO3", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO4": DialogView.Show("BS_Oven_ALO4", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO5": DialogView.Show("BS_Oven_ALO5", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO6": DialogView.Show("BS_Oven_ALO6", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
                case "ALO7": DialogView.Show("BS_Oven_ALO7", "Betriebstunden", DialogButton.Close, DialogResult.Cancel); break;
            }
           
        }

        private void NavigationButton_C(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("BS_Fahrwagen", "Betriebstunden", DialogButton.Close, DialogResult.Cancel);
        }

        private void NavigationButton_A(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("BS_Ventilator", "Betriebstunden", DialogButton.Close, DialogResult.Cancel);
        }
        private void NavigationButton_In(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("BS_In", "Betriebstunden", DialogButton.Close, DialogResult.Cancel);
        }
        private void NavigationButton_Out(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("BS_Out", "Betriebstunden", DialogButton.Close, DialogResult.Cancel);
        }
    }
}