using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using SharpTools;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace YouTubeJukebox
{
    public partial class FormMain : Form, IYTStatusListener
    {
        private enum FormState { Initial, LoadingChannel, ChannelOK, ChannelNotOK, LoadingVideos };

        private FormState _formState = FormState.Initial;
        private readonly object _formStateLock = new object();
        private bool _canChangeChannel = true;
        private bool _canBrowseChannel = false;
        private bool _canStartDownloading = false;
        private bool _canCancelDownloading = false;
        private bool _canUpgradeVLCScript = false;
        
        private Thread _threadDLChannelPicture = null;
        private readonly object _threadDLChannelPictureLock = new object();
        private readonly System.Windows.Forms.Timer _timerDLChannelPicture = new System.Windows.Forms.Timer();
        private Image _channelImage = null;

        private readonly object _threadRetrieveVideosLock = new object();
        private Thread _threadRetrieveVideos = null;

        /// <summary>
        /// Main Form Constructor
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            Settings.Load();
            
            Text = Program.Name + " v" + Program.Version;

            MediaPlayerGroupBox.Text = Translations.Get("player");
            buttonOK.Text = Translations.Get("button_play");
            buttonExit.Text = Translations.Get("button_exit");
            LoadingProgress.Text = Translations.Get("text_loading");
            YouTubeChannelReadRandom.Text = Translations.Get("setting_shuffle");
            YouTubeChannelReadReverse.Text = Translations.Get("setting_reverse");
            YoutubeTabPage.Text = Translations.Get("tab_youtube");
            PlayerTabPage.Text = Translations.Get("tab_player");
            AboutTabPage.Text = Translations.Get("tab_about");
            buttonUpgradeVlcScript.Text = Translations.Get("player_upgrade");
            UpgradeMediaPlayerGroupBox.Text = Translations.Get("text_playback_issues");
            aboutTitle.Text = Translations.Get("about_title");
            aboutSubtitle.Text = Translations.Get("about_subtitle");
            aboutDescription.Text = Translations.Get("about_description");
            aboutWebsite.Text = Translations.Get("about_visit_me");

            _timerDLChannelPicture = new System.Windows.Forms.Timer();
            _timerDLChannelPicture.Tick += new EventHandler(YoutubeChannelTimeTick);
            _timerDLChannelPicture.Interval = 1000;
            
            YouTubeChannelReadRandom.Checked = Settings.PlayRandom;
            YouTubeChannelReadReverse.Checked = Settings.PlayReverse;
            LoadMediaPlayer(Settings.MediaPlayerExe, false, true);

            UpdateForm();
        }

        /// <summary>
        /// Update form state according to application settings and form state
        /// </summary>
        /// <param name="newState">New form state, if any</param>
        private void UpdateForm(FormState? newState = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateForm(newState)));
                return;
            }

            lock (_formStateLock)
            {
                if (newState.HasValue)
                    _formState = newState.Value;

                YoutubeChannelNameHistory.Items.Clear();
                YoutubeChannelNameHistory.Items.AddRange(
                    Settings.ChannelDatabaseGet().Keys.Select(p => p.ToString()).ToArray());
                _timerDLChannelPicture.Stop();

                _canChangeChannel = true;
                _canBrowseChannel = false;
                _canStartDownloading = false;
                _canCancelDownloading = false;

                _canUpgradeVLCScript = VLC.IsVlcExe(Settings.MediaPlayerExe);

                Image channelStatusImg = null;
                Image channelProfilePic = null;

                string buttonExitText = Translations.Get("button_exit");
                string loadingProgressText = String.Empty;

                switch (_formState)
                {
                    case FormState.ChannelNotOK:
                        channelStatusImg = Properties.Resources.NotFound;
                        break;

                    case FormState.LoadingChannel:
                        channelStatusImg = Properties.Resources.Loading;
                        channelProfilePic = Tools.GetImageWithOpacity(_channelImage, 0.5f);
                        break;

                    case FormState.ChannelOK:
                        _canBrowseChannel = true;
                        _canStartDownloading = true;
                        channelProfilePic = _channelImage;
                        break;

                    case FormState.LoadingVideos:
                        _canBrowseChannel = true;
                        _canChangeChannel = false;
                        _canCancelDownloading = true;
                        channelProfilePic = _channelImage;
                        buttonExitText = Translations.Get("button_cancel");
                        loadingProgressText = Translations.Get("text_loading");
                        break;
                }

                YoutubeChannelNameHistory.Enabled = _canChangeChannel;
                YoutubeChannelImage.Image = channelStatusImg;
                YoutubeChannelImage.BackgroundImage = channelProfilePic;
                YoutubeChannelImage.Cursor = _canBrowseChannel ? Cursors.Hand : Cursors.Default;
                YoutubeChannelImage.Enabled = _canChangeChannel;

                buttonUpgradeVlcScript.Enabled = _canUpgradeVLCScript;
                buttonOK.Enabled = _canStartDownloading;
                buttonExit.Text = buttonExitText;

                LoadingAnim.Visible = _canCancelDownloading;
                LoadingProgress.Visible = _canCancelDownloading;
                LoadingProgress.Text = loadingProgressText;
            }
        }

        /// <summary>
        /// Load the specified media player executable
        /// </summary>
        private void LoadMediaPlayer(string mediaPlayerLocation, bool popup, bool switchtab)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadMediaPlayer(mediaPlayerLocation, popup, switchtab)));
            }
            else
            {
                if (Tools.IsValidExeFile(mediaPlayerLocation))
                {
                    MediaPlayerIcon.Image = new Bitmap(Icon.ExtractAssociatedIcon(mediaPlayerLocation).ToBitmap(), MediaPlayerIcon.Size);
                    MediaPlayerTextBox.Text = mediaPlayerLocation;
                    buttonUpgradeVlcScript.Enabled = _canUpgradeVLCScript;
                    Settings.MediaPlayerExe = mediaPlayerLocation;
                    Settings.Save();
                }
                else
                {
                    if (popup)
                        MessageBox.Show(Translations.Get("player_prompt"), Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (switchtab)
                        mainTabControl.SelectedIndex = 1;
                }

                UpdateForm();
            }
        }

        /// <summary>
        /// Browse for a media player executable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayerBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Translations.Get("player_filter_exe") + "|*.exe";
            dlg.Title = Translations.Get("player_select");
            if (dlg.ShowDialog() == DialogResult.OK)
                LoadMediaPlayer(dlg.FileName, false, true);
        }

        /// <summary>
        /// Update YouTube Channel information
        /// </summary>
        private void YoutubeChannelNameChanged(object sender, EventArgs e)
        {
            _timerDLChannelPicture.Stop();
            _timerDLChannelPicture.Start();
            _canStartDownloading = false;
            _canBrowseChannel = false;
        }

        /// <summary>
        /// Open the YouTube channel page by clicking the channel picture
        /// </summary>
        private void YoutubeChannelImage_Click(object sender, EventArgs e)
        {
            if (_canBrowseChannel)
            {
                Process.Start(YouTube.GetChannelUrl(YoutubeChannelNameHistory.Text));
            }
        }

        /// <summary>
        /// Update timer timeout - Start downloading channel profile picture
        /// </summary>
        private void YoutubeChannelTimeTick(object sender, EventArgs e)
        {
            lock (_threadDLChannelPictureLock)
            {
                _timerDLChannelPicture.Stop();

                if (_threadDLChannelPicture != null)
                    _threadDLChannelPicture.Abort();

                string channelName
                    = YoutubeChannelNameHistory.Text
                    = YouTube.GetChannelName(YoutubeChannelNameHistory.Text.Trim());

                if (String.IsNullOrWhiteSpace(channelName))
                {
                    YoutubeChannelNameHistory.Text = String.Empty;
                    UpdateForm(FormState.Initial);
                    return;
                }
                else UpdateForm(FormState.LoadingChannel);

                _threadDLChannelPicture = new Thread(() =>
                {
                    Image channelImage = YouTube.GetChannelImage(channelName);

                    this.Invoke(new Action(() =>
                    {
                        if (channelImage != null)
                        {
                            _channelImage = new Bitmap(channelImage, YoutubeChannelImage.Size);
                            UpdateForm(FormState.ChannelOK);
                        }
                        else UpdateForm(FormState.ChannelNotOK);

                        lock (_threadDLChannelPictureLock)
                        {
                            if (_threadDLChannelPicture == Thread.CurrentThread)
                                _threadDLChannelPicture = null;
                        }
                    }));
                });
                _threadDLChannelPicture.Start();
            }
        }

        /// <summary>
        /// Exit Button
        /// </summary>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (_canCancelDownloading)
            {
                lock (_threadRetrieveVideosLock)
                {
                    _threadRetrieveVideos.Abort();
                    _threadRetrieveVideos = null;
                    UpdateForm(FormState.ChannelOK);
                }
            }
            else Close();
        }

        /// <summary>
        /// Launch video retrieval from the selected channel
        /// </summary>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_canStartDownloading && !String.IsNullOrWhiteSpace(YoutubeChannelNameHistory.Text))
            {
                UpdateForm(FormState.LoadingVideos);

                string channelName = Settings.ChannelDatabaseGetFixedName(YoutubeChannelNameHistory.Text.Trim());
                Settings.PlayReverse = YouTubeChannelReadReverse.Checked;
                Settings.PlayRandom = YouTubeChannelReadRandom.Checked;
                Settings.Save();

                List<string> videoCache = null;
                var database = Settings.ChannelDatabaseGet();
                if (database.ContainsKey(channelName))
                    videoCache = database[channelName];
                
                lock (_threadRetrieveVideosLock)
                {
                    _threadRetrieveVideos = new Thread(() =>
                    {
                        YouTube.GetChannelVideos(channelName, videoCache, this);
                    });
                    _threadRetrieveVideos.Start();
                }
            }
        }

        /// <summary>
        /// Video Retrieval Status Update Event
        /// </summary>
        /// <param name="channelName">Name of the YouTube channel</param>
        /// <param name="videos">Videos that have been retrieved so far</param>
        /// <param name="level">Event level (normal, warning, error)</param>
        /// <param name="status">Event status (downloading, finished with error, finished without error)</param>
        public void UpdateYTDLStatus(string channelName, List<string> videos, YTEventLevel level, YTDLStatus status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateYTDLStatus(channelName, videos, level, status)));
                return;
            }

            lock (_formStateLock)
            {
                LoadingProgress.Text = Translations.Get("text_videos") + videos.Count.ToString();
            }

            if (level == YTEventLevel.Info)
            {
                if (status == YTDLStatus.FinishedOK)
                {
                    Settings.ChannelDatabasePut(channelName, videos);
                    Settings.Save();
                }
            }
            else
            {
                string title = "";
                string message = "";

                MessageBoxIcon icon = MessageBoxIcon.Information;

                switch (status)
                {
                    case YTDLStatus.InvalidData:
                        message = Translations.Get("error_invalid_data");
                        break;

                    case YTDLStatus.RequestFailed:
                        message = Translations.Get("error_other");
                        break;
                }

                message += Translations.Get("error_while_loading");

                switch (level)
                {
                    case YTEventLevel.Warning:
                        title = Translations.Get("error_loading");
                        message += Translations.Get("error_incomplete_playlist");
                        icon = MessageBoxIcon.Exclamation;
                        break;

                    case YTEventLevel.Error:
                        title = Translations.Get("error_network");
                        message += Translations.Get("error_of_webpage");
                        icon = MessageBoxIcon.Error;
                        break;
                }

                MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
            }

            if (status != YTDLStatus.Downloading && level != YTEventLevel.Error)
            {
                string tempPlayList = "playlist-" + channelName.ToLowerInvariant() + ".m3u";
                File.WriteAllLines(tempPlayList, Settings.ApplyPlayModifiers(videos).Select(video => YouTube.GetVideoUrl(video)));
                if (Tools.IsValidExeFile(Settings.MediaPlayerExe))
                {
                    Process.Start(Settings.MediaPlayerExe, tempPlayList);
                }
                else LoadMediaPlayer(Settings.MediaPlayerExe, true, true);
                UpdateForm(FormState.ChannelOK);
            }
        }

        /// <summary>
        /// Launch upgrade of the VLC YouTube script
        /// </summary>
        private void buttonUpgradeVlcScript_Click(object sender, EventArgs e)
        {
            if (_canUpgradeVLCScript)
            {
                mainTabControl.Enabled = false;
                buttonUpgradeVlcScript.Enabled = false;
                MediaPlayerBrowseButton.Enabled = false;
                buttonUpgradeVlcScript.Text = Translations.Get("player_upgrading");
                new Thread(() =>
                {
                    UpgradeVLCScript(Settings.MediaPlayerExe);
                    Invoke(new Action(() =>
                    {
                        mainTabControl.Enabled = true;
                        buttonUpgradeVlcScript.Enabled = true;
                        MediaPlayerBrowseButton.Enabled = true;
                        buttonUpgradeVlcScript.Text = Translations.Get("player_upgrade");
                    }));
                }).Start();
            }
        }

        /// <summary>
        /// Upgrade the VLC Media Player YouTube script
        /// </summary>
        /// <param name="vlcPath">Path to VLC.exe</param>
        public static void UpgradeVLCScript(string vlcExeFile)
        {
            try
            {
                try
                {
                    VLC.UpdateResult result = VLC.UpdateYouTubeScript(vlcExeFile);
                    switch (result)
                    {
                        case VLC.UpdateResult.NotFound:
                            MessageBox.Show(Translations.Get("player_upgrade_not_found"), Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case VLC.UpdateResult.UpToDate:
                            MessageBox.Show(Translations.Get("player_upgrade_up_to_date"), Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case VLC.UpdateResult.Success:
                            MessageBox.Show(Translations.Get("player_upgrade_done"), Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Tools.LaunchOtherInstance("__upgrade_vlc " + '"' + vlcExeFile + '"', true, true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Translations.Get("player_upgrade_error") + "\n" + e.Message, Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Visit by profile by clicking a link on the About tab
        /// </summary>
        private void aboutWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/ORelio/");
        }
    }
}
