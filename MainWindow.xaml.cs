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
using Wpf.Ui.Controls;
using Everything_Handhelds_Tool.Classes.Devices;
using System.Diagnostics;


namespace Everything_Handhelds_Tool
{
        
    public partial class MainWindow : UiWindow 
    {

        public Device device;

        public MainWindow()
        {
            //Get the device type (i.e. win max 2, one x fly, etc)
            device = new Device_Management().device;

            InitializeComponent();
            //run start up
            InitializeRoutines();


            ADLX_Management.SetGPUScaling(0);
            //Debug.WriteLine("GPU SCALING SUPPORT: " +ADLX_Management.HasGPUScalingSupport().ToString());
            ADLX_Management.SetGPUScaling(1);
            Debug.WriteLine("GPU SCALING SUPPORT: " + ADLX_Management.HasGPUScalingSupport().ToString());
            Debug.WriteLine("SCALING MODE SUPPORT: " + ADLX_Management.HasScalingModeSupport().ToString());
            ADLX_Management.SetScalingMode(1);
            Debug.WriteLine("SCALING MODE: " + ADLX_Management.GetScalingMode().ToString());
            ADLX_Management.SetIntegerScaling(1);

        }
        #region Set up
        private void InitializeRoutines() 
        { 

            //Write log to tell app is open
            Log_Writer.Instance.writeLog("Start Main Window");

            //tdp start up


            //set nav menu items from model
            SetNavigationMenuItemSource();

            //subscribe to events
            SubscribeEvents();

            //update status bar values and start dispatcher timer for statusbar
            SetUpStatusBarStartDispatcherTimer();
        }

        public void SetNavigationMenuItemSource()
        {
            navigationViewListBox.ItemsSource = new NavigationViewMenuItems();
            navigationViewListBox.Items.Refresh();
        }

        private void SubscribeEvents()
        {
            //subscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent += HandleControllerInputTopLevel;
            //subscribe to power changed (to update status bar)
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
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
        #region status bar updates
        private DispatcherTimer statusBarDispatcherTimer = new DispatcherTimer();
        private void SetUpStatusBarStartDispatcherTimer()
        {
            UpdateTime();
            UpdatePowerStatusBar();
            UpdateNetworkStatus();


            UpdateControllerStatus();

            statusBarDispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            statusBarDispatcherTimer.Tick += StatusBarDispatcherTimer_Tick;
            statusBarDispatcherTimer.Start();
        }

        private void StatusBarDispatcherTimer_Tick(object? sender, EventArgs e)
        {
            //Tick for updating status bar. Separate from updatestatusbar routine so it can be updated independent of the timer
            UpdateStatusBar();
        }
        private void UpdateStatusBar()
        {
            UpdateTime();
            UpdatePowerStatusBar();
            UpdateNetworkStatus();
            UpdateControllerStatus();
        }

        private void UpdateControllerStatus()
        {
            if (controllerInput == null)
            {
                controllerStatusBarIcon.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                if (controllerInput.controller == null)
                {
                    controllerStatusBarIcon.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    if (controllerInput.controller.IsConnected)
                    {
                        if (controllerStatusBarIcon.Visibility == Visibility.Collapsed)
                        {
                            controllerStatusBarIcon.Visibility = Visibility.Visible;
                            return;
                        }

                    }
                    else
                    {
                        if (controllerStatusBarIcon.Visibility == Visibility.Visible)
                        {
                            controllerStatusBarIcon.Visibility = Visibility.Collapsed;
                            return;
                        }
                    }
                }
            }
          
        }
        private void UpdateNetworkStatus()
        {
            
            //Gets internet status to display on overlay
            NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkCard in networkCards)
            {
                if (networkCard.OperationalStatus == OperationalStatus.Up)
                {
                    if (networkCard.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        
                        if (noInternetStatusBarIcon.Visibility == Visibility.Visible) { noInternetStatusBarIcon.Visibility = Visibility.Collapsed; }
                        return;
                    }
                    if (networkCard.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    {
                        if (wifiStatusBarIcon.Visibility == Visibility.Collapsed)
                        {
                            wifiStatusBarIcon.Visibility = Visibility.Visible;
                        }
                        if (noInternetStatusBarIcon.Visibility == Visibility.Visible) { noInternetStatusBarIcon.Visibility = Visibility.Collapsed; }
                        return;
                    }
                }
            }
            noInternetStatusBarIcon.Visibility = Visibility.Visible;
            if (wifiStatusBarIcon.Visibility == Visibility.Visible)
            {
                wifiStatusBarIcon.Visibility = Visibility.Collapsed;
            }
            
        }
        private void UpdateTime()
        {
            timeStatusBar.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
        }
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:

                    break;
                case PowerModes.StatusChange:
                    UpdatePowerStatusBar();
                    break;
            }

          
        }

        private void UpdatePowerStatusBar()
        {
            try
            {
                PowerStatus ps = new PowerStatus();
                if (ps.powerStatus == "AC")
                {
                    viewBoxBatteryPercentage.Visibility = Visibility.Collapsed;
                    batteryStatusBarIcon.Visibility = Visibility.Collapsed;
                    chargingStatusBarIcon.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (ps.powerStatus == "Online")
                    {
                        batteryStatusBarIcon.Visibility = Visibility.Collapsed;
                        chargingStatusBarIcon.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        batteryStatusBarIcon.Visibility = Visibility.Visible;
                        chargingStatusBarIcon.Visibility = Visibility.Collapsed;
                    }
                    viewBoxBatteryPercentage.Visibility = Visibility.Visible;
                    batteryPercentageStatusBar.Text = ps.batteryLevel+"%";
                }
            }
            catch(Exception ex)
            {

            }
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

            //stop dispatcher timer for status bar
            statusBarDispatcherTimer.Stop();

            //Write log to tell app is closing
            Log_Writer.Instance.writeLog("App closing");

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
            //unsubscribe to power changed (to update status bar)
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;


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

            //update status bar because timer takes 5 seconds to update
            UpdateStatusBar();

            //Resume statusbar update timer
            statusBarDispatcherTimer.Start();

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
                        
            //Stop statusbar update timer
            statusBarDispatcherTimer.Stop();

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
                    case "SelectSaveBack":
                        uri = new Uri("ControllerInstructionPages\\SelectSaveBack.xaml", UriKind.Relative);
                        break;
                    case "SelectAddBack":
                        uri = new Uri("ControllerInstructionPages\\SelectAddBack.xaml", UriKind.Relative);
                        break;
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
