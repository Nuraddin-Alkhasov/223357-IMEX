using HMI.Module;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using VisiWin.ApplicationFramework;
using VisiWin.UserManagement;

namespace HMI.Views.MainRegion.Protocol
{
    [ExportAdapter("ProtocolAdapter")]
    class ProtocolAdapter : AdapterBase
    {
        ProtocolAdapter()
        {
            Charges = new ObservableCollection<Charge>();
            TimeSpanFilterTypes= null;

            SetTimeSpan(HistoricalTimeSpanFilterType.Today);
            selectedTimeSpanFilterTypeIndex = 0;
            this.desiredUsers = new ObservableCollection<UserFilterItem>();
            IsAllUsersSelected = true;
            RefreshUsers();
        }

        #region - - - FILTER - - -

        private ObservableCollection<UserFilterItem> desiredUsers;
        private bool isUserSelectionEnabled;
        private bool isAllUsersSelected;
        private DateTime maxTime;
        private DateTime minTime;
        private int selectedTimeSpanFilterTypeIndex;
        private bool customTimeSpanFilterEnabled;
        private string data1Filter;
        private string data2Filter;
        private string data3Filter;
        private string mrFilter;

        public string Data1Filter
        {
            get { return this.data1Filter; }
            set
            {
                if (!Equals(value, this.data1Filter))
                {
                    this.data1Filter = value;
                    this.OnPropertyChanged("Data1Filter");
                }
            }
        }

        public string Data2Filter
        {
            get { return this.data2Filter; }
            set
            {
                if (!Equals(value, this.data2Filter))
                {
                    this.data2Filter = value;
                    this.OnPropertyChanged("Data2Filter");
                }
            }
        }

        public string Data3Filter
        {
            get { return this.data3Filter; }
            set
            {
                if (!Equals(value, this.data3Filter))
                {
                    this.data3Filter = value;
                    this.OnPropertyChanged("Data3Filter");
                }
            }
        }

        public string MRFilter
        {
            get { return this.mrFilter; }
            set
            {
                if (!Equals(value, this.mrFilter))
                {
                    this.mrFilter = value;
                    this.OnPropertyChanged("MRFilter");
                }
            }
        }

        public DateTime MaxTime
        {
            get { return maxTime; }
            set
            {
                if (value != maxTime)
                {
                    maxTime = value;
                    OnPropertyChanged("MaxTime");
                }
            }
        }

        public DateTime MinTime
        {
            get { return minTime; }
            set
            {
                if (value != minTime)
                {
                    minTime = value;
                    OnPropertyChanged("MinTime");
                }
            }
        }

        public int SelectedTimeSpanFilterTypeIndex
        {
            get { return selectedTimeSpanFilterTypeIndex; }
            set
            {
                if (value != selectedTimeSpanFilterTypeIndex)
                {
                    selectedTimeSpanFilterTypeIndex = value;
                    OnPropertyChanged("SelectedTimeSpanFilterTypeIndex");
                }
            }
        }
        public ObservableCollection<HistoricalTimeSpanFilterItem> _TimeSpanFilterTypes;
        public ObservableCollection<HistoricalTimeSpanFilterItem> TimeSpanFilterTypes
        {  
            get { return _TimeSpanFilterTypes; }
            set
            {
                ObservableCollection<HistoricalTimeSpanFilterItem> temp = new ObservableCollection<HistoricalTimeSpanFilterItem>
                {
                    new HistoricalTimeSpanFilterItem(HistoricalTimeSpanFilterType.Today),
                    new HistoricalTimeSpanFilterItem(HistoricalTimeSpanFilterType.Yesterday),
                    new HistoricalTimeSpanFilterItem(HistoricalTimeSpanFilterType.ThisWeek),
                    new HistoricalTimeSpanFilterItem(HistoricalTimeSpanFilterType.LastWeek),
                    new HistoricalTimeSpanFilterItem(HistoricalTimeSpanFilterType.Custom)
                };
                _TimeSpanFilterTypes = temp;
                OnPropertyChanged("TimeSpanFilterTypes");
            }

        }

        public bool CustomTimeSpanFilterEnabled
        {
            get { return customTimeSpanFilterEnabled; }
            set
            {
                if (value != customTimeSpanFilterEnabled)
                {
                    customTimeSpanFilterEnabled = value;
                    OnPropertyChanged("CustomTimeSpanFilterEnabled");
                }
            }
        }

        public void SetTimeSpan(HistoricalTimeSpanFilterType selectedTimeSpanFilterType)
        {
            switch (selectedTimeSpanFilterType)
            {
                case HistoricalTimeSpanFilterType.Custom:
                    break;
                case HistoricalTimeSpanFilterType.Today:
                    MinTime = DateTime.Now.Date;
                    MaxTime = (MinTime.Add(TimeSpan.FromDays(1))).AddSeconds(-1);
                    break;
                case HistoricalTimeSpanFilterType.Yesterday:
                    MinTime = DateTime.Now.Date.Add(TimeSpan.FromDays(-1.0));
                    MaxTime = (MinTime.Add(TimeSpan.FromDays(1))).AddSeconds(-1);
                    break;
                case HistoricalTimeSpanFilterType.ThisWeek:
                    MinTime = DateTime.Now.Date.Add(TimeSpan.FromDays(-(double)DateTime.Now.Date.DayOfWeek));
                    MaxTime = (MinTime.Add(TimeSpan.FromDays(7))).AddSeconds(-1);
                    break;
                case HistoricalTimeSpanFilterType.LastWeek:
                    MinTime = DateTime.Now.Date.Add(TimeSpan.FromDays(-(double)DateTime.Now.Date.DayOfWeek - 7));
                    MaxTime = (MinTime.Add(TimeSpan.FromDays(7))).AddSeconds(-1);
                    break;
                default:
                    break;
            }

        }

        public ObservableCollection<UserFilterItem> DesiredUsers
        {
            get
            {
                return this.desiredUsers;
            }
            set
            {
                if (value != this.desiredUsers)
                {
                    this.desiredUsers = value;
                    OnPropertyChanged("DesiredUsers");
                }
            }
        }

        public bool IsAllUsersSelected
        {
            get { return this.isAllUsersSelected; }
            set
            {
                if (value != this.isAllUsersSelected)
                {
                    this.isAllUsersSelected = value;
                    OnPropertyChanged("IsAllUsersSelected");

                    IsUserSelectionEnabled = !this.isAllUsersSelected;
                }
            }
        }

        public bool IsUserSelectionEnabled
        {
            get { return this.isUserSelectionEnabled; }
            set
            {
                if (value != this.isUserSelectionEnabled)
                {
                    this.isUserSelectionEnabled = value;
                    OnPropertyChanged("IsUserSelectionEnabled");
                }
            }
        }

        void RefreshUsers()
        {
            desiredUsers.Clear();

            //	Benutzer auflisten
            IUserManagementService uMS = ApplicationService.GetService<IUserManagementService>();
            if (uMS != null)
            {
                foreach (string user in uMS.UserNames)
                {
                    IUserDefinition uD = uMS.GetUserDefinition(user);
                    if (uD != null)
                        desiredUsers.Add(new UserFilterItem(user, uD.FullName));
                }
            }
        }

        public void BW_FilterSQL(List<object> param)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += FilterSQL;
            bw.RunWorkerAsync(argument: param);
        }

        private void FilterSQL(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Charges!=null)
                    Charges.Clear();
                DataTable DT = (new localDBAdapter(GenerateSQLQuery())).DB_Output();
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow r in DT.Rows)
                    {
                        Charges.Add(new Charge(
                            Convert.ToInt32(r.ItemArray[0].ToString()),
                            r.ItemArray[1].ToString(),
                            Convert.ToInt32(r.ItemArray[2].ToString()),
                            r.ItemArray[3].ToString(),
                            Convert.ToInt32(r.ItemArray[4].ToString()),
                            Convert.ToInt32(r.ItemArray[5].ToString()),
                            Convert.ToBoolean(r.ItemArray[6].ToString() == "0" ? "False" : "True"),
                            r.ItemArray[7].ToString()==""? "": Convert.ToDateTime(r.ItemArray[7].ToString()).ToString("dd.MM.yyyy HH:mm:ss"),
                            r.ItemArray[8].ToString() == "" ? "" : Convert.ToDateTime(r.ItemArray[8].ToString()).ToString("dd.MM.yyyy HH:mm:ss"),
                            r.ItemArray[9].ToString()));
                    }
                }
            });

        }

        public string GenerateSQLQuery()
        {
            string T = "Select Charges.Id, Charges.Charge, Charges.MR_Id, MachineRecipes.MR, Charges.Runs, Charges.Status, Charges.Error, datetime(Start), datetime(End),  Charges.User From Charges INNER JOIN MachineRecipes ON Charges.MR_Id = MachineRecipes.ID";
            string F = "";
            List<string> FL = new List<string>();

            if (minTime != null && maxTime != null)
            {
                string Minmonth;
                if (minTime.Month.ToString().Length == 1) { Minmonth = "0" + minTime.Month.ToString(); }
                else { Minmonth = minTime.Month.ToString(); }

                string Minday;
                if (minTime.Day.ToString().Length == 1) { Minday = "0" + minTime.Day.ToString(); }
                else { Minday = minTime.Day.ToString(); }

                string Maxmonth;
                if (maxTime.Month.ToString().Length == 1) { Maxmonth = "0" + maxTime.Month.ToString(); }
                else { Maxmonth = maxTime.Month.ToString(); }

                string Maxday;
                if (maxTime.Day.ToString().Length == 1) { Maxday = "0" + maxTime.Day.ToString(); }
                else { Maxday = maxTime.Day.ToString(); }

                FL.Add("datetime(Start) >= date('" + minTime.Year + "-" + Minmonth + "-" + Minday + "') AND date(Start) <= date('" + MaxTime.Year + "-" + Maxmonth + "-" + Maxday + "')");

            }

            if (data1Filter != "" && data1Filter != null)
            {
                FL.Add("Charge like '%" + data1Filter + "%'");
            }

           
            if (mrFilter != "" && mrFilter != null)
            {
                FL.Add("MachineRecipes.MR like '%" + mrFilter + "%'");
            }

            if (!this.isAllUsersSelected)
            {
                string userFilt = "(";
                foreach (UserFilterItem user in this.DesiredUsers)
                {
                    if (user.IsSelected)
                        userFilt = userFilt + "User ='" + user.FullName + "' OR ";
                }
                if (userFilt.Length > 3)
                {
                    userFilt = userFilt.Remove(userFilt.Length - 4);
                    userFilt += ")";
                    FL.Add(userFilt);

                }



            }
            else
            {
                //string userFilt = "(";
                //foreach (UserFilterItem user in this.DesiredUsers)
                //{
                //    userFilt = userFilt + "User ='" + user.FullName + "' OR ";
                //}
                //userFilt = userFilt.Remove(userFilt.Length - 4);
                //userFilt = userFilt + ")";
                //FL.Add(userFilt);
            }

            if (FL.Count > 0)
            {
                F += " WHERE ";
                for (int i = 0; i < FL.Count; i++)
                {
                    if (i == 0)
                    {
                        F += FL[i];
                    }
                    else
                    {
                        F = F + " AND " + FL[i];
                    }
                }
            }
            return T + F + ";";
        }

       

        #endregion

        #region - - - Charges - - -

        private string lgo;
        public string LGO
        {
            get { return this.lgo; }
            set
            {
                this.lgo = value;
                this.OnPropertyChanged("LGO");
            }
        }

        private string alo;
        public string ALO
        {
            get { return this.alo; }
            set
            {
                this.alo = value;
                this.OnPropertyChanged("ALO");
            }
        }
      
        private string nio;
        public string NIO
        {
            get { return this.nio; }
            set
            {
                this.nio = value;
                this.OnPropertyChanged("NIO");
            }
        }

        private Charge selectedCharge;
        public Charge SelectedCharge
        {
            get { return this.selectedCharge; }
            set
            {
                if (value != this.selectedCharge)
                {
                    this.selectedCharge = value;

                    if (selectedCharge != null)
                    {

                        DataTable DT = (new localDBAdapter("SELECT LGO, ALO, NIO FROM MachineRecipes WHERE Id = " + selectedCharge.MR_ID.ToString())).DB_Output();
                        if (DT.Rows.Count > 0)
                        {
                            LGO = DT.Rows[0].ItemArray[0].ToString();
                            ALO = DT.Rows[0].ItemArray[1].ToString();
                            NIO = DT.Rows[0].ItemArray[2].ToString();
                        }
                        else
                        {
                            LGO = "";
                            ALO = "";
                            NIO = "";
                        }

                        selectedCharge.SetErrorList();
                        selectedCharge.SetRemarkList();
                        selectedCharge.SetRunList();
                    }
                    else
                    {
                        LGO = "";
                        ALO = "";
                        NIO = "";
                        ErrorList = null;
                        RunList = null;
                        RemarkList = null;
                    }

                    this.OnPropertyChanged("SelectedCharge");
                }
              
            }
        }

        private ObservableCollection<Charge> charges;
        public ObservableCollection<Charge> Charges
        {
            get { return this.charges; }
            set
            {
                if (!Equals(value, this.charges))
                {
                    this.charges = value;
                    this.OnPropertyChanged("Charges");
                }
            }
        }

        public ObservableCollection<Error> errorList;
        public ObservableCollection<Error> ErrorList
        {
            get { return this.errorList; }
            set
            {
                if (!Equals(value, this.errorList))
                {
                    this.errorList = value;
                    this.OnPropertyChanged("ErrorList");
                }
            }
        }
        public ObservableCollection<Run> Runlist { set; get; }
        public ObservableCollection<Run> RunList
        {
            get { return this.Runlist; }
            set
            {
                if (!Equals(value, this.Runlist))
                {
                    this.Runlist = value;
                    this.OnPropertyChanged("RunList");
                }
            }
        }
        public ObservableCollection<Remark> Remarklist { set; get; }
        public ObservableCollection<Remark> RemarkList
        {
            get { return this.Remarklist; }
            set
            {
                if (!Equals(value, this.Remarklist))
                {
                    this.Remarklist = value;
                    this.OnPropertyChanged("RemarkList");
                }
            }
        }
        private int run;
        public int Run
        {
            get { return this.run; }
            set
            {
                this.run = value;
                this.OnPropertyChanged("Run");

            }
        }

        private string oType;
        public string OType
        {
            get { return this.oType; }
            set
            {
                this.oType = value;
                SetHelp(oType);
                switch (oType)
                {
                    case "LGO": OType = "@Protocol.Text8"; break;
                    case "ALO": OType = "@Protocol.Text7"; break;

                }
                this.OnPropertyChanged("OType");
            }
        }

        private HelpInf help;
        public HelpInf Help
        {
            get { return this.help; }
            set
            {
                this.help = value;
                this.OnPropertyChanged("Help");
            }
        }


        private void SetHelp(string _O)
        {
            if (SelectedCharge.RunList.Count-1 >= run)
            {
                switch (_O)
                {
                    case "LGO":
                        Help = new HelpInf(
                           RunList[run].SetPoints.LGO_T1,
                            RunList[run].SetPoints.LGO_H1,
                            RunList[run].SetPoints.LGO_WT1,
                            RunList[run].SetPoints.LGO_T2,
                            RunList[run].SetPoints.LGO_H2,
                            RunList[run].SetPoints.LGO_WT2,
                            RunList[run].SetPoints.LGO_T3,
                            RunList[run].SetPoints.LGO_H3,
                            RunList[run].SetPoints.LGO_WT3,
                            RunList[run].SetPoints.LGO_T4,
                            RunList[run].SetPoints.LGO_H4,
                            RunList[run].SetPoints.LGO_WT4,
                            RunList[run].SetPoints.LGO_WT,
                            RunList[run].SetPoints.LGO_TUL,
                            RunList[run].SetPoints.LGO_TULT,
                            RunList[run].SetPoints.LGO_TLL,
                            RunList[run].SetPoints.LGO_TLLT,
                            RunList[run].SetPoints.LGO_ATT,
                            RunList[run].SetPoints.LGO_AD,
                            RunList[run].SetPoints.LGO_AT,
                            RunList[run].SetPoints.LGO_AFS,
                            RunList[run].ActualValues.LGOT1_Min,
                            RunList[run].ActualValues.LGOT1_Max,
                            RunList[run].ActualValues.LGOT2_Min,
                            RunList[run].ActualValues.LGOT2_Max,
                            RunList[run].ActualValues.LGOT3_Min,
                            RunList[run].ActualValues.LGOT3_Max,
                            RunList[run].ActualValues.LGOT4_Min,
                            RunList[run].ActualValues.LGOT4_Max,
                            RunList[run].ActualValues.LGOWaitT,
                            RunList[run].ActualValues.LGOTransport);
                        break;
                    case "ALO":
                        Help = new HelpInf(
                            RunList[run].SetPoints.ALO_T1,
                            RunList[run].SetPoints.ALO_H1,
                            RunList[run].SetPoints.ALO_WT1,
                            RunList[run].SetPoints.ALO_T2,
                            RunList[run].SetPoints.ALO_H2,
                            RunList[run].SetPoints.ALO_WT2,
                            RunList[run].SetPoints.ALO_T3,
                            RunList[run].SetPoints.ALO_H3,
                            RunList[run].SetPoints.ALO_WT3,
                            RunList[run].SetPoints.ALO_T4,
                            RunList[run].SetPoints.ALO_H4,
                            RunList[run].SetPoints.ALO_WT4,
                            RunList[run].SetPoints.ALO_WT,
                            RunList[run].SetPoints.ALO_TUL,
                            RunList[run].SetPoints.ALO_TULT,
                            RunList[run].SetPoints.ALO_TLL,
                            RunList[run].SetPoints.ALO_TLLT,
                            RunList[run].SetPoints.ALO_ATT,
                            RunList[run].SetPoints.ALO_AD,
                            RunList[run].SetPoints.ALO_AT,
                            RunList[run].SetPoints.ALO_AFS,
                            RunList[run].ActualValues.ALOT1_Min,
                            RunList[run].ActualValues.ALOT1_Max,
                            RunList[run].ActualValues.ALOT2_Min,
                            RunList[run].ActualValues.ALOT2_Max,
                            RunList[run].ActualValues.ALOT3_Min,
                            RunList[run].ActualValues.ALOT3_Max,
                            RunList[run].ActualValues.ALOT4_Min,
                            RunList[run].ActualValues.ALOT4_Max,
                            RunList[run].ActualValues.ALOWaitT,
                            RunList[run].ActualValues.ALOTransport);
                        break;
                    case "NIO":
                        Help = new HelpInf(
                            RunList[run].SetPoints.NIO_D,
                            RunList[run].SetPoints.NIO_T,
                            RunList[run].SetPoints.NIO_FS);
                        break;


                }
            }
            else
            {
                Help = new HelpInf();
            }
          
        }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SelectedTimeSpanFilterTypeIndex")
            {
                if (SelectedTimeSpanFilterTypeIndex >= 0)
                {
                    HistoricalTimeSpanFilterType selectedFilterType = TimeSpanFilterTypes[SelectedTimeSpanFilterTypeIndex].FilterType;
                    CustomTimeSpanFilterEnabled = selectedFilterType == HistoricalTimeSpanFilterType.Custom;
                    SetTimeSpan(selectedFilterType);
                }
            }
        }
    }
}
