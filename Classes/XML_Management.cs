﻿using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Games;
using Handheld_Hardware_Tools.Classes.HomePage;
using Handheld_Hardware_Tools.Classes.MouseMode;
using Handheld_Hardware_Tools.Classes.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Handheld_Hardware_Tools.Classes
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
                case "MouseProfile":
                    folderFileName = "UserConfiguration\\MouseMode\\MouseProfiles.xml";
                    break;
                case "GameLibList":
                    folderFileName = "UserConfiguration\\GameLibrary\\GameLibrary.xml";
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
                    case "MouseProfile":
                        xmls = new XmlSerializer(typeof(MouseProfile));
                        MouseProfile objMP = (MouseProfile)objClass;
                        xmls.Serialize(sw, objMP);
                        objMP = null;
                        break;

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
                        objSettings = null;
                        break;
                    case "ActionList":
                        xmls = new XmlSerializer(typeof(ActionList));
                        ActionList objAL = (ActionList)objClass;
                        xmls.Serialize(sw, objAL);
                        objAL = null;
                        break;
                    case "ProfileList":
                        xmls = new XmlSerializer(typeof(ProfileList));
                        ProfileList objPL = (ProfileList)objClass;
                        xmls.Serialize(sw, objPL);
                        objPL = null;
                        break;
                    case "GameLibList":
                        xmls = new XmlSerializer(typeof(GameLibList));
                        GameLibList objGL = (GameLibList)objClass;
                        xmls.Serialize(sw, objGL);
                        objGL = null;
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
                case "MouseProfile":
                    filePath = "UserConfiguration\\MouseMode\\MouseProfiles.xml";
                    break;

                case "GameLibList":
                    filePath = "UserConfiguration\\GameLibrary\\GameLibrary.xml";
                    break;
            }

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

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
                        case "MouseProfile":
                            xmls = new XmlSerializer(typeof(MouseProfile));
                            objObject = ((MouseProfile)xmls.Deserialize(sr));
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
                            xmls = new XmlSerializer(typeof(ProfileList));
                            objObject = ((ProfileList)xmls.Deserialize(sr));
                            break;
                        case "GameLibList":
                            xmls = new XmlSerializer(typeof(GameLibList));
                            objObject = ((GameLibList)xmls.Deserialize(sr));
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
                    case "MouseProfile":
                        objObject = new MouseProfile();
                        break;
                    case "GameLibList":
                        objObject = new GameLibList();
                        break;



                }
            }
            return objObject;
        }


    }
}
