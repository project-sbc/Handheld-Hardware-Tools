using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
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

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_InActionPanelToggleSwitch : ControllerUserControl
    {
        int originalSelectedIndex = -1;

        public EditAction_InActionPanelToggleSwitch(bool actionVisible =false)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = toggleSwitch;


            //set control
           ConfigureControl(actionVisible);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        public Dictionary<string, string>actionDictionary = new DefaultActionItemDictionary();
        private void ConfigureControl(bool actionVisible = false)
        {
            if (actionVisible)
            {

            }

        }




        public override void ReturnControlToPage()
        {


            //still run normal code for return to page
            base.ReturnControlToPage();
        }


        public override void ControlChangeValueHandler()
        {
            //set originalselectedindex to the NEW selected index to prevent accidental change when returning control to page (where it will reset to original if indexes dont match)
            toggleSwitch.IsChecked = !toggleSwitch.IsChecked;
        }

    }


}
