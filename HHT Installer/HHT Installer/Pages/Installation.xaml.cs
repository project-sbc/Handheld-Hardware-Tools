using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace HHT_Installer.Pages
{
    /// <summary>
    /// Interaction logic for Installation.xaml
    /// </summary>
    public partial class Installation : Page
    {
        public Installation()
        {
            InitializeComponent();

            InstallHHT();
        }




        private void InstallHHT()
        {
            //high level installation


            //add registry keys
            string[] strings = Downloadzip_ReadXML_URL();


            //download latest zip
            


            //check/add folder to program files




            //add start menu short cut



            //



        }


        private string installationPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Handheld Hardware Tools");
        private string appDir = AppDomain.CurrentDomain.BaseDirectory;



        private void Downloadzip_Main()
        {
            string[] versionURL = Downloadzip_ReadXML_URL();

            string version = versionURL[0];
            string URL = versionURL[1];



        }

        private void Downloadzip_Download_File(string url)
        {
            try
            {
               if (url == "") { return; }
               
                // Create a WebClient instance
                using (WebClient client = new WebClient())
                {
                    // Download the zip file from the URL and save it to the specified path
                    client.DownloadFile(url, appDir);

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private string[] Downloadzip_ReadXML_URL()
        {
            string[] returnStrings = new string[2];
            returnStrings[0] = "";
            returnStrings[1] = "";
            

            string url = "https://raw.githubusercontent.com/project-sbc/Handheld-Control-Panel/master/Update.xml"; // Replace this URL with your XML file URL

            try
            {
                // Create a WebClient instance to download the XML data from the URL
                WebClient client = new WebClient();

                // Download the XML data
                string xmlData = client.DownloadString(url);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);


                XmlNodeList nodeList = xmlDoc.SelectNodes("//item/*");
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "version")
                    {
                        returnStrings[0] = node.InnerText;
                    }
                    if (node.Name == "url")
                    {
                        returnStrings[1] = node.InnerText;
                    }
                  
                }

                xmlDoc = null;

                client = null;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return returnStrings;
        }

    }
}
