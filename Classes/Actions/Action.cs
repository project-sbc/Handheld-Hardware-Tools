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



    public class Action_Management
    {
        private static Action_Management _instance = null;
        private static readonly object lockObj = new object();
        private Action_Management()
        {
        }
        public static Action_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Action_Management();
                        }
                    }
                }
                return _instance;
            }
        }
        public void SaveActions(Settings settings)
        {
            //Remember this is in a singleton and it prevents multithread access so no need to make this routine thread safe
            //create directory for settings if it doesnt exist
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Settings"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Settings");
            }
            //then save settings
            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(Settings));
                xmls.Serialize(sw, settings);
            }

        }
        public Settings LoadSettings()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(Settings));
                    return xmls.Deserialize(sr) as Settings;
                }
            }
            Settings newSettings = new Settings();
            SaveSettings(newSettings);
            return newSettings;
        }
    }


    public class DefaultActionItemList: List<DefaultActionItem>
    {
        //This is default action item list that is used during creating a new action item
        public DefaultActionItemList()   
        {
            new DefaultActionItem() { actionName = "Change_TDP", displayName = "Change TDP" };
            new DefaultActionItem() { actionName = "Cycle_TDP", displayName = "Cycle TDP" };
            new DefaultActionItem() { actionName = "Toggle_Wifi", displayName = "Toggle Wifi" };

        }
    }

    public class DefaultActionItem
    {
        public string actionName;
        public string displayName;
    }
  

}
