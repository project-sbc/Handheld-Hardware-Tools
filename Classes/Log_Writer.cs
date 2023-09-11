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
    public class Log_Writer
    {
        private static Log_Writer _instance = null;


        private static readonly object lockObj = new object();

        private string appDir = AppDomain.CurrentDomain.BaseDirectory;

        private Log_Writer()
        {

        }
        public void writeLog(string newLog, string errorNum = "")
        {
            try
            {
                if (!File.Exists(appDir + "\\Logs\\application_log.txt")) { createLogFile(); }
                using (StreamWriter w = File.AppendText(appDir + "\\Logs\\application_log.txt"))
                {
                    if (errorNum != "") { newLog = "Error " + errorNum + ": " + newLog; }

                    Log(newLog, w);

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error writing to log file. " + ex.Message);
            }


        }
        private void Log(string logMessage, TextWriter w)
        {
            try
            {
                w.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} {logMessage}");
                w.Flush();
            }
            catch { }
        }
        private void createLogFile()
        {
            try
            {
                if (!Directory.Exists(appDir + "\\Logs")) { System.IO.Directory.CreateDirectory(appDir + "\\Logs"); }
                if (!File.Exists(appDir + "\\Logs\\application_log.txt")) { File.CreateText(appDir + "\\Logs\\application_log.txt"); Thread.Sleep(300); }


            }
            catch { }

        }
        public static Log_Writer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Log_Writer();
                        }
                    }
                }



                return _instance;
            }
        }


    }


   
}
