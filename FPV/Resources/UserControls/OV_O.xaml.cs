using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.UserControls
{
    public partial class OV_O : UserControl
    {
        public OV_O()
        {
            InitializeComponent();
        }

        public string Base
        {
            set
            {
                R.Base = value;
                V1 = VS.GetVariable(value);
                V1.Change += Base_Change;
            }
        }

        public string Rack1Visibility
        {
            set
            {
                R.Rack1Visibility = value;
            }
        }

        public string Rack2Visibility
        {
            set
            {
                R.Rack2Visibility = value;
            }
        }

        public string RackStatus
        {
            set
            {
                R.R1.VariableName = value;
                R.R2.VariableName = value;
            }
        }

        public string DoorStatus
        {
            set { D.VariableName = value; }
        }

        public string OvenType
        {
            set
            {
                switch (value)
                {
                    case "LGO": O.SymbolResourceKey = "O2"; break;
                    case "ALO": O.SymbolResourceKey = "O1"; break;
                }
            }
        }

        public string NachlaufTemp
        {
            set
            {
                nachlaufTemp.VariableName = value;
            }
        }

        public string Temperature
        {
            set
            {
                Temp.VariableName = value;
            }
        }

        public string NacharbeitStatus
        {
            set
            {
                V3 = VS.GetVariable(value);
                V3.Change += NacharbeitStatus_Change;
            }
        }

        public string NachlaufStatus
        {
            set
            {
                V4 = VS.GetVariable(value);
                V4.Change += NachlaufStatus_Change;
            }
        }

        readonly IVariableService VS = ApplicationService.GetService<IVariableService>();

        IVariable V1;
        IVariable V3;
        IVariable V4;

        private void Base_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                O.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.4));
            }
            else
            {
                O.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.8));
            }
        }

        private void NachlaufStatus_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                nachlauf.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
            }
            else
            {
                nachlauf.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
            }
        }

        

        private void NacharbeitStatus_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                NL.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
            }
            else
            {
                NL.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
            }
        }

        private DoubleAnimation SetOpacity(Double _O)
        {
            return new DoubleAnimation
            {
                To = _O,
                Duration = TimeSpan.FromSeconds(1),
            };
        }
    }
}
