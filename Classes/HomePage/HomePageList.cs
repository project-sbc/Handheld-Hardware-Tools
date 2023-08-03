using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Models.DefaultModels;

namespace Everything_Handhelds_Tool.Classes
{
    public class HomePageList :Dictionary<ControllerUserControl, bool>
    {
        //This is the list of items that get displayed on the home page for the ones that are bool true (it means add them otherwise false wont be added). When new controls are
        //added they will get added to the end as false (not added)
        public HomePageList()
        {
            //Check to see if a list already exists profiles
            LoadHomePageStackPanelItems();

        }

        private void LoadHomePageStackPanelItems()
        {
            //Try loading the users home page arrangement from XML, if not then get the default setup from default models, then add to this
           

        }

    }
}
