using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace ArmRusGazProm
{
	public partial class TestModeMenu
	{
		public TestModeMenu()
		{
			this.InitializeComponent();

      kahBtn.MouseEnter += KahBtnMouseEnter;
      chajBtn.MouseEnter += KahBtnMouseEnter;
      skaaBtn.MouseEnter += KahBtnMouseEnter;
      eppBtn.MouseEnter += KahBtnMouseEnter;
      kahBtn.MouseLeave += KahBtnMouseLeave;
      chajBtn.MouseLeave += KahBtnMouseLeave;
      skaaBtn.MouseLeave += KahBtnMouseLeave;
      eppBtn.MouseLeave += KahBtnMouseLeave;
		}

    private void KahBtnMouseEnter(object sender, MouseEventArgs e)
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

    private void KahBtnMouseLeave(object sender, MouseEventArgs e)
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