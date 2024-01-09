
using HMI.Interfaces;
using HMI.Module;
using HMI.Services.Custom_Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VisiWin.Alarm;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Language;
using VisiWin.Recipe;

namespace HMI.Services
{

    [ExportService(typeof(Service_Protocol))]
    [Export(typeof(Service_Protocol))]
    class Service_Protocol : ServiceBase, IProtocol
    {

        IVariable In;
        IVariable Out;
        IVariableService VS;
        public Station LGO1 { get; private set; }
        public Station LGO2 { get; private set; }
        public Station LGO3 { get; private set; }
        public Station ALO1 { get; private set; }
        public Station ALO2 { get; private set; }
        public Station ALO3 { get; private set; }
        public Station ALO4 { get; private set; }
        public Station ALO5 { get; private set; }
        public Station ALO6 { get; private set; }
        public Station ALO7 { get; private set; }
        public Carriage Carriage { get; private set; }

        public Service_Protocol()
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
            VS = ApplicationService.GetService<IVariableService>();
            In = VS.GetVariable("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen HMI.PC.Protokollierung");
            In.Change += InChange;

            Carriage = new Carriage
            {
                VStart = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.PC.Protokollierung.Start",
                VEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.PC.Protokollierung.Ende",
                VError = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.PC.Protokollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.PC.Protokollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.PC.Protokollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                VLGOActive = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.Istwerte.Abschrecken.LGO_aktiv",
                VALOActive = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.Istwerte.Abschrecken.ALO_aktiv",
                VReworkActive = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT HMI.Istwerte.Abschrecken.NANIO_aktiv",
                StartEr = 81,
                EndEr = 544,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };

            LGO1 = new Station
            {
                VStart = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.10 LGO 1.Heizung / Ventilator.DB LGO 1 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.10 LGO 1.DB LGO 1 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType ="LGO",
                ONr="1",
                StartEr = 561,
                EndEr = 848,
                ReworkMessage= "@Protocol.Text27",
                NOKMessage= "@Protocol.Text26"
            };
            LGO2 = new Station
            {
                VStart = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.11 LGO 2.Heizung / Ventilator.DB LGO 2 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.11 LGO 2.DB LGO 2 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "LGO",
                ONr = "2",
                StartEr = 849,
                EndEr = 1152,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            LGO3 = new Station
            {
                VStart = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.12 LGO 3.Heizung / Ventilator.DB LGO 3 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.12 LGO 3.DB LGO 3 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "LGO",
                ONr = "3",
                StartEr = 849,
                EndEr = 1456,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO1 = new Station
            {
                VStart = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.13 ALO 1.Heizung / Ventilator.DB ALO 1 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.13 ALO 1.DB ALO 1 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "1",
                StartEr = 1457,
                EndEr = 1760,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO2 = new Station
            {
                VStart = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.14 ALO 2.Heizung / Ventilator.DB ALO 2 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.14 ALO 2.DB ALO 2 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "2",
                StartEr = 1761,
                EndEr = 2064,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO3 = new Station
            {
                VStart = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.15 ALO 3.Heizung / Ventilator.DB ALO 3 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.15 ALO 3.DB ALO 3 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "3",
                StartEr = 2065,
                EndEr = 2368,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO4 = new Station
            {
                VStart = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.16 ALO 4.Heizung / Ventilator.DB ALO 4 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.16 ALO 4.DB ALO 4 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "4",
                StartEr = 2369,
                EndEr = 2672,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO5 = new Station
            {
                VStart = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.17 ALO 5.Heizung / Ventilator.DB ALO 5 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.17 ALO 5.DB ALO 5 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "5",
                StartEr = 2673,
                EndEr = 2976,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO6 = new Station
            {
                VStart = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.18 ALO 6.Heizung / Ventilator.DB ALO 6 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.18 ALO 6.DB ALO 6 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "6",
                StartEr = 2977,
                EndEr = 3280,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            };
            ALO7 = new Station
            {
                VStart = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.PC.Protkollierung.Start",
                VEnd = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.PC.Protkollierung.Ende",
                VError = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.PC.Protkollierung.Sammelstoerung",
                VErrorAck = "IMEX.PLC.Blocks.HMI.DB HMI Allgemein.Gerneral.Störung Quittieren",
                VRework = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.PC.Protkollierung.Nacharbeit",
                VNOK = "IMEX.PLC.Blocks.19 ALO 7.Heizung / Ventilator.DB ALO 7 Heizung Venti HMI.PC.Protkollierung.NIO",
                VCharge = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Charge#STRING20",
                VRun = "IMEX.PLC.Blocks.19 ALO 7.DB ALO 7 Prozessdaten.Daten.Akueller Durchlauf",
                VChargeEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Charge#STRING20",
                VRunEnd = "IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Akueller Durchlauf",
                OType = "ALO",
                ONr = "7",
                StartEr = 3281,
                EndEr = 3584,
                ReworkMessage = "@Protocol.Text27",
                NOKMessage = "@Protocol.Text26"
            }; 

            Out = VS.GetVariable("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen HMI.PC.Protokollierung.Ende");
            Out.Change += OutChange;

            base.OnLoadProjectCompleted();
        }

        #region - - - Event Handlers - - -   

        private void InChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && int.Parse(e.Value.ToString()) > 0)
            {
                string MR = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Maschinenprogramm#STRING20").ToString();
                string Data1 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Charge#STRING20").ToString();
                //string Data2 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Barcode#STRING20").ToString();
                //string Data3 = "";
                string Run = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Akueller Durchlauf").ToString();
                //string Charge = "";
                string User = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.User#STRING20").ToString();

                if (VisiWin.Helper.BitHelper.GetBit((Int16)e.Value, 8))
                {
                    In.SetBit(8, false);
                    Task.Run(() => 
                    {
                        WriteCharge(MR, Data1, User, Run);
                    });
                }

                if (VisiWin.Helper.BitHelper.GetBit((Int16)e.Value, 9))
                {
                    In.SetBit(9, false);
                    Task.Run(() => WriteDateTimeEndTOCharge(Data1, Run));
                }
            }
        }

        private void OutChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                //string MR = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen ALO 7 Prozessdaten.Daten.Maschinenprogramm#STRING20").ToString();
                string Data1 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen ALO 7 Prozessdaten.Daten.Charge#STRING20").ToString();
                //string Data2 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen ALO 7 Prozessdaten.Daten.Barcode#STRING20").ToString();
                //string Data3 = "";
                string Run = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen ALO 7 Prozessdaten.Daten.Akueller Durchlauf").ToString();
                //string Charge = "";
                //string User = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Aus.DB Ausschleusen ALO 7 Prozessdaten.Daten.User#STRING20").ToString();

                if ((bool)e.Value)
                {
                    Out.Value = false;

                    Task.Run(() => WriteDateTimeEndTOCharge(Data1, Run));

                }
            }
        }
      
        #endregion

        #region - - - Private Methods - - -   

        private void WriteCharge(string _mr, string _data1, string _user, string _run)
        {
            Dictionary<string, object> r_val = GetRecipeData(_mr);
            string lgo = r_val["Recipe.MR.LGO"].ToString();
            string alo = r_val["Recipe.MR.ALO"].ToString();
            string nio = r_val["Recipe.MR.NIO"].ToString();
            string MR_ID;

            DataTable temp = (new localDBAdapter("SELECT Id FROM MachineRecipes WHERE MR = '" + _mr + "' AND LGO = '" + lgo + "' AND ALO = '" + alo + "' AND NIO = '" + nio + "'")).DB_Output();
            if (temp.Rows.Count == 0)
            {
                (new localDBAdapter("INSERT INTO MachineRecipes (MR, LGO, ALO, NIO) VALUES ('" + _mr + "','" + lgo + "','" + alo + "','" + nio + "')")).DB_Input();
                MR_ID = ((new localDBAdapter("SELECT MAX(ID) FROM MachineRecipes;")).DB_Output().Rows[0][0]).ToString();
            }
            else
            {
                MR_ID = (temp.Rows[0][0]).ToString();
            }

            temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + _data1 + "';")).DB_Output();

            if (temp.Rows.Count == 0)
            {
                (new localDBAdapter("INSERT INTO Charges (Charge, MR_Id, Runs, Status, Error, Start, User) VALUES ('" + _data1 + "'," +  MR_ID+ ",1,1,0,'" + GetDataTimeNowToFormat() + "','" + _user + "'); ")).DB_Input();
                temp = (new localDBAdapter("SELECT MAX(ID) FROM Charges")).DB_Output();
                string Charge_Id = (temp.Rows[0][0]).ToString();
                (new localDBAdapter("INSERT INTO Runs (Charge_Id, Run, Soll_Id, Start) VALUES (" + Charge_Id + "," + _run + "," + WriteSoll() + ",'" + GetDataTimeNowToFormat() + "');")).DB_Input();

            }
            else
            {
                string Charge_Id = (temp.Rows[0][0]).ToString();
                (new localDBAdapter("INSERT INTO Runs (Charge_Id, Run, Soll_Id, Start) VALUES (" + Charge_Id + "," + _run + "," + WriteSoll() + ",'" + GetDataTimeNowToFormat() + "');")).DB_Input();
                (new localDBAdapter("UPDATE Charges SET Runs = " + _run + " WHERE Id = " + Charge_Id + ";")).DB_Input();
            }        
        }

        private void WriteDateTimeEndTOCharge(string _data1,  string _run)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + _data1 + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();

                if (temp.Rows.Count != 0)
                {
                    (new localDBAdapter("UPDATE Charges SET End = '" + GetDataTimeNowToFormat() + "' WHERE Id = " + Charge_Id + ";")).DB_Input();
                    (new localDBAdapter("UPDATE Runs SET End = '" + GetDataTimeNowToFormat() + "' WHERE Charge_Id = " + Charge_Id + " AND Run = " + _run + ";")).DB_Input();
                }
            }
            
        }

        string WriteSoll()
        {
            string LGO_T1 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_1.Temperatur").ToString();
            string LGO_H1 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_1.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_1.Aufheizzeit.Min").ToString());
            string LGO_WT1 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_1.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_1.Haltezeit.Min").ToString());
            string LGO_T2 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_2.Temperatur").ToString();
            string LGO_H2 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_2.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_2.Aufheizzeit.Min").ToString());
            string LGO_WT2 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_2.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_2.Haltezeit.Min").ToString());
            string LGO_T3 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_3.Temperatur").ToString();
            string LGO_H3 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_3.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_3.Aufheizzeit.Min").ToString());
            string LGO_WT3 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_3.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_3.Haltezeit.Min").ToString());
            string LGO_T4 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_4.Temperatur").ToString();
            string LGO_H4 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_4.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_4.Aufheizzeit.Min").ToString());
            string LGO_WT4 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_4.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Heizen_4.Haltezeit.Min").ToString());
            string LGO_WT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Wartezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Wartezeit.Min").ToString());
            string LGO_TUL = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.OG_Ueberwachung.Temperaturdifferenz").ToString();
            string LGO_TULT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.OG_Ueberwachung.Zeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.OG_Ueberwachung.Zeit.Min").ToString());
            string LGO_TLL = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.UG_Ueberwachung.Temperaturdifferenz").ToString();
            string LGO_TLLT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.UG_Ueberwachung.Zeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.UG_Ueberwachung.Zeit.Min").ToString());
            string LGO_ATT = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Abschrecken.Transportzeit").ToString();
            string LGO_AD = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Abschrecken.VZT_Abschrecken").ToString();
            string LGO_AT = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Abschrecken.Abschreckzeit").ToString();
            string LGO_AFS = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.LGO.Abschrecken.DZ_Zuluft").ToString();

            string ALO_T1 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_1.Temperatur").ToString();
            string ALO_H1 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_1.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_1.Aufheizzeit.Min").ToString());
            string ALO_WT1 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_1.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_1.Haltezeit.Min").ToString());
            string ALO_T2 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_2.Temperatur").ToString();
            string ALO_H2 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_2.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_2.Aufheizzeit.Min").ToString());
            string ALO_WT2 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_2.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_2.Haltezeit.Min").ToString());
            string ALO_T3 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_3.Temperatur").ToString();
            string ALO_H3 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_3.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_3.Aufheizzeit.Min").ToString());
            string ALO_WT3 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_3.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_3.Haltezeit.Min").ToString());
            string ALO_T4 = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_4.Temperatur").ToString();
            string ALO_H4 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_4.Aufheizzeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_4.Aufheizzeit.Min").ToString());
            string ALO_WT4 = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_4.Haltezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Heizen_4.Haltezeit.Min").ToString());
            string ALO_WT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Wartezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Wartezeit.Min").ToString());
            string ALO_TUL = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.OG_Ueberwachung.Temperaturdifferenz").ToString();
            string ALO_TULT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.OG_Ueberwachung.Zeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.OG_Ueberwachung.Zeit.Min").ToString());
            string ALO_TLL = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.UG_Ueberwachung.Temperaturdifferenz").ToString();
            string ALO_TLLT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.UG_Ueberwachung.Zeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.UG_Ueberwachung.Zeit.Min").ToString());
            string ALO_ATT = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Abschrecken.Transportzeit").ToString();
            string ALO_AD = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Abschrecken.VZT_Abschrecken").ToString();
            string ALO_AT = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Abschrecken.Abschreckzeit").ToString();
            string ALO_AFS = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.ALO.Abschrecken.DZ_Zuluft").ToString();
        
            string NIO_D = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.NA_NIO_Abschrecken.VZT_Abschrecken").ToString();
            string NIO_T = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.NA_NIO_Abschrecken.Abschreckzeit").ToString();
            string NIO_FS = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.21 Aus- Einschleussen.Ein.DB Einschleusen LGO 1 Prozessdaten.Daten.Sollwerte.NA_NIO_Abschrecken.DZ_Zuluft").ToString();

            new localDBAdapter("INSERT INTO Soll (LGO_T1,LGO_H1,LGO_WT1,LGO_T2,LGO_H2,LGO_WT2,LGO_T3,LGO_H3,LGO_WT3,LGO_T4,LGO_H4,LGO_WT4,LGO_WT,LGO_TUL,LGO_TULT,LGO_TLL,LGO_TLLT,LGO_ATT,LGO_AD,LGO_AT,LGO_AFS," +
                              "ALO_T1,ALO_H1,ALO_WT1,ALO_T2,ALO_H2,ALO_WT2,ALO_T3,ALO_H3,ALO_WT3,ALO_T4,ALO_H4,ALO_WT4,ALO_WT,ALO_TUL,ALO_TULT,ALO_TLL,ALO_TLLT,ALO_ATT,ALO_AD,ALO_AT,ALO_AFS," +
                              "NIO_D,NIO_T,NIO_FS) " +
                              "VALUES (" +
                              FormatVariable(new List<string> { LGO_T1, LGO_H1, LGO_WT1, LGO_T2, LGO_H2, LGO_WT2, LGO_T3, LGO_H3, LGO_WT3, LGO_T4, LGO_H4, LGO_WT4, LGO_WT, LGO_TUL, LGO_TULT, LGO_TLL, LGO_TLLT, LGO_ATT, LGO_AD, LGO_AT, LGO_AFS }) +","+
                              FormatVariable(new List<string> { ALO_T1, ALO_H1, ALO_WT1, ALO_T2, ALO_H2, ALO_WT2, ALO_T3, ALO_H3, ALO_WT3, ALO_T4, ALO_H4, ALO_WT4, ALO_WT, ALO_TUL, ALO_TULT, ALO_TLL, ALO_TLLT, ALO_ATT, ALO_AD, ALO_AT, ALO_AFS }) + "," +
                              FormatVariable(new List<string> { NIO_D, NIO_T, NIO_FS }) + ")").DB_Input();

            return ((new localDBAdapter("SELECT MAX(ID) FROM Soll;")).DB_Output().Rows[0][0]).ToString();
        }

        string FormatTime(string _h, string _m)
        {
            if (_h.Length == 1)
                _h = "0"+_h;
            if (_m.Length == 1)
                _m = "0" + _m;
            return _h+":"+ _m;
        }

        string FormatVariable(List<string> _l)
        {
            string ret_val = _l[0];

            for (int i = 1; i < _l.Count; i++)
            {
                if (_l[i].Length >= 5)
                {
                    ret_val = ret_val + ",'" + _l[i] + "'";
                }
                else
                {
                    ret_val = ret_val + "," + _l[i];
                }
               
            }
            return ret_val;
        }

        private string GetDataTimeNowToFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private Dictionary<string, object> GetRecipeData(string RecipeName)
        {
            IRecipeClass MachineRecipe = ApplicationService.GetService<IRecipeService>().GetRecipeClass("MR");
            Dictionary<string, object> r_val = MachineRecipe.GetRecipeFile(RecipeName).GetValues();
            return r_val;
        }

        #endregion

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
