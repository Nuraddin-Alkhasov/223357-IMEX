using HMI.Interfaces;
using HMI.Module;
using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.Services
{
    [ExportService(typeof(IPData))] 
    [Export(typeof(IPData))]
    public class Service_IPData : ServiceBase, IPData
    {

        public Service_IPData()
        {
            
            if (ApplicationService.IsInDesignMode)
                return;
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

            //SetUpTimer(new TimeSpan(00, 00, 00));
            //SomeMethodRunsAt();
           // TotalTime = new Stopwatch();
            //VS = ApplicationService.GetService<IVariableService>();
            //Idle = VS.GetVariable("IMEX.PLC.Blocks.HMI.00 PC.DB PC Kommunikation.Data from PC.MR requested");
            //Idle.Change += Idle_Change;
            
            base.OnLoadProjectCompleted();
        }
       // Stopwatch TotalTime;
        private void Idle_Change(object sender, VariableEventArgs e)
        {
            //if (e.Value != e.PreviousValue && bool.Parse(e.Value.ToString()))
            //{
            //    TotalTime.Start();
            //}
            //else
            //{
            //    TotalTime.Stop();
            //    DataTable temp = (new localDBAdapter("SELECT Value FROM Config WHERE Id=6;")).DB_Output();
              //  TotalTime += DateTime.Now;
         //   }
        }

        // Hier stehen noch die VisiWin Funktionen zur Verfügung
        protected override void OnUnloadProjectStarted()
        {
            base.OnUnloadProjectStarted();
        }

        // Hier sind keine VisiWin Funktionen mehr verfügbar. Bei C/S ist die Verbindung zum Server schon getrennt.
        protected override void OnUnloadProjectCompleted()
        {
            //Stop();
            base.OnUnloadProjectCompleted();
        }

        //private void SetUpTimer(TimeSpan alertTime)
        //{
        //    //DateTime current = DateTime.Now;
        //    //TimeSpan timeToGo = alertTime - current.TimeOfDay;
        //    //if (timeToGo < TimeSpan.Zero)
        //    //{
        //    //    timeToGo = new TimeSpan(24, 5, 0) - current.TimeOfDay + alertTime;
        //    //}
        //    //timer = new Timer(x => { SomeMethodRunsAt(); }, null, timeToGo, Timeout.InfiniteTimeSpan);
           
        //}

        //private System.Threading.Timer timer;
     

        //private void SomeMethodRunsAt()
        //{
        //    //Task obTask = Task.Run(() =>
        //    //{
        //    //    Application.Current.Dispatcher.InvokeAsync((Action)delegate
        //    //    {
        //    //        DataTable temp = (new localDBAdapter("SELECT Value FROM Config WHERE Variable = 'TimePDWasReset';")).DB_Output();
        //    //        string ax= Convert.ToDateTime(temp.Rows[0].ItemArray[0].ToString()).DayOfWeek.ToString();

        //    //        if (ax!=DateTime.Now.DayOfWeek.ToString())
        //    //        {
        //    //            TotalTime.Stop();
        //    //            TotalTime.Reset();
        //    //            var b = (new localDBAdapter("UPDATE Config SET Value = '" + GetDataTimeNowToFormat() + "' WHERE Variable = 'TimePDWasReset' ;").DB_Input());
        //    //        }
        //    //    });
        //    //});
        //    //SetUpTimer(new TimeSpan(00, 00, 00));
        //}

        //private string GetDataTimeNowToFormat()
        //{
        //    return DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToLongTimeString();
        //}


        #endregion

    }

}
