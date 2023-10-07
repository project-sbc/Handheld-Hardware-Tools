using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Actions
{
    //List of items in this class

    //Action model which has all the action details


    //Action model is for general use, there will be a display model below for the action panel page
    public class Action
    {
        //action dispaly name is language specific
        string actionDisplayName;
        //action name is programming specific
        string actionName;
        bool displayInActionPanel;







        //This routine is going to be the onclick event
        public virtual void OnClick() { }
    }



    public class ActionPanelItem
    {



    }

}
