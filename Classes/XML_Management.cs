using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.HomePage;
using Everything_Handhelds_Tool.Classes.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes
{
    public class XML_Management
    {
        private static XML_Management _instance = null;
        private static readonly object lockObj = new object();
        private XML_Management()
        {
        }
        public static XML_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new XML_Management();
                        }
                    }
                }
                return _instance;
            }
        }


        public string directory = AppDomain.CurrentDomain.BaseDirectory;
        private object lockObject = new object();

        public void SaveXML(string objType, object objClass)
        {
            //objClass is the object being saved i.e. Settings
            //objType is the type of object
            string folderFileName="";
            switch (objType)
            {
                case "HomePageUserControlList":
                    folderFileName = "UserConfiguration\\HomePage\\HomePageUserControlList.xml";
                    break;
                case "Settings":
                    folderFileName = "UserConfiguration\\Settings\\Settings.xml";
                    break;
                case "ActionList":
                    folderFileName = "UserConfiguration\\Actions\\Actions.xml";
                    break;
                case "ProfileList":
                    folderFileName = "UserConfiguration\\Profiles\\Profiles.xml";
                    break;
            }


            CheckDirectoryExistsOrCreate(directory + folderFileName);
            WriteXMLToFile(folderFileName, objType, objClass);
        }
        private void CheckDirectoryExistsOrCreate(string folder)
        {
            //checks for directory, if it doesnt exist make it. FIrst check for upperfolder UserConfiguration then check for next level folder
            if (!Directory.Exists(directory + "UserConfiguration"))
            {
                Directory.CreateDirectory(directory + "UserConfiguration");
            }
            if (!Directory.Exists(Path.GetDirectoryName(folder)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(folder));
            }
        }
        private void WriteXMLToFile(string folderFileName, string objType, object objClass)
        {
            lock (lockObject)
            {
                StreamWriter sw = new StreamWriter(directory + folderFileName);
                XmlSerializer xmls = null;
                switch (objType)
                {
                    case "HomePageUserControlList":
                        xmls = new XmlSerializer(typeof(HomePageUserControlList));
                        HomePageUserControlList objCM = (HomePageUserControlList)objClass;
                        xmls.Serialize(sw, objCM);
                        objCM = null;
                        break;
                    case "Settings":
                        xmls = new XmlSerializer(typeof(Settings));
                        Settings objSettings = (Settings)objClass;
                        xmls.Serialize(sw, objSettings);
                        objCM = null;
                        break;
                    case "ActionList":
                        xmls = new XmlSerializer(typeof(ActionList));
                        ActionList objAL = (ActionList)objClass;
                        xmls.Serialize(sw, objAL);
                        objCM = null;
                        break;
                    case "ProfileList":
                        xmls = new XmlSerializer(typeof(ProfileList));
                        ProfileList objPL = (ProfileList)objClass;
                        xmls.Serialize(sw, objPL);
                        objCM = null;
                        break;

                }
                sw.Dispose();
                xmls = null;

            }
        }

        public object LoadXML(string objType)
        {
            //folder file names are 
            //         UserConfiguration\\HomePage\\HomePageUserControlList.xml
            //         UserConfiguration\\Actions\\Actions.xml
            //         UserConfiguration\\Settings\\Settings.xml

            string filePath = "";

            switch (objType)
            {
                case "HomePageUserControlList":
                    filePath = "UserConfiguration\\HomePage\\HomePageUserControlList.xml";
                    break;
                case "Settings":
                    filePath = "UserConfiguration\\Settings\\Settings.xml";
                    break;
                case "ActionList":
                    filePath = "UserConfiguration\\Actions\\Actions.xml";
                    break;
                case "ProfileList":
                    filePath = "UserConfiguration\\Profiles\\Profiles.xml";
                    break;
            }

            object objObject = null;
            if (File.Exists(filePath))
            {
                lock (lockObject)
                {
                    StreamReader sr = new StreamReader(filePath);
                    XmlSerializer xmls;

                    switch (objType)
                    {
                        case "HomePageUserControlList":
                            xmls = new XmlSerializer(typeof(HomePageUserControlList));
                            objObject = ((HomePageUserControlList)xmls.Deserialize(sr));
                        
                            break;
                        case "Settings":
                            xmls = new XmlSerializer(typeof(Settings));
                            objObject = ((Settings)xmls.Deserialize(sr));
                            break;
                        case "ActionList":
                            xmls = new XmlSerializer(typeof(ActionList));
                            objObject = ((ActionList)xmls.Deserialize(sr));
                            break;
                        case "ProfileList":
                            xmls = new XmlSerializer(typeof(Profile));
                            objObject = ((ProfileList)xmls.Deserialize(sr));
                            break;
                    }

                    sr.Dispose();
                    xmls = null;
                }
            }
            else
            {
                //make new object for it
                switch (objType)
                {
                    case "HomePageUserControlList":
                        objObject =  new HomePageUserControlList();
                        ((HomePageUserControlList)objObject).UpdateList();
                        break;
                    case "Settings":
                        objObject = new Settings();
                        break;
                    case "ActionList":
                        objObject = new DefaultActionList();
                        break;
                    case "ProfileList":
                        objObject = new ProfileList();
                        break;


                }
            }
            return objObject;
        }


    }
}
