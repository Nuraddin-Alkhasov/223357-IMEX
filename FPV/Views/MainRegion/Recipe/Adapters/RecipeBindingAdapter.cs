
using HMI.Module;
using HMI.Views.MainRegion.Recipe.Custom_Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisiWin.ApplicationFramework;
using VisiWin.Commands;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.Recipe.Adapters
{
    class RecipeBindingAdapter : AdapterBase
    {
        ObservableCollection<BCToMR> _BCToMRList;
        BCToMR _SelectedData;
        bool _isBCToMRSelected;
        bool _isEditing;
        Visibility _DialogVisible;
        public RecipeBindingAdapter()
        {
            UpdateBCToMRList();
            this._BCToMRList = new ObservableCollection<BCToMR>();
            this.PropertyChanged += this.OnSelectedDataChanged;
            this.NewCommand = new ActionCommand(NewCommandExecuted);
            this.EditCommand = new ActionCommand(EditCommandExecuted);
            this.CloseDialogCommand = new ActionCommand(CloseDialogCommandExecuted);
            this.CloseDialogViewCommand = new ActionCommand(CloseDialogViewCommandExecuted);
            
            this.SaveCommand = new ActionCommand(SaveCommandExecutedAsync);
            this.DeleteCommand = new ActionCommand(DeleteCommandExecuted);
            _DialogVisible = Visibility.Hidden;
        }

        public BCToMR SelectedData
        {
            get { return this._SelectedData; }
            set
            {
                if (!Equals(value, this._SelectedData))
                {
                    this._SelectedData = value;
                    this.OnPropertyChanged("SelectedData");
                }
            }
        }

        public bool isBCToMRSelected
        {
            get { return this._isBCToMRSelected; }
            set
            {
                this._isBCToMRSelected = value;
                this.OnPropertyChanged("isBCToMRSelected");
            }
        }

        public void OnSelectedDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SelectedData".Equals(e.PropertyName))
            {
                this.isBCToMRSelected = this.SelectedData != null;
            }
        }

        public bool isEditing
        {
            get { return this._isEditing; }
            set
            {
                this._isEditing = value;
                this.OnPropertyChanged("isEditing");
            }
        }

        public Visibility DialogVisible
        {
            get { return this._DialogVisible; }
            set
            {
                if (!Equals(value, this._DialogVisible))
                {
                    this._DialogVisible = value;
                    this.OnPropertyChanged("DialogVisible");
                }
            }
        }

        public ObservableCollection<BCToMR> BCToMRList
        {
            get { return this._BCToMRList; }
            set
            {
                if (!Equals(value, this._BCToMRList))
                {
                    this._BCToMRList = value;
                    this.OnPropertyChanged("BCToMRList");
                }
            }
        }

        private void UpdateBCToMRList()
        {
            Task T = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    DataTable _BCToMR_DT = (new localDBAdapter("Select BarcodeToMR.Id, BarcodeToMR.Barcode, MachineRecipes.MR From BarcodeToMR JOIN MachineRecipes ON BarcodeToMR.MR_ID = MachineRecipes.ID;")).DB_Output();
                    if (_BCToMR_DT.Rows.Count > 0)
                    {
                        this._BCToMRList.Clear();
                        foreach (DataRow row in _BCToMR_DT.Rows)
                        {
                            _BCToMRList.Add(new BCToMR(Convert.ToInt32(row[0]), row[1].ToString(), row[2].ToString()));
                        }
                    }
                    else
                    {
                        _BCToMRList.Clear();
                    }
                });
            });
        }

        public ICommand NewCommand { get; set; }

        private void NewCommandExecuted(object parameter)
        {
            Task T = Task.Run(() =>
            {
                bool result = (new localDBAdapter("INSERT INTO BarcodeToMR (Barcode) VALUES ('')")).DB_Input();
                UpdateBCToMRList();
            });
        }

        public ICommand EditCommand { get; set; }

        private void EditCommandExecuted(object parameter)
        {
            isEditing = true;
            DialogVisible = Visibility.Visible;
        }

        public ICommand CloseDialogCommand { get; set; }

        private void CloseDialogCommandExecuted(object parameter)
        {

            UpdateBCToMRList();

            isEditing = false;
            DialogVisible = Visibility.Hidden;
        }

        public ICommand CloseDialogViewCommand { get; set; }

        private void CloseDialogViewCommandExecuted(object parameter)
        {
            isEditing = false;
            DialogVisible = Visibility.Hidden;
            ApplicationService.SetView("DialogRegion", "EmptyView");
        }

        public ICommand SaveCommand { get; set; }

        private void SaveCommandExecutedAsync(object parameter)
        {
            if (this._SelectedData != null)
            {
                IRecipeClass Tx = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
                if(Tx.IsExistingRecipeFile(_SelectedData.MR))
                {
                    Dictionary<string, object> r_val = Tx.GetRecipeFile(_SelectedData.MR).GetValues();
                    string lgo = r_val["Recipe.MR.LGO"].ToString();
                    string alo = r_val["Recipe.MR.ALO"].ToString();
                    string nio = r_val["Recipe.MR.NIO"].ToString();

                    Task T = Task.Run(() =>
                    {
                        Application.Current.Dispatcher.InvokeAsync((Action)delegate
                        {
                            DataTable temp = (new localDBAdapter("SELECT * FROM MachineRecipes WHERE MR = '" + _SelectedData.MR + "' AND LGO = '" + lgo + "' AND ALO = '" + alo + "' AND NIO = '" + nio + "'")).DB_Output();
                            long MR_ID;
                            if (temp.Rows.Count == 0)
                            {
                                bool result = (new localDBAdapter("INSERT INTO MachineRecipes (MR, LGO, ALO, NIO) VALUES ('" + _SelectedData.MR + "','" + lgo + "','" + alo + "','" + nio + "')")).DB_Input();
                                MR_ID = (long)(new localDBAdapter("SELECT * FROM MachineRecipes WHERE MR = '" + _SelectedData.MR + "' AND LGO = '" + lgo + "' AND ALO = '" + alo + "' AND NIO = '" + nio + "'")).DB_Output().Rows[0][0];
                            }
                            else
                            {
                                MR_ID = Convert.ToInt32(temp.Rows[0][0]);
                            }


                            temp = (new localDBAdapter("SELECT * FROM BarcodeToMR WHERE Barcode ='" + _SelectedData.Barcode + "' ; ")).DB_Output();
                            if (temp.Rows.Count == 0)
                            {
                                bool result = (new localDBAdapter("UPDATE BarcodeToMR SET Barcode ='" + _SelectedData.Barcode + "', MR_ID = " + MR_ID.ToString() + " WHERE ID = " + _SelectedData.ID.ToString() + ";")).DB_Input();

                            }
                            else
                            {
                                bool result = (new localDBAdapter("UPDATE BarcodeToMR SET MR_ID = " + MR_ID.ToString() + " WHERE Barcode ='" + _SelectedData.Barcode + "';")).DB_Input();

                            }
                            isEditing = false;
                            DialogVisible = Visibility.Hidden;
                            UpdateBCToMRList();
                        });
                    });
                }
              
            } 
        }

        public ICommand DeleteCommand { get; set; }

        private void DeleteCommandExecuted(object parameter)
        {
            if (this._SelectedData != null)
            {
                Task T = Task.Run(() =>
                {
                    bool result = (new localDBAdapter("DELETE FROM BarcodeToMR WHERE ID = " + SelectedData.ID.ToString() + ";")).DB_Input();
                    UpdateBCToMRList();
                });
            }
        }

    }
}
