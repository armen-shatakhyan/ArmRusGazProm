using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace ArmRusGazProm
{
  /// <summary>
  /// Interaction logic for Kran.xaml
  /// </summary>
  public partial class Kran : UserControl
  {
    public Kran()
    {
      InitializeComponent();
      //KranColor = Brushes.Gray;
    }

    public Brush KranColor
    {
      set { ellipse1.Fill = value; polygon1.Fill = value; polygon2.Fill = value; }
    }

    public void DoSomeAnimation()
    {
      Duration duration = new Duration();
      duration = new TimeSpan(0, 0, 1);
      DoubleAnimation scaleXanimation = new DoubleAnimation(0.5, 2, duration);
      DoubleAnimation scaleYanimation = new DoubleAnimation(0.6, 2.4, duration);
      ScaleTransform transform = new ScaleTransform();
      scaleXanimation.AutoReverse = true;
      scaleXanimation.SpeedRatio = 2;
      scaleYanimation.AutoReverse = true;
      scaleYanimation.SpeedRatio = 2;
      //Double angle = Double.Parse(this.RenderTransform.GetValue(RotateTransform.AngleProperty).ToString());
     
      //transform.SetValue(RotateTransform.AngleProperty, angle);
      this.RenderTransform = transform;

      this.RegisterName("Transform", transform);

      //KranColor = myBackgroundBrush;

      Storyboard.SetTargetName(scaleXanimation, "Transform");
      Storyboard.SetTargetProperty(scaleXanimation, new PropertyPath(ScaleTransform.ScaleXProperty));
      Storyboard.SetTargetName(scaleYanimation, "Transform");
      Storyboard.SetTargetProperty(scaleYanimation, new PropertyPath(ScaleTransform.ScaleYProperty));

      Storyboard storyboard = new Storyboard();
      storyboard.Children.Add(scaleXanimation);
      storyboard.Children.Add(scaleYanimation);

      storyboard.Stop();

      storyboard.Begin(this);

      //
      //this.ellipse1.BeginAnimation(Ellipse.FillProperty, colorAnimation);
      //this.re
      
    }
  }
}
