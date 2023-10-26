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


        public void ConfigureControl(Classes.Actions.Action action = null)
        {
            if (action != null)
            {
                switch (action.actionName)
                {
                    case "Change_TDP":

                        break;
                }

            }

        }

        public void UpdateForNewAction(string actionName)
        {



        }


        private void UpdatePageActionWithSubControlValues()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (mainWindow.frame.Content is EditActionPage)
            {
                EditActionPage editActionPage = mainWindow.frame.Content as EditActionPage;
                if (editActionPage != null)
                {
                    if (editActionPage.action != null)
                    {

                    }
                }
            }

        }


        public override void ControlChangeValueHandler()
        {
           

        }
    }


    
}
