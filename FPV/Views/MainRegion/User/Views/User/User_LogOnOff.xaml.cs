using VisiWin.ApplicationFramework;

namespace HMI.User
{
	/// <summary>
	/// Interaction logic for UserAdd.xaml
	/// </summary>
    [ExportView("User_LogOnOff")]
	public partial class User_LogOnOff : VisiWin.Controls.View, IView
	{
		public User_LogOnOff()
		{
			this.InitializeComponent();
		}

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {

        }
    }
}