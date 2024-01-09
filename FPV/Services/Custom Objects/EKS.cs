using EKSEthLib;
using HMI.Views.MessageBoxRegion;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Language;
using VisiWin.UserManagement;

namespace HMI.Services.Custom_Objects
{
    public class ElectronicKeySystem
    {
        public ElectronicKeySystem() 
        {
            userService.UserLoggedOn += UserService_UserLoggedOn;
            userService.UserLoggedOff += UserService_UserLoggedOff;

            IP = "192.168.3.2";
            port = "2444";
            key = "1234567891234567";
            Status = "";
            //userService.LogOn(null, null, "00001");


            VS = ApplicationService.GetService<IVariableService>();
            UserBit = VS.GetVariable("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von MOP zu PC.Anforderung Benutzer");
            UserBit.Change += UserBit_Change;
        }

        #region - - - Properties - - -

        EKSETH EK;
        IUserManagementService userService = ApplicationService.GetService<IUserManagementService>();
        IVariableService VS;
        IVariable UserBit;

        readonly string IP;
        readonly string port;
        readonly string key;
        public string Status { set; get; }

        #endregion

        #region - - - Event Handlers - - -

        private void EKS_OnKey()
        {

            if (EK.KeyState == KeyState_def.EKS_KEY_IN)
            {
                userService.LogOn(null, null, Read());
                if (userService.CurrentUser != null)
                {
                    ILanguageService textService = ApplicationService.GetService<ILanguageService>();

                    string txt1 = textService.GetText("@Admin.Text17");
                    string txt2 = textService.GetText("@Admin.Text18");

                    new MessageBoxTask(txt1 + " " + userService.CurrentUser.FullName + Environment.NewLine + txt2, "@Admin.Text15", MessageBoxIcon.Asterisk);
                }

                Status = Read() + " - is in EKS";
            }
            else if (EK.KeyState == KeyState_def.EKS_KEY_OUT)
            {

                new MessageBoxTask("@Admin.Text20", "@Admin.Text15", MessageBoxIcon.Asterisk);
                userService.LogOff();
                Status = "No Token in EKS";
            }
        }

        private void UserBit_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von MOP zu PC.Anforderung Benutzer", false);
                if (userService.CurrentUser != null)
                {
                    bool a = userService.CurrentUser.RightNames.Contains("MoPMaintenance");
                    if (a)
                    {
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Benutzer Level.Einrichten", true);
                    }
                    else
                    {
                        bool b = userService.CurrentUser.RightNames.Contains("MoPOperator");
                        if (b)
                        {
                            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Benutzer Level.Bediener", true);
                        }
                    }

                    bool c = userService.CurrentUser.RightNames.Contains("CarriageSpecialRight");
                    if (c)
                    {
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Sonderrecht für Fahrwagen", true);
                    }

                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Anmelden", true);
                }
            }
        }

        private void UserService_UserLoggedOn(object sender, LogOnEventArgs e)
        {
            if (userService.CurrentUser != null)
            {
                bool a = userService.CurrentUser.RightNames.Contains("MoPMaintenance");
                if (a)
                {
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Benutzer Level.Einrichten", true);
                }
                else
                {
                    bool b = userService.CurrentUser.RightNames.Contains("MoPOperator");
                    if (b)
                    {
                        ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Benutzer Level.Bediener", true);
                    }
                }

                bool c = userService.CurrentUser.RightNames.Contains("CarriageSpecialRight");
                if (c)
                {
                    ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Sonderrecht für Fahrwagen", true);
                }

                ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Anmelden", true);
            }
               
        }

        private void UserService_UserLoggedOff(object sender, LogOffEventArgs e)
        {
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Abmelden", true);
            ApplicationService.SetVariableValue("IMEX.PLC.Blocks.HMI.DB User Sync.Sync.Von PC zu MOP.Sonderrecht für Fahrwagen", false);
        }

        #endregion

        #region - - - Methods - - -
        public string GetStatus()
        {
            return StatusConvert() + " |-> " + Status;
        }
        public void OpenConnection()
        {
            EK = new EKSETH
            {
                IPAddress = IP,
                Port = port,
                KeyType = KeyType_def.EKS_KEY_READWRITE,
                StartAdress = 0,
                CountData = 12
            };

            EK.OnKey += EKS_OnKey;

            Task obTask = Task.Run(async () =>
            {
                await Task.Delay(8000);
                EK.Open();
            });
        }
        public void CloseConnection()
        {
            if (EK != null)
            {
                Task obTask = Task.Run(() =>
                {
                    EK.Close();
                    EK = null;
                    new MessageBoxTask("@Admin.Text11", "@Admin.Text15", MessageBoxIcon.Information); 
                });
                try
                {
                    EK.OnKey -= EKS_OnKey;
                }
                catch
                { }
            }
            else
            {
                new MessageBoxTask("@Admin.Text16", "@Admin.Text15", MessageBoxIcon.Error);
            }
        }
        public string Read()
        {
            string ret_val = "";
            if(EK!=null)
                if (EK.KeyState == KeyState_def.EKS_KEY_IN)
                {
                    byte[] encrData = new byte[12];
                    for (short i = 0; i < 12; i++)
                    {
                        encrData[i] = Convert.ToByte(EK.getData(i));
                    }
                    try
                    {
                        return Decrypt(encrData);
                    }
                    catch
                    {
                        new MessageBoxTask("@Admin.Text19", "@Admin.Text15", MessageBoxIcon.Information);
                        ret_val = "";
                    }
                }
                else
                {
                    ret_val= "";
                }
            return ret_val;
        }
        public void Write(string data)
        {
            if (EK != null)
                if (EK.KeyState == KeyState_def.EKS_KEY_IN)
                {
                    string a = Encrypt(data);
                    byte[] encrData = UTF8Encoding.UTF8.GetBytes(a);

                    for (short i = 0; i < 12; i++)
                    {
                        EK.setData(i, encrData[i]);

                    }
                    EK.Write();
                }
        }
        private string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private string Decrypt(byte[] Data)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            byte[] toEncryptArray = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(Data, 0, Data.Length));

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private string StatusConvert() 
        {
            if (EK != null)
            {
                switch (EK.LastState) 
                {
                    case 144: return "WrongParam";  
                    case 160: return "DeviceNotOpened"; 
                    case 176: return "ReadTimeOut"; 
                    case 177: return "WriteTimeOut"; 
                    case 178: return "TimeOut"; 
                    case 192: return "NothingToRead"; 
                    case 193: return "NothingToWrite"; 
                    case 224: return "OpenFailed"; 
                    case 225: return "OpenActive"; 
                    case 232: return "Suspend"; 
                    case 233: return "ResumeSuspend"; 
                    case 234: return "ConnectionTimeOut"; 
                    case 235: return "ConnectionLost"; 
                    case 236: return "Reconnect"; 
                    case 255: return "Busy";
                    default: return "OK"; 
                }
            }
            else 
            {
                return "EK == null";
            }

        }
        #endregion

    }
}
