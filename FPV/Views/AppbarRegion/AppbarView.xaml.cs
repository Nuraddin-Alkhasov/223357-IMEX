using HMI.Diagnose;
using HMI.User;
using HMI.Views.MainRegion.Protocol;
using HMI.Views.MainRegion.Recipe;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.DataAccess;
using VisiWin.Language;

namespace HMI
{
    /// <summary>
    /// Interaction logic for AppbarView.xaml
    /// </summary>
    [ExportView("AppbarView")]
    public partial class AppbarView : View
    {
        readonly IVariableService VS = ApplicationService.GetService<IVariableService>();
        readonly IVariable ERS;
        readonly IVariable BFS;
        readonly IVariable userChange;
        readonly IRegionService iRS = ApplicationService.GetService<IRegionService>();
        readonly ILanguageService languageService = ApplicationService.GetService<ILanguageService>();
        public AppbarView()
        {
            InitializeComponent();
         
            ERS = VS.GetVariable("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Sammelstörung Anlage");
            ERS.Change += ERS_Change;
            BFS = VS.GetVariable("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Sammelbedinerführung Anlage");
            BFS.Change += BFS_Change;

            userChange = VS.GetVariable("__CURRENT_USER.FULLNAME");
            userChange.Change += UserChange_Change;

            
        }

        private void BFS_Change(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                diagnose.IsBlinkEnabled = true;
                var converter = new System.Windows.Media.BrushConverter();
                if ((bool)e.Value)
                {
                    if ((bool)ERS.Value)
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FFBE2828");  //red
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBC49");  //yellow
                    }
                    else
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FF807F7F");//  gray
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBC49");//  yellow
                    }
                }
                else
                {
                    if ((bool)ERS.Value)
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FF807F7F");//  gray
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBE2828");  //red
                    }
                    else
                    {
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FF807F7F");//  gray
                        diagnose.IsBlinkEnabled = false;
                    }
                }
            }
        }

        private void ERS_Change(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                var converter = new System.Windows.Media.BrushConverter();
                diagnose.IsBlinkEnabled = true;
                if ((bool)e.Value)
                {
                    if ((bool)BFS.Value)
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FFBE2828");  //red
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBC49");  //yellow
                    }
                    else
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FF807F7F");  //gray
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBE2828");//  red
                    }
                }
                else
                {
                    if ((bool)BFS.Value)
                    {
                        diagnose.Background = (Brush)converter.ConvertFromString("#FF807F7F");  //gray
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FFBC49");//  yellow
                    }
                    else
                    {
                        diagnose.BlinkBrush = (Brush)converter.ConvertFromString("#FF807F7F");//  gray
                        diagnose.IsBlinkEnabled = false;
                    }

                }

            }
        }

        private void UserChange_Change(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue)
            {
                if (ButtonCloseMenu.Visibility == Visibility.Visible)
                {
                    user.Text = userChange.Value.ToString();
                }
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            dashboard.LocalizableText = "@Appbar.Text1";
            moverview.LocalizableText = "@Appbar.lblAnlageübersicht";
            handmode.LocalizableText = "@Appbar.lblHandmenü";
            recipe.LocalizableText = "@Appbar.lblRezeptverwaltung";
            parameter.LocalizableText = "@Appbar.lblParameter";
            protocol.LocalizableText = "@Protocol.Text33";
            betriebstunden.LocalizableText = "@Appbar.lblBetriebstunden";
            logs.LocalizableText = "@Appbar.lbllogs";
            usermanager.LocalizableText = "@Appbar.lblBenutzerübersicht";
            diagnose.LocalizableText = "@Appbar.lblMeldungen";
            language.Text = languageService.CurrentLanguage.Text.Split(' ')[0];
            user.Text = ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString();

            turnoff.LocalizableText = "@Appbar.Exit";
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            dashboard.LocalizableText = "@Appbar.Text3";
            moverview.LocalizableText = "@Appbar.Text3";
            handmode.LocalizableText = "@Appbar.Text3";
            recipe.LocalizableText = "@Appbar.Text3";
            parameter.LocalizableText = "@Appbar.Text3";
            protocol.LocalizableText = "@Appbar.Text3";
            logs.LocalizableText = "@Appbar.Text3";
            betriebstunden.LocalizableText = "@Appbar.Text3";
            usermanager.LocalizableText = "@Appbar.Text3";
            diagnose.LocalizableText = "@Appbar.Text3";
            language.Text = "";
            user.Text = "";
            turnoff.LocalizableText = "@Appbar.Text3";
        }

        private void Moverview_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_MainView");
        }

        private void Handmode_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "Handmenu_DB");
        }

        private void Recipe_Click(object sender, RoutedEventArgs e)
        {
            Recipe_PN R_PN = (Recipe_PN)iRS.GetView("Recipe_PN");

            if (iRS.GetCurrentViewName("MainRegion") == "Recipe_PN")
            {
                if (R_PN.pn_recipe.SelectedPanoramaRegionIndex == 0)
                {
                    R_PN.pn_recipe.ScrollNext(true);
                }
                else
                {
                    R_PN.pn_recipe.ScrollPrevious(true);
                }
            }
            else
            {
                ApplicationService.SetView("MainRegion", "Recipe_PN");
            }
        }

        private void Parameter_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "Parameter_DB");
        }

        private void Protocol_Click(object sender, RoutedEventArgs e)
        {
            Protocol_PN P_PN = (Protocol_PN)iRS.GetView("Protocol_PN");
            if (iRS.GetCurrentViewName("MainRegion") == "Protocol_PN")
            {
                if (P_PN.pn_protocol.SelectedPanoramaRegionIndex == 0)
                {
                    P_PN.pn_protocol.ScrollNext(true);
                }
                else
                {
                    P_PN.pn_protocol.ScrollPrevious(true);
                }
            }
            else
            {
                ApplicationService.SetView("MainRegion", "Protocol_PN");
            }
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "LoggingView");
        }

        private void Betriebstunden_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "Betriebstunden_DB");
        }

        private void Usermanager_Click(object sender, RoutedEventArgs e)
        {
            User_PN U_PN = (User_PN)iRS.GetView("User_PN");
            if (iRS.GetCurrentViewName("MainRegion") == "User_PN")
            {
                if (U_PN.pn_benutzerverwaltung.SelectedPanoramaRegionIndex == 0)
                {
                    U_PN.pn_benutzerverwaltung.ScrollNext(true);
                }
                else
                {
                    U_PN.pn_benutzerverwaltung.ScrollPrevious(true);
                }
            }
            else
            {
                ApplicationService.SetView("MainRegion", "User_PN");
            }
        }

        private void Diagnose_Click(object sender, RoutedEventArgs e)
        {
            Diagnose_PN D_PN = (Diagnose_PN)iRS.GetView("Diagnose_PN");
            if (iRS.GetCurrentViewName("MainRegion") == "Diagnose_PN")
            {
                if (D_PN.pn_diagnose.SelectedPanoramaRegionIndex == 0)
                {
                    D_PN.pn_diagnose.ScrollNext(true);
                }
                else
                {
                    D_PN.pn_diagnose.ScrollPrevious(true);
                }
            }
            else
            {
                ApplicationService.SetView("MainRegion", "Diagnose_PN");
            }
        }

        private void Language_Click(object sender, RoutedEventArgs e)
        {
            

            if (languageService.CurrentLCID == 1031)
            {
                languageService.ChangeLanguageAsync(1033);
            }
            else
            {
                languageService.ChangeLanguageAsync(1031);
            }
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                language.Text = languageService.CurrentLanguage.Text.Split(' ')[0];
            }
            
        }

    }
}

