using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Everything_Handhelds_Tool.Classes
{
    public class Update_Management
    {

        private static Update_Management _instance = null;
        private static readonly object lockObj = new object();
        private Update_Management()
        {
        }
        public static Update_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Update_Management();
                        }
                    }
                }
                return _instance;
            }
        }

   
      
        public void checkForUpdates(bool startUpRoutine = false)
        {
            //check for updates if this is called at startup and the setting for allow check at startup is on OR if this is not at startup and called from settings

            AutoUpdater.InstallationPath = AppDomain.CurrentDomain.BaseDirectory; ;
            AutoUpdater.CheckForUpdateEvent += (args) => AutoUpdaterOnCheckForUpdateEvent(args);

            if (startUpRoutine)
            {
                AutoUpdater.InstallationPath = AppDomain.CurrentDomain.BaseDirectory; ;
                AutoUpdater.Start("https://raw.githubusercontent.com/project-sbc/Handheld-Control-Panel/master/Update.xml");

            }


        }
        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

                settings.lastUpdateCheck = DateTime.Now;

                settings.SaveToXML();


                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;
                    if (args.Mandatory.Value)
                    {
                        dialogResult =
                            System.Windows.Forms.MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is required update. Press Ok to begin updating the application.", @"Update Available",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            System.Windows.Forms.MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Do you want to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    // Uncomment the following line if you want to show standard update dialog instead.
                    // AutoUpdater.ShowUpdateForm(args);

                    if (dialogResult.Equals(System.Windows.Forms.DialogResult.Yes) || dialogResult.Equals(System.Windows.Forms.DialogResult.OK))
                    {
                        try
                        {
                            //Throw event to close main window

                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                //close window here
                                QuickAccessMenu quickAccessMenu = Local_Object.Instance.GetQAMWindow();
                                quickAccessMenu.Close();
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Windows.Forms.MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    if (1==1)
                    {
                        System.Windows.Forms.MessageBox.Show(@"There is no update available please try again later.", @"No update available",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    if (1==1)
                        System.Windows.Forms.MessageBox.Show(
                            @"There is a problem reaching update server. Please check your internet connection and try again later.",
                            @"Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(args.Error.Message,
                        args.Error.GetType().ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

        }


    }
}
