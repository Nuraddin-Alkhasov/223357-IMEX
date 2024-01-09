using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HMI.Views.MainRegion.Recipe.Custom_Objects;
using HMI.Views.MessageBoxRegion;
using VisiWin.ApplicationFramework;
using VisiWin.Commands;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe
{
   
    [ExportAdapter("RecipeBrowserAdapter")]
    public class RecipeBrowserAdapter : AdapterBase
    {

        private ObservableCollection<RecipeFileInfo> recipeFiles;
        IRecipeClass recipeClass;

        private string recipeDescriptionBuffer = "";
        private string recipeNameBuffer = "";
        private bool recipeIsSelected;
        private string RecipeClassName = "";
      
        private RecipeFileInfo selectedFile;

        private int ClassID;

        public RecipeBrowserAdapter(int _classID)
        {
            ClassID = _classID;
            RecipeClassName = ApplicationService.GetService<IRecipeService>().RecipeClassNames[ClassID];
            recipeClass = ApplicationService.GetService<IRecipeService>().GetRecipeClass(RecipeClassName);
            
            this.RecipeFiles = new ObservableCollection<RecipeFileInfo>();
            this.PropertyChanged += this.OnSelectedFileChanged;

            this.UpdateFileList();

            this.LoadFileToBuffer = new ActionCommand(this.LoadRecipeToBufferCommandExecuted);
            this.SaveFileCommand = new ActionCommand(this.SaveRecipeCommandExecuted);
            this.DeleteFileCommand = new ActionCommand(this.OnDeleteFileCommandExecuted);

            SelectedFile = GetSelectedR();
        }

        private RecipeFileInfo GetSelectedR()
        {
            string _r = "";
            switch (ClassID)
            {
                case 0:
                    _r= ApplicationService.GetVariableValue("Recipe.MR.Name").ToString(); break;
                case 1:
                    _r = ApplicationService.GetVariableValue("Recipe.ALO.Name").ToString();break;
                case 2:
                    _r = ApplicationService.GetVariableValue("Recipe.LGO.Name").ToString(); break;
                case 3:
                    _r = ApplicationService.GetVariableValue("Recipe.NIO.Name").ToString(); break;
            }

            foreach (RecipeFileInfo rf in recipeFiles)
            {
                if (rf.FileName == _r)
                {
                    return rf;
                }
            }
            return null;
        }

        public ICommand LoadFileToBuffer { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }

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

        public string RecipeNameBuffer
        {
            get { return this.recipeNameBuffer; }
            set
            {
                this.recipeNameBuffer = value;
                this.OnPropertyChanged("RecipeNameBuffer");
            }
        }

        public string RecipeDescriptionBuffer
        {
            get { return this.recipeDescriptionBuffer; }
            set
            {
                this.recipeDescriptionBuffer = value;
                this.OnPropertyChanged("RecipeDescriptionBuffer");
            }
        }

        public bool RecipeIsSelected
        {
            get { return this.recipeIsSelected; }
            set
            {
                this.recipeIsSelected = value;
                this.OnPropertyChanged("RecipeIsSelected");
            }
        }

        public RecipeFileInfo SelectedFile
        {
            get { return this.selectedFile; }
            set
            {
                if (!Equals(value, this.selectedFile))
                {
                    this.selectedFile = value;
                    if (selectedFile != null)
                    {
                        this.RecipeNameBuffer = selectedFile.FileName;
                        this.RecipeDescriptionBuffer = selectedFile.Description;
                    }
                    else
                    {
                        this.RecipeNameBuffer = "";
                        this.RecipeDescriptionBuffer = "";
                    }
                   
                    this.OnPropertyChanged("SelectedFile");
                }
            }
        }

        public void OnSelectedFileChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SelectedFile".Equals(e.PropertyName))
            {
                this.RecipeIsSelected = this.SelectedFile != null;
            }
        }

        private void LoadRecipeToBufferCommandExecuted(object parameter)
        {
            if (this.SelectedFile != null)
            {
               LoadFromFileToBufferAsync();
            }
        }

        public async void LoadFromFileToBufferAsync()
        {
            if (recipeClass.Name == "MR")
            {
                (new AutoRecipeLoad(SelectedFile.FileName)).LoadStackAsync();
            }
            else
            {
                var e = (await this.recipeClass.LoadFromFileToBufferAsync(this.SelectedFile.FileName));

                if (e.Result == LoadRecipeResult.Succeeded)
                {
                    this.RecipeNameBuffer = SelectedFile.FileName;
                    this.RecipeDescriptionBuffer = SelectedFile.Description;
                    setNameAndDescr();
                }
                else
                {
                    new MessageBoxTask("@RecipeSystem.Results.LoadError", "@RecipeSystem.View.Buttons.Load",  MessageBoxIcon.Error);
                }
            }
        }

        private void SaveRecipeCommandExecuted(object parameter)
        {
            bool saveR = true;
            if (this.recipeClass != null)
            {
                if (recipeClass.Name == "MR")
                {
                    IRegionService iRS = ApplicationService.GetService<IRegionService>();
                    if (!((Recipe_Machine)iRS.GetView("Recipe_Machine")).SaveActive)
                    { saveR = false; }
                }
               
                if (saveR)
                {
                    if (string.IsNullOrEmpty(this.RecipeNameBuffer))
                    {
                        new MessageBoxTask("@RecipeSystem.Results.EnterFilename", "@RecipeSystem.Results.SaveFile", MessageBoxIcon.Information);
                        return;
                    }

                    if (this.recipeClass.FileNames.Contains(this.RecipeNameBuffer))
                    {
                        if (MessageBoxView.Show("@RecipeSystem.Results.OverwriteFileQuery", "@RecipeSystem.Results.SaveFile", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                    SaveToFileFromBufferAsync();
                }
                else
                {
                   new MessageBoxTask("Dieses Rezept ist nicht gültig", "Speichern nicht möglich", MessageBoxIcon.Exclamation);
                }
            }
        }

        public async void SaveToFileFromBufferAsync()
        {
            var e = (await this.recipeClass.SaveToFileFromBufferAsync(this.RecipeNameBuffer, this.RecipeDescriptionBuffer, true));

            if (e.Result == SaveRecipeResult.Succeeded)
            {
                setNameAndDescr();
            }
        }

        private void OnDeleteFileCommandExecuted(object parameter)
        {
            if (this.SelectedFile != null)
            {
                if (MessageBoxView.Show("@RecipeSystem.Results.DeleteFileQuery", "@RecipeSystem.Results.DeleteFile", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.Yes)
                {
                    if (this.recipeClass.DeleteFile(this.SelectedFile.FileName))
                    {
                        this.UpdateFileList();
                        this.RecipeNameBuffer = "";
                        this.RecipeDescriptionBuffer = "";
                    }
                }
            }
        }

        private void UpdateFileList()
        {
            this.RecipeFiles.Clear();
            foreach (var nextFile in recipeClass.FileNames)
            {
                var r = recipeClass.GetRecipeFile(nextFile);
                this.RecipeFiles.Add(new RecipeFileInfo(recipeClass.Name, nextFile, r.Description, r.TimeOfLastChange, r.WhoSavedRecipe()));
            }
        }

        private void setNameAndDescr()
        {
            switch (ClassID)
            {
                case 0:
                    ApplicationService.SetVariableValue("Recipe.MR.Name", RecipeNameBuffer); break;
                case 1:
                    ApplicationService.SetVariableValue("Recipe.ALO.Name", RecipeNameBuffer); break;
                case 2:
                    ApplicationService.SetVariableValue("Recipe.LGO.Name", RecipeNameBuffer); break;
                case 3:
                    ApplicationService.SetVariableValue("Recipe.NIO.Name", RecipeNameBuffer); break;
            }
        }

    }
}