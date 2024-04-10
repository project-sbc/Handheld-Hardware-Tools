using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{
    public class Toggle_MicrophoneMute : Action
    {
        public Toggle_MicrophoneMute()
        {
            actionName = "Toggle_MicrophoneMute";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //calls main window toggle window
            Volume_Management.Instance.ToggleMasterMicrophoneMute();
        }
    }
}
