using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArmRusGazProm
{
	public partial class KameraCanvas
	{
		public KameraCanvas()
		{
			this.InitializeComponent();
		}

    public Brush KameraColor
    {
      set { pathErankjun.Fill = value; pathUxankyun.Fill = value; }
    }

    public void OpenKamera()
    {
      Duration duration = new Duration();
      duration = new TimeSpan(0, 0, 3);
      DoubleAnimation animation = new DoubleAnimation(0, 90, duration);

      //line2.BeginAnimation(RotateTransform.AngleProperty, animation);

      RotateTransform animatedRotateTransform = new RotateTransform();
      animatedRotateTransform.CenterX = 0;
      animatedRotateTransform.CenterY = -20;
      line2.RenderTransform = animatedRotateTransform;
      this.RegisterName("AnimatedRotateTransform", animatedRotateTransform);

      Storyboard.SetTargetName(animation, "AnimatedRotateTransform");
      Storyboard.SetTargetProperty(animation,
          new PropertyPath(RotateTransform.AngleProperty));

      Storyboard storyboard = new Storyboard();
      storyboard.Children.Add(animation);
      storyboard.Stop();
      storyboard.Begin(line1);

    }
	}
}