using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Classes.Actions
{
    //List of items in this class

    //Action model which has all the action details


    //Action model is for general use, there will be a display model below for the action panel page
    [XmlInclude(typeof(Change_TDP))]
    [XmlInclude(typeof(Toggle_WifiAP))]
    [XmlInclude(typeof(Cycle_TDP))]
    [XmlInclude(typeof(Cycle_Volume))]
    public class Action
    {
        //action name is programming specific, display name will be generated at runtime based on language selected
        public int ID;
        public string actionName;
        public bool displayInActionPanel;
        public string hotkeyType = "";
        public string hotKey = "";
        public List<string> arguments;

        //This routine is going to be the onclick event
        public virtual void OnActivate() { }


       
    }




    public class DefaultActionItemList: List<DefaultActionItem>
    {
        //This is default action item list that is used during creating a new action item
        public DefaultActionItemList()   
        {
            new DefaultActionItem() { actionName = "Change_TDP", displayName = "" };
            new DefaultActionItem() { actionName = "Cycle_TDP", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_Wifi", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_BT", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_QAM", displayName = "" };
            new DefaultActionItem() { actionName = "Auto_TDP", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_Controller_Keyboard", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_Windows_Keyboard", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_Volume_Mute", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_Microphone_Mute", displayName = "" };
            new DefaultActionItem() { actionName = "Open_Steam_Big_Picture", displayName = "" };
            new DefaultActionItem() { actionName = "Open_Playnite", displayName = "" };
            new DefaultActionItem() { actionName = "Change_Brightness", displayName = "" };
            new DefaultActionItem() { actionName = "Cycle_Brightness", displayName = "" };
            new DefaultActionItem() { actionName = "Change_Volume", displayName = "" };
            new DefaultActionItem() { actionName = "Cycle_Volume", displayName = "" };
            new DefaultActionItem() { actionName = "Show_Desktop", displayName = "" };
            new DefaultActionItem() { actionName = "Toggle_AMD_RSR", displayName = "" };
            new DefaultActionItem() { actionName = "Cycle_Resolution_Mode", displayName = "" };
            new DefaultActionItem() { actionName = "Cycle_Refresh_Mode", displayName = "" };

        }
    }

    public class DefaultActionItem
    {
        public string actionName;
        public string displayName;
    }
  

}
