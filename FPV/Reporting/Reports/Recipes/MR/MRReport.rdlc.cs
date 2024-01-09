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
    internal class MRReport : Report
    {
        public static readonly string DefaultExportPath = @"Reports\Recipes\MR\MRReport";
        public static readonly string FileNamePrefix = "MRReport";
        public static readonly string Path = @"Reporting\Reports\Recipes\MR\MRReport.rdlc";

        public static async Task<ReportConfiguration> GetReportConfiguration(Dictionary<string, object> Data)
        {
            //Erzeugen und Befüllen des DataSets
            var ds_LGO = new RecipeParameterDataTable();
            await ds_LGO.FillWithAsync("LGO", Data["Recipe.MR.LGO"].ToString());
            foreach (RecipeParameterRow dr in ds_LGO.Rows)
            {
                dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + "  ";
                if (dr.Name.Contains(".Std"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Stunden") + "  ";
                if (dr.Name.Contains(".Min"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Min") + "  ";
            }

            var ds_ALO = new RecipeParameterDataTable();
            await ds_ALO.FillWithAsync("ALO", Data["Recipe.MR.ALO"].ToString());
            foreach (RecipeParameterRow dr in ds_ALO.Rows)
            {
                dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + "  ";
                if (dr.Name.Contains(".Std"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Stunden") + "  ";
                if (dr.Name.Contains(".Min"))
                    dr.LocalizedParameterName = ApplicationService.GetText(dr.Description) + " - " + ApplicationService.GetText("@Einheiten.Min") + "  ";
            }

            var ds_NIO = new RecipeParameterDataTable();
            await ds_NIO.FillWithAsync("NIO", Data["Recipe.MR.NIO"].ToString());
            foreach (RecipeParameterRow dr in ds_NIO.Rows)
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
            config.DataSources.Add(new ReportDataSource("ALO", ds_ALO as DataTable));
            config.DataSources.Add(new ReportDataSource("NIO", ds_NIO as DataTable));
            //ReportParameter in die Konfiguration einfügen
            config.ReportParameters.Add(new ReportParameter(Parameters.MRName, Data["Recipe.MR.Name"].ToString()));
            config.ReportParameters.Add(new ReportParameter(Parameters.LGOName, Data["Recipe.MR.LGO"].ToString()));
            config.ReportParameters.Add(new ReportParameter(Parameters.ALOName, Data["Recipe.MR.ALO"].ToString()));
            config.ReportParameters.Add(new ReportParameter(Parameters.NIOName, Data["Recipe.MR.NIO"].ToString()));
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
                                                                                             new ParameterInfo("ReportName", "@Reporting.Reports.MR"),
                                                                                             new ParameterInfo("ParameterNameLabel", "@RecipeSystem.View.Grid.Parameter"),
                                                                                             new ParameterInfo("ValueLabel", "@RecipeSystem.View.Grid.Value"),
                                                                                             new ParameterInfo("LGOLabel", "@Reporting.Reports.LGO"),
                                                                                             new ParameterInfo("ALOLabel", "@Reporting.Reports.ALO"),
                                                                                             new ParameterInfo("NIOLabel", "@Reporting.Reports.NIO"),

                                                                                         };

            public static readonly string MRName = "MRName";
            public static readonly string LGOName = "LGOName";
            public static readonly string ALOName = "ALOName";
            public static readonly string NIOName = "NIOName";

        }
    }
}