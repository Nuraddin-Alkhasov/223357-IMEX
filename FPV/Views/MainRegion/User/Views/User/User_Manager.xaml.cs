using HMI.Views.DialogRegion;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisiWin.ApplicationFramework;

namespace HMI.User
{
	/// <summary>
	/// Interaction logic for UserManager.xaml
	/// </summary>
	[ExportView("User_Manager")]
	public partial class User_Manager : VisiWin.Controls.View
	{
		public User_Manager()
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
        private void _PreviewTouchDown(object sender, TouchEventArgs e)
        {
            dgv_users.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
        }

    }
}