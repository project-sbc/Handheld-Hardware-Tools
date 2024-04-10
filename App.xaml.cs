using Handheld_Hardware_Tools.AppWindows.MainWindow;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;


namespace Handheld_Hardware_Tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
   


        public App()
        {
          
            //Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string stringsFile = Path.Combine(Directory, "Styles", _DefaultStyle);
            //LoadStyleDictionaryFromFile(stringsFile);
            

        }
      

        protected override async void OnStartup(StartupEventArgs e)
        {
     
            bool quietStart = false;
            //if start is from system32 (task scheduled start) then set quietStart to true, means auto start
            if (String.Equals("C:\\Windows\\System32", Directory.GetCurrentDirectory(), StringComparison.OrdinalIgnoreCase))
            {
                quietStart = true;
            }

            SplashScreenStartUp splashScreen = null;


            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            if (!settings.hideSplashScreen && !quietStart)
            {
                //if not quiet start then show splashscreen
                splashScreen = new SplashScreenStartUp();
                splashScreen.Show();
            }

            
            QuickAccessMenu qam = await LoadMainWindowAsync();
            this.MainWindow = qam;
            

            if (quietStart)
            {
                qam.Hide();
            }
            else
            {
                if (!settings.hideSplashScreen)
                {
                    splashScreen.Close();
                }
                qam.Show();
            }


        }


        private async Task<QuickAccessMenu> LoadMainWindowAsync()
        {
            // Simulate loading MainWindow asynchronously
            

            // Create MainWindow instance
            var qam = new QuickAccessMenu();

            return qam;
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
