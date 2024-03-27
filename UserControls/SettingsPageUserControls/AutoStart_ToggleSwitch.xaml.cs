using AudioSwitcher.AudioApi;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using System;

using System.Windows;

namespace Everything_Handhelds_Tool.UserControls.SettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class AutoStart_ToggleSwitch : ControllerUserControl
    {
        private bool dragStarted = false;
        public AutoStart_ToggleSwitch()
        {
            InitializeComponent();
            //set virtual border
            borderControl = border;
            mainControl = toggleSwitch;
     
            //set control
            ConfigureControl();
        }
        private void ConfigureControl()
        {
            toggleSwitch.IsChecked = TaskScheduler_Management.Instance.CheckAutoStart();

        }

        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }



        public override void ControlChangeValueHandler()
        {
            HandleToggleChange();
        }

        private void HandleToggleChange()
        {
            //when this toggle is flipped, we will change whether tpd is synced or not in the settings. This setting is checked everytime 
            //tdp is changed

            //this little piece is just to stop it from running at load. The configure controls will handle all of this and it doesn't need to be run at runtime
            if (!this.IsLoaded)
            {
                return;
            }

            if (toggleSwitch.IsChecked == true)
            {
                TaskScheduler_Management.Instance.ChangeTaskService(true);
            }
            else
            {
                TaskScheduler_Management.Instance.ChangeTaskService(false);
            }


        }
        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            HandleToggleChange();

        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleToggleChange();
        }
    }
}
