namespace HMI.Views.MainRegion.Protocol
{

    class HelpInf
    {
        public HelpInf(int _T1,string _H1,string _WT1,int _T2,string _H2,string _WT2,int _T3,string _H3,string _WT3,
                        int _T4,string _H4,string _WT4,string _WT,int _TUL,string _TULT,int _TLL,string _TLLT,int _ATT,int _AD,
                        int _AT,int _AFS, int _T1_Min, int _T1_Max, int _T2_Min, int _T2_Max, int _T3_Min, int _T3_Max, int _T4_Min, int _T4_Max, string _WaitT, int _Transport)
        {
            T1 = _T1;
            H1 = _H1;
            WT1 = _WT1;
            T2 = _T2;
            H2 = _H2;
            WT2 = _WT2;
            T3 = _T3;
            H3 = _H3;
            WT3 = _WT3;
            T4 = _T4;
            H4 = _H4;
            WT4 = _WT4;
            WT = _WT;
            TUL = _TUL;
            TULT = _TULT;
            TLL = _TLL;
            TLLT = _TLLT;
            ATT = _ATT;
            AD = _AD;
            AT = _AT;
            AFS = _AFS;

            T1_Min = _T1_Min;
            T1_Max = _T1_Max;
            T2_Min = _T2_Min;
            T2_Max = _T2_Max;
            T3_Min = _T3_Min;
            T3_Max = _T3_Max;
            T4_Min = _T4_Min;
            T4_Max = _T4_Max;
            WaitT = _WaitT;
            Transport = _Transport;
        }

        public HelpInf()
        {
            T1 = 0;
            H1 = "00:00";
            WT1 = "00:00";
            T2 = 0;
            H2 = "00:00";
            WT2 = "00:00";
            T3 = 0;
            H3 = "00:00";
            WT3 = "00:00";
            T4 = 0;
            H4 = "00:00";
            WT4 = "00:00";
            WT = "00:00";
            TUL = 0;
            TULT = "00:00";
            TLL = 0;
            TLLT = "00:00";
            ATT = 0;
            AD = 0;
            AT = 0;
            AFS = 0;

            T1_Min = 0;
            T1_Max = 0;
            T2_Min = 0;
            T2_Max = 0;
            T3_Min = 0;
            T3_Max = 0;
            T4_Min = 0;
            T4_Max = 0;
            WaitT = "00:00";
            Transport = 0;

            NIO_D = 0;
            NIO_T = 0;
            NIO_FS = 0;
        }

        public HelpInf(int _NIO_D, int _NIO_T, int _NIO_FS)
        {
            NIO_D = _NIO_D;
            NIO_T = _NIO_T;
            NIO_FS = _NIO_FS;
        }

        public int NIO_D { set; get; }
        public int NIO_T { set; get; }
        public int NIO_FS { set; get; }

        public int T1 { set; get; }
        public string H1 { set; get; }
        public string WT1 { set; get; }

        public int T2 { set; get; }
        public string H2 { set; get; }
        public string WT2 { set; get; }

        public int T3 { set; get; }
        public string H3 { set; get; }
        public string WT3 { set; get; }

        public int T4 { set; get; }
        public string H4 { set; get; }
        public string WT4 { set; get; }

        public string WT { set; get; }

        public int TUL { set; get; }
        public string TULT { set; get; }

        public int TLL { set; get; }
        public string TLLT { set; get; }

        public int ATT { set; get; }
        public int AD { set; get; }
        public int AT { set; get; }
        public int AFS { set; get; }

        public int T1_Min { set; get; }
        public int T1_Max { set; get; }
        public int T2_Min { set; get; }
        public int T2_Max { set; get; }
        public int T3_Min { set; get; }
        public int T3_Max { set; get; }
        public int T4_Min { set; get; }
        public int T4_Max { set; get; }
        public string WaitT { set; get; }
        public int Transport { set; get; }
    }
}
