using AudioSwitcher.AudioApi;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Devices;
using System;

using System.Windows;

namespace Handheld_Hardware_Tools.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class TurboCPU_ToggleSwitch : ControllerUserControl
    {
        private bool dragStarted = false;
        public TurboCPU_ToggleSwitch()
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
            toggleSwitch.IsChecked = Powercfg_Management.Instance.ReadAndReturnCPUTurboStatus();

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
                Powercfg_Management.Instance.ToggleCPUTurboStatus(true);
            }
            else
            {
                Powercfg_Management.Instance.ToggleCPUTurboStatus(false);
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
