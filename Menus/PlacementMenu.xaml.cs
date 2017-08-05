using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Input;

namespace ArmRusGazProm
{
	public partial class PlacementMenu
	{
		public PlacementMenu()
		{
			this.InitializeComponent();

      mexriKajaranBtn.MouseEnter += MexriKajaranBtnMouseEnter;
      kajaranSisianBtn.MouseEnter += MexriKajaranBtnMouseEnter;
      angexakotJermukBtn.MouseEnter += MexriKajaranBtnMouseEnter;
      jermukGetapBtn.MouseEnter += MexriKajaranBtnMouseEnter;
      getapAraratBtn.MouseEnter += MexriKajaranBtnMouseEnter;
      mexriKajaranBtn.MouseLeave += MexriKajaranBtnMouseLeave;
      kajaranSisianBtn.MouseLeave += MexriKajaranBtnMouseLeave;
      angexakotJermukBtn.MouseLeave += MexriKajaranBtnMouseLeave;
      jermukGetapBtn.MouseLeave += MexriKajaranBtnMouseLeave;
      getapAraratBtn.MouseLeave += MexriKajaranBtnMouseLeave;
		}

    private void MexriKajaranBtnMouseEnter(object sender, MouseEventArgs e)
    {
      var button = sender as Button;
      var collection = new GradientStopCollection
                         {
                           new GradientStop(Color.FromRgb(255, 255, 255), 0),
                           new GradientStop(Color.FromRgb(152, 181, 207), 0.923)
                         };
      var brush = new LinearGradientBrush(collection, new Point(0.5, 0), new Point(0.5, 1));
      button.Background = brush;
    }

    private void MexriKajaranBtnMouseLeave(object sender, MouseEventArgs e)
    {
      var button = sender as Button;
      var collection = new GradientStopCollection
                         {
                           new GradientStop(Color.FromRgb(255, 255, 255), 0),
                           new GradientStop(Color.FromRgb(205, 205, 157), 0.923)
                         };
      var brush = new LinearGradientBrush(collection, new Point(0.5, 0), new Point(0.5, 1));
      button.Background = brush;
    }

	  public string Title
	  {
      get { return textBlock1.Text; }
      set { textBlock1.Text = value; }
	  }
	}
}