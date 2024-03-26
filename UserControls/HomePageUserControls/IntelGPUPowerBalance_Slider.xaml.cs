using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Everything_Handhelds_Tool.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class IntelGPUPowerBalance_Slider : ControllerUserControl
    {

        private bool dragStarted = false;
        public IntelGPUPowerBalance_Slider()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = slider;

            //set control
           ConfigureControl();

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private void ConfigureControl()
        {
            Classes.Devices.Device device = Local_Object.Instance.GetMainWindowDevice();
            if (device.cpuType == "Intel")
            {
                slider.Value = PowerBalance_Management.Instance.ReadAndReturnGpuPowerBalance();
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
            
        }

     

        private void ControllerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }

        private void control_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragStarted = false;
            ControlChangeValueHandler();
        }

        private void control_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragStarted = true;
        }

     
        public override void ControlChangeValueHandler()
        {
            Classes.Devices.Device device = Local_Object.Instance.GetMainWindowDevice();
            if (device.cpuType == "Intel")
            {
                int value = (int)Math.Round(slider.Value, 0);
                PowerBalance_Management.Instance.ChangePowerBalance(value, "GPU");
            }
                
        }
    }
}
