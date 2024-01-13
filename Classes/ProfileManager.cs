using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Profiles;
using Microsoft.Win32;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes
{
    public class ProfileManager
    {
        public Dictionary<string, Profile> profileDictionary = new Dictionary<string, Profile>();
        private string currentPowerStatus;
        public Profile activeProfile = null;

        private bool _suspendUpdateProfileToExeDictionary = false;
        public bool suspendUpdateProfileToExeDictionary
        {
            get
            {
                return _suspendUpdateProfileToExeDictionary;
            }
            set
            {
                _suspendUpdateProfileToExeDictionary = value;
            }
        }
        public ProfileManager()
        {


            Thread thread = new Thread(MainProfileMonitorThreadLoop);
            thread.IsBackground = true;
            thread.Name = "ProfileMonitorThread";
            thread.Priority = ThreadPriority.Normal;


            thread.Start();
            

        }

        private void HandlePowerStatusChanged()
        {
            //simple code, if mainwindow gets the poewr changed event (because of the status bar) it will just call this
            if (activeProfile != null)
            {
                activeProfile.ApplyProfile();
            }
        }

        private bool CheckIfProcessIsRunning(string processName)
        {
            Process[] pList = Process.GetProcessesByName(processName);
            if (pList != null)
            {
                if (pList.Length > 0) { return true; }
            }
            return false;
        }

        private Profile LoopThroughDictionaryForActiveExeProfile()
        {
            //this is the main code to find if a exe is running from the dictionary
            if (profileDictionary != null)
            {
                if (profileDictionary.Count > 0)
                {
                   
                    foreach (KeyValuePair<string, Profile> row in profileDictionary)
                    {
                        Process[] pList = Process.GetProcessesByName(row.Key);

                        if (pList.Length > 0)
                        {
                            return row.Value;
                        }


                    }


                }
            }
            //if no process found exit with null
            return null;

        }

        private bool CheckActiveProfileShouldContinueRunning()
        {
            if (activeProfile != null)
            {
                if (activeProfile.processExe != "")
                {
                    if (CheckIfProcessIsRunning(activeProfile.processExe))
                    {
                        return true;
                    }
                }


            }
            return false;
        }

        private async void MainProfileMonitorThreadLoop()
        {
            try
            {
                //Log_Writer.Instance.writeLog("Starting ProfileThreadLoop");
                               
                UpdateDictionary();

                while (this != null)
                {
                   
                    if (!suspendUpdateProfileToExeDictionary)
                    {
                        //main thread here
                     
                        //Check to see if active profile should keep running, I used a ! to capture when it SHOULDNT keep running (i.e. exe is not running anymore) or if the active profile is null
                        if (!CheckActiveProfileShouldContinueRunning())
                        {
                            //this function below gets a profile if an active exe is running, otherwise it will return null
                            Profile newActiveProfile = LoopThroughDictionaryForActiveExeProfile();

                            if (newActiveProfile != null)
                            {
                                //if not null, then apply it and make the active profile that new profile
                                newActiveProfile.ApplyProfile();
                                activeProfile = newActiveProfile;
                            }

                        }

                    }
                    else
                    {

                        if (suspendUpdateProfileToExeDictionary)
                        {
                            UpdateDictionary();
                            suspendUpdateProfileToExeDictionary = false;
                        }
                        
                    }


                    //task delay for 10 ms to match approx. 100 Hz refresh of controller
                    await Task.Delay(2000);




                }
                Log_Writer.Instance.writeLog("Ending ProfileThreadLoop");
            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Error in profile thread; " + ex.Message, "PM02");
            }
        }

        private void UpdateDictionary()
        {
            ProfileList profiles = (ProfileList)XML_Management.Instance.LoadXML("ProfileList");

            
            profileDictionary = profiles.ReturnProcessExeDictionary();


        }


    }
}
