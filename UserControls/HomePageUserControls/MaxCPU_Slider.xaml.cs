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
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class MaxCPU_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
        public MaxCPU_Slider()
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
            bool turboEnabled = Powercfg_Management.Instance.ReadAndReturnCPUClockStatus();
            if (turboEnabled)
            {
                toggleSwitch.IsChecked = false;
                toggleSwitch2.IsChecked = false;
            }
            else
            {
                int maxCPU = Powercfg_Management.Instance.ReturnMaxCPUClock();
                if (maxCPU > 0)
                {
                    toggleSwitch.IsChecked = true;
                    toggleSwitch2.IsChecked = true;
                    control.Value = (double)Math.Round((decimal)maxCPU/100, 0)*100;
                }
                else
                {
                    toggleSwitch.IsChecked = true;
                    toggleSwitch2.IsChecked = false;
                }
            }
            
        }

        public override void HandleControllerInput(string action)
        {
            if (mainControl != null)
            {


                if (toggleSwitchControl != null)
                {
                    ToggleSwitch tS = toggleSwitchControl as ToggleSwitch;
                    if (action == "X")
                    {
                        tS.IsChecked = !tS.IsChecked;
                        ChangeMainWindowControllerInstructionPage();
                    }
                    else
                    {


                        if (tS.IsChecked == true )
                        {
                            if (toggleSwitch2.IsChecked == true )
                            {
                                HandleSliderInput(action);
                            }
                            else
                            {
                                if (action == "A")
                                {
                                    toggleSwitch2.IsChecked = !toggleSwitch2.IsChecked;
                                }
                            }

                            
                        }
                        else
                        {
                            if (action == "B")
                            {
                                ReturnControlToPage();
                            }
                        }
                    }
                }
                
            }
        }

        private void HandleSliderInput(string action)
        {
            Slider slider = mainControl as Slider;
            switch (action)
            {
                case "DPadRight":
                    slider.Value = slider.Value + slider.Interval;
                    break;
                case "DPadLeft":
                    slider.Value = slider.Value - slider.Interval;
                    break;
                case "A":
                    toggleSwitch2.IsChecked = !toggleSwitch2.IsChecked;
                    break;
                case "B":
                    ControlChangeValueHandler();
                    ReturnControlToPage();
                    break;

                default: break;
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
            int maxclock = (int)Math.Round(control.Value, 0);
            Powercfg_Management.Instance.ChangeMaxCPUClock(maxclock);
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

        private void handleToggleChange(int switchNumber)
        {
            //when this toggle is flipped, we will change whether tpd is synced or not in the settings. This setting is checked everytime 
            //tdp is changed

            //this little piece is just to stop it from running at load. The configure controls will handle all of this and it doesn't need to be run at runtime
            if (!this.IsLoaded)
            {
                return;
            }
            if (switchNumber == 1)
            {
                if (toggleSwitch.IsChecked == true)
                {
                    if (Powercfg_Management.Instance.ReadAndReturnMaxCPUClock() > 0 && toggleSwitch2.IsChecked == false)
                    {
                        toggleSwitch2.IsChecked = true;
                    }
                  
                    Powercfg_Management.Instance.ChangeTurboEnabledState(false);

                }
                else
                {
                    Powercfg_Management.Instance.ChangeTurboEnabledState(false);
                }
            }
            else
            {
                //For toggleSwitch2
                if (toggleSwitch2.IsChecked == false)
                {
                    Powercfg_Management.Instance.ChangeTurboEnabledState(true);

                }
            }
         
           
        }
        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (ToggleSwitch)sender;
            if (ts.Name == "toggleSwitch")
            {
                handleToggleChange(1);
            }
            else
            {
                handleToggleChange(2);
            }
           
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (ToggleSwitch)sender;
            if (ts.Name == "toggleSwitch")
            {
                handleToggleChange(1);
            }
            else
            {
                handleToggleChange(2);
            }
        }
    }
}
