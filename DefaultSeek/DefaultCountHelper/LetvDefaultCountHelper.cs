using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    public class LetvDefaultCountHelper : CountHelper
    {

        private static LetvDefaultCountHelper helper = null;
        private LetvDefaultCountHelper()
        {
        }
        public static LetvDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new LetvDefaultCountHelper();
            }
            return helper;
        }
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://v.stat.letv.com/vplay/queryMmsTotalPCount?pid=";
            string regPattern = @"plist_play_count\D*(\d+)";

            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key, regPattern, seekItem, (x => x)));
            }
            return countList;
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("letv", series);
        }
    }
}
