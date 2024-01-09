using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.Fahrwagen
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_Fahrwagen_M_PN")]
    public partial class H_Fahrwagen_M_PN : View
    {
		
        public H_Fahrwagen_M_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_Fahrwagen_M.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_Fahrwagen_M.SelectedPanoramaRegionIndex = 1;
                pn_H_Fahrwagen_M.ScrollPrevious();
            }
        }

        private void Pn_H_Fahrwagen_M_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            switch (pn_H_Fahrwagen_M.SelectedPanoramaRegionIndex)
            {
                case 0: header.LocalizableText = "@HandMenu.Text40"; break;
                case 1: header.LocalizableText = "@HandMenu.Text43"; break;
                case 2: header.LocalizableText = "@HandMenu.Text42"; break;
                case 3: header.LocalizableText = "@HandMenu.Text47"; break;
            }
        }
    }
}