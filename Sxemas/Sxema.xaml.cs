using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;

namespace ArmRusGazProm
{
  public partial class Sxema
  {
    private ModeEnum modeEnum;

    readonly RotateTransform m_animatedRotateTransform;
    readonly TranslateTransform m_animatedTranslateTransform;
    readonly BackgroundWorker m_backgroundWorker;
    readonly DispatcherTimer m_timer;
    readonly SerialPort m_port;
    Dictionary<string, State> m_receivedElementsState, m_currentElementsState;
    // Dictionary<string, State> receivedElementsState;
    // Dictionary<string, State> currentElementsState;
    string m_errorText, m_infoText;
    //Dictionary<string, bool> tochkaKameraStates;
    private bool m_isSpd02Opened;
    private bool m_isSpd21Opened;

    public Sxema()
    {
      InitializeComponent();
      m_animatedRotateTransform = new RotateTransform();
      m_animatedTranslateTransform = new TranslateTransform();
      RegisterName("AnimatedTranslateTransform", m_animatedTranslateTransform);
      RegisterName("AnimatedRotateTransform", m_animatedRotateTransform);
      m_backgroundWorker = new BackgroundWorker();
      m_errorText = m_infoText = "";
      m_currentElementsState = new Dictionary<string, State>();
      //receivedElementsState = new Dictionary<string, State>();
      //tochkaKameraStates = new Dictionary<string, bool>();
      InitialElements();
      m_backgroundWorker.DoWork += back_DoWork;
      m_backgroundWorker.RunWorkerCompleted += back_RunWorkerCompleted;
      buttonZapusk.Click += buttonZapusk_Click;
      buttonPriem.Click += buttonPriem_Click;
      porshen.Visibility = Visibility.Hidden;
      porshenPriem.Visibility = Visibility.Hidden;
      infoLabel.Text = "Փ00 բացված է:";//"Кран N00 открыт.";
      m_timer = new DispatcherTimer();
      m_timer.Tick += timer_Tick;
      m_timer.Interval = new TimeSpan(0, 0, 5);
      m_port = new SerialPort(portNameTextBox.Text)
                 {
                   //ReadTimeout = int.Parse(portReadTimeTextBox.Text),
                   //WriteTimeout = int.Parse(portWriteTimeTextBox.Text),
                   ParityReplace = 128,
                   DtrEnable = true,
                   ReadBufferSize = 24,
                   WriteBufferSize = 4096
                 };

      
    }

    public string HeaderText
    {
      set { headerTxt.Text = value.ToUpper(); }
      get { return headerTxt.Text; }
    }

    public ModeEnum ModeEnum
    {
      get { return modeEnum; }
      set { modeEnum = value; }
    }

    private void KranAnimation(State state, params string[] list)
    {
      //System.Media.SystemSounds.Exclamation.Play();
      foreach (var i in list)
      {
        var label = canvas1.FindName("labelKran" + i) as Label;
        var kran = canvas1.FindName("kran" + i) as Kran;
        //label.Foreground = (state == State.Open) ? Brushes.Green : Brushes.Brown;
        //kran.KranColor = (state == State.Open) ? Brushes.LightGreen : Brushes.Brown;

        switch (state)
        {
            case State.Open:
                label.Foreground = Brushes.Green;
                kran.KranColor = Brushes.LightGreen;
                break;
            case State.Close:
                label.Foreground = Brushes.Brown;
                kran.KranColor = Brushes.Brown;
                break;
            case State.AvariaOpen:
                label.Foreground = Brushes.Red;
                kran.KranColor = Brushes.Red;
                break;
            case State.AvariaClose:
                label.Foreground = Brushes.Pink;
                kran.KranColor = Brushes.Pink;
                break;
        }

        Duration duration = new TimeSpan(0, 0, 2);

        var fontSizeAnimation = new DoubleAnimation(14, 100, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(fontSizeAnimation, label.Name);
        Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

        var heightAnimation = new DoubleAnimation(30, 300, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(heightAnimation, label.Name);
        Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

        var widthAnimation = new DoubleAnimation(40, 300, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(widthAnimation, label.Name);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(fontSizeAnimation);
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(widthAnimation);
        storyboard.Stop();
        storyboard.Begin(label);

        if (ModeEnum == ModeEnum.Cucadrakan) continue;
        switch (state)
        {
          case State.Open:
            m_infoText += string.Format("Փ{0} բացված է:\n", i);
            break;
          case State.Close:
            m_infoText += string.Format("Փ{0} փակված է:\n", i);
            break;
          case State.AvariaOpen:
            m_errorText += string.Format("Փ{0} բացված է ՎԹԱՐԱՅԻՆ!\n", i);
            break;
          case State.AvariaClose:
            m_errorText += string.Format("Փ{0} փակված է ՎԹԱՐԱՅԻՆ!\n", i);
            break;
        }
      }
    }

    public void KameraAnimation(State state, KameraCanvas kamera)
    {
      var brush = new SolidColorBrush(Color.FromArgb(0, 250, 235, 215));

      kamera.pathErankjun.Opacity = 0.7;
      kamera.pathUxankyun.Opacity = 0.7;

      switch (state)
      {
        case State.LowPress:
          {
            kamera.KameraColor = Brushes.LightBlue;
            var s = (kamera.Name == "kameraZapusk") ? "թողարկման" : "ընդունման";
            m_infoText += string.Format("ընթանում է ճնշման նվազում խցիկ {0}:\n", s);
          }
          break;
        case State.Produvka:
          {
            kamera.KameraColor = Brushes.White;
            var s = (kamera.Name == "kameraZapusk") ? "թողարկման" : "ընդունման";
            m_infoText += string.Format("Խցիկ {0} օդափոխվում է.\n", s);
          }
          break;
        case State.Close:
          kamera.KameraColor = brush;
          break;
      }
    }

    private void TochkaAnimation(State state, params char[] list)
    {
      foreach (var c in list)
      {
        var tochka = canvas1.FindName("tochka" + Char.ToUpper(c)) as Tochka;
        var label = canvas1.FindName("labelTochka" + Char.ToUpper(c)) as Label;

        label.Foreground = (state == State.Open) ? Brushes.Green : Brushes.Brown;
        tochka.ellipse1.Fill = (state == State.Open) ? Brushes.LightGreen : Brushes.Brown;

        if (state == State.Open)
        {
          Duration duration = new TimeSpan(0, 0, 2);

          var fontSizeAnimation = new DoubleAnimation(14, 100, duration) {AutoReverse = true, SpeedRatio = 3};
          Storyboard.SetTargetName(fontSizeAnimation, label.Name);
          Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

          var heightAnimation = new DoubleAnimation(30, 300, duration) {AutoReverse = true, SpeedRatio = 3};
          Storyboard.SetTargetName(heightAnimation, label.Name);
          Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

          var widthAnimation = new DoubleAnimation(40, 300, duration) {AutoReverse = true, SpeedRatio = 3};
          Storyboard.SetTargetName(widthAnimation, label.Name);
          Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

          var storyboard = new Storyboard();
          storyboard.Children.Add(fontSizeAnimation);
          storyboard.Children.Add(heightAnimation);
          storyboard.Children.Add(widthAnimation);
          storyboard.Stop();
          storyboard.Begin(label);

          if (ModeEnum != ModeEnum.Cucadrakan)
          {
            var t = "";
            switch (Char.ToLower(c))
            {
              case 'a':
                t = "А";
                break;
              case 'b':
                t = "Б";
                break;
              case 'v':
                t = "В";
                break;
              case 'g':
                t = "Г";
                break;
              case 'd':
                t = "Д";
                break;
            }
            m_infoText += string.Format("Մխոցը անցավ կետ {0}.\n", t);
          }
        }
      }
    }

    private void SPDAnimation(State state, string c)
    {
      var spd = canvas1.FindName("spd" + c) as SPD;
      var label = canvas1.FindName("labelSPD" + c) as Label;

      label.Foreground = (state == State.Open) ? Brushes.Green : Brushes.Brown;
      spd.polygon2.Fill = (state == State.Open) ? Brushes.LightGreen : Brushes.Brown;

      if (state == State.Open)
      {

        Duration duration = new TimeSpan(0, 0, 2);

        var fontSizeAnimation = new DoubleAnimation(14, 100, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(fontSizeAnimation, label.Name);
        Storyboard.SetTargetProperty(fontSizeAnimation, new PropertyPath(FontSizeProperty));

        var heightAnimation = new DoubleAnimation(30, 300, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(heightAnimation, label.Name);
        Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

        var widthAnimation = new DoubleAnimation(60, 400, duration) {AutoReverse = true, SpeedRatio = 3};
        Storyboard.SetTargetName(widthAnimation, label.Name);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

        var storyboard = new Storyboard();
        storyboard.Children.Add(fontSizeAnimation);
        storyboard.Children.Add(heightAnimation);
        storyboard.Children.Add(widthAnimation);
        storyboard.Stop();
        storyboard.Begin(label);

        if (ModeEnum != ModeEnum.Cucadrakan)
        {
          m_infoText += "Ստացված է ազդանշան մխոցի ՃԱՑ " + c + ".\n";
        }
      }
    }

    private void VvodPorshen()
    {
      // Open kamera Zapuska
      porshen.Visibility = Visibility.Visible;
      Duration duration = new TimeSpan(0, 0, 2);
      var animation = new DoubleAnimation(0, 90, duration) {AutoReverse = true};
      //animation.SpeedRatio = 2;

      //RotateTransform animatedRotateTransform = new RotateTransform();
      m_animatedRotateTransform.CenterX = 0;
      m_animatedRotateTransform.CenterY = -20;
      kameraZapusk.line2.RenderTransform = m_animatedRotateTransform;
      //if (NameScope.GetNameScope(kameraZapusk) == null)
      //this.kameraZapusk.RegisterName("AnimatedRotateTransform", animatedRotateTransform);

      Storyboard.SetTargetName(animation, "AnimatedRotateTransform");
      Storyboard.SetTargetProperty(animation, new PropertyPath(RotateTransform.AngleProperty));

      var storyboard = new Storyboard();
      storyboard.Children.Add(animation);
      storyboard.Stop();
      storyboard.Begin(this);

      // dvizhenie porshnja

      animation = new DoubleAnimation(0, 135, duration)
                    {BeginTime = new TimeSpan(0, 0, 2), SpeedRatio = 3, FillBehavior = FillBehavior.HoldEnd};
      porshen.RenderTransform = m_animatedTranslateTransform;
      Storyboard.SetTargetName(animation, "AnimatedTranslateTransform");
      Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.XProperty));

      storyboard.Children.Add(animation);
      storyboard.Stop();
      storyboard.Begin(this);
      //porshen.Visibility = Visibility.Hidden;

      if (ModeEnum != ModeEnum.Cucadrakan)
      {
        m_infoText += "Մխոցը տեղադրված է թողարկման համար:\n";
      }
    }

    private void BackVvodPorshen()
    {
      porshen.Visibility = Visibility.Hidden;
      porshen.RenderTransform = new TranslateTransform(-85, 0);
    }

    private void VivodPorshen()
    {
      // Open kamera Priema
      porshenPriem.Visibility = Visibility.Visible;
      Duration duration = new TimeSpan(0, 0, 2);
      var animation = new DoubleAnimation(0, -90, duration) {AutoReverse = true};

      m_animatedRotateTransform.CenterX = 0;
      m_animatedRotateTransform.CenterY = 20;
      kameraPriem.line2.RenderTransform = m_animatedRotateTransform;

      Storyboard.SetTargetName(animation, "AnimatedRotateTransform");
      Storyboard.SetTargetProperty(animation, new PropertyPath(RotateTransform.AngleProperty));

      var storyboard = new Storyboard();
      storyboard.Children.Add(animation);
      storyboard.Stop();
      storyboard.Begin(this);

      // dvizhenie porshnja

      animation = new DoubleAnimation(0, 80, duration)
                    {BeginTime = new TimeSpan(0, 0, 2), SpeedRatio = 3, FillBehavior = FillBehavior.HoldEnd};
      porshenPriem.RenderTransform = m_animatedTranslateTransform;
      Storyboard.SetTargetName(animation, "AnimatedTranslateTransform");
      Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.XProperty));

      storyboard.Children.Add(animation);
      storyboard.Stop();
      storyboard.Begin(this);
      //porshen.Visibility = Visibility.Hidden;

      if (ModeEnum != ModeEnum.Cucadrakan)
      {
        m_infoText += "Մխոցը հանված է ընդունումից հետո:\n";
      }
      TochkaAnimation(State.Close, 'b', 'v', 'g', 'd');
    }

    private void BackVivodPorshen()
    {
      porshenPriem.Visibility = Visibility.Hidden;
      porshenPriem.RenderTransform = new TranslateTransform(-80, 0);
    }

    private void InitialElements()
    {
      foreach (UIElement element in canvas1.Children)
      {
        if (element.GetType() == typeof(Kran))
        {
          Kran kran = element as Kran;
          kran.KranColor = Brushes.Brown;
          m_currentElementsState.Add(kran.Name, State.Close);
          //receivedElementsState.Add(kran.Name, State.Close);
        }
        else if (element.GetType() == typeof(Label))
        {
          Label label = element as Label;
          if (label.Name.Contains("labelKran") || label.Name.Contains("labelSPD") || label.Name.Contains("labelTochka"))
          {
            label.Foreground = Brushes.Brown;
          }
        }
        else if (element.GetType() == typeof(Tochka))
        {
          var tochka = element as Tochka;
          tochka.ellipse1.Fill = Brushes.Brown;
          m_currentElementsState.Add(tochka.Name, State.Close);
          //receivedElementsState.Add(tochka.Name, State.Close);
        }
        else if (element.GetType() == typeof(SPD))
        {
          var spd = element as SPD;
          spd.polygon2.Fill = Brushes.Brown;
          m_currentElementsState.Add(spd.Name, State.Close);
          //receivedElementsState.Add(tochka.Name, State.Close);
        }
        else if (element.GetType() == typeof(KameraCanvas))
        {
          var kamera = element as KameraCanvas;
          m_currentElementsState.Add(kamera.Name, State.Close);
          //receivedElementsState.Add(kamera.Name, State.Close);
        }
      }
      kran00.KranColor = Brushes.LightGreen;
      labelKran00.Foreground = Brushes.Green;
      // Давление воздуха, начальная инициализация;
      m_currentElementsState.Add("air", State.AirOn);
      // Питание 24В, начальная инициализация
      m_currentElementsState.Add("power", State.PowerOn);
      m_currentElementsState["kran00"] = State.Open;
      m_currentElementsState["kameraPriem"] = State.Close;
      m_currentElementsState["kameraZapusk"] = State.Close;

      //receivedElementsState = currentElementsState;
      m_receivedElementsState = new Dictionary<string, State>(m_currentElementsState);
    }

    private void buttonZapusk_Click(object sender, RoutedEventArgs e)
    {
      //workModeGroupBox.IsEnabled = false;
      groupBoxPriem.IsEnabled = false;
      groupBoxZapusk.IsEnabled = false;
      //m_port.ReadTimeout = int.Parse(portReadTimeTextBox.Text);
      //m_port.WriteTimeout = int.Parse(portWriteTimeTextBox.Text);
      m_port.PortName = portNameTextBox.Text;
      expander1.IsEnabled = false;
      Window1.process = Process.Zapusk;
      stopButton.IsEnabled = true;
      kameraZapusk.Opacity = 1;
      if (ModeEnum == ModeEnum.Cucadrakan) m_backgroundWorker.RunWorkerAsync("1");
      else if (ModeEnum == ModeEnum.Stugman)
      {
        m_port.Open();
        m_timer.Start();
      }
      else
      {
        try
        {
          m_port.Open();
          m_timer.Start();
        }
        catch
        {
          infoLabel.Foreground = Brushes.Red;
          infoLabel.Text = " Սխալ: Անհնար է կապ հաստատել ԱԿՀ - ի հետ. ";
          m_port.Close();
          m_timer.Stop();
        }
      }
    }

    private void buttonPriem_Click(object sender, RoutedEventArgs e)
    {
      //workModeGroupBox.IsEnabled = false;
      groupBoxZapusk.IsEnabled = false;
      groupBoxPriem.IsEnabled = false;
      //m_port.ReadTimeout = int.Parse(portReadTimeTextBox.Text);
      //m_port.WriteTimeout = int.Parse(portWriteTimeTextBox.Text);
      m_port.PortName = portNameTextBox.Text;
      expander1.IsEnabled = false;
      Window1.process = Process.Priem;
      stopButton.IsEnabled = true;
      if (ModeEnum == ModeEnum.Cucadrakan) m_backgroundWorker.RunWorkerAsync("33");
      else if (ModeEnum == ModeEnum.Stugman)
      {
        m_port.Open();
        m_timer.Start();
      }
      else
      {
        try
        {
          m_port.Open();
          m_timer.Start();
        }
        catch
        {
          infoLabel.Foreground = Brushes.Red;
          infoLabel.Text = " Սխալ: Անհնար է կապ հաստատել ԱԿՀ - ի հետ. ";
          m_port.Close();
          m_timer.Stop();
        }
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      infoLabel.Foreground = Brushes.Brown;
      infoLabel.Text = "";
      m_errorText = "";
      m_infoText = "";
      //currentElementsState["kran02"] = State.AvariaClose;
      //receivedElementsState["kran02"] = State.AvariaClose;
      var receiveBytes = new byte[3];
      if (ModeEnum == ModeEnum.Ashxatanqayin)
      {
        try
        {
          var sendByte = new byte[1] { 0x1 };
          m_port.Write(sendByte, 0, sendByte.Length);
          var recByte = m_port.ReadByte();

          if (recByte == 1)
          {
            var sendPriemByte = (Window1.process == Process.Priem) ? new byte[] { 0x55 } : new byte[] { 0xAA };
            m_port.Write(sendPriemByte, 0, sendPriemByte.Length);
            var receiveByte = m_port.ReadByte();

            switch (Window1.process)
            {
              case Process.Priem:
                if (receiveByte == 0x55)
                {
                  for (var i = 2; i < 5; i++)
                  {
                    m_port.Write(new byte[1] { (byte)i }, 0, 1);
                    var b = (byte)m_port.ReadByte();
                    receiveBytes[i - 2] = b;
                  }
                }
                else infoLabel.Text = " Պատասխան 0x55 ազդանշանը չի ստացվել: ";
                break;
              case Process.Zapusk:
                if (receiveByte == 0xAA)
                {
                  for (var i = 2; i < 5; i++)
                  {
                    m_port.Write(new byte[1] { (byte)i }, 0, 1);
                    var b = (byte)m_port.ReadByte();
                    receiveBytes[i - 2] = b;
                  }
                }
                else
                {
                  infoLabel.Foreground = Brushes.Red;
                  infoLabel.Text = " Պատասխան 0xAA ազդանշանը չի ստացվել: ";
                }
                break;
            }
          }
          else
          {
            infoLabel.Foreground = Brushes.Red;
            infoLabel.Text = " Պատասխան 0x01 ազդանշանը չի ստացվել: ";
          }
        }
        catch 
        {
          infoLabel.Foreground = Brushes.Red;
          infoLabel.Text = " Բացակայում է ազդանշանը պորտից: ";
          return;
        }
      }
      else if (ModeEnum == ModeEnum.Stugman)
      {
        m_port.Write(new[] { (byte)1 }, 0, 1);
        var k = (byte)m_port.ReadByte();
        //port.Write(new byte[] { (byte)0x55 }, 0, 1);
        //k = (byte)port.ReadByte();
        for (var i = 1; i <= 24; i++)
        {
          m_port.Write(new[] { (byte)i }, 0, 1);
          var c = (byte)m_port.ReadByte();
          switch (i)
          {
              case 7:
                  receiveBytes[0] = c;
                  break;
              case 8:
                  receiveBytes[1] = c;
                  break;
              case 9:
                  receiveBytes[2] = c;
                  break;
          }
          //if (i == 1) receiveBytes[0] = c;
          //else if (i == 2) receiveBytes[1] = c;
          //else if (i == 3) receiveBytes[2] = c;
        }
      }
      var receiveBits = new BitArray(receiveBytes);
      AnalyzeBits(Window1.process, receiveBits);
    }

    private void back_DoWork(object sender, DoWorkEventArgs e)
    {
      switch (e.Argument.ToString())
      {
        case "1":
          Thread.Sleep(5000);
          e.Result = "1";
          break;
        case "2":
          Thread.Sleep(5000);
          e.Result = "2";
          break;
        case "3":
          Thread.Sleep(2000);
          e.Result = "3";
          break;
        case "4":
          Thread.Sleep(5000);
          e.Result = "4";
          break;
        case "5":
          Thread.Sleep(5000);
          e.Result = "5";
          break;
        case "6":
          Thread.Sleep(5000);
          e.Result = "6";
          break;
        case "7":
          Thread.Sleep(5000);
          e.Result = "7";
          break;
        case "8":
          Thread.Sleep(5000);
          e.Result = "8";
          break;
        case "9":
          Thread.Sleep(5000);
          e.Result = "9";
          break;
        case "10":
          Thread.Sleep(5000);
          e.Result = "10";
          break;
        case "11":
          Thread.Sleep(5000);
          e.Result = "11";
          break;
        case "12":
          Thread.Sleep(5000);
          e.Result = "12";
          break;
        case "13":
          Thread.Sleep(5000);
          e.Result = "13";
          break;
        case "14":
          Thread.Sleep(5000);
          e.Result = "14";
          break;
        case "15":
          Thread.Sleep(5000);
          e.Result = "15";
          break;
        case "16":
          Thread.Sleep(5000);
          e.Result = "16";
          break;
        case "17":
          Thread.Sleep(5000);
          e.Result = "17";
          break;
        case "18":
          Thread.Sleep(5000);
          e.Result = "18";
          break;
        case "19":
          Thread.Sleep(5000);
          e.Result = "19";
          break;
        case "20":
          Thread.Sleep(5000);
          e.Result = "20";
          break;
        case "21":
          Thread.Sleep(5000);
          e.Result = "21";
          break;
        case "22":
          Thread.Sleep(5000);
          e.Result = "22";
          break;
        case "23":
          Thread.Sleep(5000);
          e.Result = "23";
          break;
        case "24":
          Thread.Sleep(5000);
          e.Result = "24";
          break;
        case "25":
          Thread.Sleep(5000);
          e.Result = "25";
          break;
        case "26":
          Thread.Sleep(5000);
          e.Result = "26";
          break;
        case "27":
          Thread.Sleep(5000);
          e.Result = "27";
          break;
        case "28":
          Thread.Sleep(5000);
          e.Result = "28";
          break;
        case "29":
          Thread.Sleep(5000);
          e.Result = "29";
          break;
        case "30":
          Thread.Sleep(5000);
          e.Result = "30";
          break;
        case "31":
          Thread.Sleep(5000);
          e.Result = "31";
          break;
        case "32":
          Thread.Sleep(5000);
          e.Result = "32";
          break;
        case "33": // SPD1
          Thread.Sleep(3000);
          e.Result = "33";
          break;
        case "34": // SPD2
          Thread.Sleep(3000);
          e.Result = "34";
          break;
      }

    }

    private void back_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      var brush = new SolidColorBrush(Color.FromArgb(0, 250, 235, 215));
      switch (e.Result.ToString())
      {
        case "1":
          Canvas.SetZIndex(labelKran11, 2);
          Canvas.SetZIndex(labelKran13, 2);
          KranAnimation(State.Open, "11", "13");
          infoLabel.Text = "Փականներ N11 և N13 բաց են, մխոցը արձակող խցիկում ընթանում է ճնշման անկում:"; //сбрасывание давления в камере запуска КЗ
          kameraZapusk.KameraColor = Brushes.LightBlue;
          m_backgroundWorker.RunWorkerAsync("2");
          break;
        case "2":
          infoLabel.Text = "Մխոցն արձակող խցիկում ճնշումը իջեցված է: Մխոցն տեղադրված է:"; //Давление в камере запуска КЗ сброшен. Поршень ввeден в камеру запуска КЗ
          kameraZapusk.pathUxankyun.Fill = brush;
          kameraZapusk.pathErankjun.Fill = brush;
          VvodPorshen();
          m_backgroundWorker.RunWorkerAsync("3");
          break;
        case "3":
          KranAnimation(State.Open, "03", "07", "09");
          infoLabel.Text = "Փականներ N09, N03 և N07 բացված են, մխոցի արձակման խցիկը և ∅700 գազատարը օդափոխվում է:"; //Краны N09, N03 и N07 открыты, продувается камера запуска КЗ и газопровода Г700
          kameraZapusk.pathUxankyun.Opacity = 0.7;
          kameraZapusk.KameraColor = Brushes.White;
          m_backgroundWorker.RunWorkerAsync("4");
          break;
        case "4":
          KranAnimation(State.Close, "11");
          kameraZapusk.pathUxankyun.Fill = brush;
          kameraZapusk.pathErankjun.Fill = brush;
          infoLabel.Text = "Փական N11 փակված է, մխոցի արձակման խցիկը և ∅700 գազատարի օդափոխումը ավարտված է:"; // Кран N11 закрыт, продувка камеры запуска КЗ и газопровода Г700 окончена.
          m_backgroundWorker.RunWorkerAsync("5");
          break;
        case "5":
          infoLabel.Text = "Փական N13 փակ է:"; // Кран N13 закрыт
          KranAnimation(State.Close, "13");
          m_backgroundWorker.RunWorkerAsync("34");
          break;
        case "6":
          KranAnimation(State.Close, "03", "07", "09");
          infoLabel.Text = "Փականներ N07, N09 և N03 փակված են:"; //Краны N07, N09 и N03 закрыты.
          m_backgroundWorker.RunWorkerAsync("7");
          break;
        case "7":
          KranAnimation(State.Open, "01", "05");
          infoLabel.Text = "Փականներ N01 և N05 բացված են:"; // Краны N01 и N05 открыты.
          m_backgroundWorker.RunWorkerAsync("8");
          break;
        case "8":
          KranAnimation(State.Close, "00");
          TochkaAnimation(State.Open, 'e');
          kameraZapusk.KameraColor = Brushes.LightGreen;
          kameraZapusk.Opacity = 1;
          BackVvodPorshen();
          infoLabel.Text = "Մխոցն արձակված, ստացված է ազդանշան Ե կետից:"; // Поршень запущен, поршень движется к точке А.
          m_backgroundWorker.RunWorkerAsync("9");
          break;
        case "9":
          infoLabel.Text = "Մխոցը հասավ կետ Զ:";//Поршень достиг точки А.
          TochkaAnimation(State.Open, 'a');
          m_backgroundWorker.RunWorkerAsync("10");
          break;
        case "10":
          KranAnimation(State.Open, "00");
          //TochkaAnimation(State.Close, 'a');
          infoLabel.Text = "Փական N00 բաց է:"; //Кран N00 открыт
          m_backgroundWorker.RunWorkerAsync("11");
          break;
        case "11":
          KranAnimation(State.Close, "01", "05");
          infoLabel.Text = "Փականներ N01 և N05 փակ են:";
          m_backgroundWorker.RunWorkerAsync("12");
          break;
        case "12":
          KranAnimation(State.Open, "11", "13");
          infoLabel.Text = "Փականներ N11 և N13 բաց են:";
          m_backgroundWorker.RunWorkerAsync("13");
          break;
        case "13":
          KranAnimation(State.Close, "11", "13");
          SPDAnimation(State.Close, "");
          infoLabel.Text = "Փականներ N11 և N13 փակ են: Պրոցեսն ավարտված է:";
          Window1.process = Process.None;
          TochkaAnimation(State.Close, 'a');
          kameraZapusk.KameraColor = brush;
          break;
        case "14":
          KranAnimation(State.Open, "04", "06");
          infoLabel.Text = "Փականներ N04 և N06 բաց են:";
          m_backgroundWorker.RunWorkerAsync("15");
          break;
        case "15":
          TochkaAnimation(State.Open, 'b');
          infoLabel.Text = "Ստացված է ազդանշան Ա կետից: Բացվում է N02 փականը:"; //Получен сигнал от точки Б. Открывается кран N02.
          m_backgroundWorker.RunWorkerAsync("16");
          break;
        case "16":
          KranAnimation(State.Open, "02");
          //TochkaAnimation(State.Close, 'b');
          infoLabel.Text = "Փական N02 բացված է:";
          m_backgroundWorker.RunWorkerAsync("17");
          break;
        case "17":
          KranAnimation(State.Close, "00");
          infoLabel.Text = "Փական N00 փակ է:";
          m_backgroundWorker.RunWorkerAsync("18");
          break;
        case "18":
          TochkaAnimation(State.Open, 'v');
          infoLabel.Text = "Ստացված է ազդանշան Բ կետից: Բացվում է փական N00:"; //Получен сигнал от точки В. Открывается кран N00.
          m_backgroundWorker.RunWorkerAsync("19");
          break;
        case "19":
          KranAnimation(State.Open, "00");
          infoLabel.Text = "Փական N00 բաց է:";
          //TochkaAnimation(State.Close, 'v');
          m_backgroundWorker.RunWorkerAsync("20");
          break;
        case "20":
          KranAnimation(State.Close, "04");
          infoLabel.Text = "Փական N04 փակ է:";
          m_backgroundWorker.RunWorkerAsync("21");
          break;
        case "21":
          TochkaAnimation(State.Open, 'g');
          infoLabel.Text = "Ստացված է ազդանշան Գ կետից: Սկսվում է աղտի կուտակում կոնդենսատահավաքիչում:"; //Получен сигнал от точки Г. Закрываются краны N02 и N06.
          truba1.rectangle1.Fill = new SolidColorBrush(Color.FromRgb(83, 77, 10));
          m_backgroundWorker.RunWorkerAsync("22");
          break;
        case "22":
          //TochkaAnimation(State.Close, 'g');
          KranAnimation(State.Close, "02", "06");
          infoLabel.Text = "Փականներ N02 և N06 փակ են:";
          m_backgroundWorker.RunWorkerAsync("23");
          break;
        case "23":
          KranAnimation(State.Open, "08", "12");
          infoLabel.Text = "Փականներ N08 և N12 բացեն:";
          m_backgroundWorker.RunWorkerAsync("24");
          break;
        case "24":
          TochkaAnimation(State.Open, 'd');
          infoLabel.Text = "Ստացված է ազդանշան Դ կետից: Փակվում են փականներ N08 և N12:"; //Получен сигнал от точки Д. Закрываются краны N08 и N12.
          m_backgroundWorker.RunWorkerAsync("25");
          break;
        case "25":
          //TochkaAnimation(State.Close, 'd');
          KranAnimation(State.Close, "08", "12");
          infoLabel.Text = "Փականներ N08 և N12 փակեն:";
          m_backgroundWorker.RunWorkerAsync("26");
          break;
        case "26":
          KranAnimation(State.Open, "08", "10");
          infoLabel.Text = "Փական N10 բաց է, ընթանում է ընդունման խցիկի ճնշման նվազում:"; //Кран N10 открыт, идет сбрасывание давления камеры приема КП
          kameraPriem.KameraColor = Brushes.LightBlue;
          m_backgroundWorker.RunWorkerAsync("27");
          break;
        case "27":
          infoLabel.Text = "Մխոցի ընդունման պատրաստումը ավարտված է:\nԸնդունման խցիկը պատրաստ է մխոցի ընդունմանը:"; //Подготовка приема поршня завершена.\nКамера приема КП готова к приему поршня
          buttonPriem.IsEnabled = true;
          porshenPriem.Visibility = Visibility.Visible;
          kameraPriem.KameraColor = Brushes.LightGreen;
          kameraPriem.pathUxankyun.Opacity = 0.7;
          buttonPriem.IsEnabled = true;
          kameraPriem.pathUxankyun.Fill = brush;
          kameraPriem.pathErankjun.Fill = brush;
          m_backgroundWorker.RunWorkerAsync("28");
          break;
        case "28":
          VivodPorshen();
          infoLabel.Text = "Ընդունման խցանի փեղկը բաց է, մխոցը տեղահանված է, փեղկը փակված է:"; //Затвор камеры приема КП открыт, поршень извлечен, затвор закрыт.
          m_backgroundWorker.RunWorkerAsync("29");
          break;
        case "29":
          BackVivodPorshen();
          KranAnimation(State.Open, "08");
          kameraPriem.pathUxankyun.Opacity = 1;
          kameraPriem.KameraColor = Brushes.White;
          infoLabel.Text = "Փական N08 բաց է, ընթանում է ընդունման խցանի օդափոխում:";//Кран N08 октрыт, идет продувка камеры приема КП
          m_backgroundWorker.RunWorkerAsync("30");
          break;
        case "30":
          KranAnimation(State.Close, "08", "10");
          kameraPriem.pathUxankyun.Opacity = 1;
          kameraPriem.KameraColor = brush;
          infoLabel.Text = "Ընդունման խցիկի օդափոխումը ավարտված է, փականներ N08 և N10 փակ են:"; //Продувка камеры приема КП окончена, краны N08 и N10 закрыты
          m_backgroundWorker.RunWorkerAsync("31");
          break;
        case "31":
          KranAnimation(State.Open, "14");
          infoLabel.Text = "Փական N14 բաց է, ընթանում է խտուցքահավաքիչի աղտահանում:"; //Кран N14 открыт, идет сбрасывание очистков в конденсатосборник Т-1
          truba1.rectangle1.Fill = new SolidColorBrush(Color.FromRgb(83, 77, 10));
          m_backgroundWorker.RunWorkerAsync("32");
          break;
        case "32":
          KranAnimation(State.Close, "14");
          SPDAnimation(State.Close, "02");
          TochkaAnimation(State.Close, 'b'); TochkaAnimation(State.Close, 'v');
          TochkaAnimation(State.Close, 'g'); TochkaAnimation(State.Close, 'd');
          truba1.rectangle1.Fill = brush;
          infoLabel.Text = "Փական N14 փակ է, խտուցքահավաքիչը աղտահանված է:\n Պրոցեսը ավարտված է:"; //Кран N14 закрыт, очистки в конденсатосборник Т-1 сброшены.\n Процесс закончен
          
          rectangle3.Fill = new SolidColorBrush(Color.FromRgb(83, 77, 10));
          Window1.process = Process.None;
          break;
        case "33": // SPD02
          SPDAnimation(State.Open, "02");
          infoLabel.Text = "Ստացված է ազդանշան ՃԱՑ 02-ից:"; //Получен сигнал от СПД 02
          m_backgroundWorker.RunWorkerAsync("14");
          break;
        case "34": // SPD21
          SPDAnimation(State.Open, "");
          infoLabel.Text = "Ստացված է ազդանշան խցիկի ՃԱՑ-ից:"; //Получен сигнал от СПД 21.
          m_backgroundWorker.RunWorkerAsync("6");
          break;
      }
    }

    private void AnalyzeBits(Process process, BitArray bits)
    {
      if (process == Process.Priem)
      {
        if (bits.Get(1) == false && bits.Get(0) == false) m_receivedElementsState["kran00"] = State.AvariaOpen;
        else if (bits.Get(1) && bits.Get(0)) m_receivedElementsState["kran00"] = State.AvariaClose;
        else if (bits.Get(1) && !bits.Get(0)) m_receivedElementsState["kran00"] = State.Close;
        else m_receivedElementsState["kran00"] = State.Open;

        if (bits.Get(3) == false && bits.Get(2) == false) m_receivedElementsState["kran02"] = State.AvariaOpen;
        else if (bits.Get(3) && bits.Get(2)) m_receivedElementsState["kran02"] = State.AvariaClose;
        else if (bits.Get(3) && !bits.Get(2)) m_receivedElementsState["kran02"] = State.Close;
        else
        {
          m_receivedElementsState["kran02"] = State.Open;
          m_receivedElementsState["spd02"] = State.Close;
          //isSPD02Opened = false;
        }

        if (!bits.Get(5) && !bits.Get(4)) m_receivedElementsState["kran04"] = State.AvariaOpen;
        else if (bits.Get(5) && bits.Get(4)) m_receivedElementsState["kran04"] = State.AvariaClose;
        else if (bits.Get(5) && !bits.Get(4)) m_receivedElementsState["kran04"] = State.Close;
        else m_receivedElementsState["kran04"] = State.Open;

        if (!bits.Get(7) && !bits.Get(6)) m_receivedElementsState["kran06"] = State.AvariaOpen;
        else if (bits.Get(7) && bits.Get(6)) m_receivedElementsState["kran06"] = State.AvariaClose;
        else if (bits.Get(7) && !bits.Get(6)) m_receivedElementsState["kran06"] = State.Close;
        else m_receivedElementsState["kran06"] = State.Open;

        if (!bits.Get(9) && !bits.Get(8)) m_receivedElementsState["kran08"] = State.AvariaOpen;
        else if (bits.Get(9) && bits.Get(8)) m_receivedElementsState["kran08"] = State.AvariaClose;
        else if (bits.Get(9) && !bits.Get(8)) m_receivedElementsState["kran08"] = State.Close;
        else m_receivedElementsState["kran08"] = State.Open;

        if (!bits.Get(11) && !bits.Get(10)) m_receivedElementsState["kran10"] = State.AvariaOpen;
        else if (bits.Get(11) && bits.Get(10)) m_receivedElementsState["kran10"] = State.AvariaClose;
        else if (bits.Get(11) && !bits.Get(10)) m_receivedElementsState["kran10"] = State.Close;
        else m_receivedElementsState["kran10"] = State.Open;

        if (!bits.Get(13) && !bits.Get(12)) m_receivedElementsState["kran12"] = State.AvariaOpen;
        else if (bits.Get(13) && bits.Get(12)) m_receivedElementsState["kran12"] = State.AvariaClose;
        else if (bits.Get(13) && !bits.Get(12)) m_receivedElementsState["kran12"] = State.Close;
        else m_receivedElementsState["kran12"] = State.Open;

        if (!bits.Get(15) && !bits.Get(14)) m_receivedElementsState["kran14"] = State.AvariaOpen;
        else if (bits.Get(15) && bits.Get(14)) m_receivedElementsState["kran14"] = State.AvariaClose;
        else if (bits.Get(15) && !bits.Get(14)) m_receivedElementsState["kran14"] = State.Close;
        else m_receivedElementsState["kran14"] = State.Open;

        if (bits.Get(16)) m_receivedElementsState["power"] = State.PowerOn; 
        else m_receivedElementsState["power"] = State.PowerOff;

        if (bits.Get(17)) m_receivedElementsState["air"] = State.AirOn;
        else m_receivedElementsState["air"] = State.AirOff;

        if (bits.Get(18) && !m_isSpd02Opened)
        {
          m_receivedElementsState["spd02"] = State.Open;
          m_isSpd02Opened = true;
        }
        //else receivedElementsState["spd02"] = State.Close;

        if (m_currentElementsState["kameraPriem"] != State.Open)
        {
          if (bits.Get(19)) m_receivedElementsState["kameraPriem"] = State.Open;
          else m_receivedElementsState["kameraPriem"] = State.Close;
        }

        if (m_currentElementsState["tochkaD"] != State.Open)
        {
          if (bits.Get(20)) m_receivedElementsState["tochkaD"] = State.Open;
          else m_receivedElementsState["tochkaD"] = State.Close;
        }

        if (m_currentElementsState["tochkaG"] != State.Open)
        {
          if (bits.Get(21)) m_receivedElementsState["tochkaG"] = State.Open;
          else m_receivedElementsState["tochkaG"] = State.Close;
        }

        if (m_currentElementsState["tochkaV"] != State.Open)
        {
          if (bits.Get(22)) m_receivedElementsState["tochkaV"] = State.Open;
          else m_receivedElementsState["tochkaV"] = State.Close;
        }

        if (m_currentElementsState["tochkaB"] != State.Open)
        {
          if (bits.Get(23)) m_receivedElementsState["tochkaB"] = State.Open;
          else m_receivedElementsState["tochkaB"] = State.Close;
        }
      }
      else if (process == Process.Zapusk)
      {
        if (!bits.Get(1) && !bits.Get(0)) m_receivedElementsState["kran00"] = State.AvariaOpen;
        else if (bits.Get(1) && bits.Get(0)) m_receivedElementsState["kran00"] = State.AvariaClose;
        else if (bits.Get(1) && !bits.Get(0)) m_receivedElementsState["kran00"] = State.Close;
        else m_receivedElementsState["kran00"] = State.Open;

        if (bits.Get(3) == false && bits.Get(2) == false) m_receivedElementsState["kran01"] = State.AvariaOpen;
        else if (bits.Get(3) == true && bits.Get(2) == true) m_receivedElementsState["kran01"] = State.AvariaClose;
        else if (bits.Get(3) == true && bits.Get(2) == false) m_receivedElementsState["kran01"] = State.Close;
        else
        {
          m_receivedElementsState["kran01"] = State.Open;
          m_receivedElementsState["spd21"] = State.Close;
        }

        if (bits.Get(5) == false && bits.Get(4) == false) m_receivedElementsState["kran03"] = State.AvariaOpen;
        else if (bits.Get(5) && bits.Get(4)) m_receivedElementsState["kran03"] = State.AvariaClose;
        else if (bits.Get(5) && bits.Get(4) == false) m_receivedElementsState["kran03"] = State.Close;
        else m_receivedElementsState["kran03"] = State.Open;

        if (bits.Get(7) == false && bits.Get(6) == false) m_receivedElementsState["kran05"] = State.AvariaOpen;
        else if (bits.Get(7) && bits.Get(6)) m_receivedElementsState["kran05"] = State.AvariaClose;
        else if (bits.Get(7) && bits.Get(6) == false) m_receivedElementsState["kran05"] = State.Close;
        else m_receivedElementsState["kran05"] = State.Open;

        if (bits.Get(9) == false && bits.Get(8) == false) m_receivedElementsState["kran07"] = State.AvariaOpen;
        else if (bits.Get(9) && bits.Get(8)) m_receivedElementsState["kran07"] = State.AvariaClose;
        else if (bits.Get(9) && bits.Get(8) == false) m_receivedElementsState["kran07"] = State.Close;
        else m_receivedElementsState["kran07"] = State.Open;

        if (bits.Get(11) == false && bits.Get(10) == false) m_receivedElementsState["kran09"] = State.AvariaOpen;
        else if (bits.Get(11) == true && bits.Get(10) == true) m_receivedElementsState["kran09"] = State.AvariaClose;
        else if (bits.Get(11) == true && bits.Get(10) == false) m_receivedElementsState["kran09"] = State.Close;
        else m_receivedElementsState["kran09"] = State.Open;

        if (bits.Get(13) == false && bits.Get(12) == false) m_receivedElementsState["kran11"] = State.AvariaOpen;
        else if (bits.Get(13) == true && bits.Get(12) == true) m_receivedElementsState["kran11"] = State.AvariaClose;
        else if (bits.Get(13) == true && bits.Get(12) == false) m_receivedElementsState["kran11"] = State.Close;
        else m_receivedElementsState["kran11"] = State.Open;

        if (bits.Get(15) == false && bits.Get(14) == false) m_receivedElementsState["kran13"] = State.AvariaOpen;
        else if (bits.Get(15) == true && bits.Get(14) == true) m_receivedElementsState["kran13"] = State.AvariaClose;
        else if (bits.Get(15) == true && bits.Get(14) == false) m_receivedElementsState["kran13"] = State.Close;
        else m_receivedElementsState["kran13"] = State.Open;

        if (bits.Get(16)) m_receivedElementsState["power"] = State.PowerOn;
        else m_receivedElementsState["power"] = State.PowerOff;

        if (bits.Get(17)) m_receivedElementsState["air"] = State.AirOn;
        else m_receivedElementsState["air"] = State.AirOff;

        if (m_currentElementsState["kameraZapusk"] != State.Open)
        {
          if (bits.Get(18)) m_receivedElementsState["kameraZapusk"] = State.Open;
          else m_receivedElementsState["kameraZapusk"] = State.Close;
        }

        if (bits.Get(19) == true && m_isSpd21Opened == false)
        {
          m_receivedElementsState["spd21"] = State.Open;
          m_isSpd21Opened = true;
        }
        else m_receivedElementsState["spd21"] = State.Close;

        if (m_currentElementsState["tochkaA"] != State.Open)
        {
          if (bits.Get(23)) m_receivedElementsState["tochkaA"] = State.Open;
          else m_receivedElementsState["tochkaA"] = State.Close;
        }
        if (m_currentElementsState["tochkaE"] != State.Open)
        {
          if (bits.Get(22)) m_receivedElementsState["tochkaE"] = State.Open;
          else m_receivedElementsState["tochkaE"] = State.Close;
        }
      }
      foreach (var item in m_receivedElementsState)
      {
        if (item.Key.StartsWith("kran") && m_receivedElementsState[item.Key] != m_currentElementsState[item.Key])
        {
          KranAnimation(item.Value, item.Key.Substring(4, 2)); 
        }

        if (item.Key == "power" && m_receivedElementsState[item.Key] == State.PowerOff)
        {
          m_errorText += "Վթարային իրավիճակ! Սնման լարումը ցածր է քան 24Վ!\n"; // АВАРИЙНОЕ СОСТОЯНИЕ! Питание ниже чем 24В!
        }

        if (item.Key == "air" && m_receivedElementsState[item.Key] == State.AirOff)
        {
          m_errorText += "Վթարային իրավիճակ! Օդի ճնշումը նորմայից ցածր է!\n";
        }

        if (item.Key.StartsWith("tochka") && m_receivedElementsState[item.Key] == State.Open &&
          m_currentElementsState[item.Key] == State.Close)
        {
          TochkaAnimation(State.Open, char.Parse(item.Key.Substring(6, 1)));
        }

        if (item.Key.StartsWith("spd") && m_receivedElementsState[item.Key] != m_currentElementsState[item.Key])
        {
          SPDAnimation(item.Value, item.Key.Substring(3, 2));
        }

        if (item.Key == "kameraZapusk" && m_receivedElementsState[item.Key] == State.Open &&
          m_currentElementsState[item.Key] == State.Close && Window1.process == Process.Zapusk)
        {
          VvodPorshen();
        }

        if (item.Key == "kameraPriem" && m_receivedElementsState[item.Key] == State.Open &&
          m_currentElementsState[item.Key] == State.Close && Window1.process == Process.Priem)
        {
          VivodPorshen();
        }
      }
      m_currentElementsState = new Dictionary<string, State>(m_receivedElementsState);

      // Kamera Zapuska
      //if (currentElementsState["kran11"] == State.Open && currentElementsState["kran13"] == State.Open)
      //  KameraAnimation(State.LowPress, kameraZapusk);
      //else if (currentElementsState["kran03"] == State.Open && currentElementsState["kran07"] == State.Open
      //  && currentElementsState["kran09"] == State.Open)
      //  KameraAnimation(State.Produvka, kameraZapusk);
      //else KameraAnimation(State.Close, kameraZapusk);

      // Kamera priema
      //if (currentElementsState["kran10"] == State.Open) KameraAnimation(State.LowPress, kameraPriem);
      //else if (currentElementsState["kran08"] == State.Open) KameraAnimation(State.Produvka, kameraPriem);
      //else KameraAnimation(State.Close, kameraPriem);

      if (m_currentElementsState["kran14"] == State.Open)
      {
        m_infoText += "Ընթանում է կոնդենսատահավաքիչի աղտահանում:\n";
        truba1.rectangle1.Fill = new SolidColorBrush(Color.FromRgb(83, 77, 10));
      }
      else truba1.rectangle1.Fill = new SolidColorBrush(Color.FromArgb(0, 250, 235, 215));

      if (m_errorText.Length > 1) infoLabel.Foreground = Brushes.Red;
      else infoLabel.Foreground = Brushes.Brown;

      infoLabel.Text += m_errorText;
      infoLabel.Text += m_infoText;
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      if (!m_port.IsOpen) return;
      m_port.Close();
      m_timer.Stop();
    }

    private void stopButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
      Button button = sender as Button;
      GradientStopCollection collection = new GradientStopCollection();
      collection.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0));
      collection.Add(new GradientStop(Color.FromRgb(152, 181, 207), 0.923));
      LinearGradientBrush brush = new LinearGradientBrush(collection, new Point(0.5, 0), new Point(0.5, 1));
      button.Background = brush;
    }

    private void stopButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
      Button button = sender as Button;
      GradientStopCollection collection = new GradientStopCollection();
      collection.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0));
      collection.Add(new GradientStop(Color.FromRgb(135, 155, 164), 0.923));
      LinearGradientBrush brush = new LinearGradientBrush(collection, new Point(0.5, 0), new Point(0.5, 1));
      button.Background = brush;
    }

    private void stopButton_Click(object sender, RoutedEventArgs e)
    {
      if (((Button)sender).Content.ToString() == "\"Դադար\" Ազդանշան")
      {
        try
        {
          m_port.Write(new byte[] { 0x66 }, 0, 1);
          if (m_port.ReadByte() == 0x66)
          {
            m_timer.Stop();
            infoLabel.Foreground = Brushes.Brown;
            infoLabel.Text += "\n ԱԿՀ դադարեցված է:";
            ((Button)sender).Content = "\"Վերսկսում\" Ազդանշան";
          }
        }
        catch
        {
          infoLabel.Foreground = Brushes.Red;
          infoLabel.Text += "\n Չկա կապ պորտի հետ:"; //Нет связи с портом
        }
      }
      else if (((Button)sender).Content.ToString() == "\"Վերսկսում\" Ազդանշան")
      {
        try
        {
          m_port.Write(new byte[] { 0x77 }, 0, 1);
          if (m_port.ReadByte() == 0x77)
          {
            m_timer.Start();
            infoLabel.Foreground = Brushes.Brown;
            infoLabel.Text += "\n ԱԿՀ վերսկսված է:";
            ((Button)sender).Content = "\"Ստոպ\" Ազդանշան";
          }
        }
        catch
        {
          infoLabel.Foreground = Brushes.Red;
          infoLabel.Text += "\n Չկա կապ պորտի հետ:";
        }
      }
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      if (stopButton != null)
      {
        if (ModeEnum == ModeEnum.Cucadrakan)
          stopButton.Visibility = Visibility.Hidden;
        else
        {
          stopButton.Visibility = Visibility.Visible;
        }
      }
      
    }

    private void Sxema_Loaded(object sender, RoutedEventArgs e)
    {
      stopButton.Visibility = ModeEnum == ModeEnum.Cucadrakan ? Visibility.Hidden : Visibility.Visible;
      expander1.Visibility = ModeEnum == ModeEnum.Cucadrakan ? Visibility.Hidden : Visibility.Visible;
    }
  }

  public enum State
  {
    Open,
    Close,
    AvariaOpen,
    AvariaClose,
    NormalDavlenie,
    Produvka,
    LowPress,
    PowerOff,
    PowerOn,
    AirOn,
    AirOff
  };

  public enum Process
  {
    Priem,
    Zapusk,
    None
  }
}