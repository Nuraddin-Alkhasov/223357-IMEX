using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.ALO
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_ALO1_PN")]
    public partial class H_ALO1_PN : View
    {
		
        public H_ALO1_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_ALO1.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_ALO1.ScrollPrevious();
            }
        }

        private void Pn_H_ALO1_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_ALO1.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text7";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text18";
            }
        }
    }
}