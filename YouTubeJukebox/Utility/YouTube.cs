using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using System.Threading;

namespace SharpTools
{
    /// <summary>
    /// Wrapper for interacting with YouTube web interface so without using any official API
    /// </summary>
    /// <remarks>
    /// By ORelio (c) 2015 - CDDL 1.0
    /// </remarks>
    public static class YouTube
    {
        //User Agent for making requests
        private static readonly string UserAgent = HTTPRawRequest.GetUserAgent();

        //Url templates for building Urls
        private static readonly string YTVideoIdParam = "?v";
        private static readonly string YTHost = "www.youtube.com";
        private static readonly string YTChannelUrlBase = "/user/";
        private static readonly string YTBaseUrl = "https://" + YTHost;
        private static readonly string YTChannelVideosPage = "/videos?sort=dd&view=0&flow=grid";
        private static readonly string YTVideoBaseUrl = YTBaseUrl + "/watch" + YTVideoIdParam + "=";

        //HTML/JSON parsing delimiters/field names
        private const string DelimiterChannelImage = "<img class=\"channel-header-profile-image\" src=\"";
        const string DelimiterVideoImageRegion = "<span class=\"yt-thumb-default\">";
        const string DelimiterVideoImageLink = "aria-hidden=\"true\" src=\"";
        const string DelimeterAjaxMoreVideos = "data-uix-load-more-href=\"";
        const string JsonMoreAjaxField = "load_more_widget_html";
        const string JsonMoreVideosField = "content_html";

        /// <summary>
        /// Get Url of the specified channel
        /// </summary>
        /// <param name="channelName">Name of the channel</param>
        /// <returns>Url of the channel</returns>
        public static string GetChannelUrl(string channelName)
        {
            return YTBaseUrl + YTChannelUrlBase + channelName;
        }

        /// <summary>
        /// Get Url of the specified video
        /// </summary>
        /// <param name="videoID">Identifier of the video</param>
        public static string GetVideoUrl(string videoID)
        {
            return YTVideoBaseUrl + videoID;
        }

        /// <summary>
        /// Get Channel name from the specified url
        /// </summary>
        /// <param name="url">Webpage url</param>
        /// <returns>channel name or url itself if failed to find</returns>
        public static string GetChannelName(string url)
        {
            if (url.StartsWith("http:"))
                url = url.Replace("http:", "https:");

            if (url.StartsWith(YTBaseUrl))
                url = FindStringWithDelimiters(url, YTChannelUrlBase, '/');

            return url;
        }

        /// <summary>
        /// Split a string using the specified delimiter
        /// </summary>
        /// <param name="toSplit">String to split</param>
        /// <param name="delimiter">Delimiter</param>
        /// <returns>Splitted string</returns>
        private static string[] SplitStringWithDelimiter(string toSplit, string delimiter)
        {
            if (String.IsNullOrEmpty(toSplit) || String.IsNullOrEmpty(delimiter))
                return new string[] { };
            return toSplit.Split(new string[] { delimiter }, StringSplitOptions.None);
        }

        /// <summary>
        /// Find the first occurence of a substring in a string to parse using a start and end delimiter
        /// </summary>
        /// <param name="toParse">String to parse</param>
        /// <param name="startDelimiter">Start delimiter</param>
        /// <param name="endDelimiter">End delimiter</param>
        /// <returns>First occurence of substring or null if not found</returns>
        private static string FindStringWithDelimiters(string toParse, string startDelimiter, char endDelimiter)
        {
            if (String.IsNullOrEmpty(toParse) || String.IsNullOrEmpty(startDelimiter))
                return null;

            string[] splittedStart = SplitStringWithDelimiter(toParse, startDelimiter);

            if (splittedStart.Length < 2)
                return null;

            string[] splittedEnd = splittedStart[1].Split(endDelimiter);

            if (splittedEnd.Length < 2)
                return null;

            return splittedEnd[0];
        }

        /// <summary>
        /// Get Image from location and referer
        /// </summary>
        /// <param name="imageUrl">Image location</param>
        /// <param name="referer">Page we are coming from</param>
        /// <returns>Image request result</returns>
        private static HTTPRequestResult GetImage(string imageUrl, string referer)
        {
            string imgHost = FindStringWithDelimiters(imageUrl, "//", '/');
            if (imgHost != null)
            {
                string imgLocation = imageUrl.Substring(imageUrl.IndexOf(imgHost) + imgHost.Length);
                List<string> getVideoPicHeaders = HTTPRawRequest.GetGETHeaders(imgHost, imgLocation, referer, UserAgent);
                return HTTPRawRequest.DoRequest(YTHost, getVideoPicHeaders, HTTPRawRequest.HTTPS);
            }
            else return new HTTPRequestResult();
        }

        /// <summary>
        /// Get Channel Image from Channel Name
        /// </summary>
        /// <param name="channelName">Channel Name</param>
        /// <returns>Channel Image or null if failed to retrieve</returns>
        public static Image GetChannelImage(string channelName)
        {
            try
            {
                //Get Channel Page
                List<string> headersGetChannelPage = HTTPRawRequest.GetGETHeaders(YTHost, YTChannelUrlBase + channelName, null, UserAgent);
                HTTPRequestResult result = HTTPRawRequest.DoRequest(YTHost, headersGetChannelPage, HTTPRawRequest.HTTPS);

                if (result.Successfull)
                {
                    //Get Channel Image
                    string referer = GetChannelUrl(channelName);
                    string imageLocation = FindStringWithDelimiters(result.BodyAsString, DelimiterChannelImage, '"');
                    result = GetImage(imageLocation, referer);

                    if (result.Successfull)
                        return Image.FromStream(new MemoryStream(result.Body, false));
                }
            }
            catch (IOException) { }

            return null;
        }

        /// <summary>
        /// Get Channel Videos from Channel Name
        /// </summary>
        /// <param name="channelName">Channel Name</param>
        /// <param name="videoCache">Video Cache for speeding up retrieval - only new videos will be added at the beginning of the list</param>
        /// <param name="updateListener">Listener to send download updates to - amount of successfully processed videos will be sent there</param>
        /// <returns>A list of video identifiers, newest first, from the specified channel or null if failed to retrieve</returns>
        public static List<String> GetChannelVideos(string channelName, List<string> videoCache = null, IYTStatusListener updateListener = null)
        {
            string videosPageResource = YTChannelUrlBase + channelName + YTChannelVideosPage;
            Dictionary<string, string> cookies = new Dictionary<string, string>();

            try
            {
                //Get Video Page
                List<string> getVideoPageHeaders = HTTPRawRequest.GetGETHeaders(YTHost, videosPageResource, null, UserAgent);
                HTTPRequestResult result = HTTPRawRequest.DoRequest(YTHost, getVideoPageHeaders, HTTPRawRequest.HTTPS);
                Thread.Sleep(new Random().Next(500, 800)); //Avoid spamming YouTube servers, wait a bit like an human

                if (result.Successfull)
                {
                    //Prepare Json Data Request Loop
                    List<string> videos = new List<string>();
                    string videoHtmlCode = result.BodyAsString;
                    string nextQueryToParse = videoHtmlCode;
                    string nextQueryUrl = null;

                    while (true)
                    {
                        //Process Cookies from previous request
                        foreach (var cookie in result.NewCookies)
                            cookies[cookie.Key] = cookie.Value;

                        //Process Videos from previous request
                        foreach (string elem in SplitStringWithDelimiter(videoHtmlCode, YTVideoIdParam))
                        {
                            if (elem.StartsWith("="))
                            {
                                string videoID = elem.Substring(1).Split('"')[0];
                                
                                //Reached already cached videos?
                                if (videoCache != null && videoCache.Contains(videoID))
                                {
                                    videos.AddRange(videoCache);
                                    if (updateListener != null)
                                        updateListener.UpdateYTDLStatus(channelName, videos, YTEventLevel.Info, YTDLStatus.FinishedOK);
                                    return videos;
                                }

                                //Add to list of fetched videos
                                if (!videos.Contains(videoID))
                                    videos.Add(videoID);
                            }
                        }

                        //Retrieve all Video Pics from previous request like an actual web browser would
                        foreach (string videoHtml in SplitStringWithDelimiter(videoHtmlCode, DelimiterVideoImageRegion))
                            if (videoHtml.Contains(DelimiterVideoImageLink))
                                GetImage(FindStringWithDelimiters(videoHtml, DelimiterVideoImageLink, '"'), YTBaseUrl + videosPageResource);

                        //Update progress callback
                        if (updateListener != null)
                            updateListener.UpdateYTDLStatus(channelName, videos, YTEventLevel.Info, YTDLStatus.Downloading);

                        //Retrieve Url for next query
                        nextQueryUrl = FindStringWithDelimiters(nextQueryToParse, DelimeterAjaxMoreVideos, '"');
                        if (String.IsNullOrEmpty(nextQueryUrl))
                            break;

                        //Make new query
                        List<string> getMoreVideosHeaders = HTTPRawRequest.GetGETHeaders(YTHost, nextQueryUrl, YTBaseUrl + videosPageResource, UserAgent, cookies);
                        result = HTTPRawRequest.DoRequest(YTHost, getMoreVideosHeaders, HTTPRawRequest.HTTPS);
                        Thread.Sleep(new Random().Next(500, 800)); //Wait a small amount of time like an human would

                        //Process new query result
                        if (result.Successfull)
                        {
                            Json.JSONData dataJson = Json.ParseJson(result.BodyAsString);
                            if (dataJson.Type == Json.JSONData.DataType.Object)
                            {
                                //Retrieve HTML code with video details
                                if (dataJson.Properties.ContainsKey(JsonMoreVideosField)
                                    && dataJson.Properties[JsonMoreVideosField].Type == Json.JSONData.DataType.String)
                                {
                                    videoHtmlCode = dataJson.Properties[JsonMoreVideosField].StringValue;
                                }
                                else break;

                                //Retrieve HTML code with Url for next query
                                if (dataJson.Properties.ContainsKey(JsonMoreAjaxField)
                                    && dataJson.Properties[JsonMoreAjaxField].Type == Json.JSONData.DataType.String)
                                {
                                    nextQueryToParse = dataJson.Properties[JsonMoreAjaxField].StringValue;
                                }
                                else break;
                            }
                            else
                            {
                                if (updateListener != null)
                                    updateListener.UpdateYTDLStatus(channelName, videos, YTEventLevel.Warning, YTDLStatus.InvalidData);
                                break;
                            }
                        }
                        else
                        {
                            if (updateListener != null)
                                updateListener.UpdateYTDLStatus(channelName, videos, YTEventLevel.Warning, YTDLStatus.RequestFailed);
                            break;
                        }
                    }
                    
                    if (updateListener != null)
                        updateListener.UpdateYTDLStatus(channelName, videos, YTEventLevel.Info, YTDLStatus.FinishedOK);

                    return videos;
                }
                else updateListener.UpdateYTDLStatus(channelName, new List<string>(), YTEventLevel.Error, YTDLStatus.RequestFailed);
            }
            catch (IOException)
            {
                if (updateListener != null)
                    updateListener.UpdateYTDLStatus(channelName, new List<string>(), YTEventLevel.Error, YTDLStatus.RequestFailed);
            }
            catch (NullReferenceException)
            {
                if (updateListener != null)
                    updateListener.UpdateYTDLStatus(channelName, new List<string>(), YTEventLevel.Error, YTDLStatus.InvalidData);
            }

            return new List<string>();
        }
    }

    /// <summary>
    /// Describe the severity of an event
    /// </summary>
    public enum YTEventLevel { Info, Warning, Error };

    /// <summary>
    /// Describe the status of the download operation
    /// </summary>
    public enum YTDLStatus { Downloading, FinishedOK, RequestFailed, InvalidData }

    /// <summary>
    /// Interface for receiving download status updates
    /// </summary>
    public interface IYTStatusListener
    {
        /// <summary>
        /// Download Status Update Event
        /// </summary>
        /// <param name="channelName">Name of the YouTube channel</param>
        /// <param name="videos">Videos that have been retrieved so far</param>
        /// <param name="level">Event level (normal, warning, error)</param>
        /// <param name="status">Event status (downloading, finished with error, finished without error)</param>
        void UpdateYTDLStatus(string channelName, List<string> videos, YTEventLevel level, YTDLStatus status);
    }
}