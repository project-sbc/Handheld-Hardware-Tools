using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Handheld_Hardware_Tools.UserControls.PowerPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class CloseProgram_Button : ControllerUserControl
    {
        private Process currentProcess = null;
      
        public CloseProgram_Button()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;

            ConfigureControl();
        }

        private void ConfigureControl()
        {
            string processName = GetProcessNameFullScreen();

            if (processName == null)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                programName.Text = processName;
            }
        }


        private string GetProcessNameFullScreen()
        {

            List<Process> listProcesses = new List<Process>();

            Process[] pList = Process.GetProcesses();
            foreach (Process p in pList)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    Debug.WriteLine(p.ProcessName);

                    if (!listProcesses.Contains(p) && ScreenProgram_Management.IsForegroundFullScreen(new HandleRef(null, p.MainWindowHandle), null) && !ScreenProgram_Management.ExcludeFullScreenProcessList.Contains(p.ProcessName))
                    {
                        currentProcess = p; 
                
                        return p.ProcessName;
                    }
                }
            }
            return null;
        }

        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (currentProcess !=null) 
            { 
                currentProcess.Kill();
                
            }
        }
    }
}
