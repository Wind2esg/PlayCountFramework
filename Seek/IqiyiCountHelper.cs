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
            string regPattern = @"playCount\D*(\d+\b)?";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key, regPattern, seekItem, (x => x)));
            }
            return countList;
        }

        //responseHtml: data-widget-searchlist-tvid="@seekItem.Key"......title="@series"
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://so.iqiyi.com/so/q_";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"\s+?data-widget-searchlist-tvid=""(?<key>\d+)";
            string regSeries = series + @"(?<title>[^""]*?)""[^<]*";
            string reg = regSeries + regPattern;

            return GetSeeds(responseHtml, reg, series, System.Text.RegularExpressions.RegexOptions.RightToLeft);
        }
    }
}
