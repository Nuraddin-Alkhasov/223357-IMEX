using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.LGO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_LGO3_PN")]
    public partial class H_LGO3_PN : View
    {
		
        public H_LGO3_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_LGO3.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_LGO3.ScrollPrevious();
            }
        }
        private void Pn_H_LGO3_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_LGO3.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text16";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text27";
            }
        }
    }
}