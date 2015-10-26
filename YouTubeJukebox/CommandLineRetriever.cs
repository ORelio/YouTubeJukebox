using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTools;
using System.IO;
using System.Diagnostics;

namespace YouTubeJukebox
{
    /// <summary>
    /// Retrieve YouTube videos from the command line
    /// </summary>
    public class CommandLineRetriever : IYTStatusListener
    {
        private bool _verbose = false;
        private int _exitcode = 0;

        /// <summary>
        /// Create a new command-line retriever
        /// </summary>
        /// <param name="verbose">enable/disable verbosity</param>
        private CommandLineRetriever(bool verbose)
        {
            _verbose = verbose;
        }

        /// <summary>
        /// Process command-line arguments
        /// </summary>
        public static int ProcessCommandLine(string[] args)
        {
            bool verbose = false;
            bool help = false;

            bool playRandom = false;
            bool playReverse = false;
            bool playOnlyNew = false;
                
            List<string> remainingArgs = new List<string>();
                
            foreach (string arg in args)
            {
                if (arg.StartsWith("-") && arg.Length > 1)
                {
                    if (arg.StartsWith("--"))
                    {
                        string arg_name = "";

                        if (arg.Length > 2)
                            arg_name = arg.Substring(2);

                        switch (arg_name.ToLower())
                        {
                            case "reverse":
                                playReverse = true;
                                break;

                            case "shuffle":
                                playRandom = true;
                                break;

                            case "onlynew":
                                playOnlyNew = true;
                                break;

                            case "nocache":
                                Settings.SavingEnabled = false;
                                break;

                            case "verbose":
                                verbose = true;
                                break;

                            case "help":
                                help = true;
                                break;

                            case "gui":
                                return Program.Main(new string[] { "--gui" });

                            default:
                                Console.Error.WriteLine(Translations.Get("error_unknown_arg"), arg_name);
                                help = true;
                                break;
                        }
                    }
                    else
                    {
                        foreach (char a in arg.Substring(1))
                        {
                            switch (a)
                            {
                                case 'r':
                                    playReverse = true;
                                    break;

                                case 's':
                                    playRandom = true;
                                    break;

                                case 'o':
                                    playOnlyNew = true;
                                    break;

                                case 'n':
                                    Settings.SavingEnabled = false;
                                    break;

                                case 'v':
                                    verbose = true;
                                    break;

                                case '?':
                                    help = true;
                                    break;

                                case 'g':
                                    return Program.Main(new string[] { "--gui" });

                                default:
                                    Console.Error.WriteLine(Translations.Get("error_unknown_arg"), a);
                                    help = true;
                                    break;
                            }
                        }
                    }
                }
                else remainingArgs.Add(arg);
            }

            if (Settings.SavingEnabled)
                Settings.Load();

            Settings.PlayRandom = playRandom;
            Settings.PlayReverse = playReverse;
            Settings.PlayOnlyNew = playOnlyNew;

            if (verbose || help || remainingArgs.Count == 0)
                Console.Error.WriteLine(Program.Name + " v" + Program.Version);

            if (help || remainingArgs.Count == 0)
            {
                Console.Error.WriteLine(Translations.Get("help_command"),
                    (Tools.IsUsingMono ? "mono " : "")
                    + Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));

                Console.Error.WriteLine(Translations.Get("help_channel"));
                Console.Error.WriteLine(Translations.Get("help_output"));
                Console.Error.WriteLine(Translations.Get("help_reverse"));
                Console.Error.WriteLine(Translations.Get("help_shuffle"));
                Console.Error.WriteLine(Translations.Get("help_onlynew"));
                Console.Error.WriteLine(Translations.Get("help_nocache"));
                Console.Error.WriteLine(Translations.Get("help_verbose"));
                Console.Error.WriteLine(Translations.Get("help_help"));
                Console.Error.WriteLine(Translations.Get("help_gui"));

                Console.Error.WriteLine();
                return 0;
            }
            else
            {
                string channelName = YouTube.GetChannelName(remainingArgs[0]);
                string outputFile = remainingArgs.Count > 1 ? remainingArgs[1] : null;
                return (new CommandLineRetriever(verbose)).RetrieveVideos(channelName, outputFile);
            }
        }

        /// <summary>
        /// Retrieve videos from the specified channel
        /// </summary>
        /// <param name="channelName">Channel Name</param>
        /// <param name="outputPlaylist">Where to output playlist (null for standard output)</param>
        public int RetrieveVideos(string channelName, string outputPlaylist)
        {
            if (_verbose)
                Console.Error.WriteLine("[INFO] " + String.Format(Translations.Get("text_retrieving"), channelName));

            channelName = Settings.ChannelDatabaseGetFixedName(channelName);
            List<string> videoCache = null;
            var database = Settings.ChannelDatabaseGet();
            if (database.ContainsKey(channelName))
                videoCache = database[channelName];
            List<string> videoCacheOrig
                    = videoCache != null
                    ? new List<string>(videoCache)
                    : new List<string>();

            string playList
                = String.Join(Tools.IsUsingMono ? "\n" : "\r\n",
                    Settings.ApplyPlayModifiers
                        (YouTube.GetChannelVideos(channelName, videoCache, this), videoCacheOrig)
                            .Select(video => YouTube.GetVideoUrl(video)));

            if (outputPlaylist != null)
                File.WriteAllText(outputPlaylist, playList);
            else Console.WriteLine(playList);

            return _exitcode;
        }

        /// <summary>
        /// Video Retrieval Status Update Event
        /// </summary>
        /// <param name="channelName">Name of the YouTube channel</param>
        /// <param name="videos">Videos that have been retrieved so far</param>
        /// <param name="level">Event level (normal, warning, error)</param>
        /// <param name="status">Event status (downloading, finished with error, finished without error)</param>
        /// <param name="metatadata">Event metadata provided when stating download using YouTube.GetChannelVideos()</param>
        public void UpdateYTDLStatus(string channelName, List<string> videos, YTEventLevel level, YTDLStatus status, object metadata)
        {
            string logLevel = "";

            switch (level)
            {
                case YTEventLevel.Error: logLevel = "[ERROR] "; break;
                case YTEventLevel.Warning: logLevel = "[WARNING] "; break;
                case YTEventLevel.Info: logLevel = "[INFO] "; break;
            }

            if (_verbose)
            {
                Console.Error.WriteLine(logLevel + Translations.Get("text_videos") + videos.Count.ToString());
            }

            if (level == YTEventLevel.Info)
            {
                if (status == YTDLStatus.FinishedOK && Settings.SavingEnabled)
                {
                    Settings.ChannelDatabasePut(channelName, videos);
                    Settings.Save();
                }
            }
            else
            {
                string message = "";
                _exitcode = 1;

                switch (status)
                {
                    case YTDLStatus.InvalidData: message = Translations.Get("error_invalid_data"); break;
                    case YTDLStatus.RequestFailed: message = Translations.Get("error_other"); break;
                }

                message += Translations.Get("error_while_loading");

                switch (level)
                {
                    case YTEventLevel.Warning: message += Translations.Get("error_incomplete_playlist"); break;
                    case YTEventLevel.Error: message += Translations.Get("error_of_webpage"); break;
                }

                Console.Error.WriteLine(logLevel
                    + Translations.Get(status == YTDLStatus.InvalidData ? "error_invalid_data" : "error_other")
                    + Translations.Get("error_while_loading")
                    + Translations.Get(level == YTEventLevel.Warning ? "error_incomplete_playlist" : "error_of_webpage"));
            }
        }
    }
}
