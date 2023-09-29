using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Everything_Handhelds_Tool.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class Volume_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
        public Volume_Slider()
        {
            InitializeComponent();
            //set virtual border
            borderControl = border;
            mainControl = control;
            toggleSwitchControl = toggleSwitch;
            //set control
            ConfigureControl();
        }
        private void ConfigureControl()
        {
            int volume = Volume_Management.Instance.ReturnVolume();
            if (volume >= 0)
            {
                control.Value = volume;
                toggleSwitch.IsChecked = Volume_Management.Instance.ReadAndReturnVolumeMute();
                
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void control_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragStarted = false;
            ControlChangeValueHandler();
        }
        private void ControllerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }
        private void control_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragStarted = true;
        }
             
        public override void ControlChangeValueHandler()
        {
            //set volume
            int volume = (int)Math.Round(control.Value, 0);
            Volume_Management.Instance.SetMasterVolume(volume);

            //set mute
            if (toggleSwitch.IsChecked == true)
            {
                Volume_Management.Instance.SetMasterVolumeMute(true);
            }
            else
            {
                Volume_Management.Instance.SetMasterVolumeMute(false);
            }

        }

        public override void ChangeMainWindowControllerInstructionPage()
        {
            if (toggleSwitch.IsChecked == true)
            {
                General_Functions.ChangeControllerInstructionPage("ChangeToggleBack");
            }
            else
            {
                General_Functions.ChangeControllerInstructionPage("ToggleBack");
            }
        }
    }
}
