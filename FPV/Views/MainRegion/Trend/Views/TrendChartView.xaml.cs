using HMI.Views.DialogRegion;
using HMI.Views.MainRegion;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Trend;
using VisiWin.UserManagement;

namespace HMI
{
    [ExportView("TrendChartView")]
    public partial class TrendChartView : VisiWin.Controls.View
    {
        public TrendChartView()
        {
            InitializeComponent();
        }
        public TrendData Trend;
        

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", Trend.BackViewName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IUserManagementService userService = ApplicationService.GetService<IUserManagementService>();
            if (userService.CurrentUser != null && userService.CurrentUser.RightNames.Contains("Trend"))
            {
                DialogView.Show("ExportView", "@TrendSystem.Views.Text8", DialogButton.Close);
            }

        }

        private void TrendChart_Loaded(object sender, RoutedEventArgs e)
        {
            Trend = (TrendData)ApplicationService.ObjectStore.GetValue("TrendChartView_KEY");
          
            curve1.ArchiveName = curve2.ArchiveName = Trend.ArchiveName;
            curve1.TrendName = Trend.TrendName_1;
            curve1.Tag = Trend.CurveTag_1;
            curve2.TrendName = Trend.TrendName_2;
            curve2.Tag = Trend.CurveTag_2;
            head.LocalizableText = Trend.Header;

            int maxIfNull = Trend.ArchiveName.Contains("LGO") ? 550 : 420;

            ITrendService trendService = ApplicationService.GetService<ITrendService>();
            float a = (float)ApplicationService.GetVariableValue((trendService.GetArchive(Trend.ArchiveName)).GetTrend(Trend.TrendName_1).GetDefinition().TrendVariableName);
            short b = (short)ApplicationService.GetVariableValue((trendService.GetArchive(Trend.ArchiveName)).GetTrend(Trend.TrendName_2).GetDefinition().TrendVariableName);
            if (a < b)
            {
                min.Value = curve1.MinValue = curve2.MinValue = Math.Floor(a - b * 0.05 < 0 ? 0 : a - b * 0.05);
                max.Value = curve1.MaxValue = curve2.MaxValue = Math.Ceiling(b + b * 0.05) == 0 ? maxIfNull : Math.Ceiling(b + b * 0.05);

            }
            else
            {
                min.Value = curve1.MinValue = curve2.MinValue = Math.Floor(b - a * 0.05 < 0 ? 0 : b - a * 0.05);
                max.Value = curve1.MaxValue = curve2.MaxValue = Math.Ceiling(a + a * 0.05) == 0 ? maxIfNull : Math.Ceiling(a + a * 0.05);
            }

            ApplicationService.ObjectStore.Remove("TrendChartView_KEY");
        }

        #region - - - VisiWin Trends Problem Fix - - -
        // I think this is a project migration problem.
        // 1) Curve wont get online -> delayed by a thread -> shit solution!, did not found a better way,
        // somehow the visiwin trend container is not completly ready even when it is firing loaded event.
        // 2) Changing curve scale. The scale of both curves has to be the same, otherwise problems are incomming
        // 3) After changing the resolution of the curves, the curves whont get that "redraw" & "update"
        private void view_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
           //     DateTimeCurveContainer.IsOnline = false;
            }
        }
        private void curve2_Loaded(object sender, RoutedEventArgs e)
        {

           // help();
        }
        private void curve1_ScaleLayoutChanged(object sender, VisiWin.Controls.ScaleLayoutChangedEventArgs e)
        {
            if (curve1 != null && curve2 != null)
            {
                curve2.MinValue = curve1.MinValue;
                curve2.MaxValue = curve1.MaxValue;
                //DateTimeCurveContainer.DrawCurves();
                //DateTimeCurveContainer.IsOnline = false;
            }

        }
        private void resolutionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //DateTimeCurveContainer.DrawCurves();
            //DateTimeCurveContainer.IsOnline = false;
        }

        private void Button_Click_(object sender, RoutedEventArgs e)
        {
          //  help2();
        }

        private void help()
        {
            Task.Run(async () =>
            {
                //await Task.Delay(250);
                //await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                //{
                //    DateTimeCurveContainer.IsOnline = true;
                //    DateTimeCurveContainer.DrawCurves();
                //});

            });
        }

        private void help2()
        {
            Task.Run(async () =>
            {
                //await Task.Delay(250);
                //await Application.Current.Dispatcher.InvokeAsync((Action)delegate
                //{
                //    DateTimeCurveContainer.DrawCurves();
                //});

            });
        }
      

        #endregion

    }
}
