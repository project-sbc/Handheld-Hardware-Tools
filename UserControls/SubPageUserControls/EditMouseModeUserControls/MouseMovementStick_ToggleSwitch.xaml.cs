using AudioSwitcher.AudioApi;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using Everything_Handhelds_Tool.Pages;
using System;

using System.Windows;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.UserControls.SubPageUserControls.EditMouseModeUserControls
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class MouseMovementStick_ToggleSwitch : ControllerUserControl
    {
      
        public MouseMovementStick_ToggleSwitch()
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
            
            MouseModeSettingsPage mouseModeSettingsPage = (MouseModeSettingsPage)Local_Object.Instance.GetMainWindowFramePage();

            if (mouseModeSettingsPage != null)
            {
                toggleSwitch.IsChecked = mouseModeSettingsPage.mouseProfile.rightScroll;
            }

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
            MouseModeSettingsPage mouseModeSettingsPage = (MouseModeSettingsPage)Local_Object.Instance.GetGeneralWindowPage(this);

            if (mouseModeSettingsPage != null)
            {
                
                if (toggleSwitch.IsChecked == true)
                {
                    mouseModeSettingsPage.mouseProfile.rightScroll = true;
                }
                else
                {
                    mouseModeSettingsPage.mouseProfile.rightScroll = false;
                }
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
