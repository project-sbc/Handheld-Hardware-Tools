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
using Everything_Handhelds_Tool.Classes.Wifi_AP;
using System.IO;
using Everything_Handhelds_Tool.ControllerInputPages;
using Wpf.Ui;
using System.Windows.Markup;
using System.Xml;
using Everything_Handhelds_Tool.Classes.Profiles;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Everything_Handhelds_Tool.AppWindows.WindowManager;
using Everything_Handhelds_Tool.Classes.MouseMode;
using WindowsInput;
using Everything_Handhelds_Tool.AppWindows.Guide;


namespace Everything_Handhelds_Tool
{
        
    public partial class MainWindow : ControllerWindow 
    {

        public Device device;
        public MouseMode mouseMode;
        public InputSimulator inputSimulator = new InputSimulator();
        public WiFiDirectHotspotManager wifiAP = null;
        public ProfileManager profileManager = new ProfileManager();
        public MouseKeyHook mouseKeyHook = new MouseKeyHook();

      
        public MainWindow()
        {
            //Get the device type (i.e. win max 2, one x fly, etc)
            device = new Device_Management().device;

            //subscribe mouse key events
            mouseKeyHook.Subscribe();

            InitializeComponent();
            //run start up
            InitializeRoutines();

            
            //set your common items between windows (this has to do with the ControllerWindow custom class)
            commonFrame = frame;
            instructionFrame = frameControllerInput;

            //run this so that everything shuts down when the main window closes
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;


            //These are for a future feature of custom formats
            //FileStream xamlFile = new FileStream("Styles\\NewTheme.xaml", FileMode.Open, FileAccess.Read);

            //Application.Current.Resources.MergedDictionaries.Add(((ResourceDictionary)XamlReader.Load(xamlFile)));

            Guide guide = new Guide();
            guide.Show();

        }

        public void CloseMouseMode()
        {//close mouse mode, make sure to unsubscribe to controller events
            if (mouseMode != null)
            {
                mouseMode.UnsubscribeControllerEvents();
                mouseMode = null;
            }
        }

        public void ToggleMouseMode()
        {
            if (mouseMode != null)
            {
                mouseMode.UnsubscribeControllerEvents();
                mouseMode = null;
            }
            else
            {
                mouseMode = new MouseMode();
            }
        }

        #region Set up
        private void InitializeRoutines() 
        { 

            //Write log to tell app is open
            Log_Writer.Instance.writeLog("Start Main Window");

            //unhide powercfg stuff
            Powercfg_Management.Instance.UnhidePowerCfgSettings();
            

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
            SubscribeControllerEvents();
            //subscribe to power changed (to update status bar)
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        }
        


        private void SetAppLocationHeight()
        {
            Double height = General_Functions.GetWindowHeight(this);

            if (1 == 0)
            {
                this.Height = height * 0.98;
                this.MinHeight = height * 0.98;
                this.MaxHeight = height * 0.98;
                this.Top = height * 0.01;
            }
            else
            {
                this.Height = height;
                this.MinHeight = height ;
                this.MaxHeight = height;
                this.Top = 0;
            }


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
       



        public override void HandleControllerInputTopLevel(object? sender, controllerInputEventArgs e)
        {
            if(this.Visibility == Visibility.Visible)
            {
                //THIS IS NEW CODE, I WANT TO USE JOYSTICK ONLY ON CERTAIN OBJECTS, for this window i want the newly added
                //joystick inputs with a string of  Joystick_DPadLeft etc etc... to work as normal dpad inputs so im going to convert it at the object level back to DPadLeft
                string action = e.Action;
                //this replaces joystick identifier to make joystick to dpad work
                if (action.Contains("Joystick_")) { action = action.Replace("Joystick_", ""); }


                if (!controllerNavigateWindow)
                {//if not navigating at the window level pass input to page level
                    ControllerPage a = (ControllerPage)frame.Content;
                    a.HandleControllerInput(action);
                }
                else
                {//send input to window handler
                    HandleControllerInput(action);
                }
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
                frame.Content = General_Functions.TranslateIconToPageURI((Wpf.Ui.Common.SymbolRegular)navigationViewListBox.SelectedItem);

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

            //determine if a full screen game is running, if so we will open the app as NON-FOCUSABLE. this makes it so that some games won't minimize
            DetermineAppFocusOnFullScreenGame();

            //set app to normal state and visible
            this.Show();

            

        }

        private void DetermineAppFocusOnFullScreenGame()
        {
            //this code checks for full screen games
            List<Process> listProcesses = new List<Process>();

            Process[] pList = Process.GetProcesses();
            foreach (Process p in pList)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    Debug.WriteLine(p.ProcessName);

                    if (ScreenProgram_Management.IsForegroundFullScreen(new HandleRef(null, p.MainWindowHandle), null) && !ScreenProgram_Management.ExcludeFullScreenProcessList.Contains(p.ProcessName))
                    {
                        //this is where we will make app non focusable
                        SetWindowAsNonFocusable();
                        return;
                    }
                }
            }

            //if no full screen game is found we end up here
            SetWindowAsFocusable();
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
        #region making the app non focusable

        //used in non focus app
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private void SetWindowAsNonFocusable()
        {
            //set app as non focusable 
            var helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }
        private void SetWindowAsFocusable()
        {
            //set app as focusable 
            var helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) & ~WS_EX_NOACTIVATE);
        }



        #endregion
        #endregion


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }

      
        private void ContextMenu_Show(object sender, RoutedEventArgs e)
        {
            TasksToggleWindowOpen();
        }
        private void ContextMenu_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

        private void notifyIcon_LeftClick(NotifyIcon sender, RoutedEventArgs e)
        {
            TasksToggleWindowOpen();
        }
    }
}
