using HMI.Views.MainRegion.Recipe.Custom_Objects;
using HMI.Views.MessageBoxRegion;
using System.Windows.Controls;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;


namespace HMI.Views.MainRegion.Recipe
{

	[ExportView("Recipe_Binding")]
	public partial class Recipe_Binding : VisiWin.Controls.View
	{

        IVariableService VS;
        IVariable newDataV;

        public Recipe_Binding()
		{
			this.InitializeComponent();
         
        }

        private void NewDataV_Change(object sender, VariableEventArgs e)
        {
            if (this.IsVisible && newDataV.Value.ToString() != "")
            {
                if (barcode.IsVisible)
                {
                    barcode.Value = e.Value.ToString(); 
                }
                else
                {
                    dgv_bctor.SelectedIndex = GetItem(e.Value.ToString());
                }
                ApplicationService.SetVariableValue("DataPicker.DatafromScanner", "");
            }
           
        } 

        private int GetItem(string a)
        {
            for (int i=0;i< dgv_bctor.Items.Count; i++)
            {
                if (((BCToMR)dgv_bctor.Items[i]).Barcode == a)
                {
                    return i;
                }
            }
            return 0;
        }

        private void TextVarOut_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "Recipe_Selector","B");
        }

        private void Barcode_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VS = ApplicationService.GetService<IVariableService>();
            newDataV = VS.GetVariable("DataPicker.DatafromScanner");
            newDataV.Change += NewDataV_Change;
        }
        private void _PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv_bctor.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
        }
    }
}