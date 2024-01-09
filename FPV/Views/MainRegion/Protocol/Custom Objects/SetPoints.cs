namespace HMI.Views.MainRegion.Protocol
{

    class SetPoints
    {
        public SetPoints(int _Id,int _LGO_T1,string _LGO_H1,string _LGO_WT1,int _LGO_T2,string _LGO_H2,string _LGO_WT2,int _LGO_T3,string _LGO_H3,string _LGO_WT3,
                        int _LGO_T4,string _LGO_H4,string _LGO_WT4,string _LGO_WT,int _LGO_TUL,string _LGO_TULT,int _LGO_TLL,string _LGO_TLLT,int _LGO_ATT,int _LGO_AD,
                        int _LGO_AT,int _LGO_AFS,int _ALO_T1,string _ALO_H1,string _ALO_WT1,int _ALO_T2,string _ALO_H2,string _ALO_WT2,int _ALO_T3,string _ALO_H3,string _ALO_WT3,
                        int _ALO_T4,string _ALO_H4,string _ALO_WT4,string _ALO_WT,int _ALO_TUL,string _ALO_TULT,int _ALO_TLL,string _ALO_TLLT,int _ALO_ATT,int _ALO_AD,
                        int _ALO_AT,int _ALO_AFS,int _NIO_D,int _NIO_T,int _NIO_FS)
        {
            Id = _Id;
            LGO_T1 = _LGO_T1;
            LGO_H1 = _LGO_H1;
            LGO_WT1 = _LGO_WT1;
            LGO_T2 = _LGO_T2;
            LGO_H2 = _LGO_H2;
            LGO_WT2 = _LGO_WT2;
            LGO_T3 = _LGO_T3;
            LGO_H3 = _LGO_H3;
            LGO_WT3 = _LGO_WT3;
            LGO_T4 = _LGO_T4;
            LGO_H4 = _LGO_H4;
            LGO_WT4 = _LGO_WT4;
            LGO_WT = _LGO_WT;
            LGO_TUL = _LGO_TUL;
            LGO_TULT = _LGO_TULT;
            LGO_TLL = _LGO_TLL;
            LGO_TLLT = _LGO_TLLT;
            LGO_ATT = _LGO_ATT;
            LGO_AD = _LGO_AD;
            LGO_AT = _LGO_AT;
            LGO_AFS = _LGO_AFS;
            ALO_T1 = _ALO_T1;
            ALO_H1 = _ALO_H1;
            ALO_WT1 = _ALO_WT1;
            ALO_T2 = _ALO_T2;
            ALO_H2 = _ALO_H2;
            ALO_WT2 = _ALO_WT2;
            ALO_T3 = _ALO_T3;
            ALO_H3 = _ALO_H3;
            ALO_WT3 = _ALO_WT3;
            ALO_T4 = _ALO_T4;
            ALO_H4 = _ALO_H4;
            ALO_WT4 = _ALO_WT4;
            ALO_WT = _ALO_WT;
            ALO_TUL = _ALO_TUL;
            ALO_TULT = _ALO_TULT;
            ALO_TLL = _ALO_TLL;
            ALO_TLLT = _ALO_TLLT;
            ALO_ATT = _ALO_ATT;
            ALO_AD = _ALO_AD;
            ALO_AT = _ALO_AT;
            ALO_AFS = _ALO_AFS;
            NIO_D = _NIO_D;
            NIO_T = _NIO_T;
            NIO_FS = _NIO_FS;
        }

        public SetPoints()
        {
            Id = 0;
            LGO_T1 = 0;
            LGO_H1 = "00:00";
            LGO_WT1 = "00:00";
            LGO_T2 = 0;
            LGO_H2 = "00:00";
            LGO_WT2 = "00:00";
            LGO_T3 = 0;
            LGO_H3 = "00:00";
            LGO_WT3 = "00:00";
            LGO_T4 = 0;
            LGO_H4 = "00:00";
            LGO_WT4 = "00:00";
            LGO_WT = "00:00";
            LGO_TUL = 0;
            LGO_TULT = "00:00";
            LGO_TLL = 0;
            LGO_TLLT = "00:00";
            LGO_ATT = 0;
            LGO_AD = 0;
            LGO_AT = 0;
            LGO_AFS = 0;
            ALO_T1 = 0;
            ALO_H1 = "00:00";
            ALO_WT1 = "00:00";
            ALO_T2 = 0;
            ALO_H2 = "00:00";
            ALO_WT2 = "00:00";
            ALO_T3 = 0;
            ALO_H3 = "00:00";
            ALO_WT3 = "00:00";
            ALO_T4 = 0;
            ALO_H4 = "00:00";
            ALO_WT4 = "00:00";
            ALO_WT = "00:00";
            ALO_TUL = 0;
            ALO_TULT = "00:00";
            ALO_TLL = 0;
            ALO_TLLT = "00:00";
            ALO_ATT = 0;
            ALO_AD = 0;
            ALO_AT = 0;
            ALO_AFS = 0;
            NIO_D = 0;
            NIO_T = 0;
            NIO_FS = 0;
        }

        public int Id { set; get; }

        public int LGO_T1 { set; get; }
        public string LGO_H1 { set; get; }
        public string LGO_WT1 { set; get; }

        public int LGO_T2 { set; get; }
        public string LGO_H2 { set; get; }
        public string LGO_WT2 { set; get; }

        public int LGO_T3 { set; get; }
        public string LGO_H3 { set; get; }
        public string LGO_WT3 { set; get; }

        public int LGO_T4 { set; get; }
        public string LGO_H4 { set; get; }
        public string LGO_WT4 { set; get; }

        public string LGO_WT { set; get; }

        public int LGO_TUL { set; get; }
        public string LGO_TULT { set; get; }

        public int LGO_TLL { set; get; }
        public string LGO_TLLT { set; get; }

        public int LGO_ATT { set; get; }
        public int LGO_AD { set; get; }
        public int LGO_AT { set; get; }
        public int LGO_AFS { set; get; }

        public int ALO_T1 { set; get; }
        public string ALO_H1 { set; get; }
        public string ALO_WT1 { set; get; }

        public int ALO_T2 { set; get; }
        public string ALO_H2 { set; get; }
        public string ALO_WT2 { set; get; }

        public int ALO_T3 { set; get; }

        public string ALO_H3 { set; get; }
        public string ALO_WT3 { set; get; }

        public int ALO_T4 { set; get; }
        public string ALO_H4 { set; get; }
        public string ALO_WT4 { set; get; }

        public string ALO_WT { set; get; }

        public int ALO_TUL { set; get; }
        public string ALO_TULT { set; get; }

        public int ALO_TLL { set; get; }
        public string ALO_TLLT { set; get; }

        public int ALO_ATT { set; get; }
        public int ALO_AD { set; get; }
        public int ALO_AT { set; get; }
        public int ALO_AFS { set; get; }

        public int NIO_D { set; get; }
        public int NIO_T { set; get; }
        public int NIO_FS { set; get; }
    }
}
