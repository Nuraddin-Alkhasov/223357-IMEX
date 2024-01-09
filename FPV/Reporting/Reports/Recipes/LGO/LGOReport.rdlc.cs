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
using static HMI.Reporting.DataSets.Recipe.RecipeParameterDataSet;

namespace HMI.Reports.Recipes
{
    internal class LGOReport : Report
    {
        public static readonly string DefaultExportPath = @"Reports\Recipes\LGO\LGOReport";
        public static readonly string FileNamePrefix = "LGOReport";
        public static readonly string Path = @"Reporting\Reports\Recipes\LGO\LGOReport.rdlc";

        public static async Task<ReportConfiguration> GetReportConfiguration(string Data)
        {
            //Erzeugen und Befüllen des DataSets
            var ds_LGO = new RecipeParameterDataTable();
            await ds_LGO.FillWithAsync("LGO", Data);
            foreach (RecipeParameterRow dr in ds_LGO.Rows)
            {
                dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + "  ";
                if (dr.Name.Contains(".Std"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Stunden") + "  ";
                if (dr.Name.Contains(".Min"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Min") + "  ";
            }
            var config = new ReportConfiguration();
            //Standard Konfiguration des Reports übertragen
            config.ReportPath = Path;
            config.DefaultExportPath = DefaultExportPath;
            config.FileNamePrefix = FileNamePrefix;
            //DataSet in die Konfiguration einfügen
            config.DataSources.Add(new ReportDataSource("LGO", ds_LGO as DataTable));

            //ReportParameter in die Konfiguration einfügen
            config.ReportParameters.Add(new ReportParameter(Parameters.LGOName, Data));

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
                                                                                             new ParameterInfo("ParameterNameLabel", "@RecipeSystem.View.Grid.Parameter"),
                                                                                             new ParameterInfo("ValueLabel", "@RecipeSystem.View.Grid.Value"),
                                                                                             new ParameterInfo("LGOLabel", "@Reporting.Reports.LGO")
                                                                                         };
            public static readonly string LGOName = "LGOName";
        }
    }
}