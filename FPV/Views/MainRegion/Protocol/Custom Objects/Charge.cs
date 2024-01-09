using HMI.Module;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{

    class Charge 
    {
        public Charge(int _id,  string _charge, int _mr_id, string _mr, int _runs, int _status, bool _error, string _start, string _end, string _user)
        {
            Id = _id;

            ChargeNr = _charge;
            MR_ID = _mr_id;
            MR = _mr;
            Runs = _runs;
            Status = _status;
            Error = _error;

            Start = _start;
            End = _end;
            User = _user;
        }

        public Charge()
        {
            Id = 0;

            ChargeNr = "";
            MR_ID = 0;
            Runs = 0;
            Status = 0;
            Error = false;

            Start = "";
            End = "";
            User = "";
            ErrorList =new ObservableCollection<Error>();
            RunList = new ObservableCollection<Run>();
            RemarkList = new ObservableCollection<Remark>();
        }

        public int Id { set; get; }

        public string ChargeNr { set; get; }

        public int MR_ID { set; get; }
        public string MR { set; get; }

        public int Runs { set; get; }

        public int Status { set; get; }

        public bool Error { set; get; }

        public string Start { set; get; }
        
        public string End { set; get; }

        public string User { set; get; }

        public ObservableCollection<Error> ErrorList { set; get; }
        public ObservableCollection<Run> RunList { set; get; }
        public ObservableCollection<Remark> RemarkList { set; get; }

        public void SetErrorList()
        {
            Task.Run(() =>
            {
                ObservableCollection<Error> ret_val = new ObservableCollection<Error>();

                DataTable DT = (new localDBAdapter("SELECT * FROM Errors WHERE Charge_Id = " + Id.ToString())).DB_Output();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow r in DT.Rows)
                    {
                        ret_val.Add(new Error(r.ItemArray[2].ToString(),
                            r.ItemArray[3].ToString(),
                             r.ItemArray[4].ToString(),
                             r.ItemArray[5].ToString()));
                    }
                }
                ErrorList = ret_val;

                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
                    adapter.ErrorList = ErrorList;
                });
            });
        }

        public void SetRunList()
        {
            Task.Run(() => 
            {
                ObservableCollection<Run> ret_val = new ObservableCollection<Run>();

                DataTable DT = (new localDBAdapter("SELECT * FROM Runs WHERE Charge_Id = " + Id.ToString())).DB_Output();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow r in DT.Rows)
                    {
                        ret_val.Add(new Run(Convert.ToInt32(r.ItemArray[2]), 
                            Convert.ToInt32(r.ItemArray[3]),
                            Convert.ToInt32(r.ItemArray[4]),
                            r.ItemArray[5].ToString(),
                            r.ItemArray[6].ToString(),
                            r.ItemArray[7].ToString(),
                            r.ItemArray[8].ToString(),
                            r.ItemArray[9].ToString(),
                            r.ItemArray[10].ToString(),
                            r.ItemArray[11].ToString(),
                            r.ItemArray[12].ToString(),
                            r.ItemArray[13].ToString(),
                            r.ItemArray[14].ToString(),
                            r.ItemArray[15].ToString(),
                            r.ItemArray[16].ToString(),
                            r.ItemArray[17].ToString(),
                            r.ItemArray[18].ToString()));
                    }
                }
                RunList = ret_val;

              
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
                    adapter.RunList = RunList;
                });
            });
        }

        public void SetRemarkList()
        {
            Task.Run(() =>
            {
                ObservableCollection<Remark> ret_val = new ObservableCollection<Remark>();

                DataTable DT = (new localDBAdapter("SELECT * FROM Remarks WHERE Charge_Id = " + Id.ToString())).DB_Output();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow r in DT.Rows)
                    {
                        ret_val.Add(new Remark(r.ItemArray[2].ToString(),
                            r.ItemArray[3].ToString(),
                            r.ItemArray[4].ToString(),
                            r.ItemArray[5].ToString()));
                    }
                }
                RemarkList = ret_val;
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    var adapter = ApplicationService.GetAdapter(nameof(ProtocolAdapter)) as ProtocolAdapter;
                    adapter.RemarkList = RemarkList;
                });
            });
        }
    }
}
