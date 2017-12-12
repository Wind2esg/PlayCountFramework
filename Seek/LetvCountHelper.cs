using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class LetvCountHelper: CountHelper
    {
        private static LetvCountHelper helper = null;
        private LetvCountHelper()
        {

        }
        public static LetvCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new LetvCountHelper();
            }
            return helper;
        }
        //letv helper
        //tips: jsonp api, plist_play_count
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://v.stat.letv.com/vplay/queryMmsTotalPCount?pid=";
            string regPattern = @"plist_play_count"":(\d+)";
            string targetUrl = null;
            CountItem countItem;

            foreach (var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;
                countItem.Key = seekItem.Key;
                if (seekItem.Key != "0")
                {
                    targetUrl = urlPattern + seekItem.Key;
                    countItem.Count = GetPlayCount(GetResponseHtml(targetUrl), regPattern);
                }
                else
                {
                    countItem.Count = "0";
                }
                countList.Add(countItem);
            }
            return countList;
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://so.le.com/s?wd=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"href=""http://www.le.com/[^/]+/(?<key>\d+).html";
            string regSeries = @"[^" + series + @"]+" + series + @"(?<title>[^""]*)""";
            string reg = regPattern + regSeries;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
