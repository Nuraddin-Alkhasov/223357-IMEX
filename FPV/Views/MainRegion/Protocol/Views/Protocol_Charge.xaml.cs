using HMI.Views.MainRegion.Recipe.Custom_Objects;
using HMI.Views.MessageBoxRegion;
using System;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{
	/// <summary>
	/// Interaction logic for ChargeView.xaml
	/// </summary>
	[ExportView("Protocol_Charge")]
	public partial class Protocol_Charge : VisiWin.Controls.View
	{
		public Protocol_Charge()
		{
			this.InitializeComponent();
		}

        private void TextVarOut_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if(MR.Value!= null && MR.Value.ToString()!="")
                (new AutoRecipeLoad(MR.Value.ToString())).LoadStackAsync();
        }

        private void TextVarOut_PreviewTouchDown_1(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (LGO.Value != null && LGO.Value.ToString() != "")
                (new AutoRecipeLoad()).LoadLGOAsync(LGO.Value.ToString());
        }
        private void TextVarOut_PreviewTouchDown_2(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (ALO.Value != null && ALO.Value.ToString() != "")
                (new AutoRecipeLoad()).LoadALOAsync(ALO.Value.ToString()); 
        }

        private void TextVarOut_PreviewTouchDown_3(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (NIO.Value != null && NIO.Value.ToString() != "")
                (new AutoRecipeLoad()).LoadNIOAsync(NIO.Value.ToString());
        }

        private void Pn_protocol_l_SelectedPanoramaRegionChanged(object sender, VisiWin.Controls.SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_protocol_l.SelectedPanoramaRegionIndex == 0)
            {
                Gb_header.LocalizableHeaderText = "@Protocol.Text12";
            }
            else
            {
                Gb_header.LocalizableHeaderText = "@Protocol.Text22";
            }
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProtocolAdapter temp = (ProtocolAdapter)this.DataContext;
            if (temp.RemarkList != null)
            {
                if (temp.RemarkList.Count > 0)
                {
                    string remark = Environment.NewLine;
                    for (int i = 0; i < temp.RemarkList.Count; i++)
                    {
                        remark += Convert.ToDateTime(temp.RemarkList[i].Time.ToString()).ToString("dd.MM.yyyy HH:mm:ss") + " : " + temp.RemarkList[i].Text + " : "+ temp.RemarkList[i].User + Environment.NewLine;
                    }

                    new MessageBoxTask(remark, "@Protocol.Text23", MessageBoxIcon.Information);
                }
                else 
                {
                    new MessageBoxTask("@Protocol.Text24", "@Protocol.Text23", MessageBoxIcon.Information);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pn_protocol_l.SelectedPanoramaRegionIndex == 0)
            {
                pn_protocol_l.ScrollNext();
                scroll.LocalizableText = "@Protocol.Text36";
            }
            else
            {
                pn_protocol_l.ScrollPrevious();
                scroll.LocalizableText = "@Protocol.Text35";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ProtocolAdapter temp = (ProtocolAdapter)this.DataContext;
            if (temp.RunList != null)
            {
                temp.OType = ((VisiWin.Controls.Button)sender).Tag.ToString();
                ApplicationService.SetView("MessageBoxRegion", "Protocol_StatusPD");
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ProtocolAdapter temp = (ProtocolAdapter)this.DataContext;
            if (temp.RunList != null)
            {
                temp.OType = ((VisiWin.Controls.Button)sender).Tag.ToString();
                ApplicationService.SetView("MessageBoxRegion", "Protocol_StatusPD_nOK");
            }
        }
    }
}