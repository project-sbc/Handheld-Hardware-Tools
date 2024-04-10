
using System.Linq;
using System.Windows;
using Handheld_Hardware_Tools.AppWindows.Guide;
using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.WindowManager;
namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{

    public class Toggle_Guide : Action
    {
        public Toggle_Guide()
        {
            actionName = "Toggle_Guide";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {

            Application.Current.Dispatcher.BeginInvoke(() =>
            {//NEEDS TO RUN ON UI THREAD OR ELSE ERROR
                if (!Application.Current.Windows.OfType<Guide>().Any())
                {
                    Guide guide = new Guide();
                    guide.Show();
                }
                else
                {
                    Guide guide = Application.Current.Windows.OfType<Guide>().FirstOrDefault();
                    if (guide != null)
                    {
                        guide.ResetTimer();
                    }
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
