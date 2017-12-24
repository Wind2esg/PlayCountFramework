using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;
using System.Text.RegularExpressions;

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
            string urlPattern = "http://v.pptv.com/page/";
            string regPattern = @"播放\D+([^<]+)";
            CountItem countItem;
            string countPattern = @"(?<digit>[\d\.]+)(?<unit>.*)";
            foreach (var seekItem in seekItemList)
            {
                countItem = GetPlayCount(urlPattern + seekItem.Key + ".html", regPattern, seekItem, (x => x));
                //the playcount data form is like digit + unit, for example 9.11亿，100.3万，12312
                Match match = Regex.Match(countItem.Count, countPattern);
                int unit;
                if (match.Groups["unit"].ToString() != "")
                {
                    unit = match.Groups["unit"].ToString() == "亿" ? 100000000 : 10000;
                }
                else
                {
                    unit = 1;
                }
                countItem.Count = (double.Parse(match.Groups["digit"].ToString()) * unit).ToString();
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
