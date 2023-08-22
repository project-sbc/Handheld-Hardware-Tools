using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Models.DefaultModels
{
    public class DefaultCompleteHomePageList : Dictionary<string, bool>
    {
        //complete list for home page items, this gets updated when new controls are added
        public DefaultCompleteHomePageList()
        {
            this.Add("TDP_Slider", true);
        }

    }

    public class DefaultHomePageList : List<string>
    {
        //Default list for home page if there is none in the user configuration folder
        public DefaultHomePageList()
        {
            this.Add("TDP_Slider");
            this.Add("TDP__Boost_Slider");
        }

    }
}
