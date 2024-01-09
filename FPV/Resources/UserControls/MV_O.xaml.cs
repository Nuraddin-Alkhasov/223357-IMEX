using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Language;

namespace HMI.UserControls
{
    public partial class MV_O : UserControl
    {
        public MV_O()
        {
            InitializeComponent();
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

       
       

        public string OvenId
        {
            set { _OvenId = value; }
        }

        private string _OvenId;

        public string OvenType
        {
            set
            {
                switch (value)
                {
                    case "LGO": O.SymbolResourceKey = "MOLGO"; break;
                    case "ALO": O.SymbolResourceKey = "MOALO"; break;
                }
            }
        }

        public string OvenStatus { get; set; }
        public string OvenSimulation { get; set; }
        public string NachlaufStatus { get; set; }
        public string DoorPosition { get; set; }
        public string _Base;
        public string Base
        {
            get { return _Base; }
            set
            {
                _Base = value;
                R.Base = value;
            }
        }

        public string OvenName
        {
            set
            {
                _Name.LocalizableText = value;
            }
        }

        public string OvenOrder
        {
            set
            {
                OvenO.VariableName = value;
            }
        }

        public string OvenActualTemp
        {
            set
            {
                OvenA.VariableName = value;
            }
        }
        public string OvenActualH
        {
            set
            {
                OvenAH.VariableName = value;
            }
        }

        public string OvenActualM
        {
            set
            {
                OvenAM.VariableName = value;
            }
        }

        public string OvenSetTemp
        {
            set
            {
                OvenS.VariableName = value;
            }
        }
        public string OvenSetH
        {
            set
            {
                OvenSH.VariableName = value;
            }
        }

        public string OvenSetM
        {
            set
            {
                OvenSM.VariableName = value;
            }
        }

       

        IVariableService VS = ApplicationService.GetService<IVariableService>();

        IVariable V1;
        IVariable V2;
        IVariable V3;
        IVariable V4;
        IVariable V5;

        private void Base_Change(object sender, VariableEventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                if ((bool)e.Value)
                {
                    O.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.4));
                }
                else
                {
                    O.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.8));
                }
            });
        }

        private void OvenStatus_Change(object sender, VariableEventArgs e)
        {
            
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                if (V4 == null)
            return;

                if ((short)V4.Value == 0)
                {
                    if (OnOff.Background.IsFrozen)
                        OnOff.Background = OnOff.Background.CloneCurrentValue();

                    if ((short)e.Value == 1)
                    {
                        OnOff.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetColor((Color)ColorConverter.ConvertFromString("#FF12C312")));
                        OvenO.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                        OvenSS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                        OvenAS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                    }
                    else
                    {
                        OnOff.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetColor((Color)ColorConverter.ConvertFromString("#FF006AAD")));
                        OvenO.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                        OvenSS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                        OvenAS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                    }
                }
            });
            
        }

        private void OvenSimulation_Change(object sender, VariableEventArgs e)
        {
            
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    if (OnOff.Background.IsFrozen)
                    OnOff.Background = OnOff.Background.CloneCurrentValue();

                    if ((short)e.Value == 1)
                    {
                        OnOff.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetColorSim((Color)ColorConverter.ConvertFromString("#FF12C312")));
                        OvenO.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                        OvenSS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                        OvenAS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                    }
                    else
                    {
                        OnOff.Background.BeginAnimation(SolidColorBrush.ColorProperty, SetColor((Color)ColorConverter.ConvertFromString("#FF006AAD")));
                        OvenO.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                        OvenSS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                        OvenAS.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0));
                    }
                });
            
        }

        private void DoorPosition_Change(object sender, VariableEventArgs e)
        {
          //  Application.Current.Dispatcher.InvokeAsync((Action)delegate
           // {
                
                    float pos = (float)e.Value;
                    Door.Margin = new Thickness(30, 0, 30, pos / 44.24 + 8);
                
            //});
        }

        private void NachlaufStatus_Change(object sender, VariableEventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                if ((bool)e.Value)
                {
                    NL.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
                }
                else
                {
                    NL.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
                }
            });
           
        }

        private void OpenOvenView(object sender, RoutedEventArgs e)
        {
            ApplicationService.SetView("MainRegion", "MO_Oven", _OvenId);
        }

        private DoubleAnimation SetOpacity(Double _O)
        {
            return new DoubleAnimation
            {
                To = _O,
                Duration = TimeSpan.FromSeconds(1),
            };
        }

        private ColorAnimation SetColor(Color _C)
        {
            return new ColorAnimation
            {
                To = _C,
                Duration = TimeSpan.FromSeconds(0.3),
            };
        }

        private ColorAnimation SetColorSim(Color _C)
        {
            return new ColorAnimation
            {
                From = (Color)ColorConverter.ConvertFromString("#FF006AAD"),
                To = (Color)ColorConverter.ConvertFromString("#FF12C312"),
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };
        }

        private bool loaded=false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                Task obTask = Task.Run(() =>
              {
                  Application.Current.Dispatcher.InvokeAsync((Action)delegate
                  {
                      Layout.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1));
                  });
              });

                V4 = VS.GetVariable(OvenSimulation);
                V4.Change += OvenSimulation_Change;

                V1 = VS.GetVariable(Base);
                V1.Change += Base_Change;

                V2 = VS.GetVariable(OvenStatus);
                V2.Change += OvenStatus_Change;

                V3 = VS.GetVariable(NachlaufStatus);
                V3.Change += NachlaufStatus_Change;

               

                V5 = VS.GetVariable(DoorPosition);
                V5.Change += DoorPosition_Change;

                loaded = true;
            }
        }

      
    }
}
