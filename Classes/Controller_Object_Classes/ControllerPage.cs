
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerPage : UiPage
    {
        //controllerNavigatePage is a bool to determine if actions go to usercontrol or stay at page level
        public bool controllerNavigatePage = true;

        public void HandleControllerInput(string action) 
        {
            if (controllerNavigatePage)
            {               
                switch (action)
                {
                    case "B":
                        //This returns navigation to the window, so unhighlight the active control and send SetControllerNavigateWindow to true
                        UnhighlightUserControl(highlightedUserControl);
                      
                        break;
                    case "Highlight First Control":
                        //This is when the page first gains controller input, highlight the first UC to show it has control
                        highlightedUserControl = 0;
                        HighlightUserControl(highlightedUserControl);
                        break;
                    case "A":
                        controllerNavigatePage = false;
                        break;
                    case "DPadUp":
                        controllerNavigatePage = false;
                        break;
                    case "DPadDown":
                        controllerNavigatePage = false;
                        break;
                    default: break;
                }
            }
            else
            {
                if (userControls.Count > 0)
                {
                    if (userControls[highlightedUserControl] != null)
                    {
                        ControllerUserControl controllerUserControl = userControls[highlightedUserControl];
                        controllerUserControl.HandleControllerInput(action);
                    }
                }
            }

          

        }
        public void ReturnControlToPage() 
        {
            //call this routine from the usercontrol to return to page control
            controllerNavigatePage = true;
        }
        public List<ControllerUserControl> userControls = new List<ControllerUserControl>();

        public int highlightedUserControl = -1;
 
        //set in page cs to the stackpanel
        public StackPanel virtualStackPanel;
        public void HandleUserControlNavigation(string action) 
        {


        }

        public void ReturnControlToWindow()
        {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.SetControllerNavigateWindow(true);
        }
        public void HighlightUserControl(int index)
        {
            //sends highlight command to usercontrol
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
            //sends unhighlight command to usercontrol
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
