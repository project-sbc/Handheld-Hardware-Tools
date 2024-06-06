using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace Handheld_Hardware_Tools.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class Scaling_Combobox : ControllerUserControl
    {
        int originalSelectedIndex = -1;
       
        public Scaling_Combobox()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = comboBox;


            //set control
           ConfigureControl();

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        
        
        private void ConfigureControl()
        {
            QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();
            double scaling = qam.GetDPIScaling();
            foreach(ComboBoxItem cbi in comboBox.Items)
            {
                if (cbi.Content.ToString() == scaling.ToString())
                {
                    originalSelectedIndex = comboBox.Items.IndexOf(cbi);
                    comboBox.SelectedIndex = originalSelectedIndex;
                    break;
                }
            }

          


        }


        public override void SetComboBoxIndexToPreviousValue()
        {
           //Run this code when a combobox opens  but the user presses B instead of clicking a new combobox item
           //itll set the index back to the previous value. needs to run at individual level because generic class doesnt know the index
           if (originalSelectedIndex > -1)
           {
                comboBox.SelectedIndex = originalSelectedIndex;
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

                if (comboBox.SelectedItem != null)
                {
                    ComboBoxItem cbi = comboBox.SelectedItem as ComboBoxItem;
                    Display_Management.Instance.SetDisplayScaling(cbi.Content.ToString());
                }
       
              
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            HandleDropDownChanged();
        }
    }


}
