using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerUserControl : UserControl
    {
        public virtual void HandleControllerInput(string action) { }
        public virtual void HighlightControl() { }
        public virtual void UnhighlightControl() { }
    }
}
