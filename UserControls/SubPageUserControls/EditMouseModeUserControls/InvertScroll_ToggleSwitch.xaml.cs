using AudioSwitcher.AudioApi;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Devices;
using Handheld_Hardware_Tools.Classes.MouseMode;
using Handheld_Hardware_Tools.Pages;
using System;

using System.Windows;
using System.Windows.Controls;

namespace Handheld_Hardware_Tools.UserControls.SubPageUserControls.EditMouseModeUserControls
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class InvertScroll_ToggleSwitch : ControllerUserControl
    {
      
        public InvertScroll_ToggleSwitch(bool value)
        {
            InitializeComponent();
            //set virtual border
            borderControl = border;
            mainControl = toggleSwitch;
     
            //set control
            ConfigureControl(value);
        }
        private void ConfigureControl(bool value)
        {

            toggleSwitch.IsChecked = value;

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

            MouseProfile mouseProfile = (MouseProfile)XML_Management.Instance.LoadXML("MouseProfile");

            if (mouseProfile != null)
            {

                if (toggleSwitch.IsChecked == true)
                {
                    mouseProfile.reverseVerticalScroll = true;
                }
                else
                {
                    mouseProfile.reverseVerticalScroll = false;
                }
            }

            mouseProfile.SaveToXML();


            QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();

            if (qam != null)
            {
                if (qam.mouseMode != null)
                {
                    qam.mouseMode.UpdateMouseProfile();
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
