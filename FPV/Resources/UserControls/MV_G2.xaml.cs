using HMI.Views.MainRegion.MachineOverview;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.UserControls
{
    public partial class MV_G2 : UserControl
    {
        public MV_G2()
        {
            InitializeComponent();
        }

        public string GhostVisibility
        {
            set
            {
                Ghost.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
                V1 = VS.GetVariable(value);
                V1.Change += GhostVisibility_Change;
            }
        }


        IVariableService VS = ApplicationService.GetService<IVariableService>();

        IVariable V1;



        private void GhostVisibility_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                Ghost.Visibility = Visibility.Visible;

            }
            else
            {
                Ghost.Visibility = Visibility.Hidden;

            }
        }


        private DoubleAnimation SetOpacity(Double _O)
        {
            return new DoubleAnimation
            {
                To = _O,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = TimeSpan.FromSeconds(1),
            };
        }


    }
}
