using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace YouTubeJukebox
{
    /// <summary>
    /// Containing various utility methods for this application
    /// </summary>
    static class Tools
    {
        /// <summary>
        /// Check if the given Exe file seems to be a valid executable
        /// </summary>
        /// <param name="exeFile">Exe file path</param>
        /// <returns>True if Exe header is valid</returns>
        public static bool IsValidExeFile(string exeFile)
        {
            if (!File.Exists(exeFile))
                return false;
            var twoBytes = new byte[2];
            using (var fileStream = File.OpenRead(exeFile))
                fileStream.Read(twoBytes, 0, 2);
            return Encoding.UTF8.GetString(twoBytes) == "MZ";
        }

        /// <summary>  
        /// method for changing the opacity of an image
        /// Source : CodeProject tip no.201129
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns>Image with applied transparency</returns>  
        public static Image GetImageWithOpacity(Image image, float opacity)
        {
            //handle missing image
            if (image == null)
                return null;

            //create a Bitmap the size of the image provided  
            Bitmap bmp = new Bitmap(image.Width, image.Height);

            //create a graphics object from the image  
            using (Graphics gfx = Graphics.FromImage(bmp))
            {

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity  
                matrix.Matrix33 = opacity;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        /// <summary>
        /// Randomize the order of a list's items
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="list">List to randomize</param>
        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Detect if the user is running the application through Mono
        /// </summary>
        public static bool IsUsingMono
        {
            get
            {
                return Type.GetType("Mono.Runtime") != null;
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// On Windows, console will not show anything unless a system call is made
        /// </summary>
        public static void BindToConsole()
        {
            if (!IsUsingMono)
            {
                AttachConsole(ATTACH_PARENT_PROCESS);
            }
        }

        /// <summary>
        /// Launch another instance of the program, with specified level and arguments
        /// </summary>
        /// <param name="arg">Arguments</param>
        /// <param name="adminprompt">Prompt for admin rights</param>
        /// <param name="waitforexit">Wait for exit</param>
        public static void LaunchOtherInstance(string arg = "", bool adminprompt = false, bool waitforexit = false)
        {
            var startInfo = new ProcessStartInfo();
            if (adminprompt) { startInfo.Verb = "runas"; }
            startInfo.FileName = Application.ExecutablePath;
            startInfo.Arguments = arg;
            var p = Process.Start(startInfo);
            if (waitforexit) { p.WaitForExit(); }
        }
    }
}
