using Everything_Handhelds_Tool.Models;
using System;
using System.Windows;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Models.DefaultModels;
using Microsoft.Win32;
using System.Windows.Threading;
using Everything_Handhelds_Tool.Classes.Models;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Interop;
using Wpf.Ui.Controls;
using Everything_Handhelds_Tool.Classes.Devices;
using System.Windows.Media.Effects;

namespace Everything_Handhelds_Tool
{
    
    
    public partial class MainWindow : UiWindow 
    {

        Device device;

        public MainWindow()
        {
            //Get the device type
            Device_Management device_Management = new Device_Management();
            device = device_Management.device;


            InitializeComponent();

            //Write log to tell app is open
            Log_Writer.writeLog("App started");

            //set nav menu items from model
            SetNavigationMenuItemSource();

            //subscribe to events
            SubscribeEvents();

            //update status bar values and start dispatcher timer for statusbar
            SetUpStatusBarStartDispatcherTimer();

        }
        #region Set up
        public void SetNavigationMenuItemSource()
        {
            navigationViewListBox.ItemsSource = new NavigationViewMenuItems();
        }

        private void SubscribeEvents()
        {
            //subscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent += HandleControllerInputTopLevel;
            //subscribe to power changed (to update status bar)
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

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

                    break;
                case "DPadUp":
                    General_Functions.NavigateListView(navigationViewListBox, action);
                    break;
                case "DPadDown":
                    General_Functions.NavigateListView(navigationViewListBox, action);
                    break;
                case "A":
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
            Log_Writer.writeLog("App closing");
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
            if (navigationViewListBox.SelectedItem != null)
            {
                Wpf.Ui.Common.SymbolRegular symbolIcon = (Wpf.Ui.Common.SymbolRegular)navigationViewListBox.SelectedItem;
                switch (symbolIcon) 
                {
                    case Wpf.Ui.Common.SymbolRegular.Home12:
                        frame.Source = new Uri("Pages/HomePage.xaml", UriKind.Relative);

                    break;
                    

                    default: break;
                
                }
            }
    
        }


        #region LoadedEvent
        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //set location
            SetAppLocationHeight();
        }

        #endregion
    }
}
