using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Classes.Actions
{
    //List of items in this class

    //Action model which has all the action details


    //Action model is for general use, there will be a display model below for the action panel page
    public class Action
    {
        //action name is programming specific, display name will be generated at runtime based on language selected
        public int ID;
        public string actionName;
        public bool displayInActionPanel;



        public List<string> arguments;





        //This routine is going to be the onclick event
        public virtual void OnActivate() { }


       
    }



    public class ActionPanelItem
    {
        public Action action;
        public SymbolRegular symbolIcon = new SymbolRegular();
        public string displayText;

        public void InitActionPanelItem(Action importAction)
        {
            action = importAction;
            if (importAction is Change_TDP)
            {
                symbolIcon = Wpf.Ui.Common.SymbolRegular.Flash20;
                displayText = "Change TDP";
            }

        }
    }


    public class DefaultActionItemList: List<DefaultActionItem>
    {
        public DefaultActionItemList()   
        {
            new DefaultActionItem() { actionName = "Change_TDP", displayName = "Change TDP" };
            new DefaultActionItem() { actionName = "Cycle_TDP", displayName = "Cycle TDP" };
            new DefaultActionItem() { actionName = "Toggle_Wifi", displayName = "Toggle Wifi" };

        }
    }


    public class DefaultActionItem
    {
        public string displayName;
        public string actionName;
    }

}
