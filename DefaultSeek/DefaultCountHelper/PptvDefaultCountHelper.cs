using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace DefaultSeek
{
    public class PptvDefaultCountHelper : CountHelper
    {

        private static PptvDefaultCountHelper helper = null;
        private PptvDefaultCountHelper()
        {
        }
        public static PptvDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new PptvDefaultCountHelper();
            }
            return helper;
        }
        //pptv helper
        //like Youku, we can get the playCount data from the search result page
        //tips: the data's format is like "123,23". remove ","
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {

            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://search.pptv.com/s_video?kw=";
            string regPattern = @"[^播放]+?播放\W+span\W+([\d,]+)?\D";
            string targetUrl = urlPattern + seekItemList.Last<ISeekItem>().Series;
            string responseHtml = GetResponseHtml(targetUrl);
            CountItem countItem;

            foreach (var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;
                countItem.Key = seekItem.Key;

                if (seekItem.Key != "0")
                {
                    string reg = @"title=""" + seekItem.Series + @"[^""]*?" +  seekItem.Key + @"[^""]*?""" + regPattern;
                    string originalData = GetPlayCount(responseHtml, reg);
                    countItem.Count = originalData.Replace(",", String.Empty);
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
            return DefaultDb.GetDefaultRepo("pptv", series);
        }
    }
}
