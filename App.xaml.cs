using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace ArmRusGazProm
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

      var processes = System.Diagnostics.Process.GetProcessesByName(processName);

      if(processes.Length > 1)
      {
        MessageBox.Show("Ծրագիրը արդեն բացված է");
        Application.Current.Shutdown();
      }
    }

    
  }
}
