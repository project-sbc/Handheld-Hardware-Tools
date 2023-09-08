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
            this.Add("TDP_Boost_Slider", true);
            this.Add("Max_CPU_Slider", true);
        }

    }

}
