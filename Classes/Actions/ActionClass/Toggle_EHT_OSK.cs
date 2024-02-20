using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_EHT_OSK : Action
    {
        public Toggle_EHT_OSK()
        {
            actionName = "Toggle_EHT_OSK";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            //calls main window toggle window
            if (!Application.Current.Windows.OfType<Everything_Handhelds_Tool.OSK.OSK>().Any())
            {
                Everything_Handhelds_Tool.OSK.OSK osk = new Everything_Handhelds_Tool.OSK.OSK();
                osk.Show();


                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.controllerInput.publicSuspendEventsForOSK = true;
            }

        }
    }
}
