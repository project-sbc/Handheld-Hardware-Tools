﻿using Everything_Handhelds_Tool.Models.DefaultModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes
{
    public static class XML_Management
    {
        private static string directory = AppDomain.CurrentDomain.BaseDirectory;
        private static object lockObject = new object();

        public static void Save_XML(string folderFileName, string objType, object objClass)
        {
            CheckDirectoryExistsOrCreate(directory + folderFileName);
            WriteXMLToFile(folderFileName, objType, objClass);
        }
        private static void CheckDirectoryExistsOrCreate(string folder)
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
        private static void WriteXMLToFile(string folderFileName, string objType, object objClass)
        {
            lock (lockObject)
            {
                StreamWriter sw = new StreamWriter(directory + folderFileName);
                XmlSerializer xmls = null;
                switch (objType)
                {
                    case "CompleteHomePageList":
                        xmls = new XmlSerializer(typeof(CompleteHomePageList));
                        CompleteHomePageList objCM = (CompleteHomePageList)objClass;
                        xmls.Serialize(sw, objCM);
                        objCM = null;
                        break;
                }
                sw.Dispose();
                xmls = null;

            }
        }

        public static object Load_XML(string folderFileName, string objType)
        {
            string filePath = directory + folderFileName;
            object objObject = null;
            if (File.Exists(filePath))
            {
                lock (lockObject)
                {
                    StreamReader sr = new StreamReader(filePath);
                    XmlSerializer xmls;

                    switch (objType)
                    {
                        case "CompleteHomePageList":
                            xmls = new XmlSerializer(typeof(CompleteHomePageList));
                            objObject = ((CompleteHomePageList)xmls.Deserialize(sr));
                            ((CompleteHomePageList)objObject).UpdateList();
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
                    case "CompleteHomePageList":
                        objObject =  new CompleteHomePageList();
                        ((CompleteHomePageList)objObject).UpdateList();
                        break;
                }
            }
            return objObject;
        }


    }
}
