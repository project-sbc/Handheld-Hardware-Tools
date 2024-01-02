using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_MicrophoneMute : Action
    {
        public Toggle_MicrophoneMute()
        {
            actionName = "Toggle_MicrophoneMute";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            //calls main window toggle window
            Volume_Management.Instance.ToggleMasterMicrophoneMute();
        }
    }
}
