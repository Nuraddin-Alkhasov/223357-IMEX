using HMI.Views.DialogRegion;
using System.Windows;
using VisiWin.ApplicationFramework;


namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_Transporter")]
    public partial class MO_Transporter : VisiWin.Controls.View
    {

        public MO_Transporter()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] m = (((System.Windows.Controls.Button)sender).Tag.ToString()).Split('*');
            new SP(m[0], m[1], m[2]);
        }

        private void PictureBox_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            string[] m = (((VisiWin.Controls.PictureBox)sender).Tag.ToString()).Split('*');
            new SP(m[0], m[1], m[2]);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_MainView");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogView.Show("MO_PD_Test", "PD DATA", DialogButton.Close);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DialogView.Show("H_L_Zuluft1", "@HandMenu.Text58", DialogButton.Close);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DialogView.Show("H_L_Zuluft2", "@HandMenu.Text58", DialogButton.Close);

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DialogView.Show("H_L_Abluft", "@HandMenu.Text48", DialogButton.Close);
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DialogView.Show("H_Fahrwagen_OP", "Oip Pump SetUp", DialogButton.Close);
        }
    }
}



