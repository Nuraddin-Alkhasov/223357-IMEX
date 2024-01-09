using HMI.Views.DialogRegion;
using HMI.Views.MessageBoxRegion;
using System;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.Helper;
using VisiWin.Language;
using VisiWin.Logging;
using VisiWin.Recipe;

namespace HMI.Views.MainRegion.MachineOverview
{

    [ExportView("MO_Status")]
	public partial class MO_Status : VisiWin.Controls.View
	{
        private readonly ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();
        ILanguageService textService = ApplicationService.GetService<ILanguageService>();

        public MO_Status()
		{
			this.InitializeComponent();
        }
       

        private void Mr_ValueChanged(object sender, VisiWin.DataAccess.VariableEventArgs e)
        {
            Task obTask = Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
                    if (T.IsExistingRecipeFile(mr.Value.ToString()))
                    {
                        descr.Value = T.GetRecipeFile(mr.Value.ToString()).Description;
                    }
                    else
                    {
                        descr.Value = "";
                    }
                });
            });
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_Status_Recipe", mr.Value);
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           if (!this.IsVisible)
           {
                new SP();
           }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MessageBoxRegion", "MO_StatusPD");
        }
    }
}