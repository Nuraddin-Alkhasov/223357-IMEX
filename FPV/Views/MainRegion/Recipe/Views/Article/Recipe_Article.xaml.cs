using HMI.Reporting;
using HMI.Reports.Recipes;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    [ExportView("Recipe_Article")]
    public partial class Recipe_Article : VisiWin.Controls.View
    {
        public int selectedR;

        public Recipe_Article()
        {
            this.InitializeComponent();
        }

        #region - - - Event Handlers - - -

        private void BtnALO_Checked(object sender, RoutedEventArgs e)
        {
            IRegionService iRS = ApplicationService.GetService<IRegionService>();
            Recipe_PN RPN = (Recipe_PN)iRS.GetView("Recipe_PN");

            RPN.HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text1";
            RPN.Rname.VariableName = "Recipe.ALO.Name";
            Item_Pic.SymbolResourceKey = "R_ALO";
            selectedR = 0;

            region_Article.Content = new Recipe_Article_ALO();
        }

        private void BtnLGO_Checked(object sender, RoutedEventArgs e)
        {
            IRegionService iRS = ApplicationService.GetService<IRegionService>();
            Recipe_PN RPN = (Recipe_PN)iRS.GetView("Recipe_PN");

            RPN.HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text2";
            RPN.Rname.VariableName = "Recipe.LGO.Name";
            Item_Pic.SymbolResourceKey = "R_LGO";
            selectedR = 1;

            region_Article.Content = new Recipe_Article_LGO();
        }

        private void BtnNIO_Checked(object sender, RoutedEventArgs e)
        {
            IRegionService iRS = ApplicationService.GetService<IRegionService>();
            Recipe_PN RPN = (Recipe_PN)iRS.GetView("Recipe_PN");

            RPN.HeaderTxt.LocalizableText = "@RecipeSystem.View.Grid.Text3";
            RPN.Rname.VariableName = "Recipe.NIO.Name";
            Item_Pic.SymbolResourceKey = "R_NIO";

            selectedR = 2;

            region_Article.Content = new Recipe_Article_NIO();
        }

        #endregion

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)btnLGO.IsChecked)
            {
                ExecuteReport("LGO"); 
            }

            if ((bool)btnALO.IsChecked)
            {
                ExecuteReport("ALO");
            }
                  
            if ((bool)btnNIO.IsChecked)
            {
                ExecuteReport("NIO"); 
            } 
        }
            
        private void ExecuteReport(string RC)
        {
            var adapter = ApplicationService.GetAdapter(nameof(ReportViewAdapter)) as ReportViewAdapter;
            IRecipeClass irc = ApplicationService.GetService<IRecipeService>().GetRecipeClass(RC);
            string rn = ApplicationService.GetVariableValue("Recipe." + RC + ".Name").ToString();
            if (rn != "")
            {
                if (irc.IsExistingRecipeFile(rn))
                {
                    Func<CancellationToken, Task<ReportConfiguration>> config;
                    switch (RC)
                    {
                        case "LGO": config = (t) => LGOReport.GetReportConfiguration(rn); break;
                        case "ALO": config = (t) => ALOReport.GetReportConfiguration(rn); break;
                        case "NIO": config = (t) => NIOReport.GetReportConfiguration(rn); break;
                        default: config = (t) => LGOReport.GetReportConfiguration(rn); break;
                    }
                    adapter?.OpenView(config);
                }
            }
        }
    }
}