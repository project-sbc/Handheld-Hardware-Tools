using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;


namespace Handheld_Hardware_Tools.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class DisplayResolution_Combobox : ControllerUserControl
    {
        int originalSelectedIndex = -1;
        List<Tuple<int,int>> displayResolution = Display_Management.Instance.GetAvailableResolutionsForPrimaryMonitor();
        public DisplayResolution_Combobox()
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
            comboBox.ItemsSource = displayResolution;

            Tuple<int,int> currentResolution = Display_Management.Instance.GetPrimaryMonitorResolution();

            foreach (Tuple<int,int> pair in comboBox.ItemsSource)
            {
                if (pair.Item1 == currentResolution.Item1 && pair.Item2 == currentResolution.Item2)
                {
                    comboBox.SelectedItem = pair;
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

                Tuple<int, int> selectedItem = (Tuple<int, int>)comboBox.SelectedItem;
       
                if (selectedItem != null)
                {
                    Display_Management.Instance.ChangePrimaryMonitorResolution(selectedItem.Item1, selectedItem.Item2);
                }

               
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            HandleDropDownChanged();
        }
    }


}
