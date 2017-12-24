using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class YoukuCountHelper: CountHelper
    {
        private static YoukuCountHelper helper = null;
        private YoukuCountHelper()
        {

        }
        public static YoukuCountHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new YoukuCountHelper();
            }
            return helper;
        }
        //youku helper
        //url example http://www.soku.com/search_video/q_@seriesName
        //key response html parsed text href=\"//index.youku.com/vr_show/showid_@seekItem.Key?type=youku\"\n  target=\"_blank\">54625.3</a>万</span>\n  
        //tips: the count results can be grasped in once httprequest.
        //      using different seekItem.Key to orgnize regExpression to match the count data.
        //      however the data's format is like "123.23". parse it into "12323"
        //we update the method already. we dont get the data from the search result page because the result isnt stable
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://list.youku.com/show/id_";
            string regPattern = @"总播放\D+([\d,]+)";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key + ".html", regPattern, seekItem, (x => x.Replace(",",String.Empty))));
            }
            return countList;
        }

        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://www.soku.com/search_video/q_";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"_iku_showid\D+(?<key>[^']+)";
            string regSeries = @"_log_title='" + series + @"(?<title>[^']*)'[^<]+";
            string reg = regSeries + regPattern;

            return GetSeeds(responseHtml, reg, series, System.Text.RegularExpressions.RegexOptions.RightToLeft);
        }
    }
}
