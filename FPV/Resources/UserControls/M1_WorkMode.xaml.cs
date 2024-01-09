using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI.UserControls
{
    public partial class M1_WorkMode : UserControl
    {
       
        string var_Automaric = "";
        string var_Manual = "";
        string var_SetUP = "";

        public string PowerOn
        {
            set
            { 
                PWO = VS.GetVariable(value);
                PWO.Change += PWO_Change;
            }
        }

        public string WorkingModeStatus
        {
            set
            {
                WM = VS.GetVariable(value);
                WM.Change += WorkingMode_ValueChanged;
            }
        }

        public string Automatic
        {
            set
            {
                var_Automaric = value;
            }
        }

        public string Hand
        {
            set
            {
                var_Manual = value;
            }
        }

        public string SetUp
        {
            set
            {
                var_SetUP = value;
            }
        }

        IVariableService VS = ApplicationService.GetService<IVariableService>();
        IVariable WM;
        IVariable PWO;

        public M1_WorkMode()
        {
            InitializeComponent();
           
        }

        private void PWO_Change(object sender, VariableEventArgs e)
        {
            if (e.Value != e.PreviousValue && (short)e.Value == 2)
            {
                if (!(bool)ApplicationService.GetVariableValue(var_Automaric) && !(bool)ApplicationService.GetVariableValue(var_Manual) && !(bool)ApplicationService.GetVariableValue(var_SetUP))
                {
                    ApplicationService.SetVariableValue(var_Automaric, false);
                    ApplicationService.SetVariableValue(var_Manual, false);
                    ApplicationService.SetVariableValue(var_SetUP, true);
                }
            }
        }

        private void WorkingMode_Click(object sender, RoutedEventArgs e)
        {
            switch (WM.Value.ToString())
            {
                case "0":
                    ApplicationService.SetVariableValue(var_Automaric, false);
                    ApplicationService.SetVariableValue(var_Manual, false);
                    ApplicationService.SetVariableValue(var_SetUP, true);
                    break;
                case "1": 
                    ApplicationService.SetVariableValue(var_Manual, false);
                    ApplicationService.SetVariableValue(var_SetUP, false);
                    ApplicationService.SetVariableValue(var_Automaric, true);
                    break;
                case "2":
                    ApplicationService.SetVariableValue(var_Automaric, false);
                    ApplicationService.SetVariableValue(var_SetUP, false);
                    ApplicationService.SetVariableValue(var_Manual, true);
                    break;
                case "3":
                    ApplicationService.SetVariableValue(var_Automaric, false);
                    ApplicationService.SetVariableValue(var_Manual, false);
                    ApplicationService.SetVariableValue(var_SetUP, true);
                    break;
                default: break;
            }
        }

        private void WorkingMode_ValueChanged(object sender, VariableEventArgs e)
        {
            switch ((short)e.Value)
            {
                default:                    //no mode
                    WorkingMode.LocalizableText = "@MainOverview.Text22";
                    BtnToInvisible();
                    BtnToCenter();
                    WorkingMode.Tag = "0";
                    break;
                case 1:                    //hand 
                    WorkingMode.LocalizableText = "@MainOverview.Text20";
                    BtnToInvisible();
                    BtnToCenter();
                    WorkingMode.Tag = "1";
                    break;
                case 2:                    //set up
                    WorkingMode.LocalizableText = "@MainOverview.Text21";
                    BtnToInvisible();
                    BtnToCenter();
                    WorkingMode.Tag = "2";
                    break;
                case 3:                    //auto
                    WorkingMode.LocalizableText = "@MainOverview.Text19";
                    btnToVisible();
                    btnToLeft();
                    WorkingMode.Tag = "3";
                    break;
            }
        }

        private void btnToVisible()
        {
            btnstart.IsEnabled = true;
            btnstop.IsEnabled = true;
            btnstart.Visibility = Visibility.Visible;
            btnstop.Visibility = Visibility.Visible;

            var animation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
            };

            btnstart.BeginAnimation(UIElement.OpacityProperty, animation);
            btnstop.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void BtnToInvisible()
        {
            btnstart.IsEnabled = false;
            btnstop.IsEnabled = false;

            var animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
            };

            btnstart.BeginAnimation(UIElement.OpacityProperty, animation);
            btnstop.BeginAnimation(UIElement.OpacityProperty, animation);

            Task obTask = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(300);    
                Application.Current.Dispatcher.InvokeAsync((Action)delegate
                {
                    if (btnstart.Opacity == 0)
                    {
                        btnstart.Visibility = Visibility.Collapsed;
                        btnstop.Visibility = Visibility.Collapsed;
                    }
                   
                });
            });
           
        }

        private void BtnToCenter()
        {
            var sb = new Storyboard();
            var ta = new ThicknessAnimation();
            ta.BeginTime = new TimeSpan(0);
            ta.SetValue(Storyboard.TargetNameProperty, "WorkingMode");
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

            ta.To = new Thickness(25, 0, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            sb.Children.Add(ta);
            sb.Begin(this);


        }

        private void btnToLeft()
        {
            var sb = new Storyboard();
            var ta = new ThicknessAnimation();
            ta.BeginTime = new TimeSpan(0);
            ta.SetValue(Storyboard.TargetNameProperty, "WorkingMode");
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

            ta.To = new Thickness(0, 0, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            sb.Children.Add(ta);
            sb.Begin(this);


        }

    }
}
