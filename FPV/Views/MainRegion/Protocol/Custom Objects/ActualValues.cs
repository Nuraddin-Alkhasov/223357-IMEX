namespace HMI.Views.MainRegion.Protocol
{

    class ActualValues
    {
        public ActualValues(int _Id,int _LGOT1_Min,int _LGOT1_Max,int _LGOT2_Min,int _LGOT2_Max,int _LGOT3_Min,int _LGOT3_Max,int _LGOT4_Min,int _LGOT4_Max,string _LGOWaitT,int _LGOTransport,
                            int _ALOT1_Min,int _ALOT1_Max,int _ALOT2_Min,int _ALOT2_Max,int _ALOT3_Min,int _ALOT3_Max,int _ALOT4_Min,int _ALOT4_Max,string _ALOWaitT,int _ALOTransport)
        {
            Id = _Id;
            LGOT1_Min = _LGOT1_Min;
            LGOT1_Max = _LGOT1_Max;
            LGOT2_Min = _LGOT2_Min;
            LGOT2_Max = _LGOT2_Max;
            LGOT3_Min = _LGOT3_Min;
            LGOT3_Max = _LGOT3_Max;
            LGOT4_Min = _LGOT4_Min;
            LGOT4_Max = _LGOT4_Max;
            LGOWaitT = _LGOWaitT;
            LGOTransport = _LGOTransport;
            ALOT1_Min = _ALOT1_Min;
            ALOT1_Max = _ALOT1_Max;
            ALOT2_Min = _ALOT2_Min;
            ALOT2_Max = _ALOT2_Max;
            ALOT3_Min = _ALOT3_Min;
            ALOT3_Max = _ALOT3_Max;
            ALOT4_Min = _ALOT4_Min;
            ALOT4_Max = _ALOT4_Max;
            ALOWaitT = _ALOWaitT;
            ALOTransport = _ALOTransport;

        }

        public ActualValues()
        {
            Id = 0;
            LGOT1_Min = 0;
            LGOT1_Max = 0;
            LGOT2_Min = 0;
            LGOT2_Max = 0;
            LGOT3_Min = 0;
            LGOT3_Max = 0;
            LGOT4_Min = 0;
            LGOT4_Max = 0;
            LGOWaitT = "00:00";
            LGOTransport = 0;
            ALOT1_Min = 0;
            ALOT1_Max = 0;
            ALOT2_Min = 0;
            ALOT2_Max = 0;
            ALOT3_Min = 0;
            ALOT3_Max = 0;
            ALOT4_Min = 0;
            ALOT4_Max = 0;
            ALOWaitT = "00:00";
            ALOTransport = 0;

        }

        public int Id { set; get; }
        public int LGOT1_Min { set; get; }
        public int LGOT1_Max { set; get; }
        public int LGOT2_Min { set; get; }
        public int LGOT2_Max { set; get; }
        public int LGOT3_Min { set; get; }
        public int LGOT3_Max { set; get; }
        public int LGOT4_Min { set; get; }
        public int LGOT4_Max { set; get; }
        public string LGOWaitT { set; get; }
        public int LGOTransport { set; get; }
        public int ALOT1_Min { set; get; }
        public int ALOT1_Max { set; get; }
        public int ALOT2_Min { set; get; }
        public int ALOT2_Max { set; get; }
        public int ALOT3_Min { set; get; }
        public int ALOT3_Max { set; get; }
        public int ALOT4_Min { set; get; }
        public int ALOT4_Max { set; get; }
        public string ALOWaitT { set; get; }
        public int ALOTransport { set; get; }

    }
}
