using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.HomePage
{
    //This is the user configuration list of user controls that has usercontrol, name in langauge, and whether to display it
    public class HomePageUserControlList : List<HomePageItem>
    {
                
        public void UpdateList()
        {
            //This routine grabs the default COMPLETE list of all controls called DefaultCompleteHomePageList
            //Compares to the current list and adds any new ones as needed.
            //if any new lines get added listUpdated becomes true and then it gets saved at the end
            bool listUpdated = false;
            Dictionary<string, bool> controls = new DefaultCompleteHomePageList();

            foreach (KeyValuePair<string, bool> control in controls)
            {

                if (this.Find(hpi => hpi.UserControlName == control.Key) == null)
                {
                    this.Add(new HomePageItem() { UserControlName = control.Key, UserControlVisible =control.Value, UserControlDisplayName = "TDP_Slider" });
                    listUpdated= true;
                }
                
            }

            //if listUpdated is true then save
            if (listUpdated) { SaveToXML(); }
        }

        
        public void SaveToXML()
        {
            XML_Management.Instance.SaveXML("HomePageUserControlList", this);
        }

    }

    public class DefaultCompleteHomePageList : Dictionary<string, bool>
    {
        //complete list for home page items, this gets updated when new controls are added
        public DefaultCompleteHomePageList()
        {
            this.Add("TDP_Slider", true);
            this.Add("TDP_Boost_Slider", true);
            this.Add("Brightness_Slider", true);
            this.Add("Volume_Slider", true);
            this.Add("PowerPlan_ComboBox", true);

        }

    }

    public class HomePageItem
    {
        public string UserControlName { get; set; }
        public bool UserControlVisible { get; set; }
        public string UserControlDisplayName { get; set; }
    }

}
