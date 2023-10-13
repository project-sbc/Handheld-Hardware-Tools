using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_WifiAP: Action
    {
        public Toggle_WifiAP()
        {
            actionName = "Toggle_WifiAP";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            //calls main window toggle window
            if (Wifi_Management.Instance.IsWifiRunning())
            {
                Wifi_Management.Instance.StopWifi();
            }
            else
            {
                Wifi_Management.Instance.StartWifi();
            }
        }
    }
}
