using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Parameter.Fahrwagen
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("P_Fahrwagen_PN")]
    public partial class P_Fahrwagen_PN : View
    {
		
        public P_Fahrwagen_PN()
        {
            this.InitializeComponent();
            
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_P_Fahrwagen_M.SelectedPanoramaRegionIndex!=0)
            {
                pn_P_Fahrwagen_M.SelectedPanoramaRegionIndex = 1;
                pn_P_Fahrwagen_M.ScrollPrevious();
            }
        }

        private void Pn_H_Fahrwagen_M_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            switch (pn_P_Fahrwagen_M.SelectedPanoramaRegionIndex)
            {
                case 0: header.LocalizableText = "@Parameter.Text43"; break;
                case 1: header.LocalizableText = "@Parameter.Text44"; break;
                case 2: header.LocalizableText = "@Parameter.Text45"; break;
            }
        }
    }
}