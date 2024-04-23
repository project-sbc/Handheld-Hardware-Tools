using AudioSwitcher.AudioApi;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Devices;
using System;

using System.Windows;

namespace Handheld_Hardware_Tools.UserControls.SettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class QAMSide_ToggleSwitch : ControllerUserControl
    {
 
        public QAMSide_ToggleSwitch()
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
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            toggleSwitch.IsChecked = settings.qamOnRightSide;

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
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
      
            if (toggleSwitch.IsChecked == true)
            {
                settings.qamOnRightSide = true;
            }
            else
            {
                settings.qamOnRightSide = false;
            }

            settings.SaveToXML();

            QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();
            qam.ChangeAppLocation(settings.qamOnRightSide);

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
