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
            if (!Directory.Exists(directory + folderFileName))
            {
                Directory.CreateDirectory(directory + folderFileName);
            }

            lock (lockObject)
            {
                StreamWriter sw = new StreamWriter(directory + folderFileName);
                XmlSerializer xmls = null;
                switch (objType)
                {
                    case "HomePageStackPanelItems":
                        xmls = new XmlSerializer(typeof(HomePageList));
                        HomePageList objCM = (HomePageList)objClass;
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
                        case "HomePageStackPanelItems":
                            xmls = new XmlSerializer(typeof(HomePageList));
                            objObject = ((HomePageList)xmls.Deserialize(sr));
                            break;
                    }

                    sr.Dispose();
                    xmls = null;
                }
            }
            return objObject;
        }


    }
}
