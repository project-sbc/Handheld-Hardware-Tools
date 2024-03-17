
using System.Linq;
using System.Windows;
using Everything_Handhelds_Tool.AppWindows.Guide;
using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.AppWindows.WindowManager;
namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
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

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
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
