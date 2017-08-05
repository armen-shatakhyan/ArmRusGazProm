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
	public partial class ModeMenu
	{
		public ModeMenu()
		{
			this.InitializeComponent();

      cucadrakanBtn.MouseEnter += CucadrakanBtnMouseEnter;
      ashxatanqayinBtn.MouseEnter += CucadrakanBtnMouseEnter;
      stugmanBtn.MouseEnter += CucadrakanBtnMouseEnter;
      cucadrakanBtn.MouseLeave += CucadrakanBtnMouseLeave;
      ashxatanqayinBtn.MouseLeave += CucadrakanBtnMouseLeave;
      stugmanBtn.MouseLeave += CucadrakanBtnMouseLeave;
		}

    private void CucadrakanBtnMouseEnter(object sender, MouseEventArgs e)
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

    private void CucadrakanBtnMouseLeave(object sender, MouseEventArgs e)
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
	}
}