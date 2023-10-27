
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

        public virtual void HandleControllerInput(string action) 
        {
            if (controllerNavigatePage)
            {               
                switch (action)
                {
                    case "B":
                        //This returns navigation to the window, so unhighlight the active control and send SetControllerNavigateWindow to true
                        UnhighlightUserControl();
                        ReturnControlToWindow();
                        //change controller input to SelectHide for window control
                        General_Functions.ChangeControllerInstructionPage("SelectHide");
                        break;
                    case "Highlight First Control":
                        //This is when the page first gains controller input, highlight the first UC to show it has control
                        highlightedUserControl = 0;
                        HighlightUserControl();
                        General_Functions.ChangeControllerInstructionPage("SelectBack");
                        break;
                    case "A":
                        controllerNavigatePage = false;
                        SelectUserControl();
                        break;
                    case "DPadUp" or "DPadDown":
                        HandleUserControlNavigation(action);

                        break;
                   
                    default: break;
                }
            }
            else
            {
                //if controllerNavigatePage is false, send the input to the usercontrol level
                SendControllerInputToUserControl(action);
            }

          

        }

        public void SendControllerInputToUserControl(string action)
        {
            //make sure the highlighted usercontrol isnt null then send command
            if (userControls.Count > 0)
            {
                if (userControls[highlightedUserControl] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[highlightedUserControl];
                    controllerUserControl.HandleControllerInput(action);
                }
            }
        }

        public void ReturnControlToPage() 
        {
            //call this routine from the usercontrol to return to page control
            controllerNavigatePage = true;
            //put controller instruction back
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }
        public List<ControllerUserControl> userControls = new List<ControllerUserControl>();

        public int highlightedUserControl = -1;
 
        //set in page cs to the stackpanel
        public StackPanel virtualStackPanel;
        public WrapPanel virtualWrapPanel;
        public ScrollViewer scrollViewer;
        public virtual void HandleUserControlNavigation(string action) 
        {
            //handles moving up an down in stack panel. also moves to screen so that the scrollviewer adjusts
            UnhighlightUserControl();
            if (action == "DPadUp")
            {
                if (highlightedUserControl > 0) 
                { //subtract 1 if > 0, otherwise go to bottom of stackpanel
                    highlightedUserControl -= 1;
                }
                else
                {
                    highlightedUserControl = userControls.Count - 1;
                }
            }
            else
            {
                if (highlightedUserControl < userControls.Count -1)
                { //add 1 if < total controls - 1, otherwise go to top of stackpanel
                    highlightedUserControl += 1;
                }
                else
                {
                    highlightedUserControl = 0;
                }
            }
            HighlightUserControl();
        }

        public void ReturnControlToWindow()
        {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.SetControllerNavigateWindow(true);
        }
        public void HighlightUserControl()
        {
            //sends highlight command to usercontrol
            if (userControls.Count> 0)
            {
                if (userControls[highlightedUserControl] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[highlightedUserControl];
                    controllerUserControl.HighlightControl();
                    controllerUserControl.BringIntoView();
                }
            }
        }
        public void SelectUserControl()
        {
            //sends highlight command to usercontrol
            if (userControls.Count > 0 && highlightedUserControl > -1)
            {
                if (userControls[highlightedUserControl] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[highlightedUserControl];
                    controllerUserControl.SelectControl();
                    controllerUserControl.BringIntoView();
                }
            }
        }
        public void UnhighlightUserControl()
        {
            //sends unhighlight command to usercontrol
            if (userControls.Count > 0 && highlightedUserControl > -1)
            {
                if (userControls[highlightedUserControl] != null)
                {
                    ControllerUserControl controllerUserControl = userControls[highlightedUserControl];
                    controllerUserControl.UnhighlightControl();
                }
            }
        }
    }
}
