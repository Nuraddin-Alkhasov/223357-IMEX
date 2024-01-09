using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.Fahrwagen
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_Fahrwagen_PN")]
    public partial class H_Fahrwagen_PN : View
    {
		
        public H_Fahrwagen_PN()
        {
            this.InitializeComponent();
            
        }

        private void Pn_H_Fahrwagen_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pn_H_Fahrwagen.SelectedPanoramaRegionIndex == 0)
            {
                pn_H_Fahrwagen.ScrollNext();
            }
            else
            {
                pn_H_Fahrwagen.ScrollPrevious();
            }
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_Fahrwagen.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_Fahrwagen.ScrollPrevious();
            }
        }
    }
}