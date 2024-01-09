using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.Hubtisch
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_HubtischL_PN")]
    public partial class H_HubtischL_PN : View
    {
		
        public H_HubtischL_PN()
        {
            this.InitializeComponent();
            
        }

        private void Pn_H_HubtischL_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pn_H_HubtischL.SelectedPanoramaRegionIndex == 0)
            {
                pn_H_HubtischL.ScrollNext();
            }
            else
            {
                pn_H_HubtischL.ScrollPrevious();
            }
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_HubtischL.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_HubtischL.ScrollPrevious();
            }
        }
    }
}