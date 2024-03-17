using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_BT : Action
    {
        public Toggle_BT()
        {
            actionName = "Toggle_BT";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            Wifi_BT_Management.Instance.ToggleBT();
        }
    }
}
