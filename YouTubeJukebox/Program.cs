using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharpTools;

namespace YouTubeJukebox
{
    /// <summary>
    /// Simple Application allowing to load a user's channel videos into the specified media player
    /// Inspired by youtube.nestharion.de, but not using YouTube API so no weird limitations
    /// By ORelio - (c) 2015 - Available under the CDDL-1.0 license
    /// </summary>
    public static class Program
    {
        public const string Name = "YouTube Jukebox";
        public const string Version = "1.0";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "__upgrade_vlc" && VLC.IsVlcExe(args[1]))
            {
                //Upgrade VLC's YouTube script
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                FormMain.UpgradeVLCScript(args[1]);
                return 0;
            }
            else if ((args.Length == 0 && !Tools.IsUsingMono) || args.Contains("--gui"))
            {
                //Run in graphical mode
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
                return 0;
            }
            else
            {
                //Run in command-line mode
                Tools.BindToConsole();
                return CommandLineRetriever.ProcessCommandLine(args);
            }
        }
    }
}
