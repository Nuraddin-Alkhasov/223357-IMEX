﻿using HMI.Views.MessageBoxRegion;
using System;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.DialogRegion
{

	[ExportView("DialogView")]
	public partial class DialogView : VisiWin.Controls.View
	{
		public static bool dialogIsOpen;

		public DialogView()
		{
			this.InitializeComponent();
		}


        public static DialogResult Show(string view, string caption, DialogButton type = DialogButton.OKCancel, DialogResult defaultResult = DialogResult.OK, bool modal = true, EventHandler<DialogResultEventArgs> VerifyDialogResultFunction = null, string leftButtonText = null, string middleButtonText = null, string rightButtonText = null)
        {
           
            if (DialogView.dialogIsOpen)
            {
                ApplicationService.ObjectStore.Remove("DialogView_RESULT");
                ApplicationService.ObjectStore.Remove("DialogView_KEY");
                ApplicationService.SetView("DialogRegion", "EmptyView");

                //	fertig
                DialogView.dialogIsOpen = false;
            }
            DialogView.dialogIsOpen = true;
            //	die Antwort wird im ObjectStore erwartet, d.h. Ergebnis vorher löschen
            ApplicationService.ObjectStore.Remove("DialogView_RESULT");

            //	Verify-Funktion anmelden?
            if (VerifyDialogResultFunction != null) DialogView.VerifyDialogResultEvent += VerifyDialogResultFunction;

            //	Dialog zeigen, die Übergabeparameter kommen ebenfalls in den ObjectStore
            ApplicationService.SetView("DialogRegion", "DialogView", new DialogParams { HeaderText = caption, Content = view, Type = (InternalDialogButtons)type, DefaultResult = (InternalDialogResult)defaultResult, Icon = MessageBoxIcon.None, Modal = modal, LeftButtonText = leftButtonText, MiddleButtonText = middleButtonText, RightButtonText = rightButtonText });

            //	das Beenden des Dialogs kann verhindert werden
            object result;
            while (true)
            {
                //	jetzt warten wir, bis das Bearbeitungsergebnis im ObjectStore gespeichert wurde
                result = null;
                while ((result = ApplicationService.ObjectStore["DialogView_RESULT"]) == null && Application.Current != null) DialogAdapter.DoEvents();

                //	ObjectStore aufräumen
                ApplicationService.ObjectStore.Remove("DialogView_RESULT");
                ApplicationService.ObjectStore.Remove("DialogView_KEY");

                //	keine Verify-Funktion angemeldet, dann sind wir fertig
                if (VerifyDialogResultEvent == null) break;

                //	Instanz des Dialogs ermitteln
                IView v = null;
                IRegionService iRS = ApplicationService.GetService<IRegionService>();
                if (iRS != null) v = iRS.GetView("DialogView");

                //	jemand möchte den Dialog überprüfen und ggf. abbrechen
                DialogResultEventArgs verifyResult = new DialogResultEventArgs(view, (DialogResult)result);
                VerifyDialogResultEvent(v, verifyResult);
                if (!verifyResult.CancelDialogClosing) break;
            }

            //	Verify-Funktion abmelden
            if (VerifyDialogResultFunction != null) DialogView.VerifyDialogResultEvent -= VerifyDialogResultFunction;

      
            //	Dialog verschwinden lassen
            ApplicationService.SetView("DialogRegion", "EmptyView");

            //	fertig
            DialogView.dialogIsOpen = false;

            return (result is InternalDialogResult) ? (DialogResult)result : DialogResult.Cancel;
        }

        public static event EventHandler<DialogResultEventArgs> VerifyDialogResultEvent;

       
    }
}