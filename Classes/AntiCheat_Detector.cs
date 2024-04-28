using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Documents;

namespace Handheld_Hardware_Tools.Classes
{
    public static class AntiCheat_Detector
    {
        //THIS CLASS IS TO CHECK IF ANTI CHEAT IS RUNNING AND STOP TDP AND OTHER RELATED CALLS TO PREVENT 
        //BANS IN GAME
        private static List<string> antiCheatServiceNames = new List<string>()
        {
            "EasyAntiCheat",
            "BattlEye"
        };
        public static bool AntiCheatRunning()
        {
            
            if (CheckEasyAntiCheatRunning())
            {
                return true;
            }


            return false;
        }
         
        private static bool CheckEasyAntiCheatRunning()
        {
            try
            {
            

                var sc = ServiceController.GetServices();
                foreach (string serviceName in antiCheatServiceNames)
                {
                    var service = sc.FirstOrDefault(s => s.ServiceName.Contains(serviceName));
                    if (service !=null)
                    {
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            Log_Writer.Instance.writeLog(service.DisplayName + " anti-cheat detected running.");
                            return true;
                        }
                    }
                   
                }

                return false;


            }
            catch (Exception ex)
            {
                // Handle exceptions such as service not found
                
                return false;
            }
        }
    }
}
