/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.Net;
using MCGalaxy.Network;
using MCGalaxy.Tasks;

namespace MCGalaxy 
{
    /// <summary> Checks for and applies software updates. </summary>
    public static class Updater 
    {    
        public static string SourceURL = "https://github.com/Evanzap/KDEGalaxy";
        public const string BaseURL    = "https://raw.githubusercontent.com/Evanzap/KDEGalaxy/master/";
        public const string UploadsURL = "https://github.com/Evanzap/KDEGalaxy/tree/master/Uploads";
        
        const string CurrentVersionURL = BaseURL + "Uploads/current_version.txt";
#if MCG_STANDALONE
        static string dllURL = "https://cs.classicube.net/mcgalaxy/" + IOperatingSystem.DetectOS().StandaloneName;
#elif TEN_BIT_BLOCKS
        const string dllURL = BaseURL + "Uploads/KDEGalaxy_infid.dll";
#else
        const string dllURL = BaseURL + "Uploads/KDEGalaxy_.dll";
#endif
        const string changelogURL = BaseURL + "Changelog.txt";
        const string guiURL = BaseURL + "Uploads/KDEGalaxy.exe";
        const string cliURL = BaseURL + "Uploads/KDEGalaxyCLI.exe";

        public static event EventHandler NewerVersionDetected;
        
        public static void UpdaterTask(SchedulerTask task) {
            UpdateCheck();
            task.Delay = TimeSpan.FromHours(2);
        }

        static void UpdateCheck() {
            if (!Server.Config.CheckForUpdates) return;
            WebClient client = HttpUtil.CreateWebClient();

            try {
                string latest = client.DownloadString(CurrentVersionURL);
                
                if (new Version(Server.Version) >= new Version(latest)) {
                    Logger.Log(LogType.SystemActivity, "No update found!");
                } else if (NewerVersionDetected != null) {
                    NewerVersionDetected(null, EventArgs.Empty);
                }
            } catch (Exception ex) {
                Logger.LogError("Error checking for updates", ex);
            }
            
            client.Dispose();
        }

        public static void PerformUpdate() {
            try {
                try {
                    DeleteFiles("Changelog.txt", "KDEGalaxy_.update", "KDEGalaxy.update", "KDEGalaxyCLI.update",
                                "prev_KDEGalaxy_.dll", "prev_KDEGalaxy.exe", "prev_KDEGalaxyCLI.exe");
                } catch {
                }
                
                WebClient client = HttpUtil.CreateWebClient();
                client.DownloadFile(dllURL, "KDEGalaxy_.update");
#if !MCG_STANDALONE
                client.DownloadFile(guiURL, "KDEGalaxy.update");
                client.DownloadFile(cliURL, "KDEGalaxyCLI.update");
#endif
                client.DownloadFile(changelogURL, "Changelog.txt");

                Server.SaveAllLevels();
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players) pl.SaveStats();
                
                string serverDLL = Server.GetServerDLLPath();
                
                // Move current files to previous files (by moving instead of copying, 
                //  can overwrite original the files without breaking the server)
                AtomicIO.TryMove(serverDLL,         "prev_KDEGalaxy_.dll");
                AtomicIO.TryMove("KDEGalaxy.exe",    "prev_KDEGalaxy.exe");
                AtomicIO.TryMove("KDEGalaxyCLI.exe", "prev_KDEGalaxyCLI.exe");

                // Move update files to current files
                AtomicIO.TryMove("KDEGalaxy_.update",   serverDLL);
                AtomicIO.TryMove("KDEGalaxy.update",    "KDEGalaxy.exe");
                AtomicIO.TryMove("KDEGalaxyCLI.update", "KDEGalaxyCLI.exe");                             

                Server.Stop(true, "Updating server.");
            } catch (Exception ex) {
                Logger.LogError("Error performing update", ex);
            }
        }
        
        static void DeleteFiles(params string[] paths) {
            foreach (string path in paths) { AtomicIO.TryDelete(path); }
        }
    }
}
