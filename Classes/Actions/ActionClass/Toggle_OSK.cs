
using System.Linq;
using System.Windows;
using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.UserControls.PowerPageUserControls;
namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_OSK : Action
    {
        public Toggle_OSK()
        {
            actionName = "Toggle_OSK";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {


            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!Application.Current.Windows.OfType<OSK>().Any())
                {
                    OSK osk = new OSK();
                    osk.Show();
                }

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                   
                    if (mainWindow.Visibility == Visibility.Visible)
                    {//menu if open, we dont want it getting in the way
                        mainWindow.ToggleWindow();
                    }
                 
                }


            });



        }
      
    }
   
}
