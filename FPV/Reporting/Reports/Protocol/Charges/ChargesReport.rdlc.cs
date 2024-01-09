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
    internal class ChargesReport : Report
    {
        public static readonly string DefaultExportPath = @"Reports\Protocol\Charges\ChargesReport";
        public static readonly string FileNamePrefix = "ChargesReport";
        public static readonly string Path = @"Reporting\Reports\Protocol\Charges\ChargesReport.rdlc";

        public static async Task<ReportConfiguration> GetReportConfiguration()
        {
            await Task.Run(() => { });
            //Erzeugen und Befüllen des DataSets
            ProtocolAdapter adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
          //  DataTable Charges = (new localDBAdapter(SQLquery.Replace("datetime(Orders.TimeStamp)", "TimeStamp"))).DB_Output();
            string Q = "Select Charges.Id, Charges.Charge, Charges.MR_Id, MachineRecipes.MR, Charges.Runs, Charges.Status, Charges.Error, Start, End,  Charges.User From Charges INNER JOIN MachineRecipes ON Charges.MR_Id = MachineRecipes.ID WHERE Charges.Id = -1";

            if (adapter.Charges.Count >= 1)
            {
                Q = "Select Charges.Id, Charges.Charge, Charges.MR_Id, MachineRecipes.MR, Charges.Runs, Charges.Status, Charges.Error, Start, End,  Charges.User From Charges INNER JOIN MachineRecipes ON Charges.MR_Id = MachineRecipes.ID";

                for (int i = 0; i < adapter.Charges.Count; i++)
                {
                    if (i == 0)
                    {
                        Q += " WHERE Charges.Id = " + adapter.Charges[0].Id.ToString();
                    }
                    else
                    {

                    }
                    Q += " OR Charges.Id = " + adapter.Charges[i].Id.ToString();
                }
            }


            DataTable Charges = (new localDBAdapter(Q)).DB_Output();

            var config = new ReportConfiguration
            {
                //Standard Konfiguration des Reports übertragen
                ReportPath = Path,
                DefaultExportPath = DefaultExportPath,
                FileNamePrefix = FileNamePrefix
            };
            //DataSet in die Konfiguration einfügen

            config.DataSources.Add(new ReportDataSource("Charges", Charges));

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
                                                                                             new ParameterInfo("EndLabel", "@Protocol.Text15"),
                                                                                             new ParameterInfo("ChargeLabel", "@Protocol.Text13"),
                                                                                             new ParameterInfo("RunLabel", "@Protocol.Text34"),
                                                                                             new ParameterInfo("ErrorLabel", "@Protocol.Text40"),
                                                                                             new ParameterInfo("StartLabel", "@Protocol.Text14"),
                                                                                              new ParameterInfo("ChargesLabel", "@Protocol.Text33"),
                                                                                                new ParameterInfo("MRLabel", "@Protocol.Text3"),
                                                                                                 new ParameterInfo("UserLabel", "@Protocol.Text4")
                                                                                         };
        }
    }
}