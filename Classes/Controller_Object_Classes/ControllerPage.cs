
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.Classes.Controller_Object_Classes
{
    public class ControllerPage : UiPage
    {
        //controllerNavigatePage is a bool to determine if actions go to usercontrol or stay at page level
        public bool controllerNavigatePage = true;


       public ControllerPage()
        {
            this.Unloaded += ControllerPage_Unloaded;
        }

        private void ControllerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            OnUnload();
        }

        public virtual void OnUnload() { userControls.Clear(); }
        public virtual void HandleControllerInput(string action) 
        {
            if (controllerNavigatePage)
            {               
                switch (action)
                {
                    case "B":
                        PressBPageHandler();
                        break;
                    case "Highlight First Control":
                        //This is when the page first gains controller input, highlight the first UC to show it has control with controller input
                        highlightedUserControl = 0;
                        HighlightUserControl();
                        SetPageDefaultInstruction();
                        break;
                    case "A":
                        //make sure a control is highlighted (index not -1), otherwise there is a navigation bug that comes up
                        if (highlightedUserControl > -1)
                        {
                            //I AM ADDING THIS SPECIAL BUTTON A action so that
                            //people dont need to press A twice for button stuff
                            //otherwise they have to press A twice to click a button
                            //WE CHECK TO SEE IF MAINCONTROL IS BUTTON AND THERE ISNT A TOGGLE SWITCH
                            //OTHERWISE WE SELECT THE CONTROL
                            ControllerUserControl uc = userControls[highlightedUserControl];
                            if (uc.mainControl is System.Windows.Controls.Button && uc.toggleSwitchControl == null)
                            {
                                SendControllerInputToUserControl("Button_A");
                            }
                            else
                            {
                                controllerNavigatePage = false;
                                SelectUserControl();
                            }
                            
                          
                            
                        }
            
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
                //check to make sure a control is selected
                if (highlightedUserControl > -1)
                {
                    SendControllerInputToUserControl(action);
                }

                
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
        public virtual void PressBPageHandler()
        {
            //I created this because sometimes when B is pressed it goes to a different page like in sub setting pages
            //so rather than always just giving control bck to the window, this will reroute to a different page
            //The default behavior will be give control bcak to window but it can be overridden
            //This returns navigation to the window, so unhighlight the active control and send SetControllerNavigateWindow to true
            UnhighlightUserControl();
            ReturnControlToWindow();
            //change controller input to SelectHide for window control
            General_Functions.ChangeControllerInstructionPage("SelectHide");

        }
        public virtual void ReturnControlToPage() 
        {
            //call this routine from the usercontrol to return to page control
            controllerNavigatePage = true;
            //put controller instruction back on for the page
           SetPageDefaultInstruction();
        }
        public virtual void SetPageDefaultInstruction()
        {
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
            DependencyObject parent = VisualTreeHelper.GetParent(this);

            // Traverse up the visual tree until we find a Window
            while (parent != null && !(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            // Check if we found a Window
            if (parent is Window)
            {
                ControllerWindow window = (ControllerWindow)parent;
                window.SetControllerNavigateWindow(true);
            }

            //OLD CODE WHEN I ONLY HAD ONE WINDOW, NOW TRYING TO COMMONIZE
            //MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            //wnd.SetControllerNavigateWindow(true);
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
