using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTools;
using System.Diagnostics;
using System.IO;

namespace YouTubeJukebox
{
    /// <summary>
    /// Contains application settings, channel database and save/load from an INI file
    /// </summary>
    public static class Settings
    {
        private static readonly Dictionary<string, List<string>> Channels = new Dictionary<string, List<string>>();
        private static readonly string MediaPlayerDefaultLocation = "C:\\Program Files" + (Environment.Is64BitOperatingSystem ? " (x86)" : "") + "\\VideoLAN\\VLC\\vlc.exe";
        private static readonly string CurrentExePath = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string ConfigFile = Path.GetDirectoryName(CurrentExePath) + "\\" + Path.GetFileNameWithoutExtension(CurrentExePath) + ".ini";

        /// <summary>
        /// Specify whether the playlist is to be read in reverse order
        /// </summary>
        public static bool PlayReverse { get; set; }

        /// <summary>
        /// Specify whether the playlist is to be read in random order
        /// </summary>
        public static bool PlayRandom { get; set; }

        /// <summary>
        /// Specify which media player to use for reading the generated playlist
        /// </summary>
        public static string MediaPlayerExe { get; set; }

        /// <summary>
        /// Specify if saving via Settings.Save() will generate an INI file
        /// </summary>
        public static bool SavingEnabled { get; set; }

        /// <summary>
        /// Static class initializer to automatically load settings
        /// </summary>
        static Settings()
        {
            MediaPlayerExe = MediaPlayerDefaultLocation;
            SavingEnabled = true;
        }

        /// <summary>
        /// Apply video presets to the specified list of videos
        /// </summary>
        /// <param name="videos">list of videos</param>
        /// <returns>modified list of videos</returns>
        public static List<string> ApplyPlayModifiers(List<string> videos)
        {
            if (PlayReverse)
                videos.Reverse();

            if (PlayRandom)
                Tools.Shuffle(videos);

            return videos;
        }

        /// <summary>
        /// Get database of already processed YouTube channels
        /// </summary>
        /// <returns>channel names</returns>
        public static Dictionary<string, List<string>> ChannelDatabaseGet()
        {
            return Channels;
        }

        /// <summary>
        /// Check if the database contains the specified channel name and fix case if necessary
        /// </summary>
        /// <param name="channelName"></param>
        public static string ChannelDatabaseGetFixedName(string channelName)
        {
            string existing = Channels.Keys.FirstOrDefault(name => name.ToLower().Equals(channelName.ToLower()));
            if (existing != null)
                return existing;
            return channelName;
        }

        /// <summary>
        /// Add or update the specified channel data to channel history
        /// </summary>
        /// <param name="channelName">Channel name to add</param>
        public static void ChannelDatabasePut(string channelName, IEnumerable<string> videos)
        {
            lock (Channels)
            {
                string existing = Channels.Keys.FirstOrDefault(name => name.ToLower().Equals(channelName.ToLower()));

                if (existing != null)
                    Channels.Remove(existing);

                Channels[channelName] = new List<string>(videos);
            }
        }

        /// <summary>
        /// Clear channel history
        /// </summary>
        public static void ChannelDatabaseClearAll()
        {
            lock (Channels)
            {
                Channels.Clear();
            }
        }

        /// <summary>
        /// Clear channel videos cache
        /// </summary>
        public static void ChannelDatabaseClearVideos()
        {
            lock (Channels)
            {
                foreach (var item in Channels)
                    item.Value.Clear();
            }
        }

        /// <summary>
        /// Write the INI file with application settings
        /// </summary>
        public static void Save()
        {
            if (SavingEnabled)
            {
                var config = new Dictionary<string, Dictionary<string, string>>();

                config["Player"] = new Dictionary<string, string>();
                config["Player"]["Exe"] = MediaPlayerDefaultLocation;
                config["Playlist"] = new Dictionary<string, string>();
                config["Playlist"]["Reverse"] = PlayReverse.ToString();
                config["Playlist"]["Random"] = PlayRandom.ToString();
                config["Channels"] = new Dictionary<string, string>();

                lock (Channels)
                {
                    foreach (var channel in Channels)
                        config["Channels"][channel.Key] = String.Join(",", channel.Value);
                }

                INIFile.WriteFile(ConfigFile, config, Program.Name + " Configuration File", false);
            }
        }

        /// <summary>
        /// Load the INI file with application settings
        /// </summary>
        public static void Load()
        {
            if (File.Exists(ConfigFile))
            {
                var settingsRaw = INIFile.ParseFile(ConfigFile, false);
                foreach (var settingsSection in settingsRaw)
                {
                    switch (settingsSection.Key.ToLower())
                    {
                        case "player":
                            foreach (var setting in settingsSection.Value)
                            {
                                switch (setting.Key.ToLower())
                                {
                                    case "exe":
                                        MediaPlayerExe = setting.Value;
                                        break;
                                }
                            }
                            break;

                        case "playlist":
                            foreach (var setting in settingsSection.Value)
                            {
                                switch (setting.Key.ToLower())
                                {
                                    case "reverse":
                                        PlayReverse = INIFile.Str2Bool(setting.Value);
                                        break;

                                    case "random":
                                        PlayRandom = INIFile.Str2Bool(setting.Value);
                                        break;
                                }
                            }
                            break;

                        case "channels":
                            lock (Channels)
                            {
                                ChannelDatabaseClearAll();
                                foreach (var setting in settingsSection.Value)
                                {
                                    ChannelDatabasePut(setting.Key, setting.Value.Split(','));
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
