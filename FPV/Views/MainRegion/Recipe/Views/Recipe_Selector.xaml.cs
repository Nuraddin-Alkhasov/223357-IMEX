using HMI.Views.DialogRegion;
using HMI.Views.MainRegion.MachineOverview;
using System.Windows;
using System.Windows.Controls;
using VisiWin.ApplicationFramework;


namespace HMI.Views.MainRegion.Recipe
{

    [ExportView("Recipe_Selector")]
    public partial class Recipe_Selector : VisiWin.Controls.View
    {

        public Recipe_Selector()
        {
            this.InitializeComponent();
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            IRegionService iRS = ApplicationService.GetService<IRegionService>();

            string WindowId = ApplicationService.ObjectStore["Recipe_Selector_KEY"].ToString();
            ApplicationService.ObjectStore.Remove("Recipe_Selector_KEY");

            switch (WindowId)
            {
                case "MO_DataOverwrite" :
                    //MO_DO_Data MODOData = (MO_DO_Data)iRS.GetView("MO_DO_Data");
                    //MODOData.mr.Value = tbName.Value.ToString();
                    //MODOData.rd.Value = tbDescription.Value.ToString();
                    //MODOData.user.Value = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();
                    break;
                case "L":
                    MO_StatusIn MODataPicker = (MO_StatusIn)iRS.GetView("MO_StatusIn");
                    MODataPicker.mr.Value = tbName.Value.ToString();
                    MODataPicker.rd.Value = tbDescription.Value.ToString();
                    MODataPicker.user.Value = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();
                    break;
                case "B":
                    Recipe_Binding RecipeBinding = (Recipe_Binding)iRS.GetView("Recipe_Binding");
                    RecipeBinding.mr.Value = tbName.Value.ToString();
                    break;

            }

            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.DataContext = new RecipeSelectorAdapter();
            }
        }

        private void DataGridRow_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            RSdgv_recipe.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}