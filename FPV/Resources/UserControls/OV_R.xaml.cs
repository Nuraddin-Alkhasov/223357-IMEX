﻿using HMI.Views.MainRegion.MachineOverview;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.UserControls
{
    public partial class OV_R : UserControl
    {
        public OV_R()
        {
            InitializeComponent();
        }

        public string Base
        {
            set
            {
                V1 = VS.GetVariable(value);
                V1.Change += Base_Change;
            }
        }

        public string Rack1Visibility
        {
            set
            {
                V2 = VS.GetVariable(value);
                V2.Change += Rack1Visibility_Change;
            }
        }

        public string Rack2Visibility
        {
            set
            {
                V3 = VS.GetVariable(value);
                V3.Change += Rack2Visibility_Change;
            }
        }

        public string RackStatus
        {
            set
            {
                R1.VariableName = value;
                R2.VariableName = value;
            }
        }

        IVariableService VS = ApplicationService.GetService<IVariableService>();

        IVariable V1;
        IVariable V2;
        IVariable V3;
        
        private void Base_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                B.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
            }
            else
            {
                B.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
            }
        }

        private void Rack1Visibility_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                R1.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
            }
            else
            {
                R1.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
            }
        }

        private void Rack2Visibility_Change(object sender, VariableEventArgs e)
        {
            if ((bool)e.Value)
            {
                R2.BeginAnimation(UIElement.OpacityProperty, SetOpacity(1.0));
            }
            else
            {
                R2.BeginAnimation(UIElement.OpacityProperty, SetOpacity(0.0));
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
