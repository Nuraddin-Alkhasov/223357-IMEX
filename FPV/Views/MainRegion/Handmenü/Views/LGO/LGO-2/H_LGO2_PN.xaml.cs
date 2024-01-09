using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.LGO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_LGO2_PN")]
    public partial class H_LGO2_PN : View
    {
		
        public H_LGO2_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_LGO2.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_LGO2.ScrollPrevious();
            }
        }

        private void Pn_H_LGO2_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_LGO2.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text15";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text26";
            }
        }
    }
}