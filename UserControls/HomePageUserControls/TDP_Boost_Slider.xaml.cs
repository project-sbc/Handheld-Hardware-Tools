using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
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

namespace Handheld_Hardware_Tools.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class TDP_Boost_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
        public TDP_Boost_Slider()
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
            if (settings.syncSustainedBoostTDP)
            {
                toggleSwitch.IsChecked = true;
            }
            else
            {
                toggleSwitch.IsChecked = false;
            }
            control.Maximum = settings.maxTDP;
            control.Minimum = settings.minTDP;

            control.Value = TDP_Management.Instance.ReadAndReturnBoostTDP();
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

        private void ReadTDP()
        {
            int tdp = TDP_Management.Instance.ReadAndReturnSustainedTDP();
        }
        public override void ControlChangeValueHandler()
        {
            int tdp = (int)Math.Round(control.Value, 0);
            TDP_Management.Instance.ChangeBoostTDP(tdp);
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

            int sustainedTDP = TDP_Management.Instance.ReturnSustainedTDP();
            if (toggleSwitch.IsChecked == true)
            {
                settings.syncSustainedBoostTDP = false;
                control.Value = TDP_Management.Instance.ReturnSustainedTDP();
                
            }
            else
            {
                settings.syncSustainedBoostTDP = true;
            }
            settings.SaveToXML();

            TDP_Management.Instance.ChangeSustainedBoostTDP(sustainedTDP, sustainedTDP);
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
