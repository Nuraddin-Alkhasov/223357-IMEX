using HMI.Views.DialogRegion;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace HMI.Handmenu.Fahrwagen
{
    /// <summary>
    /// Interaction logic for ButtonsView.xaml
    /// </summary>
    [ExportView("H_Luftabschreckung")]
 
    public partial class H_Luftabschreckung : View
    {
        public H_Luftabschreckung()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("H_L_Abluft", "@HandMenu.Text48", DialogButton.Close);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("H_L_Zuluft1", "@HandMenu.Text58", DialogButton.Close);
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogView.Show("H_L_Zuluft2", "@HandMenu.Text59", DialogButton.Close);
        }
    }
}