using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using Shell32;
using System.Diagnostics.Eventing.Reader;
namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{
    public class Toggle_Controller : Action
    {
        public Toggle_Controller()
        {
            actionName = "Toggle_Controller";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //Shell objects need to run on STA thread, thats why i need to use a new thread, otherwise it wont work

            ControllerStatus_Management.Instance.ToggleEnableDisableController();
           
       


        }

     
       
    }
    
}
