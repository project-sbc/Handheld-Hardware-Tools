using Handheld_Hardware_Tools.AppWindows.MainWindow;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Threading;


namespace Handheld_Hardware_Tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public SplashScreenStartUp splashWindow;

        public App()
        {

            //run this so that everything shuts down when the main window closes
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;


            //string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string stringsFile = Path.Combine(dir, "Styles", "DefaultTheme.xaml");
            //LoadStyleDictionaryFromFile(stringsFile);


        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message = "An unhandled exception just occurred: " + e.Exception.Message + ". Stack Trace: " + e.Exception.StackTrace + ". Source: " + e.Exception.Source + ". Inner Exception: " + e.Exception.InnerException;
            MessageBox.Show(message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            Log_Writer.Instance.writeLog(message);
            Environment.Exit(0);

        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            



            //determine if this is an autostart by checking if running from system32
            bool quietStart = false;
            //if start is from system32 (task scheduled start) then set quietStart to true, means auto start
            if (String.Equals("C:\\Windows\\System32", Directory.GetCurrentDirectory(), StringComparison.OrdinalIgnoreCase))
            {
                quietStart = true;
            }
                      

            //lets check if quiet start is enabled (no splashscreen)
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");


            QuickAccessMenu qam = null;
            if (!settings.hideSplashScreen && !quietStart)
            {
                //if not quiet start then show splashscreen using separate thread so we can load the 
                //QAM on the UI thread but at the same time not block a UI thread for the splashscreen so the window loading spins :)  <-- smiley face for me
               
                Thread splashScreenThread = new Thread(StartSplashScreenThread);
               
                splashScreenThread.SetApartmentState(ApartmentState.STA);
                splashScreenThread.IsBackground = true;

                splashScreenThread.Start();
                qam = new QuickAccessMenu(splashScreenThread);
            }
            else
            {
                qam = new QuickAccessMenu();
            }
            
            this.MainWindow = qam;


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
        public void CancelSplashScreen()
        {
            splashWindow.Dispatcher.Invoke(() => splashWindow.Close());
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
