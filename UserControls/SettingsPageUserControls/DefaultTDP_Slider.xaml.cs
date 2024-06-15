using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Handheld_Hardware_Tools.UserControls.SettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class DefaultTDP_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
        public DefaultTDP_Slider()
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
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            if (settings.defaultTDP == 0)
            {
                toggleSwitch.IsChecked = false;
            }
            else
            {
                toggleSwitch.IsChecked = true;
            }
            control.Maximum = settings.maxTDP;
            control.Minimum = settings.minTDP;
            control.Value = settings.defaultTDP;

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
            int tdp = (int)Math.Round(control.Value, 0);
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            settings.defaultTDP = tdp;
            settings.SaveToXML();
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

        private void handleToggleChange()
        {
            //when this toggle is flipped, we will change whether tpd is synced or not in the settings. This setting is checked everytime 
            //tdp is changed

            //this little piece is just to stop it from running at load. The configure controls will handle all of this and it doesn't need to be run at runtime
            if (!this.IsLoaded)
            {
                return;
            }

            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            if (toggleSwitch.IsChecked == true)
            {
                settings.defaultTDP = (int)Math.Round(control.Value,0);
                

            }
            else
            {
                settings.defaultTDP = 0;
            }
            settings.SaveToXML();

        }
        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            handleToggleChange();
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            handleToggleChange();
        }
    }
}



