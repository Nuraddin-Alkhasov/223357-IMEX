using System.ComponentModel.Composition;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using VisiWin.DataAccess;
using System.Collections.Generic;
using System.Windows.Media;
using System.Data;
using HMI.Module;

namespace HMI.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardWidgetBar.xaml
    /// </summary>
    [ExportDashboardWidget("DB_Widget_Prod2", "Dashboard.Text33", "@Dashboard.Text13", 1, 2)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DB_Widget_Prod2 : View
    {
      
      

        public DB_Widget_Prod2()
        {
            InitializeComponent();
            DataTable d1 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d2 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d3 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d4 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d5 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d6 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            DataTable d7 = (new localDBAdapter("SELECT * From Charges WHERE Start >= '" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title ="Week Production",
                    Values = new ChartValues<double>
                    {
                         d7.Rows.Count,
                         d6.Rows.Count,
                         d5.Rows.Count,
                         d4.Rows.Count,
                         d3.Rows.Count,
                         d2.Rows.Count,
                         d1.Rows.Count
                    }
                }
            };

            Labels = new[] {

                DateTime.Now.AddDays(-6).DayOfWeek.ToString(),
                DateTime.Now.AddDays(-5).DayOfWeek.ToString(),
                DateTime.Now.AddDays(-4).DayOfWeek.ToString(),
                DateTime.Now.AddDays(-3).DayOfWeek.ToString(),
                DateTime.Now.AddDays(-2).DayOfWeek.ToString(),
                DateTime.Now.AddDays(-1).DayOfWeek.ToString(),
                DateTime.Now.DayOfWeek.ToString()};
            
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
