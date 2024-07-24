using ABI.System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Handheld_Hardware_Tools.Classes
{
    public class Update_Management
    {

        private static Update_Management _instance = null;
        private static readonly object lockObj = new object();
        private Update_Management()
        {
        }
        public static Update_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Update_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        public async void CheckForUpdates(bool checkAtStartup)
        {

            string[] xmlValues = await CheckLatestVersionGetDownloadLink();

            // Get the currently executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get the version information
            string version = assembly.GetName().Version.ToString();

            if (xmlValues[0] != "" && xmlValues[0] != version)
            {
                //means we have a new update, ask to update
                DialogResult dialogResult;
                dialogResult =
                        System.Windows.Forms.MessageBox.Show(
                            $@"There is new version {xmlValues[0]} available. You are using version {version}. Do you want to update the application now?", @"Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                // Uncomment the following line if you want to show standard update dialog instead.
                // AutoUpdater.ShowUpdateForm(args);

                if (dialogResult.Equals(System.Windows.Forms.DialogResult.Yes))
                {
                    try
                    {
                        //Throw event to close main window

                        if (xmlValues[1] != "")
                        {
                            bool downloadResult = await DownloadLatestHHT(xmlValues[1]);
                            if (downloadResult)
                            {
                                RunZipExtractor();
                            }
                        }
                    }
                    catch (System.Exception exception)
                    {
         
                    }
                }
            }
            else
            {
                Notification_Management.Instance.TaskbarNotification("No updates available.");
            }




           

        }
        private async Task<bool> DownloadLatestHHT(string url)
        {
           
            string outputFilePath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\ZipExtractor\\Handheld.Hardware.Tools.zip"; 

            bool result = await DownloadZipFile(url, outputFilePath);

            return result;
        }
        private static async Task<bool> DownloadZipFile(string url, string outputFilePath)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                  fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }

                    Console.WriteLine("Download completed successfully.");
                    return true;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Network error occurred: {ex.Message}");
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Request timed out: {ex.Message}");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                return false;
            }
        }
        private async Task<string[]> CheckLatestVersionGetDownloadLink()
        {
                     
            //Get latest version from XML file
            string[] xmlValues = await ReadXMLFile();
                      

            return xmlValues;
        
        }

        private async Task<string[]> ReadXMLFile()
        {
            using (HttpClient client = new HttpClient())
            {
                string[] returnValues = new string[2];
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/project-sbc/Handheld-Hardware-Tools/master/Update.xml");
                    response.EnsureSuccessStatusCode();
                    string xmlContent = await response.Content.ReadAsStringAsync();

                    // Load the XML content into an XmlDocument
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);

                    // Return the XML content as string
                    XmlNode versionNode = doc.SelectSingleNode("//item/version");
                    if (versionNode != null)
                    {
                        returnValues[0]= versionNode.InnerText;
                    }
                    else
                    {
                        returnValues[0] = string.Empty;
                    }

                    XmlNode urlNode = doc.SelectSingleNode("//item/url");
                    if (urlNode != null)
                    {
                        returnValues[1] = urlNode.InnerText;
                    }
                    else
                    {
                        returnValues[1] = string.Empty;
                    }

                    Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

                    settings.lastUpdateCheck = DateTime.Now.ToString("G");
                    settings.lastUpdateCheckVersion = returnValues[0];

                    settings.SaveToXML();

                    return returnValues;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return returnValues;
                }
            }
        }


        private void RunZipExtractor()
        {

            ProcessStartInfo psi = new ProcessStartInfo();
            string zipextractorfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\ZipExtractor");
            var arguments = new Collection<string>
                {
                    "--input",
                    Path.Combine(zipextractorfolder, "Handheld.Hardware.Tools.zip"),
                    "--output",
                    AppDomain.CurrentDomain.BaseDirectory,
                    "--current-exe",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Handheld Hardware Tools.exe")
                };
            psi.FileName = Path.Combine(zipextractorfolder, "ZipExtractor.exe");
            psi.Arguments = PasteArguments.BuildArguments(arguments);
            Process.Start(psi);

            QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();
            qam.Close();
        }


    }
    public static class PasteArguments
    {
        private const char Quote = '\"';
        private const char Backslash = '\\';


        public static string BuildArguments(Collection<string> argumentList)
        {
            var arguments = new StringBuilder();
            if (argumentList is not { Count: > 0 })
            {
                return string.Empty;
            }

            foreach (string argument in argumentList) PasteArguments.AppendArgument(ref arguments, argument);
            return arguments.ToString();
        }
        internal static void AppendArgument(ref StringBuilder stringBuilder, string argument)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Append(' ');
            }

            // Parsing rules for non-argv[0] arguments:
            //   - Backslash is a normal character except followed by a quote.
            //   - 2N backslashes followed by a quote ==> N literal backslashes followed by unescaped quote
            //   - 2N+1 backslashes followed by a quote ==> N literal backslashes followed by a literal quote
            //   - Parsing stops at first whitespace outside of quoted region.
            //   - (post 2008 rule): A closing quote followed by another quote ==> literal quote, and parsing remains in quoting mode.
            if (argument.Length != 0 && ContainsNoWhitespaceOrQuotes(argument))
            {
                // Simple case - no quoting or changes needed.
                stringBuilder.Append(argument);
            }
            else
            {
                stringBuilder.Append(Quote);
                var idx = 0;
                while (idx < argument.Length)
                {
                    char c = argument[idx++];
                    switch (c)
                    {
                        case Backslash:
                            {
                                var numBackSlash = 1;
                                while (idx < argument.Length && argument[idx] == Backslash)
                                {
                                    idx++;
                                    numBackSlash++;
                                }

                                if (idx == argument.Length)
                                {
                                    // We'll emit an end quote after this so must double the number of backslashes.
                                    stringBuilder.Append(Backslash, numBackSlash * 2);
                                }
                                else if (argument[idx] == Quote)
                                {
                                    // Backslashes will be followed by a quote. Must double the number of backslashes.
                                    stringBuilder.Append(Backslash, numBackSlash * 2 + 1);
                                    stringBuilder.Append(Quote);
                                    idx++;
                                }
                                else
                                {
                                    // Backslash will not be followed by a quote, so emit as normal characters.
                                    stringBuilder.Append(Backslash, numBackSlash);
                                }

                                continue;
                            }
                        case Quote:
                            // Escape the quote so it appears as a literal. This also guarantees that we won't end up generating a closing quote followed
                            // by another quote (which parses differently pre-2008 vs. post-2008.)
                            stringBuilder.Append(Backslash);
                            stringBuilder.Append(Quote);
                            continue;
                        default:
                            stringBuilder.Append(c);
                            break;
                    }
                }

                stringBuilder.Append(Quote);
            }
        }

        private static bool ContainsNoWhitespaceOrQuotes(string s)
        {
            return s.All(c => !char.IsWhiteSpace(c) && c != Quote);
        }
    }
}
