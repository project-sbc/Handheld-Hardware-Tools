using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Profiles;
using Microsoft.Win32;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
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
        public bool suspendUpdateProfileToExeDictionary
        {
            get
            {
                return suspendUpdateProfileToExeDictionary;
            }
            set
            {
                suspendUpdateProfileToExeDictionary = value;
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

        private Profile LoopThroughDictionaryForActiveExe()
        {
            //this is the main code to find if a exe is running from the dictionary
            if (profileDictionary != null)
            {
                if (profileDictionary.Count > 0)
                {
                    Process[] pList = Process.GetProcesses();
                    foreach (KeyValuePair<string, Profile> row in profileDictionary)
                    {
                        string processName = row.Key;

                        if (pList.Single(c => c.ProcessName.ToString() == processName) != null)
                        {
                            return row.Value;
                        }
                    }


                }
            }
            //if no process found exit with null
            return null;

        }

        private async void MainProfileMonitorThreadLoop()
        {
            try
            {
                Log_Writer.Instance.writeLog("Starting ProfileThreadLoop");
                               
                UpdateDictionary();

                while (this != null)
                {
                    //this is going to check a few things



                    if (!suspendUpdateProfileToExeDictionary)
                    {
                        
                       
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
