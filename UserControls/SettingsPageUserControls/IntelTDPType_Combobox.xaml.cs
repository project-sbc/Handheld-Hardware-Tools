using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
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

namespace Handheld_Hardware_Tools.UserControls.SettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class IntelTDPType_Combobox : ControllerUserControl
    {
        int originalSelectedIndex = -1;

        public IntelTDPType_Combobox()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = comboBox;


            //set control
            //if its AMD we need to hide this
            if (Local_Object.Instance.DeviceCPUType() == "Intel")
            {
                ConfigureControl();
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private List<string> intelTDPTypeList = new List<string>()
        {
            { "MMIO" },
            { "MMIO+MSR" },
            { "MSR" }
        };
        
        private void ConfigureControl()
        {
            comboBox.ItemsSource = intelTDPTypeList;

            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

          

            foreach (string value in comboBox.ItemsSource)
            {
                if (value == settings.intelTDPType)
                {
                    comboBox.SelectedItem = value;
                    originalSelectedIndex = comboBox.SelectedIndex;
                }
            }

        }




        public override void ReturnControlToPage()
        {
            //NEED TO DO EXTRA, if combobox ORIGINAL selected index != new,  we go back to the old value. It only changes when you press A
            if (comboBox.SelectedIndex != originalSelectedIndex)
            {
                comboBox.SelectedIndex = originalSelectedIndex;
            }

            //still run normal code for return to page
            base.ReturnControlToPage();
        }


        public override void ControlChangeValueHandler()
        {
            //set originalselectedindex to the NEW selected index to prevent accidental change when returning control to page (where it will reset to original if indexes dont match)
            HandleDropDownChanged();
        }

        private void HandleDropDownChanged()
        {
            if (comboBox.SelectedIndex != originalSelectedIndex && comboBox.SelectedIndex > -1)
            {
                originalSelectedIndex = comboBox.SelectedIndex;

                Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
                settings.intelTDPType = comboBox.Text;
                settings.SaveToXML();
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            HandleDropDownChanged();
        }
    }


}
