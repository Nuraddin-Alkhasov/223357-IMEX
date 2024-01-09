using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe.Custom_Objects
{
    public class AutoRecipeLoad
    {
        private readonly string MR_Name;
        IRecipeClass MR;
        Dictionary<string, object> r_val;
        IRegionService iRS = ApplicationService.GetService<IRegionService>();

        public AutoRecipeLoad(string _MR)
        {
            MR_Name = _MR;
        }

        public AutoRecipeLoad() { }

        public async void LoadStackAsync()
        {
            MR = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            
            if ((await MR.LoadFromFileToBufferAsync(MR_Name)).Result == LoadRecipeResult.Succeeded)
            {
                r_val = MR.GetRecipeFile(MR_Name).GetValues();
                ApplicationService.SetVariableValue("Recipe.MR.Name", MR_Name);

                if ((await LoadFromFileTOBufferLGOAsync("")) == LoadRecipeResult.Succeeded)
                {
                    if ((await LoadFromFileTOBufferALOAsync("")) == LoadRecipeResult.Succeeded)
                    {
                        if ((await LoadFromFileTOBufferNIOAsync("")) == LoadRecipeResult.Succeeded)
                        {
                            AppbarView AV = (AppbarView)iRS.GetView("AppbarView");
                            AV.recipe.IsChecked = true;
                            Recipe_PN R_PN = (Recipe_PN)iRS.GetView("Recipe_PN");
                            if (iRS.GetCurrentViewName("MainRegion") != "Recipe_PN")
                            {
                                ApplicationService.SetView("MainRegion", "Recipe_PN");
                                if (R_PN.pn_recipe.SelectedPanoramaRegionIndex != 0)
                                {
                                    R_PN.pn_recipe.ScrollPrevious(true);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async void LoadLGOAsync(string _LGO)
        {
            if ((await LoadFromFileTOBufferLGOAsync(_LGO)) == LoadRecipeResult.Succeeded)
            {
                AppbarView AV = (AppbarView)iRS.GetView("AppbarView");
                AV.recipe.IsChecked = true;

                ApplicationService.SetView("MainRegion", "Recipe_PN");
                Recipe_PN R_PN = (Recipe_PN)iRS.GetView("Recipe_PN");
                R_PN.externLoad = true;
                R_PN.rId = 1;
            }
        }

        public async void LoadALOAsync(string _ALO)
        {
            if ((await LoadFromFileTOBufferALOAsync(_ALO)) == LoadRecipeResult.Succeeded)
            {
                AppbarView AV = (AppbarView)iRS.GetView("AppbarView");
                AV.recipe.IsChecked = true;

                ApplicationService.SetView("MainRegion", "Recipe_PN");
                Recipe_PN R_PN = (Recipe_PN)iRS.GetView("Recipe_PN");
                R_PN.externLoad = true;
                R_PN.rId = 2;
            }
        }

        public async void LoadNIOAsync(string _NIO)
        {
            if ((await LoadFromFileTOBufferNIOAsync(_NIO)) == LoadRecipeResult.Succeeded)
            {
                AppbarView AV = (AppbarView)iRS.GetView("AppbarView");
                AV.recipe.IsChecked = true;
               
                ApplicationService.SetView("MainRegion", "Recipe_PN");
                Recipe_PN R_PN = (Recipe_PN)iRS.GetView("Recipe_PN");
                R_PN.externLoad = true;
                R_PN.rId = 3;
            }
        }

        public async void LoadLGOTask(string _LGO)
        {
            if (_LGO != "")
                await LoadFromFileTOBufferLGOAsync(_LGO);
        }

        public async void LoadALOTask(string _ALO)
        {
            if (_ALO != "")
                await LoadFromFileTOBufferALOAsync(_ALO);
        }

        public async void LoadNIOTask(string _NIO)
        {
            if(_NIO!="")
                await LoadFromFileTOBufferNIOAsync(_NIO);
        }

        private async Task<LoadRecipeResult> LoadFromFileTOBufferALOAsync(string _a)
        {
            if (_a == "")
            {
                string alo = r_val["Recipe.MR.ALO"].ToString();
                if (alo != "")
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("ALO");
                    if ((await T.LoadFromFileToBufferAsync(alo)).Result == LoadRecipeResult.Succeeded)
                    {
                        ApplicationService.SetVariableValue("Recipe.ALO.Name", alo);
                        return LoadRecipeResult.Succeeded;
                    }
                }
            }
            else
            {
                IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("ALO");
                if ((await T.LoadFromFileToBufferAsync(_a)).Result == LoadRecipeResult.Succeeded)
                {
                    ApplicationService.SetVariableValue("Recipe.ALO.Name", _a);
                    return LoadRecipeResult.Succeeded;
                }
            }
            return LoadRecipeResult.LoadedWithErrors;
        }

        private async Task<LoadRecipeResult> LoadFromFileTOBufferLGOAsync(string _a)
        {
            if (_a == "")
            {
                string lgo = r_val["Recipe.MR.LGO"].ToString();
                if (lgo != "")
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("LGO");
                    if ((await T.LoadFromFileToBufferAsync(lgo)).Result == LoadRecipeResult.Succeeded)
                    {
                        ApplicationService.SetVariableValue("Recipe.LGO.Name", lgo);
                        return LoadRecipeResult.Succeeded;
                    }
                }
            }
            else
            {
                IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("LGO");
                if ((await T.LoadFromFileToBufferAsync(_a)).Result == LoadRecipeResult.Succeeded)
                {
                    ApplicationService.SetVariableValue("Recipe.LGO.Name", _a);
                    return LoadRecipeResult.Succeeded;
                }
            }
            return LoadRecipeResult.LoadedWithErrors;
        }

        private async Task<LoadRecipeResult> LoadFromFileTOBufferNIOAsync(string _a)
        {
            if (_a == "")
            {
                string nio = r_val["Recipe.MR.NIO"].ToString();
                if (nio != "")
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("NIO");
                    if ((await T.LoadFromFileToBufferAsync(nio)).Result == LoadRecipeResult.Succeeded)
                    {
                        ApplicationService.SetVariableValue("Recipe.NIO.Name", nio);
                        return LoadRecipeResult.Succeeded;
                    }
                }
            }
            else
            {
                IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("NIO");
                if ((await T.LoadFromFileToBufferAsync(_a)).Result == LoadRecipeResult.Succeeded)
                {
                    ApplicationService.SetVariableValue("Recipe.NIO.Name", _a);
                    return LoadRecipeResult.Succeeded;
                }
            }
            return LoadRecipeResult.LoadedWithErrors;
        }

    }
}
