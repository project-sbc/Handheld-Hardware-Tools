using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Profiles;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes
{
    public class ProfileManager
    {
        public Dictionary<string, Profile> profileDictionary = new Dictionary<string, Profile>();


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




        private async void MainProfileMonitorThreadLoop()
        {
            try
            {
                Log_Writer.Instance.writeLog("Starting ProfileThreadLoop");
                               
                UpdateDictionary();

                while (this != null)
                {


                    if (!suspendUpdateProfileToExeDictionary)
                    {
                        if (profileDictionary != null)
                        {
                            if (profileDictionary.Count > 0)
                            {

                                foreach(KeyValuePair<string,Profile> row in profileDictionary)
                                {

                                }


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
