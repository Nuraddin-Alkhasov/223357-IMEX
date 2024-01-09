using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.In
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_In_PN")]
    public partial class H_In_PN : View
    {
		
        public H_In_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_In.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_In.ScrollPrevious();
            }
        }

        private void Pn_H_In_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_In.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text33";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text34";
            }
        }
    }
}