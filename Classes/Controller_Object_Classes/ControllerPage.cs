
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerPage : UiPage
    {
        public void HandleControllerInput(string action) 
        {
            switch (action)
            {
                case "B":
                    //This returns navigation to the window, so unhighlight the active control and send SetControllerNavigateWindow to true
                    UnhighlightUserControl(highlightedUserControl);
                    MainWindow wnd = (MainWindow)Application.Current.MainWindow;
                    wnd.SetControllerNavigateWindow(true);
                    break;
                case "Highlight First Control":
                    //This is when the page first gains controller input, highlight the first UC to show it has control
                    highlightedUserControl = 0;
                    HighlightUserControl(highlightedUserControl);
                    break;
                default: break;
            }

        }
        public virtual void ReturnControlToPage() { }
        public List<ControllerUserControl> userControls = new List<ControllerUserControl>();

        public int highlightedUserControl = -1;
        public int selectedUserControl = -1;

        public StackPanel virtualStackPanel;
        public void HandleUserControlNavigation(string action) 
        {


        }

        public void HighlightUserControl(int index)
        {
            if (userControls.Count> 0)
            {
                if (userControls[index] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[index];
                    controllerUserControl.HighlightControl();
                }
            }
        }
        public void UnhighlightUserControl(int index)
        {
            if (userControls.Count > 0)
            {
                if (userControls[index] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[index];
                    controllerUserControl.UnhighlightControl();
                }
            }
        }
    }
}
