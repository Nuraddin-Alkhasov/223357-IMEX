using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Reporting.WinForms;
using VisiWin.ApplicationFramework;
using VisiWin.Reporting.Components;

using Report = HMI.Reporting.Report;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.Recipe;
using HMI.Reporting;
using HMI.Reporting.DataSets.Recipe;
using HMI.Views.MainRegion.Protocol;
using HMI.Module;

namespace HMI.Reports.Recipes
{
    internal class ChargeReport : Report
    {
        public static readonly string DefaultExportPath = @"Reports\Protocol\Charges\ChargeReport";
        public static readonly string FileNamePrefix = "ChargeReport";
        public static readonly string Path = @"Reporting\Reports\Protocol\Charge\ChargeReport.rdlc";

        public static async Task<ReportConfiguration> GetReportConfiguration()
        {
            await Task.Run(() => { });
            //Erzeugen und Befüllen des DataSets



            var config = new ReportConfiguration
            {
                //Standard Konfiguration des Reports übertragen
                ReportPath = Path,
                DefaultExportPath = DefaultExportPath,
                FileNamePrefix = FileNamePrefix
            };
            //DataSet in die Konfiguration einfügen
            var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
            string q1 = "SELECT * FROM MachineRecipes WHERE Id = -1";
            string q3 = "SELECT * FROM Charges WHERE Id =  -1";
            string q4 = "SELECT * FROM Errors WHERE Charge_Id =  -1";
            string q5 = "SELECT * FROM Remarks WHERE Charge_Id = -1";
            string q6 = "SELECT * FROM Runs WHERE Charge_Id = -1";
            string q7 = "SELECT * FROM Runs WHERE Charge_Id = -1";
            string q8 = "Select * FROM Soll WHERE Id = -1";
            string q9 = "Select * FROM Soll WHERE Id = -1";
            string q10 = "Select * FROM Ist WHERE Id = -1";
            string q11 = "Select * FROM Ist WHERE Id = -1";
            if (adapter.SelectedCharge != null)
            {
                q1 = "SELECT * FROM MachineRecipes WHERE Id = " + adapter.SelectedCharge.MR_ID.ToString() + ";";
                q3 = "SELECT * FROM Charges WHERE Id = " + adapter.SelectedCharge.Id.ToString() + ";";
                q4 = "SELECT * FROM Errors WHERE Charge_Id = " + adapter.SelectedCharge.Id.ToString() + ";";
                q5 = "SELECT * FROM Remarks WHERE Charge_Id = " + adapter.SelectedCharge.Id.ToString() + ";";
                q6 = "SELECT * FROM Runs WHERE Charge_Id = " + adapter.SelectedCharge.Id.ToString() + " AND Run = 1;";
                q7 = "SELECT * FROM Runs WHERE Charge_Id = " + adapter.SelectedCharge.Id.ToString() + " AND Run = 2;";

                switch (adapter.SelectedCharge.RunList.Count)
                {
                    case 1:
                        q8 = "SELECT * FROM Soll WHERE Id = " + adapter.SelectedCharge.RunList[0].SollId.ToString() + ";";
                        q10 = "SELECT * FROM Ist WHERE Id = " + adapter.SelectedCharge.RunList[0].IstId.ToString() + ";";
                        break;
                    case 2:
                        q8 = "SELECT * FROM Soll WHERE Id = " + adapter.SelectedCharge.RunList[0].SollId.ToString() + ";";
                        q10 = "SELECT * FROM Ist WHERE Id = " + adapter.SelectedCharge.RunList[0].IstId.ToString() + ";";
                        q9 = "SELECT * FROM Soll WHERE Id = " + adapter.SelectedCharge.RunList[1].SollId.ToString() + ";";
                        q11 = "SELECT * FROM Ist WHERE Id = " + adapter.SelectedCharge.RunList[1].IstId.ToString() + ";";
                        break;
                }



            }

            config.DataSources.Add(new ReportDataSource("MachineRecipes", (new localDBAdapter(q1)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Charges", (new localDBAdapter(q3)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Errors", (new localDBAdapter(q4)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Remarks", (new localDBAdapter(q5)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Run1", (new localDBAdapter(q6)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Run2", (new localDBAdapter(q7)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Soll1", (new localDBAdapter(q8)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Soll2", (new localDBAdapter(q9)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Ist1", (new localDBAdapter(q10)).DB_Output()));
            config.DataSources.Add(new ReportDataSource("Ist2", (new localDBAdapter(q11)).DB_Output()));

            //Lokalisierbare ReportParameter in die Konfiguration einfügen
            foreach (var paraInfo in Parameters.LocalizableParameter)
            {
                config.ReportParameters.Add(new ReportParameter(paraInfo.Name, ApplicationService.GetText(paraInfo.LocaliableString)));
            }

            return config;
        }

        private class Parameters
        {
            public static readonly IEnumerable<ParameterInfo> LocalizableParameter = new Collection<ParameterInfo>
                                                                                         {
                                                                                             new ParameterInfo("Data1Label", "@Protocol.Text2"),
                                                                                             new ParameterInfo("MRLabel", "@Protocol.Text3"),
                                                                                             new ParameterInfo("LGOLabel", "@Protocol.Text8"),
                                                                                             new ParameterInfo("ALOLabel", "@Protocol.Text7"),
                                                                                             new ParameterInfo("NIOLabel", "@Protocol.Text9"),
                                                                                             new ParameterInfo("UserLabel", "@Protocol.Text4"),
                                                                                             new ParameterInfo("StartLabel", "@Protocol.Text14"),
                                                                                             new ParameterInfo("EndLabel", "@Protocol.Text15"),
                                                                                             new ParameterInfo("ChargeLabel", "@Protocol.Text13"),
                                                                                             new ParameterInfo("LGOALabel", "@Protocol.Text16"),
                                                                                             new ParameterInfo("ALOALabel", "@Protocol.Text17"),
                                                                                             new ParameterInfo("LGONRLabel", "@Protocol.Text20"),
                                                                                             new ParameterInfo("ALONRLabel", "@Protocol.Text21"),
                                                                                             new ParameterInfo("TimeLabel", "@Protocol.Text1"),
                                                                                             new ParameterInfo("TextLabel", "@Protocol.Text11"),
                                                                                             new ParameterInfo("RunNrLabel", "@Protocol.Text25"),
                                                                                             new ParameterInfo("SollLabel", "@StatusView.Text50"),
                                                                                             new ParameterInfo("IstLabel", "@StatusView.Text49"),
                                                                                             new ParameterInfo("AbschreckenLabel", "@RecipeSystem.View.Grid.Text18"),
                                                                                             new ParameterInfo("NrLabel", "@Protocol.Text29"),
                                                                                             new ParameterInfo("TempLabel", "@Protocol.Text28"),
                                                                                             new ParameterInfo("HLabel", "@Protocol.Text30"),
                                                                                             new ParameterInfo("WTLabel", "@Protocol.Text31"),
                                                                                             new ParameterInfo("TDOG", "@RecipeSystem.View.Grid.Text14"),
                                                                                             new ParameterInfo("TDUG", "@RecipeSystem.View.Grid.Text15"),
                                                                                             new ParameterInfo("UZOG", "@RecipeSystem.View.Grid.Text16"),
                                                                                             new ParameterInfo("UZUG", "@RecipeSystem.View.Grid.Text17"),
                                                                                             new ParameterInfo("WT", "@RecipeSystem.View.Grid.Text13"),
                                                                                             new ParameterInfo("TZ", "@RecipeSystem.View.Grid.Text19"),
                                                                                             new ParameterInfo("VZ", "@RecipeSystem.View.Grid.Text20"),
                                                                                             new ParameterInfo("AZ", "@RecipeSystem.View.Grid.Text21"),
                                                                                             new ParameterInfo("DZ", "@RecipeSystem.View.Grid.Text22"),
                                                                                             new ParameterInfo("Remark", "@Protocol.Text23")
                                                                                         };
        }
    }
}