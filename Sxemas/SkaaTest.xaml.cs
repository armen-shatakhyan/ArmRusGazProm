using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArmRusGazProm.Sxemas
{
  /// <summary>
  /// Interaction logic for SpdTest.xaml
  /// </summary>
  public partial class SkaaTest : UserControl
  {
    private ModeEnum modeEnum;
    readonly BackgroundWorker m_backgroundWorker;

    public SkaaTest()
    {
      InitializeComponent();
      m_backgroundWorker = new BackgroundWorker();
      m_backgroundWorker.DoWork += back_DoWork;
      m_backgroundWorker.RunWorkerCompleted += back_RunWorkerCompleted;
      lcdLabel.Visibility = Visibility.Hidden;
    }

    public ModeEnum ModeEnum
    {
      get { return modeEnum; }
      set { modeEnum = value; }
    }

    private void back_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      var brush = new SolidColorBrush(Color.FromArgb(0, 250, 235, 215));
      switch (e.Result.ToString())
      {
        case "1":
          PakanAnimation(State.Open, "1");
          infoLabel.Visibility = Visibility.Visible;
          infoLabel.Text = "V1 փականը բաց է:";
          m_backgroundWorker.RunWorkerAsync("2");
          break;
        case "2":
          PakanAnimation(State.Open, "2");
          txtPchamakrum.Visibility = Visibility.Visible;
          pthPchamakrum.Visibility = Visibility.Visible;
          rctPchamakrum1.Fill = Brushes.White;
          rctPchamakrum2.Fill = Brushes.White;
          infoLabel.Text = "V2 փականը բաց է: Կատարվում է փչամաքրում:";
          m_backgroundWorker.RunWorkerAsync("3");
          break;
        case "3":
          {
            var bc = new BrushConverter();
            var b = (SolidColorBrush) bc.ConvertFrom("#FF8DD7F7");
            PakanAnimation(State.Close, "1");
            infoLabel.Text = "V1 փականը փակ է:"; // #FF8DD7F7
            txtPchamakrum.Visibility = Visibility.Hidden;
            pthPchamakrum.Visibility = Visibility.Hidden;
            rctPchamakrum1.Fill = b;
            rctPchamakrum2.Fill = b;
            m_backgroundWorker.RunWorkerAsync("4");
            break;
          }

        case "4":
          {
            PakanAnimation(State.Close, "2");
            infoLabel.Text = "V2 փականը փակ է:";
            m_backgroundWorker.RunWorkerAsync("5");
            break;
          }
        case "5":
          //PakanAnimation(State.Close, "2");
          lcdLabel.Text = "100";
          lcdLabel.Visibility = Visibility.Visible;
          LcdAnimation();
          infoLabel.Text = "Ճնշման արժեքը 100 կՊա է:";
          m_backgroundWorker.RunWorkerAsync("6");
          break;
        case "6":
          PakanAnimation(State.Open, "1");
          infoLabel.Text = "V1 փականը բաց է:";
          m_backgroundWorker.RunWorkerAsync("7");
          break;
        case "7":
          lcdLabel.Text = "2,5";
          lcdLabel.Visibility = Visibility.Visible;
          LcdAnimation();
          infoLabel.Text = "Ճնշման արժեքը 2,5 ՄՊա է: Ստուգումն ավարտված է:";
          //m_backgroundWorker.RunWorkerAsync("6");
          break;
      }
    }

    private void back_DoWork(object sender, DoWorkEventArgs e)
    {
      switch (e.Argument.ToString())
      {
        case "1":
          Thread.Sleep(3000);
          e.Result = "1";
          break;
        case "2":
          Thread.Sleep(3000);
          e.Result = "2";
          break;
        case "3":
          Thread.Sleep(3000);
          e.Result = "3";
          break;
        case "4":
          Thread.Sleep(3000);
          e.Result = "4";
          break;
        case "5":
          Thread.Sleep(3000);
          e.Result = "5";
          break;
        case "6":
          Thread.Sleep(3000);
          e.Result = "6";
          break;
        case "7":
          Thread.Sleep(3000);
          e.Result = "7";
          break;
      }
    }

    private void radioButtonAshxatanqayin_Checked(object sender, RoutedEventArgs e)
    {
      ModeEnum = radioButtonAshxatanqayin.IsChecked == true ? ModeEnum.Ashxatanqayin : ModeEnum.Cucadrakan;
    }

    private void PakanAnimation(State state, params string[] list)
    {
      //System.Media.SystemSounds.Exclamation.Play();
      foreach (var i in list)
      {
        var textBlock = LayoutRootCanvas.FindName("PakanLabel" + i) as TextBlock;
        var pakan = LayoutRootCanvas.FindName("Pakan" + i) as Path;
        //label.Foreground = (state == State.Open) ? Brushes.Green : Brushes.Brown;
        //kran.KranColor = (state == State.Open) ? Brushes.LightGreen : Brushes.Brown;

        switch (state)
        {
          case State.Open:
            textBlock.Foreground = Brushes.Green;
            pakan.Fill = Brushes.LightGreen;
            break;
          case State.Close:
            textBlock.Foreground = Brushes.Brown;
            pakan.Fill = Brushes.Brown;
            break;
          case State.AvariaOpen:
            textBlock.Foreground = Brushes.Red;
            pakan.Fill = Brushes.Red;
            break;
          case State.AvariaClose:
            textBlock.Foreground = Brushes.Pink;
            pakan.Fill = Brushes.Pink;
            break;
        }

        Duration duration = new TimeSpan(0, 0, 2);

        var fontSizeAnimation = new DoubleAnimation(14, 100, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(fontSizeAnimation, textBlock.Name);
        Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

        var heightAnimation = new DoubleAnimation(30, 300, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(heightAnimation, textBlock.Name);
        Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

        var widthAnimation = new DoubleAnimation(40, 300, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(widthAnimation, textBlock.Name);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(fontSizeAnimation);
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(widthAnimation);
        storyboard.Stop();
        storyboard.Begin(textBlock);

        //if (ModeEnum == ModeEnum.Cucadrakan) continue;
        //switch (state)
        //{
        //  case State.Open:
        //    m_infoText += string.Format("Փ{0} բացված է:\n", i);
        //    break;
        //  case State.Close:
        //    m_infoText += string.Format("Փ{0} փակված է:\n", i);
        //    break;
        //  case State.AvariaOpen:
        //    m_errorText += string.Format("Փ{0} բացված է ՎԹԱՐԱՅԻՆ!\n", i);
        //    break;
        //  case State.AvariaClose:
        //    m_errorText += string.Format("Փ{0} փակված է ՎԹԱՐԱՅԻՆ!\n", i);
        //    break;
        //}
      }
    }

    private void LcdAnimation()
    {
        Duration duration = new TimeSpan(0, 0, 2);

        var fontSizeAnimation = new DoubleAnimation(64, 256, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(fontSizeAnimation, lcdLabel.Name);
        Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

        var heightAnimation = new DoubleAnimation(300, 3000, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(heightAnimation, lcdLabel.Name);
        Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

        var widthAnimation = new DoubleAnimation(400, 3000, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(widthAnimation, lcdLabel.Name);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(fontSizeAnimation);
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(widthAnimation);
        storyboard.Stop();
        storyboard.Begin(lcdLabel);
      
    }

    private void buttonZapusk_Click(object sender, RoutedEventArgs e)
    {
      if (ModeEnum == ModeEnum.Cucadrakan)
      {
        m_backgroundWorker.RunWorkerAsync("1");
        var color = (Color)ColorConverter.ConvertFromString("#FF8D0707");
        var myBrush = new SolidColorBrush(color);
        infoLabel.Text = "";
        infoLabel.Foreground = myBrush;
      }
      else
      {
        infoLabel.Foreground = Brushes.Red;
        infoLabel.Text = " Սխալ: Անհնար է կապ հաստատել ԱԿՀ - ի հետ. ";
      }
    }
  }
}
