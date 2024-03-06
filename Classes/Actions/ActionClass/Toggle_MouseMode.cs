using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_MouseMode : Action
    {
        public Toggle_MouseMode()
        {
            actionName = "Toggle_MouseMode";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            //calls main window toggle window
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ToggleMouseMode();

            });
        }
    }
}
