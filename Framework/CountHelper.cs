using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

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
                            using (var streamReader = new StreamReader(stream))
                            {
                                string responseHtml = streamReader.ReadToEnd();
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
        protected string GetPlayCount(string responseHtml, string regPattern)
        {
            var match = Regex.Match(responseHtml, regPattern);
            return match.Groups[1].ToString();
        }
        protected IEnumerable<ISeekItem> GetSeeds(string responseHtml, string regPattern, string series)
        {
            List<ISeekItem> searchSeeds = new List<ISeekItem>();
            MatchCollection matches = Regex.Matches(responseHtml, regPattern);
            if(matches.Count == 0)
            {
                Console.WriteLine("no seeds match");
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
                //Console.WriteLine("no seekItemList");
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
