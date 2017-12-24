using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Framework;

namespace Present
{
    public class HtmlPresentHelper : PresentHelper
    {
        private static HtmlPresentHelper helper = null;
        private HtmlPresentHelper() { }
        public static HtmlPresentHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new HtmlPresentHelper();
            }
            return helper;
        }
        protected override object Present(ICountResult countResult, NameSwitcher nameSwitcher)
        {
            StringBuilder html = new StringBuilder();

            string platformZh = nameSwitcher.NameSwitch(countResult.Platform);
            if (countResult.CountList.Count() == 0)
            {
                //We werent able to get the playcount result, because the platform didnt have the resource. so it return "". Exception comes when we dont get the data from the response.
                return "";
            }

            html.AppendLine(@"<div class='row'>&nbsp;</div>");
            html.AppendLine(@"<div class='row'>&nbsp;</div>");
            html.AppendLine(@"<div class='row'>&nbsp;</div>");
            html.AppendLine(@"<div class='row'>");
            html.AppendLine(@"<div class='col-xs-1'></div>");
            html.AppendLine(@"<button class='col-xs-10 btn btn-success btn-lg active'>");
            html.AppendLine(platformZh + "&nbsp;&nbsp;&nbsp;&nbsp;" + countResult.Series + "&nbsp;&nbsp;&nbsp;&nbsp;播放量(亿)</button>");
            html.AppendLine(@"<div class='col-xs-1'></div>");
            html.AppendLine(" </div>");
            
            foreach(var countItem in countResult.CountList)
            {
                html.AppendLine(@"<div class='row'>");
                html.AppendLine(@"<div class='col-xs-1'></div>");
                html.AppendLine(@"<button class='col-xs-4 btn btn-default btn-lg' >");
                html.AppendLine(countItem.Title + "</button>");
                html.AppendLine(@"<button class='col-xs-1 btn btn-default btn-lg singal' >&nbsp</button>");
                html.AppendLine(@"<button class='col-xs-5 btn btn-info btn-lg' data-clipboard-text=");
                //count unit format
                string viewCount = (Math.Round((double.Parse(countItem.Count) / 1000000), MidpointRounding.AwayFromZero) / 100).ToString();
                html.AppendLine("'" + viewCount + "'>" + viewCount + "</button>");
                html.AppendLine(@"<div class='col-xs-1'></div>");
                html.AppendLine(" </div>");
            }
            html.AppendLine("<div class='clearfix'></div>");

            return html.ToString();
        }
    }
}
