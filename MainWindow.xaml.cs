﻿using Everything_Handhelds_Tool.Models;
using ModernWpf;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Models.DefaultModels;
using Microsoft.Win32;
using System.Windows.Threading;
using Everything_Handhelds_Tool.Classes.Models;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Everything_Handhelds_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

            //Write log to tell app is open
            Log_Writer.writeLog("App started");

            //set nav menu items from model
            SetNavigationMenuItemSource();

            //subscribe to events
            SubscribeEvents();

            //start statusbar dispatcher timer
            StartStatusBarDispatcherTimer();


            //update time and power status
            UpdateTime();
            UpdatePowerStatusBar();
            UpdateNetworkStatus();
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
       

        #endregion
        #region status bar updates
        private DispatcherTimer statusBarDispatcherTimer = new DispatcherTimer();
        private void StartStatusBarDispatcherTimer()
        {
            statusBarDispatcherTimer.Interval = new TimeSpan(0, 0, 6);
            statusBarDispatcherTimer.Tick += StatusBarDispatcherTimer_Tick;
            statusBarDispatcherTimer.Start();
        }

        private void StatusBarDispatcherTimer_Tick(object? sender, EventArgs e)
        {
            UpdateTime();
            UpdatePowerStatusBar();
            UpdateNetworkStatus();
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
                        if (ethernetStatusBarIcon.Visibility == Visibility.Collapsed)
                        {
                            ethernetStatusBarIcon.Visibility = Visibility.Visible;
                        }
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
            if (ethernetStatusBarIcon.Visibility == Visibility.Visible)
            {
                ethernetStatusBarIcon.Visibility = Visibility.Collapsed;
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
        public bool controllerNavigateWindow = true;

        private void SetControllerNavigateWindow(bool navigateValue)
        {
            //set the controllerNavigateWindow from page level
            controllerNavigateWindow= navigateValue;
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

        }
        #endregion
        #region Closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //unsubscribe to all events to make sure it doesn't keep the app open
            UnsubscribeEvents();

            //stop dispatcher timer for status bar
            statusBarDispatcherTimer.Stop();


        }
        private void UnsubscribeEvents()
        {
            //unsubscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent -= HandleControllerInputTopLevel;
            //unsubscribe to power changed (to update status bar)
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;


        }
        #endregion

      
    }
}
