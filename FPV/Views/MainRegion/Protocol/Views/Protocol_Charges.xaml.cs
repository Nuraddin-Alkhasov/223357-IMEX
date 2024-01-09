using HMI.Views.DialogRegion;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{
	/// <summary>
	/// Interaction logic for Protocol.xaml
	/// </summary>
	[ExportView("Protocol_Charges")]
	public partial class Protocol_Charges : VisiWin.Controls.View
	{
        
		public Protocol_Charges()
		{
			this.InitializeComponent();
		}

		private void LayoutRoot_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
            if (this.IsVisible)
            {
                //List<object> paramList = new List<object>();
                //paramList.Add(oldIndex);
                //paramList.Add(dgv_charges);
              
                //((ProtocolAdapter)this.DataContext).BW_FilterSQL(paramList);

            }
		}

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            if (DialogView.Show("Protocol_Filter", "@Protocol.Text5", DialogButton.OKCancel, DialogResult.Cancel) == DialogResult.OK)
            {
                ProtocolAdapter temp = (ProtocolAdapter)this.DataContext;
                temp.BW_FilterSQL(null);
            }
        }

        private void Dgv_charges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //oldIndex = dgv_charges.SelectedIndex;
        }
        private void Dgr_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv_charges.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
        }
    }
}