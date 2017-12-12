using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class IqiyiCountHelper: CountHelper
    {
        private static IqiyiCountHelper helper = null;
        private IqiyiCountHelper()
        {
        }
        public static IqiyiCountHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new IqiyiCountHelper();
            }
            return helper;
        }

        //iqiyi helper
        //url example http://mixer.video.iqiyi.com/jp/mixin/videos/@seekItem.Key
        //tips: it is a jsonp api. in the json data, playCount is the key, and its value is the count result;
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://mixer.video.iqiyi.com/jp/mixin/videos/";
            string regPattern = @"playCount\D\D(\d+\b)?";
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

        //responseHtml: data-widget-searchlist-tvid="@seekItem.Key"......title="@series"
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://so.iqiyi.com/so/q_";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"\s+?data-widget-searchlist-tvid=""(?<key>\d+)""[\S\s]+?";
            string regSeries = @"""" + series + @"(?<title>[\S\s]*?)""";
            string reg = regPattern + regSeries;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
