﻿using Handheld_Hardware_Tools.AppWindows.MainWindow;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Threading;
using Handheld_Hardware_Tools.Classes.Devices;
using System.Diagnostics;


namespace Handheld_Hardware_Tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Device device = null;
        public SplashScreenStartUp splashWindow;
        public static System.Windows.Forms.NotifyIcon icon;
        public Thread splashScreenThread = null;
        public App()
        {
            


            //run this so that everything shuts down when the main window closes
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            SetUpNotifyIcon();
            //string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string stringsFile = Path.Combine(dir, "Styles", "DefaultTheme.xaml");
            //LoadStyleDictionaryFromFile(stringsFile);
           
            
        }

        private void SetUpNotifyIcon()
        {
            App.icon = new System.Windows.Forms.NotifyIcon();
            icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + "Handheld Hardware Tools.exe");
            icon.Visible = true;
            icon.Text = "Handheld Hardware Tools"; 
            icon.Click += Icon_Click;
        
        }
             

        private void Icon_Click(object? sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs mouseEventArgs = (System.Windows.Forms.MouseEventArgs)e;

            if (mouseEventArgs.Button == System.Windows.Forms.MouseButtons.Left)
            {
                QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();

                if (qam != null)
                {
                    qam.ToggleWindow();
                }
            }

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message = "An unhandled exception just occurred: " + e.Exception.Message + ". Stack Trace: " + e.Exception.StackTrace + ". Source: " + e.Exception.Source + ". Inner Exception: " + e.Exception.InnerException;
            MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            Log_Writer.Instance.writeLog(message);
            Environment.Exit(0);

        }
        protected override async void OnStartup(StartupEventArgs e)
        {

            CheckMultipleHHTProcessRunning();
            
            //determine if this is an autostart by checking if running from system32
            bool quietStart = false;
            //if start is from system32 (task scheduled start) then set quietStart to true, means auto start
            if (String.Equals("C:\\Windows\\System32", Directory.GetCurrentDirectory(), StringComparison.OrdinalIgnoreCase))
            {
                quietStart = true;
            }

            //lets check if quiet start is enabled (no splashscreen)
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            //run start up routines
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                device = new Device_Management().device;
                SetDefaultTDP(settings.defaultTDP);

            }));


         


            QuickAccessMenu qam = null;
            //Thread splashScreenThread = null;
            if (!settings.hideSplashScreen && !quietStart)
            {
                //if not quiet start then show splashscreen using separate thread so we can load the 
                //QAM on the UI thread but at the same time not block a UI thread for the splashscreen so the window loading spins :)  <-- smiley face for me
               
                splashScreenThread = new Thread(StartSplashScreenThread);
               
                splashScreenThread.SetApartmentState(ApartmentState.STA);
                splashScreenThread.IsBackground = true;

                splashScreenThread.Start();
                qam = new QuickAccessMenu();
            }
            else
            {
                qam = new QuickAccessMenu();
            }
            
            this.MainWindow = qam;


            qam.Show();
            if (quietStart)
            {
                qam.Hide();
            }
            else
            {
                if (settings.launchLargeWindow)
                {
                    qam.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                }
                else
                {
                    qam.Show();
                 
     
                }
               
            }


        }
        private void StartSplashScreenThread()
        {
            splashWindow = new SplashScreenStartUp();
            splashWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
          
        }

        private void SetDefaultTDP(int defaultTDP)
        {
            if (defaultTDP != 0)
            {
                TDP_Management.Instance.ChangeSustainedBoostTDP(defaultTDP, defaultTDP);
            }
           
        }

        private void CheckMultipleHHTProcessRunning()
        {
            Process[] hhtProcesses = Process.GetProcessesByName("Handheld Hardware Tools");

            if (hhtProcesses != null)
            {
                if (hhtProcesses.Length > 1) 
                {
                    this.Shutdown();
                }
            }
        }
        public void CancelSplashScreen()
        {
            if(splashWindow != null)
            {
                splashWindow.Dispatcher.Invoke(() => splashWindow.Close());
            }
            
        }
        /// <summary>
        /// This funtion loads a ResourceDictionary from a file at runtime
        /// </summary>
        public void LoadStyleDictionaryFromFile(string inFileName)
        {
            if (File.Exists(inFileName))
            {
                try
                {
                    using (var fs = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read in ResourceDictionary File
                        var dic = (ResourceDictionary)XamlReader.Load(fs);
                        // Clear any previous dictionaries loaded
                        Resources.MergedDictionaries.Clear();
                        // Add in newly loaded Resource Dictionary
                        Resources.MergedDictionaries.Add(dic);
                    }
                }
                catch
                {
                }
            }
        }

    }

   
}
