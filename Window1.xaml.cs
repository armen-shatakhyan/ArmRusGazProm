using System.Windows;
using System.ComponentModel;
using ArmRusGazProm.Sxemas;

namespace ArmRusGazProm
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    PlacementMenu placementMenu;
    WelcomeMenu welcomeMenu;
    private TestModeMenu testModeMenu;
    private ModeEnum modeEnum;
    private ModeMenu modeMenu;
    //MegriKagharanSxema sxema;
    //AngexakotGhermukSxema angexakotGhermukSxema;
    //GetapAraratSxema getapAraratSxema;
    //GhermukGetapSxema ghermukGetapSxema;
    //KagharanSisianSxema kagharanSisianSxema;
    //MegriKagharanSxema megriKagharanSxema;
    //MegriKagharanSxema2 megriKagharanSxema2;
    Sxema sxema;
    private SkaaTest skaaTest;
    private SpdTest spdTest;
    private EppTest eppTest;
    //public static bool isProcess;
    public static Process process;

    public Window1()
    {
      sxema = new Sxema();
      skaaTest = new SkaaTest();
      spdTest = new SpdTest();
      eppTest = new EppTest();
      //isProcess = false;
      process = Process.None;
      InitializeComponent();
      InitialElements();
      this.Closing += Window1_Closing;
    }

    private void InitialElements()
    {
      placementMenu = new PlacementMenu();
      welcomeMenu = new WelcomeMenu();
      modeMenu = new ModeMenu();
      testModeMenu = new TestModeMenu();

      placementMenu.mexriKajaranBtn.Click += mexriKajaranBtn_Click;
      placementMenu.kajaranSisianBtn.Click += kajaranSisian_Click;
      placementMenu.angexakotJermukBtn.Click += angexakotJermukBtn_Click;
      placementMenu.jermukGetapBtn.Click += jermukGetapBtn_Click;
      placementMenu.getapAraratBtn.Click += getapAraratBtn_Click;
      placementMenu.returnToModeBtn.Click += returnToModeBtn_Click;

      testModeMenu.returnToModeBtn.Click +=returnToModeBtn_Click;

      testModeMenu.skaaBtn.Click += skaaBtn_Click;
      testModeMenu.chajBtn.Click += chajBtn_Click;
      testModeMenu.eppBtn.Click += eppBtn_Click;

      spdTest.cancelButton.Click += spdTestCancelBtn_Click;
      skaaTest.cancelButton.Click += skaaTestCancelBtn_Click;
      eppTest.cancelButton.Click += eppTestCancelBtn_Click;
      
      welcomeMenu.buttonEnter.Click += buttonEnter_Click;
      welcomeMenu.exitButton.Click += exitButton_Click;
      modeMenu.returnToWelcomeMenu.Click += returnToWelcomeMenu_Click;
      modeMenu.cucadrakanBtn.Click += cucadrakanBtn_Click;
      modeMenu.ashxatanqayinBtn.Click += ashxatanqayinBtn_Click;
      modeMenu.stugmanBtn.Click += stugmanBtn_Click;
      sxema.cancelButton.Click += cancelButton_Click;
    }

    void eppTestCancelBtn_Click(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(testModeMenu);
    }

    void skaaTestCancelBtn_Click(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(testModeMenu);
    }

    void spdTestCancelBtn_Click(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(testModeMenu);
    }

    void eppBtn_Click(object sender, RoutedEventArgs e)
    {
      eppTest = new EppTest();
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(eppTest);
    }

    void chajBtn_Click(object sender, RoutedEventArgs e)
    {
      spdTest = new SpdTest();
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(spdTest);
    }

    void skaaBtn_Click(object sender, RoutedEventArgs e)
    {
      skaaTest = new SkaaTest();
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(skaaTest);
    }

    void stugmanBtn_Click(object sender, RoutedEventArgs e)
    {
      canvas1.Children.Clear();
      InitialElements();
      canvas1.Children.Add(testModeMenu);

      modeEnum = ModeEnum.Stugman;
    }

    void ashxatanqayinBtn_Click(object sender, RoutedEventArgs e)
    {
      canvas1.Children.Clear();
      InitialElements();
      placementMenu.textBlock1.Text = "ԱՇԽԱՏԱՆՔԱՅԻՆ ՌԵԺԻՄ";
      canvas1.Children.Add(placementMenu);

      modeEnum = ModeEnum.Ashxatanqayin;
    }

    void cucadrakanBtn_Click(object sender, RoutedEventArgs e)
    {
      canvas1.Children.Clear();
      InitialElements();
      placementMenu.textBlock1.Text = "ՑՈՒՑԱԴՐԱԿԱՆ ՌԵԺԻՄ";
      canvas1.Children.Add(placementMenu);

      modeEnum = ModeEnum.Cucadrakan;

    }

    void returnToWelcomeMenu_Click(object sender, RoutedEventArgs e)
    {
      canvas1.Children.Clear();
      InitialElements();
      canvas1.Children.Add(welcomeMenu);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      this.canvas1.Children.Add(welcomeMenu);
      //Sxema sx = new Sxema();
      //this.canvas1.Children.Add(sx);
      //sx.LabelAnimation(KranState.Open, 29, 26);
      //sx.VvodPorshen();
    }

    private void buttonEnter_Click(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(modeMenu);
    }

    private void mexriKajaranBtn_Click(object sender, RoutedEventArgs e)
    {
      //MegriKagharanSxema sxema = new MegriKagharanSxema();
      sxema = new Sxema {HeaderText = "քաջարան", groupBoxPriem = {Visibility = Visibility.Hidden}, ModeEnum = modeEnum};
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(sxema);
    }

    private void kajaranSisian_Click(object sender, RoutedEventArgs e)
    {
      sxema = new Sxema {HeaderText = "սիսիան", ModeEnum = modeEnum};
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(sxema);
    }

    private void angexakotJermukBtn_Click(object sender, RoutedEventArgs e)
    {
      sxema = new Sxema { HeaderText = "ջերմուկ", ModeEnum = modeEnum };
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(sxema);
    }

    private void jermukGetapBtn_Click(object sender, RoutedEventArgs e)
    {
      sxema = new Sxema { HeaderText = "եղեգնաձոր", ModeEnum = modeEnum };
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(sxema);
    }

    private void getapAraratBtn_Click(object sender, RoutedEventArgs e)
    {
      sxema = new Sxema { HeaderText = "արարատ", groupBoxZapusk = { Visibility = Visibility.Hidden }, ModeEnum = modeEnum };
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(sxema);
    }

    private void returnToModeBtn_Click(object sender, RoutedEventArgs e)
    {
      //MegriKagharanSxema sxema = new MegriKagharanSxema();
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(modeMenu);
    }

    private void exitButton_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Shutdown();
    }

    private void Window1_Closing(object sender, CancelEventArgs e)
    {
      //if (isProcess)
      //{
      //  MessageBox.Show("Выход невозможен, так как идет процесс.\nДождитесь окончания процесса, чтобы выйти", 
      //    "Стоп", MessageBoxButton.OK, MessageBoxImage.Stop);
      //  e.Cancel = true;
      //}
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      this.canvas1.Children.Clear();
      InitialElements();
      this.canvas1.Children.Add(placementMenu);

      switch (modeEnum)
      {
        case ModeEnum.Ashxatanqayin:
          placementMenu.Title = "ԱՇԽԱՏԱՆՔԱՅԻՆ ՌԵԺԻՄ";
          break;

        case ModeEnum.Cucadrakan:
          placementMenu.Title = "ՑՈՒՑԱԴՐԱԿԱՆ ՌԵԺԻՄ";
          break;

        case ModeEnum.Stugman:
          placementMenu.Title = "ՍՏՈՒԳՄԱՆ ՌԵԺԻՄ";
          break;

      }
    }
  }

  public enum ModeEnum
  {
    Cucadrakan,
    Stugman,
    Ashxatanqayin,
  }
}
