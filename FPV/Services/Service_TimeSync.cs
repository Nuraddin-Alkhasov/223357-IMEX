using HMI.Interfaces;
using HMI.Module;
using HMI.Services.Custom_Objects;
using HMI.Views.MessageBoxRegion;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.Services
{
    [ExportService(typeof(ITimeSync))]
    [Export(typeof(ITimeSync))]
    public class Service_TimeSync : ServiceBase, ITimeSync
    {
        public Service_TimeSync()
        {
            if (ApplicationService.IsInDesignMode)
                return;
        }

        #region OnProject


        // Hier stehen noch keine VisiWin Funktionen zur Verfügung
        protected override void OnLoadProjectStarted()
        {
            (new VWSafeStart()).DoWork();

            base.OnLoadProjectStarted();
        }

        // Hier kann auf die VisiWin Funktionen zugegriffen werden
        protected override void OnLoadProjectCompleted()
        {
            Time = new TimeSpan(9, 00, 00);
            Start();
            DoLifeBit();
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
            Stop();
            base.OnUnloadProjectCompleted();
        }

        private static Timer timer;

        public TimeSpan Time { get; set; }
        public bool IsRunning { get; set; }

        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                timeToGo = new TimeSpan(24, 0, 0) - current.TimeOfDay + alertTime;
            }
            timer = new Timer(x => { this.Start(); }, null, timeToGo, Timeout.InfiniteTimeSpan);

        }

        public void Start()
        {
            SetUpTimer(Time);
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Uhrzeiten von PC.Aktuelles Datum und Zeit#DATE_AND_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Uhrzeiten von PC.Übernehmen", true);

            IsRunning = true;
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }

            IsRunning = false;
        }

        private void DoLifeBit()
        {
            Task obTask = Task.Run(() =>
            {
                while (true)
                {
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Life Bit von PC",true);
                    Thread.Sleep(1000);
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Life Bit von PC", false);
                    Thread.Sleep(1000);
                }
            });

        }
       

        #endregion

    }
}
