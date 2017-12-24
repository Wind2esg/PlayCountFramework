using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class SohuCountHelper: CountHelper
    {
        private static SohuCountHelper helper = null;
        private SohuCountHelper()
        {

        }
        public static SohuCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new SohuCountHelper();
            }
            return helper;
        }

        //sohu helper
        //url example https://count.vrs.sohu.com/count/query_Album.action?albumId=@seekItem.Key
        //tips: good sohu. it provides count api. return data is like " var count = "
        //i update the method instead of getting data from the api above.
        //grasp the url of the series then get the data from there
        //this can avoid the problem of two forms for the series, one is like Octonauts, another is Peppa Pig
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "http://tv.sohu.com/";
            string regPattern = @"播放数\D*(\d+)";
            foreach (var seekItem in seekItemList)
            {
                countList.Add(GetPlayCount(urlPattern + seekItem.Key, regPattern, seekItem, (x => x)));
            }
            return countList;
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            string searchUrlPattern = "http://so.tv.sohu.com/mts?wd=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"href=""//tv.sohu.com/(?<key>[^""]+)[^>]+";
            string regSeries = series + @"(?<title>[^""]*)""";
            string reg = regPattern + regSeries;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
