using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_Wifi : Action
    {
        public Toggle_Wifi()
        {
            actionName = "Toggle_Wifi";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            Wifi_BT_Management.Instance.ToggleWifi();
        }
    }
}
