
using System.Linq;
using System.Windows;
using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.QuickActionWheel;
using Handheld_Hardware_Tools.AppWindows.WindowManager;
namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{
    public class Toggle_QuickActionWheel : Action
    {
        public Toggle_QuickActionWheel()
        {
            actionName = "Toggle_QuickActionWheel";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
          

            Application.Current.Dispatcher.BeginInvoke(() =>
            {//NEEDS TO RUN ON UI THREAD OR ELSE ERROR
                if (!Application.Current.Windows.OfType<QuickActionWheel>().Any())
                {
                    QuickActionWheel qaw = new QuickActionWheel();
                    qaw.Show();
                }

                QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    if (mainWindow.Visibility == Visibility.Visible)
                    {
                        mainWindow.ToggleWindow();
                    }
            
                }


            });



        }
      
    }
   
}
