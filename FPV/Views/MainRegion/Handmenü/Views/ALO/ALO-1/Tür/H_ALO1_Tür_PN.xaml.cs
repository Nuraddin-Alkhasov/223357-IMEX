﻿using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.ALO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_ALO1_Tür_PN")]
    public partial class H_ALO1_Tür_PN : View
    {
		
        public H_ALO1_Tür_PN()
        {
            this.InitializeComponent();
            
        }

        private void Pn_H_ALO1_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pn_H_ALO1_Tür.SelectedPanoramaRegionIndex == 0)
            {
                pn_H_ALO1_Tür.ScrollNext();
            }
            else
            {
                pn_H_ALO1_Tür.ScrollPrevious();
            }
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_ALO1_Tür.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_ALO1_Tür.ScrollPrevious();
            }
        }
    }
}