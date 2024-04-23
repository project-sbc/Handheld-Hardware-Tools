
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.HomePage;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Models;
using Handheld_Hardware_Tools.UserControls.HomePageUserControls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System;
using Handheld_Hardware_Tools.UserControls.EditActionUserControls;
using Handheld_Hardware_Tools.AppWindows.MainWindow;
using System.Windows;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : ControllerPage
    {
        
        public HomePage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddUserControlsToStackPanel();

            //hide top row when in main window, but keep when in QAM. reminder this is used in the QAM and the larger screen window
            Window parentWindow = Local_Object.Instance.GetGeneralWindow(this);
            if (parentWindow is MainWindow)
            {
                grid.RowDefinitions[0].Height = new GridLength(0);
            }
        }

        public async void AddUserControlsToStackPanel()
        {
            //Get the XML list from the UserConfiguration folder for what controls go on the home page
            HomePageUserControlList chpl = (HomePageUserControlList)XML_Management.Instance.LoadXML("HomePageUserControlList");
            chpl.UpdateList();
            //loop through and add the usercontrols
            foreach (HomePageItem hpi in chpl)
            {
                if (hpi.UserControlVisible)
                {
                    ControllerUserControl controllerUserControl = ReturnUserControlByName(hpi.UserControlName);
                    if (controllerUserControl != null)
                    {
                        if (controllerUserControl.Visibility == System.Windows.Visibility.Visible && controllerUserControl.useableOnDevice)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                stackPanel.Children.Add(controllerUserControl);
                                userControls.Add(controllerUserControl);
                            });
                         
                        }
                    }

                }
            }


        }
        private ControllerUserControl ReturnUserControlByName(string name)
        {
            switch(name)
            {
                case "Usercontrol_TDP":
                    return new TDP_Slider();
                case "Usercontrol_TDP2":
                    return new TDP_Boost_Slider();
                case "Usercontrol_Brightness":
                    return new Brightness_Slider();
                case "Usercontrol_Volume":
                    return new Volume_Slider();
                case "Usercontrol_PowerPlan":
                    return new PowerPlan_Combobox();
                case "Usercontrol_EPP":
                    return new EPP_Slider();
                case "Usercontrol_ActiveCores":
                    return new ActiveCores_Slider();
                case "Usercontrol_MaxCPUClock":
                    return new MaxCPU_Slider();
                case "Usercontrol_CPUTurbo":
                    return new TurboCPU_ToggleSwitch();

                case "Usercontrol_PowerBalanceCPU":
                    return new IntelCPUPowerBalance_Slider();
                case "Usercontrol_PowerBalanceGPU":
                    return new IntelGPUPowerBalance_Slider();

                case "Usercontrol_GPUCLK":
                    return new GPUClock_Slider();
                case "Usercontrol_DisplayResolution":
                    return new DisplayResolution_Combobox();
                case "Usercontrol_DisplayRefreshRate":
                    return new DisplayRefreshRate_Combobox();
                default:
                    return null;
            }
        }

       
    }
}
