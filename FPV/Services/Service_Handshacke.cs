using HMI.Interfaces;
using HMI.Views.MessageBoxRegion;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Recipe;

namespace HMI.Services
{
    [ExportService(typeof(IHandshacke))] 
    [Export(typeof(IHandshacke))]
    public class Service_Handshacke : ServiceBase, IHandshacke
    {
        IVariableService VS;
        IVariable DataToPC ;

        BackgroundWorker loadMR;


        public Service_Handshacke()
        {
            if (ApplicationService.IsInDesignMode)
                return;
        }

        private void DataToPC_Change(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && bool.Parse(e.Value.ToString()))
            {
                loadMR.RunWorkerAsync();          
            }
            
        }

        private void W1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                ApplicationService.SetView("TouchpadRegion", "WaitScreen", 1);
            });
            
            try
            {
                LoadFromFIleTOPRocessMRAsync();
            }
            catch {  }
        }

        private async void LoadFromFIleTOPRocessMRAsync()
        {
            string MR_Name = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.MR.Header.Maschinenprogramm#STRING20").ToString();

            IRecipeClass T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            var LoadResult =(await T.LoadFromFileToProcessAsync(MR_Name));
            if (LoadResult.Result == SendRecipeResult.Succeeded)
            {
                Task taskA = new Task(async () =>
                {
                    await Task.Delay(500); 
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.Recipe loaded", true);
                });

                taskA.Start();

            }
            else
            {
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.Recipe not loaded", true);
                string outputtext = "\n";
                foreach (var a in LoadResult.ErrorParams)
                {
                    outputtext += a.Member.Replace("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.","") + " -> " + a.Error + "\n";
                }

                new MessageBoxTask(outputtext, "@RecipeSystem.Results.Text2", MessageBoxIcon.Error);
            }

            Task obTask = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1500);
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    ApplicationService.SetView("TouchpadRegion", "EmptyView");
                });
            });

        }

        #region OnProject

  
        // Hier stehen noch keine VisiWin Funktionen zur Verfügung
        protected override void OnLoadProjectStarted()
        {
            base.OnLoadProjectStarted();
        }

        // Hier kann auf die VisiWin Funktionen zugegriffen werden
        protected override void OnLoadProjectCompleted()
        {
            VS = ApplicationService.GetService<IVariableService>();
            DataToPC = VS.GetVariable("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.MR requested");
            DataToPC.Change += DataToPC_Change;

            loadMR = new BackgroundWorker();
            loadMR.DoWork += W1_DoWork;

            base.OnLoadProjectCompleted();
        }

        // Hier stehen noch die VisiWin Funktionen zur Verfügung
        protected override void OnUnloadProjectStarted()
        {
            base.OnUnloadProjectStarted();
        }

        // Hier sind keine VisiWin Funktionen mehr verfügbar. Bei C/S ist die Verbindung zum Server schon getrennt.
        protected override void OnUnloadProjectCompleted()
        {
            base.OnUnloadProjectCompleted();
        }

        #endregion

    }
}
