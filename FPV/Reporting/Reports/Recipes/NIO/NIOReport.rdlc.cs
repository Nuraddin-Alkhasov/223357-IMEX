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
    internal class NIOReport : Report
    {
        public static readonly string DefaultExportPath = @"Reports\Recipes\NIO\NIOReport";
        public static readonly string FileNamePrefix = "NIOReport";
        public static readonly string Path = @"Reporting\Reports\Recipes\NIO\NIOReport.rdlc";

        public static async Task<ReportConfiguration> GetReportConfiguration(string Data)
        {
            //Erzeugen und Befüllen des DataSets
            var ds_NIO = new RecipeParameterDataTable();
            await ds_NIO.FillWithAsync("NIO", Data);
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
            config.DataSources.Add(new ReportDataSource("NIO", ds_NIO as DataTable));
            //ReportParameter in die Konfiguration einfügen
            config.ReportParameters.Add(new ReportParameter(Parameters.NIOName, Data));
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
                                                                                             new ParameterInfo("NIOLabel", "@Reporting.Reports.NIO")
                                                                                         };
            public static readonly string NIOName = "NIOName";
        }
    }
}