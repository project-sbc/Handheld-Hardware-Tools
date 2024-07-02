using Handheld_Hardware_Tools.Models;
using System;
using System.Windows;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Microsoft.Win32;
using System.Windows.Threading;
using Handheld_Hardware_Tools.Classes.Models;
using System.Globalization;
using System.Net.NetworkInformation;
using Wpf.Ui.Controls;
using Handheld_Hardware_Tools.Classes.Devices;
using System.Diagnostics;
using Handheld_Hardware_Tools.Classes.Wifi_AP;
using System.IO;
using Handheld_Hardware_Tools.ControllerInputPages;
using Wpf.Ui;
using System.Windows.Markup;
using System.Xml;
using Handheld_Hardware_Tools.Classes.Profiles;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Handheld_Hardware_Tools.AppWindows.WindowManager;
using Handheld_Hardware_Tools.Classes.MouseMode;
using WindowsInput;
using Handheld_Hardware_Tools.AppWindows.Guide;
using Handheld_Hardware_Tools.AppWindows.QuickActionWheel;
using Handheld_Hardware_Tools.AppWindows.MainWindow;
using System.Threading;
using Handheld_Hardware_Tools.AppWindows.OSK;
using System.Linq;
using Handheld_Hardware_Tools.AppWindows.AyaNeoFlipDSApp;
using System.Reflection.Metadata;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using Handheld_Hardware_Tools.Classes.Motherboard_Info;
using System.Threading.Tasks;




namespace Handheld_Hardware_Tools
{
        
    public partial class QuickAccessMenu : ControllerWindow 
    {

       
        public MouseMode mouseMode;
        public InputSimulator inputSimulator = new InputSimulator();
        public WiFiDirectHotspotManager wifiAP = null;
        public ProfileManager profileManager = new ProfileManager();
        public MouseKeyHook mouseKeyHook = new MouseKeyHook();

      
        public QuickAccessMenu()
        {
            

            //subscribe mouse key events
            mouseKeyHook.Subscribe();

            InitializeComponent();

            //run start up routines
            Application.Current.Dispatcher.BeginInvoke(new Action(() => InitializeRoutines()));

            ADLX_Management.SetFPSLimit(0, true, 60);

            
            //set your common items between windows (this has to do with the ControllerWindow custom class)
            commonFrame = frame;
            instructionFrame = frameControllerInput;

          
            //These are for a future feature of custom formats
            //FileStream xamlFile = new FileStream("Styles\\NewTheme.xaml", FileMode.Open, FileAccess.Read);

            //Application.Current.Resources.MergedDictionaries.Add(((ResourceDictionary)XamlReader.Load(xamlFile)));


            string picLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Styles", Application.Current.Resources["SplashWindowLogo"].ToString());
            

            //
            MainWindowLogo.Source = new BitmapImage(new Uri(picLocation));
           

          
            
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
            Task.Run(() => Log_Writer.Instance.writeLog("Start Main Window"));
            Task.Run(() => Powercfg_Management.Instance.UnhidePowerCfgSettings());
            

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
            //subscribe to 333333 changed (to update status bar)
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        }
        

        public void ChangeAppLocation(bool qamRight)
        {
         
            if (qamRight)
            {
                System.Windows.Forms.Screen windowScreen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);

                double scaling = GetDPIScaling() / 100;
                double leftPosition = Math.Round(windowScreen.Bounds.Width / (scaling), 0) - this.Width;
                this.Left = leftPosition;

            }
            else
            {
                this.Left = 0;
            }
        }

        public double GetDPIScaling()
        {
            return Math.Round(VisualTreeHelper.GetDpi(this).DpiScaleX * 100, 0);
        }

        public void SetAppLocationHeight()
        {
            //Things to consider setting location and height
            //DPI scaling (need monitor resolution AND scaling to calculate height and left
            //need setting preference
 
            this.Top = 0;
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            System.Windows.Forms.Screen windowScreen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);

            this.Width = 565;
            this.MaxWidth = 565;
            this.MinWidth = 565;


            double scaling = GetDPIScaling()/100;
            double leftPosition = Math.Round(windowScreen.Bounds.Width / (scaling),0)-this.Width;
            
            double height = Math.Round(windowScreen.WorkingArea.Height / scaling, 0);
            this.MinHeight = height;
            this.MaxHeight = height;
      
            if (settings.qamOnRightSide)
            {
                this.Left = leftPosition;
            }
            else
            {
                this.Left = 0;
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
                case PowerModes.Suspend:
                    Log_Writer.Instance.writeLog("about to sleep!");
                    break;

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
                Classes.Models.PowerStatus ps = new Classes.Models.PowerStatus();
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

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }

            //set app to normal state and visible
            this.Visibility = Visibility.Visible;
            //this.Show();

            

        }


        Process gameProcess = null;

        private void DetermineAppFocusOnFullScreenGame()
        {
            //this code checks for full screen games
            List<Process> listProcesses = new List<Process>();

            Process[] pList = Process.GetProcesses();
            foreach (Process p in pList)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    

                    if (ScreenProgram_Management.IsForegroundFullScreen(new HandleRef(null, p.MainWindowHandle), null) && !ScreenProgram_Management.ExcludeFullScreenProcessList.Contains(p.ProcessName))
                    {
                        //this is where we will make app non focusable
                        Debug.WriteLine(p.ProcessName);
                        gameProcess = p;
                        SetWindowAsNonFocusable(p.MainWindowHandle);

                        //SetWindowAsFocusable();
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
            HandleNonNullGameProcess();

            //Stop statusbar update timer
            statusBarDispatcherTimer.Stop();

            //unload page from frame so it needs to be reloaded
            frame.Source = null;
            
            //set app to hidden
            this.Visibility = Visibility.Hidden;
                       
        }

        private void HandleNonNullGameProcess()
        {
            //this handles if a full window app was running when the app was opened, we can check if the window is not open and reopen it for us
            if (gameProcess != null)
            {
                if (!ScreenProgram_Management.IsForegroundFullScreen(new HandleRef(null, gameProcess.MainWindowHandle), null))
                {
                    //the above checks if the app is still fullscreen, if not  (i.e. using !) then we get here
                    ScreenProgram_Management.SetWindowState(gameProcess.MainWindowHandle, WindowState.Normal);

                }
                gameProcess = null;
            }

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
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        //used to keep track of 
        private IntPtr gameHandle = IntPtr.Zero;
        private void SetWindowAsNonFocusable(IntPtr game)
        {
            //set app as non focusable 
            var helper = new WindowInteropHelper(this);
            SetForegroundWindow(helper.Handle);
            //SetWindowLong(helper.Handle, GWL_EXSTYLE, GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);

           
            //gameHandle = game;
            //set game as non focusable to see if it stops controller input
            //SetWindowLong(game, GWL_EXSTYLE, GetWindowLong(game, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }
        private void SetWindowAsFocusable()
        {
            //set app as focusable 

            var helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE, GetWindowLong(helper.Handle, GWL_EXSTYLE) & ~WS_EX_NOACTIVATE);

            if (gameHandle != IntPtr.Zero)
            {
                //SetWindowLong(gameHandle, GWL_EXSTYLE, GetWindowLong(gameHandle, GWL_EXSTYLE) & ~WS_EX_NOACTIVATE);
               // gameHandle = IntPtr.Zero;
            }
           
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

        private void quickAccessMenu_ContentRendered(object sender, EventArgs e)
        {
            ((App)Application.Current).CancelSplashScreen();


            if (String.Equals("C:\\Windows\\System32", Directory.GetCurrentDirectory(), StringComparison.OrdinalIgnoreCase))
            {
                this.Hide();
            }
            else
            {
                SetAppLocationHeight();
            }
        }
    }
}
