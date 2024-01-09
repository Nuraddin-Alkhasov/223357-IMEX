using HMI.Module;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Protocol
{

    class Run
    {
        public Run(int _run, int _SollId, int _IstId, string _start, string _lgo, string _lgos, string _lgoe, string _lgoas, string _lgoae, string _alo, string _alos, string _aloe, string _aloas, string _aloae, string _nioas, string _nioae, string _end)
        {
            RunNr = _run;

            SollId = _SollId;
            IstId =  _IstId;

            SETSetPoints();
            SETActualValues();

            Start = _start;

            LGO = _lgo.ToString();
            LGOS = _lgos;
            LGOE = _lgoe;
            LGOAS = _lgoas;
            LGOAE = _lgoae;

            ALO = _alo.ToString();
            ALOS = _alos;
            ALOE = _aloe;
            ALOAS = _aloas;
            ALOAE = _aloae;

            NIOAS = _nioas;
            NIOAE = _nioae;

            End = _end;
        }

        public Run()
        {
            RunNr = 0;
            SollId = 0;
            IstId = 0;

            SetPoints = new SetPoints();
            ActualValues = new ActualValues();

            Start = "";
            LGO = "-";
            LGOS = "";
            LGOE = "";
            LGOAS = "";
            LGOAE = "";

            ALO = "-";
            ALOS = "";
            ALOE = "";
            ALOAS = "";
            ALOAE = "";

            NIOAS = "";
            NIOAE = "";
            End = "";
        }

        public SetPoints SetPoints { set; get; }
        public ActualValues ActualValues { set; get; }

        public int RunNr { set; get; }

        public int SollId { set; get; }

        public int IstId { set; get; }

        public string Start { set; get; }

        public string LGO { set; get; }

        public string LGOS { set; get; }

        public string LGOE { set; get; }

        public string LGOAS { set; get; }
       
        public string LGOAE { set; get; }
       
        public string ALO { set; get; }
       
        public string ALOS { set; get; }
        
        public string ALOE { set; get; }
       
        public string ALOAS { set; get; }
        
        public string ALOAE { set; get; }

        public string NIOAS { set; get; }

        public string NIOAE { set; get; }

        public string End { set; get; }

        private void SETSetPoints()
        {

            SetPoints ret_val = new SetPoints();

            DataTable DT = (new localDBAdapter("SELECT * FROM Soll WHERE Id = " + SollId.ToString())).DB_Output();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow r in DT.Rows)
                {
                    ret_val = new SetPoints(
                            Convert.ToInt32(r.ItemArray[0]),
                            Convert.ToInt32(r.ItemArray[1]),
                        r.ItemArray[2].ToString(),
                        r.ItemArray[3].ToString(),
                        Convert.ToInt32(r.ItemArray[4]),
                        r.ItemArray[5].ToString(),
                        r.ItemArray[6].ToString(),
                        Convert.ToInt32(r.ItemArray[7]),
                        r.ItemArray[8].ToString(),
                        r.ItemArray[9].ToString(),
                        Convert.ToInt32(r.ItemArray[10]),
                        r.ItemArray[11].ToString(),
                        r.ItemArray[12].ToString(),
                        r.ItemArray[13].ToString(),
                        Convert.ToInt32(r.ItemArray[14]),
                        r.ItemArray[15].ToString(),
                        Convert.ToInt32(r.ItemArray[16]),
                        r.ItemArray[17].ToString(),
                            Convert.ToInt32(r.ItemArray[18]),
                            Convert.ToInt32(r.ItemArray[19]),
                            Convert.ToInt32(r.ItemArray[20]),
                            Convert.ToInt32(r.ItemArray[21]),
                            Convert.ToInt32(r.ItemArray[22]),
                            r.ItemArray[23].ToString(),
                        r.ItemArray[24].ToString(),
                        Convert.ToInt32(r.ItemArray[25]),
                        r.ItemArray[26].ToString(),
                        r.ItemArray[27].ToString(),
                        Convert.ToInt32(r.ItemArray[28]),
                        r.ItemArray[29].ToString(),
                        r.ItemArray[30].ToString(),
                        Convert.ToInt32(r.ItemArray[31]),
                        r.ItemArray[32].ToString(),
                        r.ItemArray[33].ToString(),
                        r.ItemArray[34].ToString(),
                            Convert.ToInt32(r.ItemArray[35]),
                            r.ItemArray[36].ToString(),
                            Convert.ToInt32(r.ItemArray[37]),
                            r.ItemArray[38].ToString(),
                            Convert.ToInt32(r.ItemArray[39]),
                            Convert.ToInt32(r.ItemArray[40]),
                            Convert.ToInt32(r.ItemArray[41]),
                            Convert.ToInt32(r.ItemArray[42]),
                            Convert.ToInt32(r.ItemArray[43]),
                            Convert.ToInt32(r.ItemArray[44]),
                            Convert.ToInt32(r.ItemArray[45]));
                }

            }
            SetPoints = ret_val;
        }

        private void SETActualValues()
        {

            ActualValues ret_val = new ActualValues();

            DataTable DT = (new localDBAdapter("SELECT * FROM Ist WHERE Id = " + IstId.ToString())).DB_Output();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow r in DT.Rows)
                {
                    ret_val =new ActualValues(
                        Convert.ToInt32(r.ItemArray[0]),
                        Convert.ToInt32(r.ItemArray[1]),
                        Convert.ToInt32(r.ItemArray[2]),
                        Convert.ToInt32(r.ItemArray[3]),
                        Convert.ToInt32(r.ItemArray[4]),
                        Convert.ToInt32(r.ItemArray[5]),
                        Convert.ToInt32(r.ItemArray[6]),
                        Convert.ToInt32(r.ItemArray[7]),
                        Convert.ToInt32(r.ItemArray[8]),
                        r.ItemArray[9].ToString(),
                        Convert.ToInt32(r.ItemArray[10]),
                        Convert.ToInt32(r.ItemArray[11]),
                        Convert.ToInt32(r.ItemArray[12]),
                        Convert.ToInt32(r.ItemArray[13]),
                        Convert.ToInt32(r.ItemArray[14]),
                        Convert.ToInt32(r.ItemArray[15]),
                        Convert.ToInt32(r.ItemArray[16]),
                        Convert.ToInt32(r.ItemArray[17]),
                        Convert.ToInt32(r.ItemArray[18]),
                        r.ItemArray[19].ToString(),
                        Convert.ToInt32(r.ItemArray[20]));
                }
            }
            ActualValues = ret_val;

        }
    }
}
