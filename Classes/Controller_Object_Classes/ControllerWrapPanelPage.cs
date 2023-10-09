
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
    public class ControllerWrapPanelPage : ControllerPage
    {
       
        public override void HandleControllerInput(string action) 
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
                        SendControllerInputToUserControl(action);
                        break;
                    case "DPadUp" or "DPadDown" or "DPadLeft" or "DPadRight":
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
         
 
        public override void HandleUserControlNavigation(string action) 
        {
            //handles moving up an down in stack panel. also moves to screen so that the scrollviewer adjusts
            UnhighlightUserControl();
            if (action == "DPadLeft")
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
            if (action == "DPadRight")
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
            if (action == "DPadDown")
            {
                //REMEMBER THIS IS WRAP PANEL WHERE UP AND DOWN MOVE 3 OBJECTS AT A TIME
                //If highlightedUserControl is more than 3 away from the end, just move 3
                if (highlightedUserControl < (userControls.Count - 3))
                { 
                    highlightedUserControl += 3;
                }
                else
                {
                    //if the highlightedUserControl is on the last one, move to the beginning
                    //OTHERWISE, move to the last control regardless if its 3,2, or 1 away
                    if (highlightedUserControl == (userControls.Count - 1))
                    {
                        highlightedUserControl = 0;
                    }
                    else
                    {
                        highlightedUserControl = userControls.Count - 1;
                    }
                    
                }
            }
            if (action == "DPadUp")
            {
                //REMEMBER THIS IS WRAP PANEL WHERE UP AND DOWN MOVE 3 OBJECTS AT A TIME
                //If highlightedUserControl is more than 3 away from the beginning, just move 3
                if (highlightedUserControl > 2)
                { 
                    highlightedUserControl -= 3;
                }
                else
                {
                    //if the highlightedUserControl is on the first one, move to the end which is usercontrols.count - 1
                    //OTHERWISE, move to the first control regardless if its 3,2, or 1 away
                    if (highlightedUserControl == 0)
                    {
                        highlightedUserControl = userControls.Count-1;
                    }
                    else
                    {
                        highlightedUserControl = 0;
                    }

                }
            }
            HighlightUserControl();
        }

       
    }
}
