using HMI.Interfaces;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;

namespace HMI.Services
{
    [ExportService(typeof(IBarcode))]
    [Export(typeof(IBarcode))]
    public class Service_Barcode : ServiceBase, IBarcode
    {
        HMI.Services.Custom_Objects.Cognex C;
        public Service_Barcode()
        {
            if (ApplicationService.IsInDesignMode)
                return;
        }

       

        #region Device Events



        #endregion
        #region OnProject


        // Hier stehen noch keine VisiWin Funktionen zur Verfügung
        protected override void OnLoadProjectStarted()
        {
            base.OnLoadProjectStarted();
        }


        // Hier kann auf die VisiWin Funktionen zugegriffen werden
        protected override void OnLoadProjectCompleted()
        {
            C = new Custom_Objects.Cognex();
            C.OpenConnection();
               
            base.OnLoadProjectCompleted();
        }

    

        // Hier stehen noch die VisiWin Funktionen zur Verfügung
        protected override void OnUnloadProjectStarted()
        {
            if (C != null)
                C.CloseConnection();
            base.OnUnloadProjectStarted();
        }

        // Hier sind keine VisiWin Funktionen mehr verfügbar. Bei C/S ist die Verbindung zum Server schon getrennt.
        protected override void OnUnloadProjectCompleted()
        {
            base.OnUnloadProjectCompleted();
        }

        public void OpenConnection()
        {
            C.OpenConnection();
        }

        public void CloseConnection()
        {
            C.CloseConnection();
        }

        public string CheckConnection()
        {
            return C.CheckConnection();
        }





        #endregion
    }
}
