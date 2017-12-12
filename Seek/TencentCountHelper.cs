using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Seek
{
    public class TencentCountHelper: CountHelper
    {
        private static TencentCountHelper helper = null;
        private TencentCountHelper()
        {

        }
        public static TencentCountHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new TencentCountHelper();
            }
            return helper;
        }

        //tencent helper
        //url example https://v.qq.com/x/cover/@seekItem.Key.html
        //tips: it seemed that the data is written into frontend from backend. 
        //      when you enter url the series's id + .html, it will lead to the 1 episode of the series.
        //      in the response html, the count result can be reached in <meta> tags like <mete interactionCount="..." content="..."> 
        //      or get the result in json data which is like "var COVER_INFO={...}"
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            
            List<CountItem> countList = new List<CountItem>();
            string urlPattern = "https://v.qq.com/x/cover/";
            string regPattern = @"interactionCount\W+content\D\D(\d+\b)";
            string targetUrl = null;
            CountItem countItem;
            string responseHtml = null;
            foreach(var seekItem in seekItemList)
            {
                countItem = new CountItem();
                countItem.Title = seekItem.Title;
                countItem.Key = seekItem.Key;
                if (seekItem.Key != "0")
                {
                    targetUrl = urlPattern + seekItem.Key + ".html";
                    responseHtml = GetResponseHtml(targetUrl);
                    countItem.Count = GetPlayCount(responseHtml, regPattern);
                    if(countItem.Count == "")
                    {
                        countItem.Count = "0";
                    }
                    System.Threading.Thread.Sleep(1000);
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
            string searchUrlPattern = "https://v.qq.com/x/search/?q=";
            string searchUrl = searchUrlPattern + series;
            string responseHtml = GetResponseHtml(searchUrl);

            string regPattern = @"href=""http://v.qq.com/detail/./(?<key>[\w\d]+).html""";
            string regSeries = @"[^" + series + @"]+title=""" + series + @"(?<title>[\S\s]*?)""";
            string reg = regPattern + regSeries;

            return GetSeeds(responseHtml, reg, series);
        }
    }
}
