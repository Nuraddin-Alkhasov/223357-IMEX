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
    [ExportDashboardWidget("DB_Widget_Prod4", "Dashboard.Text35", "@Dashboard.Text13", 1, 1)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DB_Widget_Prod4 : View
    {

        public DB_Widget_Prod4()
        {
            InitializeComponent();
            DataTable temp = (new localDBAdapter("SELECT * FROM Charges WHERE Start >= '" + DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND Start<='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "';")).DB_Output();
            if (temp.Rows.Count > 0)
            {
                gauge.Value = temp.Rows.Count*2;
            }
            else
            {
                gauge.Value = 0;
            }


            
        }
        
    }
}
