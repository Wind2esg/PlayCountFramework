using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    public class TencentDefaultCountHelper: CountHelper
    {

        private static TencentDefaultCountHelper helper = null;
        private TencentDefaultCountHelper()
        {
        }
        public static TencentDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new TencentDefaultCountHelper();
            }
            return helper;
        }
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "https://v.qq.com/x/cover/";
            string regPattern = @"interactionCount\W+content\D+(\d+\b)";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key + ".html", regPattern, seekItem, (x => x)));
            }
            return countList;
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("tencent", series);
        }
    }
}
