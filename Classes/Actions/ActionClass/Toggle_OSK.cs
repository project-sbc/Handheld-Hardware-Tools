
using System.Linq;
using System.Windows;
using Everything_Handhelds_Tool.AppWindows.OSK;
namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_OSK : Action
    {
        public Toggle_OSK()
        {
            actionName = "Toggle_OSK";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            if (!Application.Current.Windows.OfType<OSK> ().Any())
            {
                OSK osk = new OSK();
                osk.Show();
            }

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.controllerInput.publicSuspendEventsForOSK = true;
                    mainWindow.ToggleWindow();
                }


            });



        }
      
    }
   
}
