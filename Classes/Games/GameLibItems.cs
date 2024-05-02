using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Preview.GamesEnumeration;
using Windows.Management.Deployment;
using GameLib;
using GameLib.Core;
using Windows.ApplicationModel;


namespace Handheld_Hardware_Tools.Classes.Games
{
    public class GameLibList: List<GameLibItem>
    {
        public GameLibList()
        {
            SyncList();
        }

        public List<GameLibItem> GetUpdatedList()
        {
            List<GameLibItem> list = new List<GameLibItem>();
            //gamelauncher
            LauncherManager gameLauncher = new LauncherManager(new GameLib.Core.LauncherOptions() { QueryOnlineData = true });

            foreach (var launcher in gameLauncher.GetLaunchers())
            {

                switch (launcher.Name)
                {
                    case "Steam":
                        foreach (var game in launcher.Games)
                        {
                            if (game.Id != "228980")
                            {
                                GameLibItem launcherItem = new GameLibItem();
                                launcherItem.gameName = game.Name;
                                launcherItem.gameID = game.Id;
                                launcherItem.launchCommand = game.LaunchString;

                                if (game.Executables.Count() == 1)
                                {
                                    launcherItem.path = game.Executables.First();
                                    launcherItem.exe = Path.GetFileNameWithoutExtension(game.Executables.First());
                                }
                                else
                                {

                                    string[] array = launcherItem.gameName.Split(' ');
                                    foreach (string exe in game.Executables)
                                    {

                                        string exeName = Path.GetFileNameWithoutExtension(exe);
                                        if (game.Name.Contains("Call of duty", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (exeName.Contains("cod", StringComparison.OrdinalIgnoreCase))
                                            {
                                                launcherItem.path = exe;
                                                launcherItem.exe = exeName;
                                                break;
                                            }
                                        }
                                        foreach (string arr in array)
                                        {
                                            if (exeName.Contains(arr, StringComparison.OrdinalIgnoreCase))
                                            {
                                                launcherItem.path = exe;
                                                launcherItem.exe = exeName;
                                                break;
                                            }
                                        }
                                        if (launcherItem.path != null) { break; }
                                    }
                                }
                                if (launcherItem.path == "" || launcherItem.exe == "")
                                {
                                    launcherItem.path = game.Executables.Last();
                                    launcherItem.exe = Path.GetFileNameWithoutExtension(game.Executables.Last());
                                }


                                launcherItem.appType = launcher.Name;
                                list.Add(launcherItem);
                            }

                        }
                        break;
                    case "Battle.net":
                        foreach (var game in launcher.Games)
                        {
                            GameLibItem launcherItem = new GameLibItem();
                            launcherItem.gameName = game.Name;
                            launcherItem.gameID = game.Id;
                            launcherItem.launchCommand = game.LaunchString;
                            switch (game.Name)
                            {
                                case "Call of Duty Black Ops Cold War":
                                    launcherItem.path = game.WorkingDir + "\\BlackOpsColdWar.exe";
                                    launcherItem.exe = "BlackOpsColdWar";
                                    break;

                                default:
                                    launcherItem.path = game.Executables.First();
                                    launcherItem.exe = Path.GetFileNameWithoutExtension(launcherItem.path);
                                    break;
                            }

                            launcherItem.appType = launcher.Name;
                            list.Add(launcherItem);



                        }
                        break;
                    case "Epic Games":
                        foreach (var game in launcher.Games)
                        {
                            GameLibItem launcherItem = new GameLibItem();
                            launcherItem.gameName = game.Name;
                            launcherItem.gameID = game.Id;
                            launcherItem.launchCommand = game.LaunchString;
                            launcherItem.path = game.Executable.Replace("/", "\\");
                            launcherItem.exe = Path.GetFileNameWithoutExtension(launcherItem.path);
                            launcherItem.appType = launcher.Name;
                            list.Add(launcherItem);

                        }
                        break;

                    default:
                        foreach (var game in launcher.Games)
                        {
                            GameLibItem launcherItem = new GameLibItem();
                            launcherItem.gameName = game.Name;
                            launcherItem.gameID = game.Id;
                            launcherItem.launchCommand = game.LaunchString;
                            launcherItem.path = game.Executable;
                            launcherItem.exe = Path.GetFileNameWithoutExtension(launcherItem.path);
                            launcherItem.appType = launcher.Name;
                            list.Add(launcherItem);
                        }
                        break;

                }
              

            }

            //microsoft store apps below

            PackageManager packageManager = new PackageManager();
            IEnumerable<Windows.ApplicationModel.Package> packages = packageManager.FindPackages();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                string xboxGameDirectory = Path.Combine(d.Name, "XboxGames");
                string[] filesInDirectory;
                if (Directory.Exists(xboxGameDirectory))
                {
                    filesInDirectory = Directory.GetDirectories(xboxGameDirectory);

                    if (filesInDirectory.Length > 0)
                    {
                        string[] strings = filesInDirectory.Select(x => Path.GetFileName(x)).ToArray();

                        if (strings.Length > 0)
                        {
                            foreach (Package package in packages)
                            {
                                string install = package.InstalledLocation.Path;
                                string sig = package.SignatureKind.ToString();

                                if (install.Contains("WindowsApps") && sig == "Store" && package.IsFramework == false)
                                {
                                    if (strings.Contains(package.DisplayName))
                                    {
                                        GameLibItem launcherItem = new GameLibItem();
                                        launcherItem.gameName = package.DisplayName;
                                        launcherItem.gameID = package.Id.FullName;
                                        launcherItem.launchCommand = package.Id.FullName;


                                        //launcherItem.path = game.Executable;
                                        //launcherItem.exe = Path.GetFileNameWithoutExtension(launcherItem.path);
                                        launcherItem.appType = "Microsoft Store";
                                        launcherItem.imageLocation = package.Logo.AbsolutePath;
                                        list.Add(launcherItem);

                                    }

                                }

                            }

                        }

                    }

                }
            }

            return list;
        }


        public void SyncList()
        {
            List<GameLibItem> gameLibItems = GetUpdatedList();

            foreach(GameLibItem gameLibItem in gameLibItems)
            {

                if(this.FirstOrDefault(s => s.gameID == gameLibItem.gameID) == null)
                {
                    this.Add(gameLibItem);
                }
            }
        }


        public void SaveToXML()
        {
            XML_Management.Instance.SaveXML("GameLibList", this);
        }
    }
    public class GameLibItem
    {
        public string gameID { get; set; }
        public string gameName { get; set; }
        public string appType { get; set; }
        public string launchCommand { get; set; }
        public string path { get; set; }
        public string exe { get; set; }
        public string imageLocation { get; set; } = "";
        public bool displayGame { get; set; } = true;
    }
}
