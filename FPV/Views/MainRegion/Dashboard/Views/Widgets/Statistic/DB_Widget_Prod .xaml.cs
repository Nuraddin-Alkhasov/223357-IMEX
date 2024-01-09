using System.ComponentModel.Composition;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using System;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Data;
using HMI.Module;

namespace HMI.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardWidgetBar.xaml
    /// </summary>
    [ExportDashboardWidget("DB_Widget_Prod", "Dashboard.Text34", "@Dashboard.Text13", 1, 2)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DB_Widget_Prod : View
    {



        public DB_Widget_Prod()
        {
            InitializeComponent();
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            pieeee.ChartLegend.FontSize = 14;
            pieeee.ChartLegend.Foreground = Brushes.White;
            DataContext = this;
            DataTable temp = (new localDBAdapter("SELECT Status FROM Charges WHERE Start >= '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' AND Start<='" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59';")).DB_Output();
            if (temp.Rows.Count > 0)
            {
                double ok = 0;
                double rework = 0;
                double nok = 0;
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    switch (temp.Rows[i].ItemArray[0].ToString())
                    {
                        case "1":
                            ok++;
                            break;
                        case "2":
                            rework++;
                            break;
                        case "3":
                            nok++;
                            break;

                    }
                }

                pieeee.Series[0].Values[0] = ok;
                pieeee.Series[1].Values[0] = rework;
                pieeee.Series[2].Values[0] = nok;
            }
            else
            {
                pieeee.Series[0].Values[0] = Convert.ToDouble(0);
                pieeee.Series[1].Values[0] = Convert.ToDouble(0);
                pieeee.Series[2].Values[0] = Convert.ToDouble(0);
            }
            
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
