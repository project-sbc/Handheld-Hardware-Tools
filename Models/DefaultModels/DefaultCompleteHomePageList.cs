using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Models.DefaultModels
{
    //This is the default complete list of usercontrols and their name in the language of the user
    //This is used on the customize home page page to display all available options 
    public class DefaultCompleteHomePageList : Dictionary<ControllerUserControl, string>
    {
        DefaultCompleteHomePageList() 
        {
            //add ALL home page user controls here
            this.Add(new TDP_Slider(), Application.Current.Resources["UserControl_TDP_Slider"].ToString());

        }

    }
}
