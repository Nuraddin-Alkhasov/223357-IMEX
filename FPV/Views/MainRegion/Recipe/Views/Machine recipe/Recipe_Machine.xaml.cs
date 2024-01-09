using HMI.Reports.Recipes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.Recipe;
using HMI.Reporting;

namespace HMI.Views.MainRegion.Recipe
{

    [ExportView("Recipe_Machine")]
    public partial class Recipe_Machine : VisiWin.Controls.View
    {
        public bool SaveActive;
        bool isLoaded;
        Point _lastTapLocation_LGO;
        Point _lastTapLocation_ALO;
        Point _lastTapLocation_NIO;

        public Recipe_Machine()
        {
            isLoaded = false;
            SaveActive = false;
            this.InitializeComponent();
        }

        #region - - - Event Handlers - - -

        #region - - - ALO - - -

        private void ALO_name_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            if (e.Value.ToString() != "")
            {
                SetDescriptionToRecipe(e.Value.ToString(), "ALO");
                ALO.IsBlinkEnabled = false;
            }
            else
            {
                ALO.IsBlinkEnabled = true;
            }
            CheckifSaveActive();
        }

        private void Dgv_alo_GotFocus(object sender, RoutedEventArgs e)
        {
            ALOList.IsChecked = true;
        }

        private void ALO_del_Click(object sender, RoutedEventArgs e)
        {
            ALO_name.Value = "";
            ALO_descr.Value = "";
            ALO.IsChecked = true;
        }

        private void Dgv_alo_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (ALO.IsChecked == true && IsOverALO(e.GetTouchPoint(this).Position))
            {
                ItemDrop();
            }
            DragItem.Visibility = Visibility.Hidden;
        }

        private void Dgv_alo_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (dgv_alo.SelectedIndex != -1)
            {
                RecalculatePosition(e.GetTouchPoint(this).Position, _lastTapLocation_ALO, 2);

                if (IsOverALO(e.GetTouchPoint(this).Position))
                {
                    if (ALO.IsChecked == false)
                    {
                        ALO.IsChecked = true;
                    }
                }
            }
        }

        private void ALOList_Checked(object sender, RoutedEventArgs e)
        {
            ALO.IsChecked = true;
            dgv_alo.SelectedItem = GetItem(ALO_name.Value.ToString(), dgv_alo);
        }

        private void ALOList_Unchecked(object sender, RoutedEventArgs e)
        {
            dgv_alo.SelectedItem = null;
        }

        private void ALO_Checked(object sender, RoutedEventArgs e)
        {
            ALOList.IsChecked = true;
        }


        #endregion

        #region - - - LGO - - -

        private void LGO_name_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            if (e.Value.ToString() != "")
            {
                SetDescriptionToRecipe(e.Value.ToString(), "LGO");
                LGO.IsBlinkEnabled = false;
            }
            else
            {
                LGO.IsBlinkEnabled = true;
            }
            CheckifSaveActive();
        }

        private void Dgv_lgo_GotFocus(object sender, RoutedEventArgs e)
        {
            LGOList.IsChecked = true;
        }

        private void LGO_del_Click(object sender, RoutedEventArgs e)
        {
            LGO_name.Value = "";
            LGO_descr.Value = "";
            LGO.IsChecked = true;
        }

        private void Dgv_lgo_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (LGO.IsChecked == true && IsOverLGO(e.GetTouchPoint(this).Position))
            {
                ItemDrop();
            }
            DragItem.Visibility = Visibility.Hidden;
        }

        private void Dgv_lgo_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (dgv_lgo.SelectedIndex != -1)
            {
                RecalculatePosition(e.GetTouchPoint(this).Position, _lastTapLocation_LGO, 1);

                if (IsOverLGO(e.GetTouchPoint(this).Position))
                {
                    if (LGO.IsChecked == false)
                    {
                        LGO.IsChecked = true;
                    }
                }
            }
        }

        private void LGOList_Checked(object sender, RoutedEventArgs e)
        {
            LGO.IsChecked = true;
            dgv_lgo.SelectedItem = GetItem(LGO_name.Value.ToString(), dgv_lgo);
        }

        private void LGOList_Unchecked(object sender, RoutedEventArgs e)
        {
            dgv_lgo.SelectedItem = null;
        }

        private void LGO_Checked(object sender, RoutedEventArgs e)
        {
            if(LGOList!=null)
            LGOList.IsChecked = true;
        }

        private void LGO_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                Task obTask = Task.Run(() =>
                {
                    Application.Current.Dispatcher.InvokeAsync((Action)delegate
                    {
                        LGO.IsChecked = true;
                        isLoaded = true;
                    });
                });
            }
        }

        #endregion

        #region - - - NIO - - -

        private void NIO_name_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            if (e.Value.ToString() != "")
            {
                SetDescriptionToRecipe(e.Value.ToString(), "NIO");
                NIO.IsBlinkEnabled = false;
            }
            else
            {
                NIO.IsBlinkEnabled = true;
            }
            CheckifSaveActive();
        }

        private void Dgv_nio_GotFocus(object sender, RoutedEventArgs e)
        {
            NIOList.IsChecked = true;
        }

        private void NIO_del_Click(object sender, RoutedEventArgs e)
        {
            NIO_name.Value = "";
            NIO_descr.Value = "";
            NIO.IsChecked = true;
        }

        private void Dgv_nio_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (NIO.IsChecked == true && IsOverNIO(e.GetTouchPoint(this).Position))
            {
                ItemDrop();
            }
            DragItem.Visibility = Visibility.Hidden;
        }

        private void Dgv_nio_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (dgv_nio.SelectedIndex != -1)
            {
                RecalculatePosition(e.GetTouchPoint(this).Position, _lastTapLocation_NIO, 3);

                if (IsOverNIO(e.GetTouchPoint(this).Position))
                {
                    if (NIO.IsChecked == false)
                    {
                        NIO.IsChecked = true;
                    }
                }
            }
        }

        private void NIOList_Checked(object sender, RoutedEventArgs e)
        {
            NIO.IsChecked = true;
            dgv_nio.SelectedItem = GetItem(NIO_name.Value.ToString(), dgv_nio);
        }

        private void NIOList_Unchecked(object sender, RoutedEventArgs e)
        {
            dgv_nio.SelectedItem = null;
        }

        private void NIO_Checked(object sender, RoutedEventArgs e)
        {
            NIOList.IsChecked = true;
        }

        #endregion

        private void Rlist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemDrop();
        }

        private void List_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            switch (((DataGrid)sender).Name)
            {
                case "dgv_lgo": _lastTapLocation_LGO = e.GetTouchPoint(this).Position; break;
                case "dgv_alo": _lastTapLocation_ALO = e.GetTouchPoint(this).Position; break;
                case "dgv_nio": _lastTapLocation_NIO = e.GetTouchPoint(this).Position; break;
            }
        }

        #endregion

        #region - - - Methods - - -

        public void dgvUpdate()
        {
            int oldIndex = dgv_alo.SelectedIndex;
            ((RecipeAdapter_ALO)dgv_alo.DataContext).UpdateFileList();
            dgv_alo.SelectedIndex = oldIndex;
            oldIndex= dgv_lgo.SelectedIndex;
            ((RecipeAdapter_LGO)dgv_lgo.DataContext).UpdateFileList();
            dgv_lgo.SelectedIndex = oldIndex;
            oldIndex = dgv_nio.SelectedIndex;
            ((RecipeAdapter_NIO)dgv_nio.DataContext).UpdateFileList();
            dgv_nio.SelectedIndex = oldIndex;
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
                        switch (_b)
                        {
                            case "ALO": ALO_descr.Value = T.GetRecipeFile(_a).Description; break;
                            case "LGO": LGO_descr.Value = T.GetRecipeFile(_a).Description; break;
                            case "NIO": NIO_descr.Value = T.GetRecipeFile(_a).Description; break;
                        }
                    });
                });
            }
        }

        private RecipeFileInfo GetItem(string _a, DataGrid _b)
        {
            if(_b!=null)
            for (int i = 0; i < _b.Items.Count; i++)
            {
                if (((RecipeFileInfo)_b.Items[i]).FileName == _a)
                {
                    return (RecipeFileInfo)_b.Items[i];
                }
            }
            return null;
        }

        #endregion

        private bool IsOverLGO(Point a)
        {
            if (a.X >= 30 && a.X <= 810 && a.Y >= 90 && a.Y <= 240)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsOverALO(Point a)
        {
            if (a.X >= 30 && a.X <= 810 && a.Y >= 380 && a.Y <= 530)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsOverNIO(Point a)
        {
            if (a.X >= 30 && a.X <= 810 && a.Y >= 660 && a.Y <= 810)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ItemDrop()
        {
            if (dgv_alo.SelectedIndex != -1)
            {
                RecipeFileInfo T = (RecipeFileInfo)dgv_alo.SelectedItem;
                ALO_name.Value = T.FileName;
                ALO_descr.Value = T.Description;
            }

            if (dgv_lgo.SelectedIndex != -1)
            {
                RecipeFileInfo T = (RecipeFileInfo)dgv_lgo.SelectedItem;
                LGO_name.Value = T.FileName;
                LGO_descr.Value = T.Description;
            }

            if (dgv_nio.SelectedIndex != -1)
            {
                RecipeFileInfo T = (RecipeFileInfo)dgv_nio.SelectedItem;
                NIO_name.Value = T.FileName;
                NIO_descr.Value = T.Description;
            }
        }

        private void RecalculatePosition(Point a, Point b, int c)
        {
            DragItem.Margin = new Thickness(a.X - 30, a.Y - 70, 0, 0);

            if (Math.Abs(a.X - b.X) > 25 && DragItem.Visibility == Visibility.Hidden)
            {
                RecipeFileInfo T = null;

                switch (c)
                {
                    case 1: T = (RecipeFileInfo)dgv_lgo.SelectedItem; DragItem_Pic.SymbolResourceKey = "R_LGO"; break;
                    case 2: T = (RecipeFileInfo)dgv_alo.SelectedItem; DragItem_Pic.SymbolResourceKey = "R_ALO"; break;
                    case 3: T = (RecipeFileInfo)dgv_nio.SelectedItem; DragItem_Pic.SymbolResourceKey = "R_NIO"; break;
                }

                if (T != null)
                {
                    DragItem_Name.Text = T.FileName;
                   
                    DragItem.Width = T.FileName.Length * 8.4 + 55;
                    DragItem.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adapter = ApplicationService.GetAdapter(nameof(ReportViewAdapter)) as ReportViewAdapter;
            IRecipeClass MR = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            string MRName = ApplicationService.GetVariableValue("Recipe.MR.Name").ToString();
            if (MRName != "")
            {
                if (MR.IsExistingRecipeFile(MRName))
                {
                    Dictionary<string, object> r_val = MR.GetRecipeFile(MRName).GetValues();
                    r_val.Add("Recipe.MR.Name", MR.GetRecipeFile(MRName).FileName);
                    Func<CancellationToken, Task<ReportConfiguration>> config;
                    config = (t) => MRReport.GetReportConfiguration(r_val);

                    //ReportView mit der Konfiguration öffnen
                    adapter?.OpenView(config);

                }
            }
            
        }

        private void CheckifSaveActive()
        {
            if (LGO_name.Value == "" || ALO_name.Value == "" || NIO_name.Value == "")
            {
                SaveActive= false;
            }
            else
            {
                SaveActive = true;
            }
        }

        private void _PreviewTouchDown1(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv_lgo.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
           
            LGOList.IsChecked = true;
        }
        private void _PreviewTouchDown2(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv_alo.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
           
            ALOList.IsChecked = true;
        }
        private void _PreviewTouchDown3(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dgv_nio.UnselectAllCells();
            ((DataGridRow)sender).IsSelected = true;
           
            NIOList.IsChecked = true;
        }

    }
}