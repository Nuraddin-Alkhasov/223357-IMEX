using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.ALO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_ALO2_PN")]
    public partial class H_ALO2_PN : View
    {
		
        public H_ALO2_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_ALO2.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_ALO2.ScrollPrevious();
            }
        }

        private void Pn_H_ALO2_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_ALO2.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text8";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text19";
            }
        }
    }
}