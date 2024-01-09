﻿using System.Collections.ObjectModel;
using VisiWin.ApplicationFramework;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe
{
   
    [ExportAdapter("RecipeAdapter_LGO")]
    public class RecipeAdapter_LGO : AdapterBase
    {
        public IRecipeClass recipeClass;
        private ObservableCollection<RecipeFileInfo> recipeFiles;

        public RecipeAdapter_LGO()
        {
            this.recipeClass = ApplicationService.GetService<IRecipeService>().GetRecipeClass("LGO");
            this.RecipeFiles = new ObservableCollection<RecipeFileInfo>();
            this.UpdateFileList();
        }


        public ObservableCollection<RecipeFileInfo> RecipeFiles
        {
            get { return this.recipeFiles; }
            set
            {
                if (!Equals(value, this.recipeFiles))
                {
                    this.recipeFiles = value;
                    this.OnPropertyChanged("RecipeFiles");
                }
            }
        }

        public void UpdateFileList()
        {
            if (this.recipeClass != null)
            {
                this.RecipeFiles.Clear();
                foreach (var nextFile in this.recipeClass.FileNames)
                {
                    var r = this.recipeClass.GetRecipeFile(nextFile);
                    this.RecipeFiles.Add(new RecipeFileInfo(recipeClass.Name, nextFile, r.Description, r.TimeOfLastChange, r.WhoSavedRecipe()));
                }
            }
        }
    }

   
}