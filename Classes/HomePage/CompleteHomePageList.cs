using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
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
            UpdateList();
        }
        public void UpdateList()
        {
            List<string> controls = new DefaultCompleteHomePageList();

            foreach (string control in controls)
            {
                if (this.Find(hpi => hpi.UserControlName == control) == null)
                {
                    this.Add(new HomePageItem() { UserControlName = control, UserControlVisible = false, UserControlDisplayName = "TDP_Slider" });
                }
                
            }
        }
        public void SaveToXML()
        {
            XML_Management.Save_XML("UserConfiguration\\HomePage\\HomePage.xml", "CompleteHomePageList", this);
        }

    }
}
