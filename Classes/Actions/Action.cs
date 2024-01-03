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
    [XmlInclude(typeof(Toggle_IntegerScaling))]
    [XmlInclude(typeof(Toggle_MicrophoneMute))]
    [XmlInclude(typeof(Toggle_VolumeMute))]
    [XmlInclude(typeof(Toggle_Wifi))]
    [XmlInclude(typeof(Toggle_BT))]
    [XmlInclude(typeof(Toggle_WinOSK))]
    public class Action
    {
        //action name is programming specific, display name will be generated at runtime based on language selected
        public int ID = -1;
        public string actionName;
        public bool displayInActionPanel;
        public string hotkeyType = "";
        public string hotKey = "";
        public List<string> parameters;

        //This routine is going to be the onclick event
        public virtual void OnActivate() { }


       
    }




    public class DefaultActionItemDictionary: Dictionary<string, string>
    {
        //This is default action item list that is used during creating a new action item
        public DefaultActionItemDictionary()   
        {
            this.Add("Change_TDP", Application.Current.Resources["Action_Change_TDP"].ToString());
            this.Add("Cycle_TDP", Application.Current.Resources["Action_Cycle_TDP"].ToString());
            this.Add("Cycle_Volume", Application.Current.Resources["Action_Cycle_Volume"].ToString());
            this.Add("Toggle_Wifi", Application.Current.Resources["Action_Toggle_Wifi"].ToString());
            this.Add("Toggle_WifiAP", Application.Current.Resources["Action_Toggle_WifiAP"].ToString());
            this.Add("Toggle_IntegerScaling", Application.Current.Resources["Action_Toggle_IntegerScaling"].ToString());
            this.Add("Toggle_MicrophoneMute", Application.Current.Resources["Action_Toggle_MicrophoneMute"].ToString());
            this.Add("Toggle_VolumeMute", Application.Current.Resources["Action_Toggle_VolumeMute"].ToString());
            this.Add("Toggle_BT", Application.Current.Resources["Action_Toggle_BT"].ToString());
            this.Add("Toggle_WinOSK", Application.Current.Resources["Action_Toggle_WinOSK"].ToString());
            //this.Add("Toggle_QAM", Application.Current.Resources["Action_Toggle_QAM"].ToString());


            //new DefaultActionItem() { actionName = "Auto_TDP", displayName = "" };
            //new DefaultActionItem() { actionName = "Toggle_Controller_Keyboard", displayName = "" };
            //new DefaultActionItem() { actionName = "Toggle_Windows_Keyboard", displayName = "" };
       
            //new DefaultActionItem() { actionName = "Toggle_Microphone_Mute", displayName = "" };
            //new DefaultActionItem() { actionName = "Open_Steam_Big_Picture", displayName = "" };
            //new DefaultActionItem() { actionName = "Open_Playnite", displayName = "" };
            //new DefaultActionItem() { actionName = "Change_Brightness", displayName = "" };
            //new DefaultActionItem() { actionName = "Cycle_Brightness", displayName = "" };
            //new DefaultActionItem() { actionName = "Change_Volume", displayName = "" };
            //new DefaultActionItem() { actionName = "Cycle_Volume", displayName = "" };
            //new DefaultActionItem() { actionName = "Show_Desktop", displayName = "" };
            //new DefaultActionItem() { actionName = "Toggle_AMD_RSR", displayName = "" };
            //new DefaultActionItem() { actionName = "Cycle_Resolution_Mode", displayName = "" };
            //new DefaultActionItem() { actionName = "Cycle_Refresh_Mode", displayName = "" };

        }
    }

   
  

}
