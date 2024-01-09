using VisiWin.ApplicationFramework;
using VisiWin.Controls;
namespace HMI.Handmenu.Hubtisch
{
    /// <summary>
    /// Interaction logic for DigitalIOView.xaml
    /// </summary>
    [ExportView("H_Hubtisch_PN")]
    public partial class H_Hubtisch_PN : View
    {
		
        public H_Hubtisch_PN()
        {
            this.InitializeComponent();
            
        }

        private void Pn_H_Hubtisch_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pn_H_Hubtisch.SelectedPanoramaRegionIndex == 0)
            {
                pn_H_Hubtisch.ScrollNext();
            }
            else
            {
                pn_H_Hubtisch.ScrollPrevious();
            }
        }

        private void View_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && pn_H_Hubtisch.SelectedPanoramaRegionIndex!=0)
            {
                pn_H_Hubtisch.ScrollPrevious();
            }
        }
    }
}