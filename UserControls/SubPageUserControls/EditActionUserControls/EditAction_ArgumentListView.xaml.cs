using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class EditAction_ArgumentListView : ControllerUserControl
    {


        public EditAction_ArgumentListView(Classes.Actions.Action action = null)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = listView;


            //get page's action details

            //set control
           ConfigureControl(action);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        private void ConfigureParameterListType(string actionName)
        {
            listView.Items.Clear();
            this.Visibility = Visibility.Visible;
            switch (actionName)
            {
                case "Cycle_TDP":
                    listView.SelectionMode = SelectionMode.Multiple;
                    Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

                    //add all the options, check to see if the value is divisible by 5 to see if you should add 3 or 2
                    int tdp = 5;
                    while (tdp <= settings.maxTDP)
                    {
                        listView.Items.Add(new ListViewItem { Content = tdp.ToString() });
                        if ((double)(tdp/5) == Math.Round((double)tdp / 5, 0))
                        {
                            tdp = tdp + 3;
                        }
                        else
                        {
                            tdp = tdp + 2;
                        }
                    }

                    break;
                case "Cycle_Volume":
                    listView.SelectionMode = SelectionMode.Multiple;
                   

                    //add all the options, check to see if the value is divisible by 5 to see if you should add 3 or 2
                    int vol = 0;
                    while (vol <= 100)
                    {
                        listView.Items.Add(new ListViewItem { Content = vol.ToString() });
                        vol = vol + 5;
                    }

                    break;
                case "Change_TDP":
                    int x = -5;
                    while (x <=5)
                    {
                        if (x != 0) { listView.Items.Add(new ListViewItem {Content=x.ToString() }) ; }
                        x++;
                    }

                    break;
                case "Toggle_WifiAP" or "Toggle_QAM" or "Toggle_IntegerScaling" or "Toggle_Desktop" or "Toggle_MicrophoneMute" or "Toggle_VolumeMute" or "Toggle_Wifi" or "Toggle_BT" or "Toggle_WinOSK" or "Toggle_OSK":
                    //this handles the non list items like toggle wifi, where this list needs to be hidden
                    this.Visibility = Visibility.Collapsed;
                    break;
                default:
                    MessageBox.Show("NO HANDLER FOR THIS ACTION, ADD IT YOU DUMMY. EditAction_ArgumentListView.xaml.cs " + actionName);
                    break;
            }

        }


        
        private void ConfigureParameterListSelectedItems(List<string> list)
        {
            if (listView.Items.Count > 0)
            {
                foreach(ListViewItem item in listView.Items)
                {
                    if (list.Contains(item.Content))
                    {
                        //if single select set value and return, otherwise keep looping for all values                  
                        if (listView.SelectionMode == SelectionMode.Single) 
                        {
                            listView.SelectedValue = item;
                            return; 
                        }
                        else
                        {
                            listView.SelectedItems.Add(item);
                        }
                    }
                }
            }

        }

        public void ConfigureControl(Classes.Actions.Action action = null)
        {
            if (action != null)
            {
                ConfigureParameterListType(action.actionName);
                ConfigureParameterListSelectedItems(action.parameters);

            }

        }

        public void UpdateForNewAction(string actionName)
        {

            ConfigureParameterListType(actionName);

        }

      

        private void UpdatePageActionWithSubControlValues(List<string> values)
        {
            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();

            if (mainWindow.frame.Content is EditActionPage)
            {
                EditActionPage editActionPage = mainWindow.frame.Content as EditActionPage;
                if (editActionPage != null)
                {
                    if (editActionPage.action != null)
                    {
                        editActionPage.action.parameters = values;
                    }
                }
            }

        }


        public override void ControlChangeValueHandler()
        {
            List<string> values = new List<string>();
            foreach (ListViewItem value in listView.SelectedItems)
            {
                values.Add(value.Content.ToString());
                
            }
            UpdatePageActionWithSubControlValues(values);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlChangeValueHandler();
        }
    }


    
}
