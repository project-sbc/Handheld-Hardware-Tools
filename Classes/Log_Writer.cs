using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool
{
    public static class Log_Writer
    {

        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        public static Object objLock = new Object();
        public static void writeLog(string newLog, string errorNum = "")
        {
            try
            {
                lock (objLock)
                {
                    if (!File.Exists(BaseDir + "\\Logs\\application_log.txt")) { createLogFile(); }
                    using (StreamWriter w = File.AppendText(BaseDir + "\\Logs\\application_log.txt"))
                    {
                        if (errorNum != "") { newLog = "Error " + errorNum + ": " + newLog; }

                        Log(newLog, w);

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error writing to log file. " + ex.Message);
            }


        }
        public static void Log(string logMessage, TextWriter w)
        {
            try
            {
                w.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} {logMessage}");
                w.Flush();
            }
            catch { }
        }
        public static void createLogFile()
        {
            try
            {
                if (!Directory.Exists(BaseDir + "\\Logs")) { System.IO.Directory.CreateDirectory(BaseDir + "\\Logs"); }
                if (!File.Exists(BaseDir + "\\Logs\\application_log.txt")) { File.CreateText(BaseDir + "\\Logs\\application_log.txt"); Thread.Sleep(300); }
   
                
            }
            catch { }

        }

    }
}
