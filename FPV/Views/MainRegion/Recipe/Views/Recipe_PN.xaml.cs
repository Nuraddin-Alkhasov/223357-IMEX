using HMI.Views.MainRegion.Recipe.Custom_Objects;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe
{

    [ExportView("Recipe_PN")]
    public partial class Recipe_PN : VisiWin.Controls.View
    {
        public bool externLoad = false;
        public int rId = 0;
        Recipe_Machine RM;
        Recipe_Article RA;

        public Recipe_PN()
        {
            IRegionService iRS = ApplicationService.GetService<IRegionService>();
            RM = (Recipe_Machine)iRS.GetView("Recipe_Machine");
            RA = (Recipe_Article)iRS.GetView("Recipe_Article");
            this.InitializeComponent();
        }

        private void Pn_recipe_SelectedPanoramaRegionChanged(object sender, SelectedPanoramaRegionChangedEventArgs e)
        {
            switch (pn_recipe.SelectedPanoramaRegionIndex)
            {
                case 0:
                    HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text5";
                    Rname.VariableName = "Recipe.MR.Name";
                    RM.dgvUpdate();
                    if (RA.btnLGO.IsChecked == true)
                    {
                        RM.LGO.IsChecked = true;
                        RM.LGOList.IsChecked = true;
                    }
                    if (RA.btnALO.IsChecked == true)
                    {
                        RM.ALO.IsChecked = true;
                        RM.ALOList.IsChecked = true;
                    }
                    if (RA.btnNIO.IsChecked == true)
                    {
                        RM.NIO.IsChecked = true;
                        RM.NIO.IsChecked = true;
                    }
                    break;
                case 1:
                    if (externLoad)
                    {
                        switch (rId)
                        {
                            case 1: RM.LGOList.IsChecked = true; break;
                            case 2: RM.ALOList.IsChecked = true; break;
                            case 3: RM.NIOList.IsChecked = true; break;
                        }
                        rId = 0;
                    }
                    
                    if (RM.LGOList.IsChecked == true)
                    {
                        HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text2";
                        Rname.VariableName = "Recipe.LGO.Name";
                        if (!externLoad)
                            (new AutoRecipeLoad()).LoadLGOTask(RM.LGO_name.Value.ToString());
                        RA.btnLGO.IsChecked = true;
                    }

                    if (RM.ALOList.IsChecked == true)
                    {
                        HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text1";
                        Rname.VariableName = "Recipe.ALO.Name";
                        if (!externLoad)
                            (new AutoRecipeLoad()).LoadALOTask(RM.ALO_name.Value.ToString());
                        RA.btnALO.IsChecked = true;
                    }

                    if (RM.NIOList.IsChecked == true)
                    {
                        HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text3";
                        Rname.VariableName = "Recipe.NIO.Name";
                        if (!externLoad)
                            (new AutoRecipeLoad()).LoadNIOTask(RM.NIO_name.Value.ToString());
                        RA.btnNIO.IsChecked = true;
                    }
                    externLoad = false;
                    break;
            }

        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (pn_recipe.SelectedPanoramaRegionIndex)
            {
                case 0:
                    ApplicationService.SetView("DialogRegion", "Recipe_Browser", 0);
                    break;
                case 1:
                    IRegionService iRS = ApplicationService.GetService<IRegionService>();

                    Recipe_Article RA = (Recipe_Article)iRS.GetView("Recipe_Article");
                    switch (RA.selectedR)
                    {
                        case 0: ApplicationService.SetView("DialogRegion", "Recipe_Browser", 1); break;
                        case 1: ApplicationService.SetView("DialogRegion", "Recipe_Browser", 2); break;
                        case 2: ApplicationService.SetView("DialogRegion", "Recipe_Browser", 3); break;
                    }
                    break;
            }
        }

        private void Rname_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            switch (Rname.VariableName)
            {
                case "Recipe.MR.Name": SetDescriptionToRecipe(Rname.Value.ToString(), "MR"); break;
                case "Recipe.LGO.Name": SetDescriptionToRecipe(Rname.Value.ToString(), "LGO"); break;
                case "Recipe.ALO.Name": SetDescriptionToRecipe(Rname.Value.ToString(), "ALO"); break;
                case "Recipe.NIO.Name": SetDescriptionToRecipe(Rname.Value.ToString(), "NIO"); break;
            }
        }

        private void SetDescriptionToRecipe(string _a, string _b)
        {
            if (_a != "")
            {
                Task obTask = Task.Run(() =>
                {
                    Application.Current.Dispatcher.InvokeAsync((Action)delegate
                    {
                        IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass(_b);
                        Descr.Value = T.GetRecipeFile(_a).Description;
                    });
                });
            }
            else
            {
                Descr.Value = "";
            }
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            if (pn_recipe.SelectedPanoramaRegionIndex == 0 && rId != 0)
            {
                pn_recipe.ScrollNext(true);
            }
            else
            {
                switch (rId)
                {
                    case 1: RA.btnLGO.IsChecked = true; break;
                    case 2: RA.btnALO.IsChecked = true; break;
                    case 3: RA.btnNIO.IsChecked = true; break;
                }
                rId = 0;
            }
        }
    }
}