using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes
{
    public static class Load_Settings
    {
        public static object lockObjectSettings = new object();
        public static Settings LoadSettings()
        {
            lock (lockObjectSettings)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
                {
                    using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
                    {
                        XmlSerializer xmls = new XmlSerializer(typeof(Settings));
                        return xmls.Deserialize(sr) as Settings;
                    }
                }
            }



            Settings newSettings = new Settings();
            return newSettings;
        }
    }

    public class Settings
    {
        public string language { get; set; } = "English";


        public void Save()
        {
            //use lock to prevent multiple threads from using stream writer which will cause an error
            lock (Load_Settings.lockObjectSettings)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Settings"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Settings");
                }

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Settings\\Settings.xml"))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(Settings));
                    xmls.Serialize(sw, this);
                }
            }

        }
    }
   
}
