using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Models.DefaultModels
{
    public class DefaultCompleteHomePageList : List<string>
    {
        //complete list for home page items, this gets updated when new controls are added
        public DefaultCompleteHomePageList()
        {
            this.Add("TDP_Slider");
        }

    }

    public class DefaultHomePageList : List<string>
    {
        //Default list for home page if there is none in the user configuration folder
        public DefaultHomePageList()
        {
            this.Add("TDP_Slider");
        }

    }
}
