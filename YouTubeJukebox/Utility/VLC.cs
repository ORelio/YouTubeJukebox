using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace SharpTools
{
    /// <summary>
    /// Tools for the VLC Media Player
    /// By ORelio - (c) 2015 - Available under the CDDL-1.0 license
    /// </summary>
    public static class VLC
    {
        private static readonly string OnlineYouTubeLua = "https://raw.githubusercontent.com/videolan/vlc/master/share/lua/playlist/youtube.lua";
        private static readonly string LocalYouTubeLua = "/lua/playlist/youtube.lua";
        private static readonly string LocalYouTubeLuac = LocalYouTubeLua + 'c';
        private static readonly string PlayerExeFile = "/vlc.exe";
        private static readonly string PlayerFile = "/vlc";

        public enum UpdateResult { NotFound, UpToDate, Success };

        /// <summary>
        /// Check that the provided file is named "vlc.exe"
        /// </summary>
        /// <param name="path">path to test</param>
        /// <returns>true if VLC.exe</returns>
        public static bool IsVlcExe(string path)
        {
            return Path.GetFileName(path) == Path.GetFileName(PlayerExeFile)
                || Path.GetFileName(path) == Path.GetFileName(PlayerFile);
        }

        /// <summary>
        /// Update the YouTube playback Lua script straight from the VLC GitHub repository
        /// </summary>
        /// <param name="vlcExeFile">Path to vlc.exe</param>
        public static UpdateResult UpdateYouTubeScript(string vlcExeFile)
        {
            string baseDirectory = Path.GetDirectoryName(vlcExeFile);

            if (File.Exists(baseDirectory + PlayerExeFile)
                || File.Exists(baseDirectory + PlayerFile))
            {
                string newScript = new WebClient().DownloadString(OnlineYouTubeLua);

                if (File.Exists(baseDirectory + LocalYouTubeLuac))
                    File.Delete(baseDirectory + LocalYouTubeLuac);

                if (File.Exists(baseDirectory + LocalYouTubeLua))
                {
                    string currentFile = File.ReadAllText(baseDirectory + LocalYouTubeLua);

                    if (currentFile == newScript)
                        return UpdateResult.UpToDate;

                    File.Delete(baseDirectory + LocalYouTubeLua);
                }

                File.WriteAllText(baseDirectory + LocalYouTubeLua, newScript);

                return UpdateResult.Success;
            }

            return UpdateResult.NotFound;
        }
    }
}
