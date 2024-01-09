using HMI.Reporting;
using HMI.Reports.Recipes;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{
	/// <summary>
	/// Interaction logic for ProtocolPN.xaml
	/// </summary>
	[ExportView("Protocol_PN")]
	public partial class Protocol_PN : VisiWin.Controls.View
	{
        IRegionService iRS = ApplicationService.GetService<IRegionService>();

        public Protocol_PN()
		{
			this.InitializeComponent();
		}

        private void Pn_protocol_SelectedPanoramaRegionChanged(object sender, VisiWin.Controls.SelectedPanoramaRegionChangedEventArgs e)
        {
            switch (pn_protocol.SelectedPanoramaRegionIndex)
            {
                case 0:
                    header.LocalizableText = "@Protocol.Text33";
                    var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
                    adapter.BW_FilterSQL(null);
                    Protocol_Charges PCs = (Protocol_Charges)iRS.GetView("Protocol_Charges");
                    PCs.dgv_charges.SelectedIndex = -1;
                    break;
                case 1:
                    header.LocalizableText = "@Protocol.Text13"; break;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var adapter = ApplicationService.GetAdapter(nameof(ReportViewAdapter)) as ReportViewAdapter;
            Func<CancellationToken, Task<ReportConfiguration>> config;

            switch (pn_protocol.SelectedPanoramaRegionIndex)
            {
                case 0:
                    config = (t) => ChargesReport.GetReportConfiguration();
                    adapter?.OpenView(config);
                    break;
                case 1:
                    config = (t) => ChargeReport.GetReportConfiguration();
                    adapter?.OpenView(config);
                    break;
              
            }
        }

        private void LayoutRoot_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                if (pn_protocol.SelectedPanoramaRegionIndex == 1)
                {
                    pn_protocol.ScrollPrevious();
                    var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
                    adapter.BW_FilterSQL(null);
                }

            }

        }
    }
}