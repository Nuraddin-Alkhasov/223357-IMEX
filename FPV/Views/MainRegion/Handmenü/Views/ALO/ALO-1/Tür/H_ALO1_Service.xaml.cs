﻿using HMI.Views.MessageBoxRegion;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;

namespace HMI.Handmenu.ALO
{
    /// <summary>
    /// Interaction logic for KeyAndSwitchView.xaml
    /// </summary>
    [ExportView("H_ALO1_Service")]

    public partial class H_ALO1_Service : View
    {
        private readonly VisiWin.Logging.ILoggingService loggingService;

        public H_ALO1_Service()
        {
            this.InitializeComponent();
            this.loggingService = ApplicationService.GetService<ILoggingService>();
        }

        private void Key_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBoxView.Show("@HandMenu.Text1", "@HandMenu.Text2", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.Yes)
            {
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.13 ALO 1.Türe.DB ALO 1 Türe HMI.PC.Antireb.Vorwahl Referenz", 1);
                ILanguageService textService = ApplicationService.GetService<ILanguageService>();

                string txt = textService.GetText("@Logging.Service.Text1");
                this.loggingService.Log("Service", "New Reference", txt, FastDateTime.Now);
            }
        }
    }
}