using HMI.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisiWin.Alarm;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Language;

namespace HMI.Services.Custom_Objects
{
    class Station
    {
        IVariable Start;
        IVariable End;
        IVariable Error;
        IVariable ErrorAck;
        IVariable Rework;
        IVariable NOK;

        IVariable Charge;
        IVariable Run;
        IVariable ChargeEnd;
        IVariable RunEnd;

        IAlarmItem[] OLD_Error = new IAlarmItem[0];
        readonly ICurrentAlarms2 CurrentAlarmList = ApplicationService.GetService<IAlarmService>().GetCurrentAlarms2();
        readonly IVariableService VS = ApplicationService.GetService<IVariableService>();
        readonly ILanguageService textService = ApplicationService.GetService<ILanguageService>();
        public Station()
        {
            
        }

        private string _vStart;
        public string VStart
        {
            get { return _vStart; }
            set
            {
                _vStart = value;
                Start = VS.GetVariable(value);
                Start.Change += StartChange;
            }
        }

        private string _vEnd;
        public string VEnd
        {
            get { return _vEnd; }
            set
            {
                _vEnd = value;
                End = VS.GetVariable(value);
                End.Change += EndChange;
            }
        }

        private string _vError;
        public string VError
        {
            get { return _vError; }
            set
            {
                _vError = value;
                Error = VS.GetVariable(value);
                Error.Change += ErrorChange;
            }
        }
        private string _vErrorAck;
        public string VErrorAck
        {
            get { return _vErrorAck; }
            set
            {
                _vErrorAck = value;
                ErrorAck = VS.GetVariable(value);
                ErrorAck.Change += ErrorAckChange;
            }
        }

        private string _vRework;
        public string VRework
        {
            get { return _vRework; }
            set
            {
                _vRework = value;
                Rework = VS.GetVariable(value);
                Rework.Change += ReworkChange;
            }
        }

        private string _vNOK;
        public string VNOK
        {
            get { return _vNOK; }
            set
            {
                _vNOK = value;
                NOK = VS.GetVariable(value);
                NOK.Change += NOKChange;
            }
        }

        public string OType { set; get; }
        public string ONr { set; get; }
        public int StartEr { set; get; }
        public int EndEr { set; get; }
        public string ReworkMessage { set; get; }
        public string NOKMessage { set; get; }

        public string VCharge { set { Charge = VS.GetVariable(value); } }
        public string VRun { set { Run = VS.GetVariable(value); } }

        public string VChargeEnd { set { ChargeEnd = VS.GetVariable(value); } }
        public string VRunEnd { set { RunEnd = VS.GetVariable(value); } }


        private void StartChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                Start.Value = false;
                Task.Run(() =>
                {
                    WriteDateTimeTOCharge("_S");
                    WriteOvenNrTOCharge();
                });
            }
        }
        private void EndChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                End.Value = false;
                Task.Run(() =>
                {
                    WriteDateTimeEndTOCharge("_E");
                    WriteIst();
                });
            }
        }
        private void ErrorChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                Error.Value = false;
                Task.Run(() => WriteError());
            }
        }
        private void ErrorAckChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                OLD_Error = new IAlarmItem[0];
            }
        }
        private void ReworkChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                Rework.Value = false;
                string message = textService.GetText(ReworkMessage);
                Task.Run(() => 
                {
                    WriteMessageToCharge(message);
                    WriteStatusToCharge(2);
                });
            }
        }
        private void NOKChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                NOK.Value = false;
                string message = textService.GetText(NOKMessage);
                Task.Run(() =>
                {
                    WriteMessageToCharge(message);
                    WriteStatusToCharge(3);
                });
            }
        }

        private void WriteDateTimeTOCharge(string clmn)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();

                if (temp.Rows.Count != 0)
                {
                    (new localDBAdapter("UPDATE Runs SET " + OType + clmn + " = '" + GetDataTimeNowToFormat() + "' WHERE Charge_Id = " + Charge_Id + " AND Run = " + Run.Value.ToString() + ";")).DB_Input();
                }
            }
        }

        private void WriteDateTimeEndTOCharge(string clmn)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + ChargeEnd.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();

                if (temp.Rows.Count != 0)
                {
                    (new localDBAdapter("UPDATE Runs SET " + OType + clmn + " = '" + GetDataTimeNowToFormat() + "' WHERE Charge_Id = " + Charge_Id + " AND Run = " + RunEnd.Value.ToString() + ";")).DB_Input();
                }
            }
        }

        private void WriteOvenNrTOCharge()
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();
                (new localDBAdapter("UPDATE Runs SET " + OType + " = " + ONr + " WHERE Charge_Id = " + Charge_Id + " AND Run = " + Run.Value.ToString() + ";")).DB_Input();
            }
        }

        private string GetDataTimeNowToFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void WriteIst()
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + ChargeEnd.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();
                temp = (new localDBAdapter("SELECT Id, Ist_Id FROM Runs WHERE Charge_Id = " + Charge_Id + " AND Run = " + RunEnd.Value.ToString() + ";")).DB_Output();
                if (temp.Rows.Count != 0)
                {
                    string Run_Id = temp.Rows[0][0].ToString();
                    string Ist_Id = temp.Rows[0][1].ToString();

                    string T1_Min = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_1.Temperatur_Min").ToString();
                    string T1_Max = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_1.Temperatur_Max").ToString();
                    string T2_Min = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_2.Temperatur_Min").ToString();
                    string T2_Max = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_2.Temperatur_Max").ToString();
                    string T3_Min = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_3.Temperatur_Min").ToString();
                    string T3_Max = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_3.Temperatur_Max").ToString();
                    string T4_Min = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_4.Temperatur_Min").ToString();
                    string T4_Max = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Heizen_4.Temperatur_Max").ToString();
                    string WaitT = FormatTime(ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Wartezeit.Std").ToString(), ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Wartezeit.Min").ToString());
                    string TransportLGO = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Abschrecken.Transportzeit LGO").ToString();
                    string TransportALO = ApplicationService.GetVariableValue("IMEX.PLC.Blocks.22 Fahrwagen.Hubtische.LA.DB FW LA HT Prozessdaten.Daten.Istwerte.Abschrecken.Transportzeit ALO").ToString();

                    switch (OType)
                    {
                        case "LGO":
                            if (Ist_Id == "0")
                            {
                                new localDBAdapter("INSERT INTO Ist (LGOT1_Min,LGOT1_Max,LGOT2_Min,LGOT2_Max,LGOT3_Min,LGOT3_Max,LGOT4_Min,LGOT4_Max,LGOWaitT,LGOTransport) VALUES (" +
                                                            FormatVariable(new List<string> { T1_Min, T1_Max, T2_Min, T2_Max, T3_Min, T3_Max, T4_Min, T4_Max, WaitT, TransportLGO }) + ")").DB_Input();
                                Ist_Id = ((new localDBAdapter("SELECT MAX(ID) FROM Ist;")).DB_Output().Rows[0][0]).ToString();

                                (new localDBAdapter("UPDATE Runs SET Ist_Id = " + Ist_Id + " WHERE Id = " + Run_Id + ";")).DB_Input();
                            }
                            else
                            {
                                new localDBAdapter("UPDATE Ist SET LGOT1_Min = " + T1_Min + ",LGOT1_Max=" + T1_Max + ",LGOT2_Min=" + T2_Min + ",LGOT2_Max=" + T2_Max + ",LGOT3_Min=" + T3_Min + ",LGOT3_Max=" + T3_Max +
                                ",LGOT4_Min=" + T4_Min + ",LGOT4_Max=" + T4_Max + ",LGOWaitT='" + WaitT + "',LGOTransport= " + TransportLGO + " WHERE Id=" + Ist_Id + ";").DB_Input();

                            }
                            break;
                        case "ALO":
                            if (Ist_Id == "0")
                            {
                                new localDBAdapter("INSERT INTO Ist (ALOT1_Min,ALOT1_Max,ALOT2_Min,ALOT2_Max,ALOT3_Min,ALOT3_Max,ALOT4_Min,ALOT4_Max,ALOWaitT,ALOTransport) VALUES (" +
                                                           FormatVariable(new List<string> { T1_Min, T1_Max, T2_Min, T2_Max, T3_Min, T3_Max, T4_Min, T4_Max, WaitT, TransportALO }) + ")").DB_Input();
                                Ist_Id = ((new localDBAdapter("SELECT MAX(ID) FROM Ist;")).DB_Output().Rows[0][0]).ToString();

                                (new localDBAdapter("UPDATE Runs SET Ist_Id = " + Ist_Id + " WHERE Id = " + Run_Id + ";")).DB_Input();
                            }
                            else
                            {
                                new localDBAdapter("UPDATE Ist SET ALOT1_Min = " + T1_Min + ",ALOT1_Max=" + T1_Max + ",ALOT2_Min=" + T2_Min + ",ALOT2_Max=" + T2_Max + ",ALOT3_Min=" + T3_Min + ",ALOT3_Max=" + T3_Max +
                                     ",ALOT4_Min=" + T4_Min + ",ALOT4_Max=" + T4_Max + ",ALOWaitT='" + WaitT + "',ALOTransport= " + TransportALO + " WHERE Id=" + Ist_Id + ";").DB_Input();

                            }
                            break;
                    }
                }
            }
        }

        string FormatTime(string _h, string _m)
        {
            if (_h.Length == 1)
                _h = "0" + _h;
            if (_m.Length == 1)
                _m = "0" + _m;
            return _h + ":" + _m;
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

        private void WriteError()
        {
            IAlarmItem[] TT = CurrentAlarmList.Alarms.Where(x => x.Group.Name == "Errors" && x.AlarmState == AlarmState.Active && x.Param2 >= StartEr && x.Param2 <= EndEr).ToArray();

            if (TT.Length != 0)
                if (!Enumerable.SequenceEqual(TT, OLD_Error))
                {
                    WriteErrorToCharge(TT);
                    OLD_Error = TT;
                }
        }

        private void WriteErrorToCharge(IAlarmItem[] TT)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();

                new localDBAdapter("UPDATE Charges SET Error = 1 WHERE Id = " + Charge_Id + ";").DB_Input();

                foreach (IAlarmItem AI in TT)
                {
                    (new localDBAdapter("INSERT INTO Errors (Charge_Id, Time, Text, User) VALUES (" + Charge_Id + ",'" + GetDataTimeNowToFormat() + "','" + AI.Text + "','" + ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() + "')")).DB_Input();
                }
            }
        }

        private void WriteMessageToCharge(string _message)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();
                new localDBAdapter("UPDATE Charges SET Error = 1 WHERE Id = " + Charge_Id + ";").DB_Input();
                (new localDBAdapter("INSERT INTO Errors (Charge_Id, Time, Text, User) VALUES (" + Charge_Id + ",'" + GetDataTimeNowToFormat() + "','" + _message + "','" + ApplicationService.GetVariableValue("__CURRENT_USER.FULLNAME").ToString() + "')")).DB_Input();
            }
        }

        private void WriteStatusToCharge(int _status)
        {
            DataTable temp = (new localDBAdapter("SELECT Id FROM Charges WHERE Charge = '" + Charge.Value.ToString() + "';")).DB_Output();
            if (temp.Rows.Count != 0)
            {
                string Charge_Id = temp.Rows[0][0].ToString();
                new localDBAdapter("UPDATE Charges SET Status = " +_status + " WHERE Id = " + Charge_Id + ";").DB_Input();
            }
        }

    }
}
