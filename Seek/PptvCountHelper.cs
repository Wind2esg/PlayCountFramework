using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class PptvCountHelper: CountHelper
    {
        private static PptvCountHelper helper = null;
        private PptvCountHelper()
        {

        }
        public static PptvCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new PptvCountHelper();
            }
            return helper;
        }
        //pptv helper
        //like Youku, we can get the playCount data from the search result page
        //tips: the data's format is like "123,23". remove ","
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<ICountItem> countList = new List<ICountItem>();
            CountItem countItem = null;

            foreach (var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;
                countItem.Count = (double.Parse(seekItem.Key.Replace(",", ""))).ToString();
                countItem.Key = seekItem.Key;
                countList.Add(countItem);
            }
            return countList;

        }

        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://search.pptv.com/s_video?kw=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"[^播放]+?播放\D+(?<key>[\d,]+)";
            string regSeries = @"title=""" + series + @"(?<title>[^""]*)"">";
            string reg = regSeries + regPattern;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
