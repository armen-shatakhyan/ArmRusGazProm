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
  public partial class EppTest : UserControl
  {
    private ModeEnum modeEnum;
    readonly BackgroundWorker m_backgroundWorker;

    public EppTest()
    {
      InitializeComponent();
      m_backgroundWorker = new BackgroundWorker();
      m_backgroundWorker.DoWork += back_DoWork;
      m_backgroundWorker.RunWorkerCompleted += back_RunWorkerCompleted;
    }

    public ModeEnum ModeEnum
    {
      get { return modeEnum; }
      set { modeEnum = value; }
    }

    private void radioButtonCucadrakan_Checked(object sender, RoutedEventArgs e)
    {
      ModeEnum = radioButtonAshxatanqayin.IsChecked == true ? ModeEnum.Ashxatanqayin : ModeEnum.Cucadrakan;
    }

    private void back_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      infoLabel.Visibility = Visibility.Visible; // #FF8D0707
      var convertFromString = ColorConverter.ConvertFromString("#FF8D0707");
      if (convertFromString != null)
      {
        var color = (Color)convertFromString;
        infoLabel.Foreground = new SolidColorBrush(color);
      }

      switch (e.Result.ToString())
      {
        case "00":
          PakanAnimation(State.Open, "00");
          infoLabel.Text = "Փական N00 բացված է"; 
          m_backgroundWorker.RunWorkerAsync("00c");
          break;
        case "00c":
          PakanAnimation(State.Close, "00");
          infoLabel.Text = "Փական N00 փակված է"; 
          m_backgroundWorker.RunWorkerAsync("01");
          break;
        case "01":
          PakanAnimation(State.Open, "01");
          infoLabel.Text = "Փական N01 բացված է"; 
          m_backgroundWorker.RunWorkerAsync("01c");
          break;
        case "01c":
          PakanAnimation(State.Close, "01");
          infoLabel.Text = "Փական N01 փակված է:"; 
          m_backgroundWorker.RunWorkerAsync("02");
          break;
        case "02":
          PakanAnimation(State.Open, "02");
          infoLabel.Text = "Փական N02 բացված է";
          m_backgroundWorker.RunWorkerAsync("02c");
          break;
        case "02c":
          PakanAnimation(State.Close, "02");
          infoLabel.Text = "Փական N02 փակված է";
          m_backgroundWorker.RunWorkerAsync("03");
          break;
        case "03":
          PakanAnimation(State.Open, "03");
          infoLabel.Text = "Փական N03 բացված է";
          m_backgroundWorker.RunWorkerAsync("03c");
          break;
        case "03c":
          PakanAnimation(State.Close, "03");
          infoLabel.Text = "Փական N03 փակված է:";
          m_backgroundWorker.RunWorkerAsync("04");
          break;
        case "04":
          PakanAnimation(State.Open, "04");
          infoLabel.Text = "Փական N04 բացված է";
          m_backgroundWorker.RunWorkerAsync("04c");
          break;
        case "04c":
          PakanAnimation(State.Close, "04");
          infoLabel.Text = "Փական N04 փակված է";
          m_backgroundWorker.RunWorkerAsync("05");
          break;
        case "05":
          PakanAnimation(State.Open, "05");
          infoLabel.Text = "Փական N05 բացված է";
          m_backgroundWorker.RunWorkerAsync("05c");
          break;
        case "05c":
          PakanAnimation(State.Close, "05");
          infoLabel.Text = "Փական N05 փակված է:";
          m_backgroundWorker.RunWorkerAsync("06");
          break;
        case "06":
          PakanAnimation(State.Open, "06");
          infoLabel.Text = "Փական N06 բացված է";
          m_backgroundWorker.RunWorkerAsync("06c");
          break;
        case "06c":
          PakanAnimation(State.Close, "06");
          infoLabel.Text = "Փական N06 փակված է";
          m_backgroundWorker.RunWorkerAsync("07");
          break;
        case "07":
          PakanAnimation(State.Open, "07");
          infoLabel.Text = "Փական N07 բացված է";
          m_backgroundWorker.RunWorkerAsync("07c");
          break;
        case "07c":
          PakanAnimation(State.Close, "07");
          infoLabel.Text = "Փական N07 փակված է:";
          m_backgroundWorker.RunWorkerAsync("08");
          break;
        case "08":
          PakanAnimation(State.Open, "08");
          infoLabel.Text = "Փական N08 բացված է";
          m_backgroundWorker.RunWorkerAsync("08c");
          break;
        case "08c":
          PakanAnimation(State.Close, "08");
          infoLabel.Text = "Փական N08 փակված է";
          m_backgroundWorker.RunWorkerAsync("09");
          break;
        case "09":
          PakanAnimation(State.Open, "09");
          infoLabel.Text = "Փական N09 բացված է";
          m_backgroundWorker.RunWorkerAsync("09c");
          break;
        case "09c":
          PakanAnimation(State.Close, "09");
          infoLabel.Text = "Փական N09 փակված է:";
          m_backgroundWorker.RunWorkerAsync("10");
          break;
        case "10":
          PakanAnimation(State.Open, "10");
          infoLabel.Text = "Փական N10 բացված է";
          m_backgroundWorker.RunWorkerAsync("10c");
          break;
        case "10c":
          PakanAnimation(State.Close, "10");
          infoLabel.Text = "Փական N10 փակված է";
          m_backgroundWorker.RunWorkerAsync("11");
          break;
        case "11":
          PakanAnimation(State.Open, "11");
          infoLabel.Text = "Փական N11 բացված է";
          m_backgroundWorker.RunWorkerAsync("11c");
          break;
        case "11c":
          PakanAnimation(State.Close, "11");
          infoLabel.Text = "Փական N11 փակված է:";
          m_backgroundWorker.RunWorkerAsync("12");
          break;
        case "12":
          PakanAnimation(State.Open, "12");
          infoLabel.Text = "Փական N12 բացված է";
          m_backgroundWorker.RunWorkerAsync("12c");
          break;
        case "12c":
          PakanAnimation(State.Close, "12");
          infoLabel.Text = "Փական N12 փակված է";
          m_backgroundWorker.RunWorkerAsync("13");
          break;
        case "13":
          PakanAnimation(State.Open, "13");
          infoLabel.Text = "Փական N13 բացված է";
          m_backgroundWorker.RunWorkerAsync("13c");
          break;
        case "13c":
          PakanAnimation(State.Close, "13");
          infoLabel.Text = "Փական N13 փակված է:";
          m_backgroundWorker.RunWorkerAsync("14");
          break;
        case "14":
          PakanAnimation(State.Open, "14");
          infoLabel.Text = "Փական N14 բացված է";
          m_backgroundWorker.RunWorkerAsync("14c");
          break;
        case "14c":
          PakanAnimation(State.Close, "14");
          infoLabel.Text = "Փական N14 փակված է";
          m_backgroundWorker.RunWorkerAsync("15");
          break;
        case "15":
          PakanAnimation(State.Open, "15");
          infoLabel.Text = "Փական N15 բացված է";
          m_backgroundWorker.RunWorkerAsync("15c");
          break;
        case "15c":
          PakanAnimation(State.Close, "15");
          infoLabel.Text = "Փական N15 փակված է:";
          m_backgroundWorker.RunWorkerAsync("16");
          break;
        case "16":
          PakanAnimation(State.Open, "16");
          infoLabel.Text = "Փական N16 բացված է";
          m_backgroundWorker.RunWorkerAsync("16c");
          break;
        case "16c":
          PakanAnimation(State.Close, "16");
          infoLabel.Text = "Փական N16 փակված է";
          m_backgroundWorker.RunWorkerAsync("17");
          break;
        case "17":
          PakanAnimation(State.Open, "17");
          infoLabel.Text = "Փական N17 բացված է";
          m_backgroundWorker.RunWorkerAsync("17c");
          break;
        case "17c":
          PakanAnimation(State.Close, "17");
          infoLabel.Text = "Փական N17 փակված է:";
          m_backgroundWorker.RunWorkerAsync("18");
          break;
        case "18":
          //PakanAnimation(State.Close, "17");
          Thread.Sleep(2000);
          infoLabel.Text = "Էլեկտրապնևմափականների ստուգման գործընթացն ավարտված է: Բոլոր փականները սարքին վիճակում են:";
          //m_backgroundWorker.RunWorkerAsync("18");
          break;
      }
      
    }

    private void back_DoWork(object sender, DoWorkEventArgs e)
    {
      switch (e.Argument.ToString())
      {
        case "00":
          Thread.Sleep(2000);
          e.Result = "00";
          break;
        case "00c":
          Thread.Sleep(2000);
          e.Result = "00c";
          break;
        case "01":
          Thread.Sleep(2000);
          e.Result = "01";
          break;
        case "01c":
          Thread.Sleep(2000);
          e.Result = "01c";
          break;
        case "02":
          Thread.Sleep(2000);
          e.Result = "02";
          break;
        case "02c":
          Thread.Sleep(2000);
          e.Result = "02c";
          break;
        case "03":
          Thread.Sleep(2000);
          e.Result = "03";
          break;
        case "03c":
          Thread.Sleep(2000);
          e.Result = "03c";
          break;
        case "04":
          Thread.Sleep(2000);
          e.Result = "04";
          break;
        case "04c":
          Thread.Sleep(2000);
          e.Result = "04c";
          break;
        case "05":
          Thread.Sleep(2000);
          e.Result = "05";
          break;
        case "05c":
          Thread.Sleep(2000);
          e.Result = "05c";
          break;
        case "06":
          Thread.Sleep(2000);
          e.Result = "06";
          break;
        case "06c":
          Thread.Sleep(2000);
          e.Result = "06c";
          break;
        case "07":
          Thread.Sleep(2000);
          e.Result = "07";
          break;
        case "07c":
          Thread.Sleep(2000);
          e.Result = "07c";
          break;
        case "08":
          Thread.Sleep(2000);
          e.Result = "08";
          break;
        case "08c":
          Thread.Sleep(2000);
          e.Result = "08c";
          break;
        case "09":
          Thread.Sleep(2000);
          e.Result = "09";
          break;
        case "09c":
          Thread.Sleep(2000);
          e.Result = "09c";
          break;
        case "10":
          Thread.Sleep(2000);
          e.Result = "10";
          break;
        case "10c":
          Thread.Sleep(2000);
          e.Result = "10c";
          break;
        case "11":
          Thread.Sleep(2000);
          e.Result = "11";
          break;
        case "11c":
          Thread.Sleep(2000);
          e.Result = "11c";
          break;
        case "12":
          Thread.Sleep(2000);
          e.Result = "12";
          break;
        case "12c":
          Thread.Sleep(2000);
          e.Result = "12c";
          break;
        case "13":
          Thread.Sleep(2000);
          e.Result = "13";
          break;
        case "13c":
          Thread.Sleep(2000);
          e.Result = "13c";
          break;
        case "14":
          Thread.Sleep(2000);
          e.Result = "14";
          break;
        case "14c":
          Thread.Sleep(2000);
          e.Result = "14c";
          break;
        case "15":
          Thread.Sleep(2000);
          e.Result = "15";
          break;
        case "15c":
          Thread.Sleep(2000);
          e.Result = "15c";
          break;
        case "16":
          Thread.Sleep(2000);
          e.Result = "16";
          break;
        case "16c":
          Thread.Sleep(2000);
          e.Result = "16c";
          break;
        case "17":
          Thread.Sleep(2000);
          e.Result = "17";
          break;
        case "17c":
          Thread.Sleep(2000);
          e.Result = "17c";
          break;
        case "18":
          Thread.Sleep(2000);
          e.Result = "18";
          break;
      }
    }

    private void PakanAnimation(State state, params string[] list)
    {
      //System.Media.SystemSounds.Exclamation.Play();
      foreach (var i in list)
      {
        //var textBlock = canvas1.FindName("PakanLabel" + i) as TextBlock; Pakan17Close
        var str = state == State.Open ? "Open" : "Close";
        var pakan = canvas1.FindName("Pakan" + i + str) as Path;
        //label.Foreground = (state == State.Open) ? Brushes.Green : Brushes.Brown;
        //kran.KranColor = (state == State.Open) ? Brushes.LightGreen : Brushes.Brown;
        pakan.Visibility = Visibility.Visible;

        switch (state)
        {
          case State.Open:
            {
              var convertFromString = ColorConverter.ConvertFromString("#FF10C44D");
              if (convertFromString != null)
              {
                var color = (Color)convertFromString;
                pakan.Fill = new SolidColorBrush(color);
              }
              //pakan.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDFD991")); //Brushes.LightGreen; //#FF10C44D
              break;
            }
            //textBlock.Foreground = Brushes.Green;
          case State.Close:
            {
              var convertFromString = ColorConverter.ConvertFromString("#FF10C44D");
              if (convertFromString != null)
              {
                var color = (Color)convertFromString;
                pakan.Fill = new SolidColorBrush(color);
              }
              break;
            }
            //textBlock.Foreground = Brushes.Brown;
          case State.AvariaOpen:
            //textBlock.Foreground = Brushes.Red;
            pakan.Fill = Brushes.Red;
            break;
          case State.AvariaClose:
            //textBlock.Foreground = Brushes.Pink;
            pakan.Fill = Brushes.Pink;
            break;
        }

        Duration duration = new TimeSpan(0, 0, 2);

        //var fontSizeAnimation = new DoubleAnimation(14, 100, duration) { AutoReverse = true, SpeedRatio = 3 };
        //Storyboard.SetTargetName(fontSizeAnimation, textBlock.Name);
        //Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

        var heightAnimation = new DoubleAnimation(pakan.Height, 300, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(heightAnimation, pakan.Name);
        Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

        var widthAnimation = new DoubleAnimation(pakan.Width, 300, duration) { AutoReverse = true, SpeedRatio = 3 };
        Storyboard.SetTargetName(widthAnimation, pakan.Name);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

        var storyboard = new Storyboard();
        //storyboard.Children.Add(fontSizeAnimation);
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(widthAnimation);
        storyboard.Stop();
        storyboard.Begin(pakan);

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

    private void buttonZapusk_Click(object sender, RoutedEventArgs e)
    {
      if (ModeEnum == ModeEnum.Cucadrakan)
      {
        m_backgroundWorker.RunWorkerAsync("00");
        var color = (Color)ColorConverter.ConvertFromString("#FF8D0707");
        var myBrush = new SolidColorBrush(color);
        infoLabel.Text = "";
        infoLabel.Foreground = myBrush;
      }
      else
      {
        infoLabel.Visibility = Visibility.Visible;
        infoLabel.Foreground = Brushes.Red;
        infoLabel.Text = "Սխալ: Անհնար է կապ հաստատել ԱԿՀ - ի հետ.";
      }
    }


  }
}
