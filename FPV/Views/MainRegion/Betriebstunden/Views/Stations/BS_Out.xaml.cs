﻿using VisiWin.ApplicationFramework;

namespace HMI.Views.MainRegion.Betriebstunden
{
	/// <summary>
	/// Interaction logic for BSStepEdit.xaml
	/// </summary>
	[ExportView("BS_Out")]
	public partial class BS_Out : VisiWin.Controls.View
	{
       
        public BS_Out()
		{
			this.InitializeComponent();
        }

        private void Btn1_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn1.IsSelected = true;
        }

        private void Btn2_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn2.IsSelected = true;
        }

        private void Btn3_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            btn3.IsSelected = true;
        }
    }
}