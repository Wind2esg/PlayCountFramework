using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    public class IqiyiDefaultCountHelper: CountHelper
    {
        private static IqiyiDefaultCountHelper helper = null;
        private IqiyiDefaultCountHelper()
        {
        }
        public static IqiyiDefaultCountHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new IqiyiDefaultCountHelper();
            }
            return helper;
        }
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
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("iqiyi", series);
        }
    }
}
