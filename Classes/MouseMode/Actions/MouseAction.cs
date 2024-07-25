using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;


namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{

    public class MouseAction
    {
        public string mouseActionName;
      
        public virtual void ClickEvent() { }

        public virtual string ReturnArgument()
        {
            return "";
        }
    }
}
