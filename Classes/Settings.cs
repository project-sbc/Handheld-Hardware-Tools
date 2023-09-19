using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes
{

    public class Load_Settings
    {
        private static Load_Settings _instance = null;
        private static readonly object lockObj = new object();
        private Load_Settings()
        {
        }
        public static Load_Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Load_Settings();
                        }
                    }
                }
                return _instance;
            }
        }
        public void SaveSettings(Settings settings)
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
                xmls.Serialize(sw, this);
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

    public class Settings
    {
        public string language { get; set; } = "English";
        public int defaultTDP { get; set; } = 15;
        public int maxTDP { get; set; } = 30;
        public int minTDP { get; set; } = 5;
       
    }
   
}
