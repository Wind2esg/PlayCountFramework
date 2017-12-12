using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace DefaultSeek
{
    public class YoukuDefaultCountHelper : CountHelper
    {

        private static YoukuDefaultCountHelper helper = null;
        private YoukuDefaultCountHelper()
        {
        }
        public static YoukuDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new YoukuDefaultCountHelper();
            }
            return helper;
        }

        //youku helper
        //url example http://www.soku.com/search_video/q_@seriesName
        //key response html parsed text href=\"//index.youku.com/vr_show/showid_@seekItem.Key?type=youku\"\n  target=\"_blank\">54625.3</a>万</span>\n  
        //tips: the count results can be grasped in once httprequest.
        //      using different seekItem.Key to orgnize regExpression to match the count data.
        //      however the data's format is like "123.23". parse it into "12323"
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://www.soku.com/search_video/q_";
            string regPattern = @"\Wtype=youku[\S\s]+?([\d.]+)<\b?";
            string targetUrl = urlPattern + seekItemList.First<ISeekItem>().Series;
            string responseHtml = GetResponseHtml(targetUrl);
            CountItem countItem;

            foreach (var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;

                if (seekItem.Key != "0")
                {
                    countItem.Count = (double.Parse(GetPlayCount(responseHtml, seekItem.Key + regPattern)) * 10000).ToString();

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
            return DefaultDb.GetDefaultRepo("youku", series);
        }
    }
}
