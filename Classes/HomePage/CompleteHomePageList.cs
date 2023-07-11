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

namespace Everything_Handhelds_Tool.Models.DefaultModels
{
    //This is the user configuration list of user controls that has usercontrol, name in langauge, and whether to display it
    public class CompleteHomePageList : List<HomePageItem>
    {
         
        public CompleteHomePageList() 
        {
        }
        
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
            XML_Management.Save_XML("UserConfiguration\\HomePage\\HomePage.xml", "CompleteHomePageList", this);
        }

    }
}
