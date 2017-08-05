using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Shapes;

namespace ArmRusGazProm
{
	public partial class Sxema
	{
		public Sxema()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

    public void Zapusk()
    {
      this.kran19.Background = Brushes.Red;
      Thread.Sleep(2000);
      this.kran4.Background = new SolidColorBrush(Color.FromRgb(225,238,228));
      //this.kran3.Background = Brushes.Red;
      Thread.Sleep(2000);

      //DoubleAnimation widthAnimation = new DoubleAnimation();
      //widthAnimation.From = button1.ActualWidth;
      //widthAnimation.To = this.Width - 50;
      //widthAnimation.Duration = TimeSpan.FromSeconds(5);
      //widthAnimation.AutoReverse = true;
      //widthAnimation.SpeedRatio = 5;
      //widthAnimation.RepeatBehavior = new RepeatBehavior(new TimeSpan(13));
      //widthAnimation.AccelerationRatio = 0.5;
      //button1.BeginAnimation(Button.WidthProperty, widthAnimation);
      //this.kamera1.line1.BeginAnimation(Line.RenderTransformProperty.AddOwner(
    }

    public void DoSomeAnimation(Kran[] krans)
    {
      
      

      foreach (var kran in krans)
      {
        Duration duration = new Duration();
        duration = new TimeSpan(0, 0, 5);
        ColorAnimation colorAnimation = new ColorAnimation(Color.FromRgb(250, 235, 215), Colors.LightGreen, duration);

        SolidColorBrush myBackgroundBrush = new SolidColorBrush();
        myBackgroundBrush.Color = Colors.Blue;
        //this.RegisterName("myAnimatedBrush", myBackgroundBrush);
        kran.RegisterName("myAnimatedBrush", myBackgroundBrush);

        kran.KranColor = myBackgroundBrush;

        Storyboard.SetTargetName(colorAnimation, "myAnimatedBrush");
        Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(colorAnimation);

        storyboard.Begin(kran);
      }
    }
	}
}