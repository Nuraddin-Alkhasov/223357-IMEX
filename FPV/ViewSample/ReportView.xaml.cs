using VisiWin.ApplicationFramework;
using VisiWin.Controls;

namespace VisiWin.Reporting.Components.ViewSample
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    [ExportView("ReportView")]
    public partial class ReportView : View
    {
        private static bool isOpen;

        public ReportView()
        {
            this.InitializeComponent();
        }

        public static void Show(ReportConfiguration reportConfiguration)
        {
            if (isOpen)
            {
                return;
            }

            isOpen = true;

            var adapter = ApplicationService.GetAdapter("ReportViewAdapter") as ReportViewAdapter;

            if (adapter != null)
            {
                adapter.ReportConfiguration = reportConfiguration;
            }

            isOpen = false;
        }
    }
}