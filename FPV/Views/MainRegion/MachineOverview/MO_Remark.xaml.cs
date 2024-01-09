using HMI.Module;
using HMI.Views.MessageBoxRegion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

	[ExportView("MO_Remark")]
	public partial class MO_Remark : VisiWin.Controls.View
	{
        public MO_Remark()
		{
			this.InitializeComponent();
        }
        private readonly ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();

        string MR;
        string Charge;
        string Charge_Id;
        string WindowType;
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "EmptyView");
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                remark.Value = "";
                MR = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Maschinenprogramm#STRING20").ToString();
                Charge = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Charge#STRING20").ToString();
                //string Barcode = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Barcode#STRING20").ToString();

                Dictionary<string, object> r_val = GetRecipeData(MR);
                string lgo = r_val["Recipe.MR.LGO"].ToString();
                string alo = r_val["Recipe.MR.ALO"].ToString();
                string nio = r_val["Recipe.MR.NIO"].ToString();


           
                DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge + "';")).DB_Output(); 
                if (temp.Rows.Count > 0)
                {
                    Charge_Id = temp.Rows[0].ItemArray[0].ToString();
                }
                else { Charge_Id = "0"; }
          

                WindowType = ApplicationService.ObjectStore["MO_Remark_KEY"].ToString();
                ApplicationService.ObjectStore.Remove("MO_Remark_KEY");
            }
        }

        private Dictionary<string, object> GetRecipeData(string RecipeName)
        {
            IRecipeClass MachineRecipe = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            if (MachineRecipe.IsExistingRecipeFile(RecipeName))
            {
                Dictionary<string, object> r_val = MachineRecipe.GetRecipeFile(RecipeName).GetValues();
                return r_val;
            }
            else { return null; }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (remark.Value.ToString().Length > 5)
            {
                DataTable DT = (new localDBAdapter("SELECT Id FROM Remarks WHERE Charge_Id = " + Charge_Id + ";")).DB_Output();

                string WT = "";
                if (WindowType == "W")
                {
                    WT = textService.GetText("@StatusView.Text51");
                }
                else
                {
                    WT = textService.GetText("@StatusView.Text53");
                }
                bool result = (new localDBAdapter("INSERT INTO Remarks (Charge_Id, Text, Comment, User) VALUES ('" + Charge_Id + "','" + WT + " : " + remark.Value + "','" + WT + "','" + ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() + "')")).DB_Input();


                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Freigabe Nacharbeite oder NIO Charge", true);

                Task.Run(() =>
                {
                    Thread.Sleep(250);
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Allgemein.Freigabe Nacharbeite oder NIO Charge", false);
                });

                string txt = textService.GetText("@Datapicker.Text1") + " : " + Charge + " ||| " + textService.GetText("@Datapicker.Text2") + " : " + MR + " ||| -> " + WT + " :  " + remark.Value;
                this.loggingService.Log("Service", "RackRelease", txt, FastDateTime.Now);

                ApplicationService.SetView("DialogRegion", "EmptyView");
                ApplicationService.SetView("MessageBoxRegion", "EmptyView");
            }
            else
            {
                new MessageBoxTask("@Remark.Text2", "@Remark.Text3", MessageBoxIcon.Exclamation);
            }
        }

        private string GetDataTimeNowToFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}