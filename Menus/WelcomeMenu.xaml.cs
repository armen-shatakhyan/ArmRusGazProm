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
	public partial class WelcomeMenu
	{
		public WelcomeMenu()
		{
			this.InitializeComponent();
          buttonEnter.MouseEnter += buttonEnter_MouseEnter;
          buttonEnter.MouseLeave += buttonEnter_MouseLeave;

          this.textBlock1.Text = "Իրան - Հայաստան Մայր Գազատարի\n Մաքրման Հանգույցների\n Կառավարման Վահանակ";

			// Insert code required on object creation below this point.
		}

        private void buttonEnter_MouseEnter(object sender, MouseEventArgs e)
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

        private void buttonEnter_MouseLeave(object sender, MouseEventArgs e)
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