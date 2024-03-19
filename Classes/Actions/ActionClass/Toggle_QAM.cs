using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_QAM : Action
    {
        public Toggle_QAM()
        {
            actionName = "Toggle_QAM";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //calls main window toggle window
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;
                mainWindow.ToggleWindow();

            });
        }
    }
}
