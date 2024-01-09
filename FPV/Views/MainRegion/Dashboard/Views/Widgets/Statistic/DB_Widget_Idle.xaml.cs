using HMI.Module;
using System.ComponentModel.Composition;
using System.Data;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;


namespace HMI.Dashboard
{
    [ExportDashboardWidget("DB_Widget_Idle", "Dashboard.Text37", "@Dashboard.Text13", 1, 1)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DB_Widget_Idle : View
    {
        public DB_Widget_Idle()
        {
            this.InitializeComponent();

            DataTable temp = (new localDBAdapter("SELECT Value FROM Config WHERE Id=6;")).DB_Output();
            if (temp.Rows.Count > 0)
            {
                tbTime.Value = temp.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                tbTime.Value ="00:00:00";
            }
            
        }

    }
}