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
    class Carriage
    {
        IVariable Start;
        IVariable End;
        IVariable Error;
        IVariable ErrorAck;
        IVariable Rework;
        IVariable NOK;

        IVariable Charge;
        IVariable Run;
        IVariable LGOActive;
        IVariable ALOActive;
        IVariable ReworkActive;

        IAlarmItem[] OLD_Error;

        readonly ICurrentAlarms2 CurrentAlarmList;
        readonly IVariableService VS;
        readonly ILanguageService textService;
        public Carriage()
        {
            VS = ApplicationService.GetService<IVariableService>();
            OLD_Error = new IAlarmItem[0];
            CurrentAlarmList = ApplicationService.GetService<IAlarmService>().GetCurrentAlarms2();
            textService = ApplicationService.GetService<ILanguageService>();
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

        public int StartEr { set; get; }
        public int EndEr { set; get; }
        public string ReworkMessage { set; get; }
        public string NOKMessage { set; get; }

        public string VCharge { set { Charge = VS.GetVariable(value); } }
        public string VRun { set { Run = VS.GetVariable(value); } }
        public string VLGOActive { set { LGOActive = VS.GetVariable(value); } }
        public string VALOActive { set { ALOActive = VS.GetVariable(value); } }
        public string VReworkActive { set { ReworkActive = VS.GetVariable(value); } }

        private void StartChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                Start.Value = false;
                Task.Run(() => { WriteDateTimeTOCharge("_A_S"); });
            }
        }

        private void EndChange(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (bool)e.Value)
            {
                End.Value = false;
                Task.Run(() => { WriteDateTimeTOCharge("_A_E"); });
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
                Rework.Value = false;
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
                    (new localDBAdapter("UPDATE Runs SET " + GetStationType() + clmn + " = '" + GetDataTimeNowToFormat() + "' WHERE Charge_Id = " + Charge_Id + " AND Run = " + Run.Value.ToString() + ";")).DB_Input();
                }
            }
        }

        private string GetDataTimeNowToFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string GetStationType()
        {
            if (LGOActive.Value.ToString() == "True")
            {
                return "LGO";
            }

            if (ALOActive.Value.ToString() == "True")
            {
                return "ALO";
            }
            if (ReworkActive.Value.ToString() == "True")
            {
                return "NIO";
            }
            return "";
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
