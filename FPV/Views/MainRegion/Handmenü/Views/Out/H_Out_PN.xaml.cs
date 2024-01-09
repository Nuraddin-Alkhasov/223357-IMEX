using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.Out
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_Out_PN")]
    public partial class H_Out_PN : View
    {
		
        public H_Out_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_Out.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_Out.ScrollPrevious();
            }
        }

        private void Pn_H_Out_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            if (pn_H_Out.SelectedPanoramaRegionIndex == 1)
            {
                header.LocalizableText = "@HandMenu.Text36";
            }
            else
            {
                header.LocalizableText = "@HandMenu.Text37";
            }
        }
    }
}