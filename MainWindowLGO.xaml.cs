using Everything_Handhelds_Tool.Models;
using System;
using System.Windows;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Microsoft.Win32;
using System.Windows.Threading;
using Everything_Handhelds_Tool.Classes.Models;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Interop;
using Wpf.Ui.Controls;
using Everything_Handhelds_Tool.Classes.Devices;
using System.Windows.Media.Effects;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Common;
using Everything_Handhelds_Tool.Classes.Actions;
using Action = Everything_Handhelds_Tool.Classes.Actions.Action;
using System.Diagnostics;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.ControllerInputPages;
using System.Runtime.InteropServices;

namespace Everything_Handhelds_Tool
{
        
    public partial class MainWindowLGO : UiWindow 
    {

       
        public MainWindowLGO()
        {
         
            InitializeComponent();
            //run start up
            InitializeRoutines();

            
            
        }
        #region Set up
        private void InitializeRoutines() 
        { 
                
            //subscribe to events
            SubscribeEvents();

        }
             

        private void SubscribeEvents()
        {
            //subscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent += HandleControllerInputTopLevel;
        
            //subscribe to controller connection changed event
            controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent += HandleControllerConnectionChanged;
        }
      


        private void SetAppLocationHeight()
        {
            Double height = General_Functions.GetWindowHeight(this);
            this.Height = height * 0.98;
            this.MinHeight = height * 0.98;
            this.Top = height * 0.01;

        }

        #endregion
    
        #region Controller Navigation
        public ControllerInput controllerInput = new ControllerInput();
        //controllerNavigateWindow determines if input stays on window level or gets passed down
        public bool controllerNavigateWindow = true;

        public void SetControllerNavigateWindow(bool navigateValue)
        {
            //set the controllerNavigateWindow from page level
            controllerNavigateWindow= navigateValue;
            if (!controllerNavigateWindow )
            {
                ControllerPage a = (ControllerPage)frame.Content;
                a.HandleControllerInput("Highlight First Control");
            }
            
        }

        private void HandleControllerConnectionChanged(object sender, controllerConnectionChangedEventArgs e)
        {
            if (e.Connected) { SetControllerInputPage("SelectHide"); }
            else { SetControllerInputPage(""); }
        }
        private void HandleControllerInputTopLevel(object? sender, controllerInputEventArgs e)
        {
            if (!controllerNavigateWindow)
            {//if not navigating at the window level pass input to page level
                ControllerPage a = (ControllerPage)frame.Content;
                a.HandleControllerInput(e.Action);
            }
            else
            {//send input to window handler
                HandleControllerInput(e.Action);
            }
        }
        private void HandleControllerInput(string action)
        {
            switch(action)
            {
                case "B":
                    ToggleWindow();
                    break;
                case "DPadUp" or "DPadDown":
                    General_Functions.NavigateListView(navigationViewListBox, action);
                    break;
                case "A" or "DPadRight":
                    SetControllerNavigateWindow(false);
                    break;
                default: break;
            }
        }
        #endregion
        #region ClosingEvents
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //unsubscribe to all events to make sure it doesn't keep the app open
            UnsubscribeEvents();

         
            //unload notify icon
            UnloadNotifyIcon();
        }

        private void UnloadNotifyIcon()
        {
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void UnsubscribeEvents()
        {
            //unsubscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent -= HandleControllerInputTopLevel;
          

        }

        #endregion

        private void navigationViewListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadPageInFrame();   
        }

        private void LoadPageInFrame()
        {
            //loads page in frame when navigation menu item changes
            if (navigationViewListBox.SelectedItem != null)
            {
                frame.Source = General_Functions.TranslateIconToPageURI((Wpf.Ui.Common.SymbolRegular)navigationViewListBox.SelectedItem);
            }
        }

        public void LoadPageInFrameExternal(Uri uri)
        {
            frame.Source = uri;
        }


        #region LoadedEvent
        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //set location
            SetAppLocationHeight();

        }

        #endregion

        #region ToggleWindow
        private void TasksToggleWindowOpen()
        {
            //Tasks to do when window is reopening
                       
            //set location height
            SetAppLocationHeight();

            //Reload last page
            LoadPageInFrame();

            //set app to normal state and visible
            this.Show();

        }
        private void TasksToggleWindowClosed()
        {
            //Tasks to do when window is hiding
                        
           
            //unload page from frame so it needs to be reloaded
            frame.Source = null;
            
            //set app to hidden
            this.Visibility = Visibility.Hidden;
                       
        }
        public void ToggleWindow()
        {
            if (this.Visibility == Visibility.Hidden || this.WindowState == WindowState.Minimized)
            {
                TasksToggleWindowOpen();
            }
            else
            {
                TasksToggleWindowClosed();
            }
        }

        #endregion


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }

        public void SetControllerInputPage(string pageName)
        {
            //Sets the controller instruction on the bottom of the page
            //If controller is not connected the uri should be null to make the instruction disappear
            Uri uri = null;
            //if the controller icon, which updates based on controller connection, is visible, then change instruction
            if (controllerStatusBarIcon.Visibility == Visibility.Visible)
            {
                switch (pageName)
                {
                    case "SelectBack":
                        uri = new Uri("ControllerInstructionPages\\SelectBack.xaml", UriKind.Relative);
                        break;
                    case "SelectHide":
                        uri = new Uri("ControllerInstructionPages\\SelectHide.xaml", UriKind.Relative);
                        break;
                    case "ChangeBack":
                        uri = new Uri("ControllerInstructionPages\\ChangeBack.xaml", UriKind.Relative);
                        break;
                    case "ChangeToggleBack":
                        uri = new Uri("ControllerInstructionPages\\ChangeToggleBack.xaml", UriKind.Relative);
                        break;
                    case "ToggleBack":
                        uri = new Uri("ControllerInstructionPages\\ToggleBack.xaml", UriKind.Relative);
                        break;
                    case "EditDeleteBackMoveUpDown":
                        uri = new Uri("ControllerInstructionPages\\EditDeleteBackMoveUpDown.xaml", UriKind.Relative);
                        break;

                        
                    default:
                        break;
                }
            }
         

            frameControllerInput.Source = uri;
        }

        private void notifyIcon_LeftClick(NotifyIcon sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }
    }
}
