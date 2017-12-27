using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;

namespace Framework
{
    public abstract class CountHelper
    {
        public string GetResponseHtml(string targetUrl)
        {
            HttpWebRequest request = null;
            try
            {
                request = HttpWebRequest.CreateHttp(targetUrl);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Headers.Add("Accept-Encoding: gzip");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("request denied");
                return null;
            }
            try
            {
                if (request != null)
                {
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {

                        using (var stream = response.GetResponseStream())
                        {
                            using (var streamReader = new StreamReader(new GZipStream(stream, CompressionMode.Decompress)))
                            {
                                string responseHtml = streamReader.ReadToEnd();
                                streamReader.Close();
                                stream.Close();
                                response.Close();
                                return responseHtml;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("error in reponse");
                return null;
            }
            return null;

        }
        protected CountItem GetPlayCount(string targetUrl, string reg, ISeekItem seekItem, Func<string, string> countFormatter, RegexOptions regexOptions = RegexOptions.None)
        {
            CountItem countItem = new CountItem();
            countItem.Title = seekItem.Title;
            string responseHtml = GetResponseHtml(targetUrl);
            if(responseHtml != null)
            {
                Match match = Regex.Match(responseHtml, reg, regexOptions);
                if (match.Groups.Count != 1)
                {
                    countItem.Count = countFormatter(match.Groups[1].ToString());
                    countItem.Key = seekItem.Key;
                    return countItem;
                }
            }
            countItem.Count = "N/A";
            countItem.Key = targetUrl;
            return countItem;
        }
        protected IEnumerable<ISeekItem> GetSeeds(string responseHtml, string regPattern, string series, RegexOptions regexOptions = RegexOptions.None)
        {
            List<ISeekItem> searchSeeds = new List<ISeekItem>();
            MatchCollection matches = Regex.Matches(responseHtml, regPattern, regexOptions);
            if(matches.Count == 0)
            {
                //there is no series on the platform
                return searchSeeds;
            }
            ISeekItem seekItem = null;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string matchTitle = null;
            string matchKey = null;
            foreach (Match match in matches)
            {
                matchTitle = match.Groups["title"].ToString();
                matchKey = match.Groups["key"].ToString();
                if (dict.ContainsKey(matchKey))
                {
                    continue;
                }
                dict.Add(matchKey, matchTitle);
                seekItem = new SeekItem();
                seekItem.Series = series;
                seekItem.Title = matchTitle.Replace(" ", "");
                seekItem.Key = matchKey;
                searchSeeds.Add(seekItem);
            }
            return searchSeeds;
        }
        protected abstract IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList);
        protected abstract IEnumerable<ISeekItem> SeekSeriesSeeds(string series);
        public IEnumerable<ICountItem> GetCount(IEnumerable<ISeekItem> seekItemList)
        {
            if (seekItemList.Count() == 0)
            {
                //there is no series on that platform.
                return new List<ICountItem>();
            }
            return SeekCount(seekItemList);
        }
        public IEnumerable<ISeekItem> GetSeriesSeeds(string series)
        {
            return SeekSeriesSeeds(series);
        }
    }
}
