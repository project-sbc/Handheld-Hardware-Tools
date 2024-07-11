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

namespace Handheld_Hardware_Tools.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_Combobox : ControllerUserControl
    {
        int originalSelectedIndex = -1;

        public EditAction_Combobox(string actionName = "")
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = comboBox;


            //set control
           ConfigureControl(actionName);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        public Dictionary<string, string>actionDictionary = new DefaultActionItemDictionary();
        private void ConfigureControl(string actionName = "")
        {
            foreach (KeyValuePair<string, string> pair in actionDictionary)
            {
                comboBox.Items.Add(pair.Value);
                if (actionName == pair.Key) 
                { 
                    //if the name of the action matches the default list's action then REMEMBER IT so we can highlight it
                    comboBox.SelectedIndex = comboBox.Items.Count - 1; 
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
                string actionLookup = actionDictionary.FirstOrDefault(o => o.Value == comboBox.SelectedValue).Key;
                Classes.Actions.Action newAction = null;

                switch (actionLookup)
                {
                    case "Change_TDP":
                        newAction = new Classes.Actions.ActionClass.Change_TDP();
                        break;
                    case "Toggle_WifiAP":
                        newAction = new Classes.Actions.ActionClass.Toggle_WifiAP();
                        break;
                    case "Toggle_Wifi":
                        newAction = new Classes.Actions.ActionClass.Toggle_Wifi();
                        break;
                    case "Toggle_BT":
                        newAction = new Classes.Actions.ActionClass.Toggle_BT();
                        break;
                    case "Toggle_QAM":
                        newAction = new Classes.Actions.ActionClass.Toggle_QAM();
                        break;
                    case "Toggle_Desktop":
                        newAction = new Classes.Actions.ActionClass.Toggle_Desktop();
                        break;
                    case "Toggle_MouseMode":
                        newAction = new Classes.Actions.ActionClass.Toggle_MouseMode();
                        break;
                    case "Cycle_TDP":
                        newAction = new Classes.Actions.ActionClass.Cycle_TDP();
                        break;
                    case "Toggle_IntegerScaling":
                        newAction = new Classes.Actions.ActionClass.Toggle_IntegerScaling();
                        break;
                    case "Toggle_Guide":
                        newAction = new Classes.Actions.ActionClass.Toggle_Guide();
                        break;
                    case "Toggle_MicrophoneMute":
                        newAction = new Classes.Actions.ActionClass.Toggle_MicrophoneMute();
                        break;
                    case "Toggle_VolumeMute":
                        newAction = new Classes.Actions.ActionClass.Toggle_VolumeMute();
                        break;
                    case "Toggle_WinOSK":
                        newAction = new Classes.Actions.ActionClass.Toggle_WinOSK();
                        break;
                    case "Toggle_OSK":
                        newAction = new Classes.Actions.ActionClass.Toggle_OSK();
                        break;
                    case "Toggle_Controller":
                        newAction = new Classes.Actions.ActionClass.Toggle_Controller();
                        break;
                    case "Cycle_Volume":
                        newAction = new Classes.Actions.ActionClass.Cycle_Volume();
                        break;
                    case "Cycle_EPP":
                        newAction = new Classes.Actions.ActionClass.Cycle_EPP();
                        break;
                    case "Toggle_WindowManager":
                        newAction = new Classes.Actions.ActionClass.Toggle_WindowManager();
                        break;
                    case "Toggle_QuickActionWheel":
                        newAction = new Classes.Actions.ActionClass.Toggle_QuickActionWheel();
                        break;
                    case "Open_SteamBP":
                        newAction = new Classes.Actions.ActionClass.Open_SteamBP();
                        break;
                    case "Toggle_FreeSync":
                        newAction = new Classes.Actions.ActionClass.Toggle_FreeSync();
                        break;
                    default:
                        MessageBox.Show("NO HANDLER FOR THIS ACTION, ADD IT YOU DUMMY. EditAction_ComboBox.xaml.cs " + actionLookup);
                        break;
                }


                //the value has changed, lets update the page's action with the new action item
                Page page = (Page)Local_Object.Instance.GetMainWindowFramePage();

                if (page is EditActionPage)
                {
                    EditActionPage editActionPage = page as EditActionPage;
                    if (editActionPage != null)
                    {
                        if (editActionPage.action != null)
                        {
                            newAction.displayInActionPanel = editActionPage.action.displayInActionPanel;
                            newAction.hotKey = editActionPage.action.hotKey;
                            newAction.hotkeyType = editActionPage.action.hotkeyType;
                            if (editActionPage.action.ID != null)
                            {
                                newAction.ID = editActionPage.action.ID;
                            }
                        }
                        editActionPage.action = newAction;

                        //this routine updates the argument list OR collapses it if the new action type doesnt need 
                        //an argument
                        editActionPage.actionArgumentListView.UpdateForNewAction(newAction.actionName);
                        //call this to update page usercontrol list (so if argument list is hidden controller input wont accidentally pick it up)
                        editActionPage.RebaseUserControlsAfterActionTypeUpdate();
                    }
                }
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            HandleDropDownChanged();
        }
    }


}
