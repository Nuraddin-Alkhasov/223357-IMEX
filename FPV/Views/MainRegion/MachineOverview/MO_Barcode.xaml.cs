using HMI.Module;
using HMI.Views.MessageBoxRegion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;

namespace HMI.Views.MainRegion.MachineOverview
{

	[ExportView("MO_Barcode")]
	public partial class MO_Barcode : VisiWin.Controls.View
    {
        private readonly VisiWin.Logging.ILoggingService loggingService;
        //IVariableService VS;
        //IVariable newDataV;

        public MO_Barcode()
		{
			this.InitializeComponent();
            this.loggingService = ApplicationService.GetService<ILoggingService>();
        }
 
        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                barcode.Value = 0;
                string temp = "1";//ApplicationService.GetVariableValue("  ").ToString();
                if (temp == "")
                {
                    HeaderText.LocalizableText = "@StatusView.Text46";
                }
                else
                {
                    HeaderText.LocalizableText = "@StatusView.Text47";
                }
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (barcode.Value == 0)
            {
                MessageBoxView.Show("@Logging.Barcode.Text1", "@Logging.Barcode.Text2", MessageBoxButton.OK, icon: MessageBoxIcon.Exclamation);
            }
            else
            {
                if (MessageBoxView.Show("@Logging.Barcode.Text5", "@Logging.Barcode.Text2", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.Yes)
                {
                    ApplicationService.SetVariableValue(" ", 1);
                    ILanguageService textService = ApplicationService.GetService<ILanguageService>();
                    string txt;
                    if (status.IsChecked == true)
                    {
                        txt = textService.GetText("@Logging.Barcode.Text4") + " " + barcode.Value.ToString() + " " + textService.GetText("@Logging.Barcode.Text3");
                    }
                    else
                    {
                        txt = textService.GetText("@Logging.Barcode.Text6") + " " + "RACK NUMBER VAR" + " " + textService.GetText("@Logging.Barcode.Text7");
                    }
                    this.loggingService.Log("Barcode", "Scan", txt, FastDateTime.Now);
                    ApplicationService.SetView("DialogRegion", "EmptyView");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxView.Show("@Logging.Barcode.Text8", "@Logging.Barcode.Text2", MessageBoxButton.YesNo, icon: MessageBoxIcon.Question) == MessageBoxResult.Yes)
            {
                ApplicationService.SetVariableValue(" ", 1);
                ILanguageService textService = ApplicationService.GetService<ILanguageService>();
                string txt = textService.GetText("@Logging.Barcode.Text6") + " " + "RACK NUMBER VAR" + " " + textService.GetText("@Logging.Barcode.Text7");
                this.loggingService.Log("Barcode", "Scan", txt, FastDateTime.Now);
                ApplicationService.SetView("DialogRegion", "EmptyView");
            }
        }


        //private void NewDataV_Change(object sender, VariableEventArgs e)
        //{
        //    if (e.Value != e.PreviousValue)
        //    {
        //        if (this.IsVisible && newDataV.Value.ToString() != "")
        //        {
        //            if (ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() != "")
        //            {
        //                int value=0;
        //                if (int.TryParse(newDataV.Value.ToString(), out value))
        //                barcode.Value = value;
        //            }
        //            else
        //            {
        //                new MessageBoxTask("@Datenauswahl.Text13", "@Datenauswahl.Text15", MessageBoxIcon.Warning);
        //            }
        //        }
        //        ApplicationService.SetVariableValue("DataPicker.DatafromScanner", "");
        //    }
        //}
    }
}