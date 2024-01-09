using HMI.Interfaces;
using System.ComponentModel.Composition;
using VisiWin.ApplicationFramework;
using VisiWin.Recipe;

namespace HMI.Services
{
    [ExportService(typeof(IIni))] 
    [Export(typeof(IIni))]
    public class Service_Ini : ServiceBase, IIni
    {


        public Service_Ini()
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
            InitializeRecipe();
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

        private void InitializeRecipe()
        {
            
            IRecipeClass T;
            T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("ALO");
            T.StartEdit();
            T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("LGO");
            T.StartEdit();
            T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("NIO");
            T.StartEdit();
            T = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            T.StartEdit();
        }
        #endregion

    }
}
