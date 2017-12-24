using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

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
            string urlPattern = "http://list.youku.com/show/id_";
            string regPattern = @"总播放\D+([\d,]+)";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key + ".html", regPattern, seekItem, (x => x.Replace(",", String.Empty))));
            }
            return countList;
        }
        //the method is based on the search engine in the platform, however, the results arent stable. As a result we have to change it.
        //protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        //{
        //    List<CountItem> countList = new List<CountItem>();
        //    string urlPattern = "http://www.soku.com/search_video/q_";
        //    string regPattern = @"\Wtype=youku[\S\s]+?([\d.]+)<\b?";
        //    string targetUrl = urlPattern + seekItemList.First<ISeekItem>().Series;
        //    string responseHtml = GetResponseHtml(targetUrl);
        //    CountItem countItem;

        //    foreach (var seekItem in seekItemList)
        //    {
        //        countItem = new CountItem();
        //        countItem.Title = seekItem.Title;
        //        countItem.Key = seekItem.Key;

        //        double countDouble = 0;
        //        if (double.TryParse(GetPlayCount(responseHtml, seekItem.Key + regPattern), out countDouble))
        //        {
        //            countItem.Count = (countDouble * 10000).ToString();
        //        }
        //        else
        //        {
        //            countItem.Count = "优酷搜索引擎关键字 " + seekItem.Series + " 的搜索结果已发生变化，无法获得数据，需到优酷搜索";
        //        }
        //        countList.Add(countItem);
        //    }
        //    return countList;
        //}
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("youku", series);
        }
    }
}
