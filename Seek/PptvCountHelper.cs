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
        //we update the method already. we dont get the data from the search result page because the result isnt stable
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://v.pptv.com/page/";
            string regPattern = @"播放\D+([\d\.]+)";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key + ".html", regPattern, seekItem, (x => (double.Parse(x) * 10000).ToString())));
            }
            return countList;
        }

        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://search.pptv.com/s_video?kw=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"href=""http://v.pptv.com/show/(?<key>[^\.]).html""[^>]+";
            string regSeries = series + @"(?<title>[^""]*)""";
            string reg = regSeries + regPattern;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
