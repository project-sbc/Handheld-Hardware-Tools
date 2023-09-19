using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes
{
    public class Run_CLI
    {
        private static Run_CLI _instance = null;
        private static readonly object lockObj = new object();
        private Run_CLI()
        {
        }
        public static Run_CLI Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Run_CLI();
                        }
                    }
                }
                return _instance;
            }
        }

        public string RunCommand(string arguments, bool readOutput, string processName = "cmd.exe", int waitExit = 6000, bool runasadmin = true)
        {
            //Runs CLI, if readOutput is true then returns output

            try
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = false;
                if (readOutput) { startInfo.RedirectStandardOutput = true; } else { startInfo.RedirectStandardOutput = false; }

                startInfo.FileName = processName;
                //startInfo.Arguments = "/c " + arguments;
                startInfo.Arguments = arguments;
                startInfo.CreateNoWindow = true;
                if (runasadmin) { startInfo.Verb = "runas"; } else { startInfo.Verb = ""; }
                startInfo.RedirectStandardError = readOutput;
                startInfo.RedirectStandardOutput = readOutput;

                process.EnableRaisingEvents = true;
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit(waitExit);
                if (readOutput)
                {
                    //int Errorlevel = process.ExitCode;

                    string output = process.StandardOutput.ReadToEnd();
                    process.Close();
                    return output;

                }
                else
                {
                    process.Close();
                    return "COMPLETE";
                }


            }
            catch (Exception ex)
            {
                string error = "Error running CLI: " + ex.Message + " " + arguments;

                return error;

            }


        }



    }
}
