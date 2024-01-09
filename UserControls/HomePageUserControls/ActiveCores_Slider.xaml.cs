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
    public partial class ActiveCores_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
        public ActiveCores_Slider()
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
            
            control.Maximum = Powercfg_Management.Instance.ReturnMaxNumberActiveCores();
            control.Value = Powercfg_Management.Instance.ReadAndReturnActiveCores();
            if (control.Maximum == control.Value)
            {
                toggleSwitch.IsChecked = false;
            }
            else
            {
                toggleSwitch.IsChecked = true;
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
            int activeCore = (int)Math.Round(control.Value, 0);
            Powercfg_Management.Instance.ChangeActiveCores(activeCore);
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
                        
            if (toggleSwitch.IsChecked == false)
            {
                int maxCores = (int)Math.Round(control.Maximum, 0);
                Powercfg_Management.Instance.ChangeActiveCores(maxCores);
            }


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
