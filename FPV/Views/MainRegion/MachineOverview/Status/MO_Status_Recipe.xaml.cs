using HMI.Views.DialogRegion;
using HMI.Views.MainRegion.Recipe.Custom_Objects;
using System.Collections.Generic;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

	[ExportView("MO_Status_Recipe")]
	public partial class MO_Status_Recipe : VisiWin.Controls.View
	{

        public MO_Status_Recipe()
		{
			this.InitializeComponent();
        }


        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                string MR_Name =  ApplicationService.ObjectStore.GetValue("MO_Status_Recipe_KEY").ToString();


                Dictionary<string, object> r_val;
                IRecipeClass MachineRecipe = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
                if (MachineRecipe.IsExistingRecipeFile(MR_Name))
                {
                    MR.Value = MR_Name;
                    MR_descr.Value = MachineRecipe.GetRecipeFile(MR_Name).Description;
                    r_val = MachineRecipe.GetRecipeFile(MR_Name).GetValues();

                    string lgo = r_val["Recipe.MR.LGO"].ToString();
                    LGO.Value = lgo;
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("LGO");
                    LGO_descr.Value = T.GetRecipeFile(lgo).Description;

                    string alo = r_val["Recipe.MR.ALO"].ToString();
                    ALO.Value = alo;
                    T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("ALO");
                    ALO_descr.Value = T.GetRecipeFile(alo).Description;

                    string nio = r_val["Recipe.MR.NIO"].ToString();
                    NIO.Value = nio;
                    T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("NIO");
                    NIO_descr.Value = T.GetRecipeFile(nio).Description;
                }
            }
        }

        private void _PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (((VisiWin.Controls.TextVarOut)sender).Value.ToString() != "")
            {
                ApplicationService.SetView("MessageBoxRegion", "EmptyView");
                ApplicationService.ObjectStore.SetValue("DialogView_RESULT", MessageBoxResult.Cancel);
                switch (((VisiWin.Controls.TextVarOut)sender).Tag.ToString())
                {
                    case "MR": (new AutoRecipeLoad(MR.Value.ToString())).LoadStackAsync(); break;
                    case "LGO": (new AutoRecipeLoad()).LoadLGOAsync(LGO.Value.ToString()); break;
                    case "ALO": (new AutoRecipeLoad()).LoadALOAsync(ALO.Value.ToString()); break;
                    case "NIO": (new AutoRecipeLoad()).LoadNIOAsync(NIO.Value.ToString()); break;
                }
            }
            
        }
    }
}