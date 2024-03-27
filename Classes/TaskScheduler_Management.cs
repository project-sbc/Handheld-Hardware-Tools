
using System;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Win32.TaskScheduler;

namespace Everything_Handhelds_Tool.Classes
{
    public class TaskScheduler_Management
    {

        private static TaskScheduler_Management _instance = null;
        private static readonly object lockObj = new object();
        private TaskScheduler_Management()
        {
        }
        public static TaskScheduler_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new TaskScheduler_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        private string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        private string appName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;

        public void ChangeTaskService(bool systemAutoStart)
        {
            TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService();
            Task task = ts.GetTask(appName);

            if (!systemAutoStart && task != null)
            {
                task.RegisterChanges();
                ts.RootFolder.DeleteTask(appName);
            }
            if (systemAutoStart && task == null)
            {
                TaskDefinition td = ts.NewTask();

                td.RegistrationInfo.Description = appName;
                td.Triggers.AddNew(TaskTriggerType.Logon);
                td.Principal.RunLevel = TaskRunLevel.Highest;
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.StopIfGoingOnBatteries = false;
                td.Settings.RunOnlyIfIdle = false;

                td.Actions.Add(new ExecAction(BaseDir + "\\" + appName + ".exe"));

                Microsoft.Win32.TaskScheduler.TaskService.Instance.RootFolder.RegisterTaskDefinition(appName, td);
            }

        }

        public bool CheckAutoStart()
        {
            bool checkStart = false;

            TaskService ts = new TaskService();
            Task task = ts.GetTask("Handheld_Control_Panel");
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (task != null)
            {
                TaskDefinition td = task.Definition;
                if (td != null)
                {
                    foreach (Microsoft.Win32.TaskScheduler.Action action in td.Actions)
                    {


                        if (action.ActionType == TaskActionType.Execute)
                        {
                            if (String.Compare(Path.GetFullPath(action.ToString()), Path.GetFullPath(BaseDir + "\\Handheld Control Panel.exe"), StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                checkStart = true;
                            }
                            else
                            {
                                //this code deletes a task IF the app running directory doesn't match the task directory.
                                //its just to clean up outdated tasks and doesn't actually delete the current task
                                task.RegisterChanges();
                                ts.RootFolder.DeleteTask("Handheld_Control_Panel");
                            }
                        }
                    }
                }

            }
            return checkStart;

        }


    }
}
