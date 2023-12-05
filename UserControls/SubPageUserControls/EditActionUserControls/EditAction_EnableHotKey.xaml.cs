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
using Windows.Foundation.Collections;

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_EnableHotKey : ControllerUserControl
    {
        int originalSelectedIndex = -1;

        public EditAction_EnableHotKey(string actionhotkey ="")
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = toggleSwitch;


            //set control
           ConfigureControl(actionhotkey);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        private void ConfigureControl(string actionhotkey)
        {
            if (actionhotkey != "")
            {
                toggleSwitch.IsChecked = true;
            }
            else
            {
                toggleSwitch.IsChecked= false;
            }

        }




        public override void ReturnControlToPage()
        {


            //still run normal code for return to page
            base.ReturnControlToPage();
        }


        public override void ControlChangeValueHandler()
        {
           
            bool isToggled;
            if (toggleSwitch.IsChecked == false)
            {
                isToggled = false;
            }
            else
            {
                isToggled = true;
            }


            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();

            if (mainWindow.frame.Content is EditActionPage)
            {
                EditActionPage editActionPage = mainWindow.frame.Content as EditActionPage;
                if (editActionPage != null)
                {
                    if (toggleSwitch.IsChecked == true)
                    {
                        editActionPage.actionEnableHotKey.Visibility = Visibility.Visible;
                    }
                    else
                    {

                    }
                }
            }
        }


        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }
    }


}
