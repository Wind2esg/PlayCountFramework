using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    public class SohuDefaultCountHelper : CountHelper
    {

        private static SohuDefaultCountHelper helper = null;
        private SohuDefaultCountHelper()
        {
        }
        public static SohuDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new SohuDefaultCountHelper();
            }
            return helper;
        }
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://tv.sohu.com/";
            string regPattern = @"var PLAYLIST_ID\D*(\d+)";
            string urlPattern2 = "https://count.vrs.sohu.com/count/query_Album.action?albumId=";
            string regPattern2 = @"count\D*(\d+)";
            CountItem countItem;
            foreach (var seekItem in seekItemList)
            {
                //get PLAYLIST_ID
                countItem = GetPlayCount(urlPattern + seekItem.Key, regPattern, seekItem, (x => x));
                //get play count
                countList.Add(GetPlayCount(urlPattern2 + countItem.Count, regPattern2, seekItem, (x => x)));
            }
            return countList;
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("sohu", series);
        }
    }
}
