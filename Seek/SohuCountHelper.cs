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
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "https://count.vrs.sohu.com/count/query_Album.action?albumId=";
            string regPattern = @"(\d+)";
            string targetUrl = null;
            CountItem countItem;

            foreach (var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;
                countItem.Key = seekItem.Key;
                if (seekItem.Key != "0")
                {
                    targetUrl = urlPattern + seekItem.Key;
                    countItem.Count = GetPlayCount(GetResponseHtml(targetUrl), regPattern);
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
            string searchUrlPattern = "http://so.tv.sohu.com/mts?wd=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"[^>]+?_s_a\D+(?<key>\d+)""";
            string regSeries = @"href=""//tv.sohu.com/s[^" + series + @"]+title=""" + series + @"(?<title>[^""]*)""";
            string reg = regSeries + regPattern;

            IEnumerable<ISeekItem> seekItemList =  GetSeeds(responseHtml, reg, series);
            if(seekItemList.Count() != 0)
            {
                return seekItemList;
            }
            
            //for sohu integrated series, for example: peppa pig
            string alumHref = @"href=""//tv.sohu.com/(?<key>s[^""]+)""[^t]*title=""" + series + @"(?<title>[^""]*)""";
            seekItemList = GetSeeds(responseHtml, alumHref, series);
            foreach(ISeekItem seekItem in seekItemList)
            {
                searchUrlPattern = "http://tv.sohu.com/";
                searchUrl = searchUrlPattern + seekItem.Key;
                responseHtml = GetResponseHtml(searchUrl);
                reg = @"PLAYLIST_ID=""([^""]+)""";
                seekItem.Key = GetPlayCount(responseHtml, reg);
            }
            return seekItemList;
        }
    }
}
