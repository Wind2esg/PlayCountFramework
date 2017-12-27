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
        protected override object Present(string[] platforms, string[] seriesList,
            string countHelperTypeStringSuffix, string countHelperTypeNameSpaceString, 
            string[] countHelperDependences, NameSwitcher nameSwitcher,
            string countHelperGetInstance,
            string getSeriesSeeds,
            string getCount,
            object[] countHelperGetInstanceParameters,
            object[] getCountParametersNoSeeds)
        {
            StringBuilder log = new StringBuilder();
            string upperPlatform;
            IEnumerable<ICountItem> countList;
            string fileName = System.Environment.CurrentDirectory + @"\html\" + "PlayCount_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".html";
            using (StreamWriter sw = File.CreateText(fileName))
            {
                try
                {
                    sw.WriteLine("<html>");
                    sw.WriteLine("<head>");
                    sw.WriteLine(@"<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
                    sw.WriteLine(@"<meta name='description' content='playCount Data!'>");
                    sw.WriteLine(@"<link rel='stylesheet' type='text/css' href='https://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css'>");
                    sw.WriteLine(@"<script type='text/javascript' src='https://cdn.bootcss.com/jquery/3.2.1/jquery.min.js'></script>");
                    sw.WriteLine(@"<script type='text/javascript' src='https://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js'></script>");
                    sw.WriteLine(@"<script type='text/javascript' src='https://cdn.jsdelivr.net/npm/clipboard@1/dist/clipboard.min.js'></script>");
                    sw.WriteLine("<style>.notextflow{overflow: hidden;text-overflow:ellipsis;}.nav{z-index:999;top:0;width:100%;position:fixed;}</style>");
                    sw.WriteLine("</head>");
                    sw.WriteLine("<body>");
                    sw.WriteLine("<div class='nav'><div class='row'>&nbsp;</div><div class='row'>&nbsp;</div><div class='container'></div></div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");

                    int count = 0;
                    foreach (var series in seriesList)
                    {
                        sw.WriteLine("<div class='container series " + "series" + count + "'>");
                        foreach (var platform in platforms)
                        {
                            upperPlatform = platform.Substring(0, 1).ToUpper() + platform.Substring(1);
                            countList = (IEnumerable<ICountItem>)SeekResult(platform, series, countHelperTypeNameSpaceString + "." + upperPlatform + countHelperTypeStringSuffix, countHelperDependences, nameSwitcher, countHelperGetInstance, getSeriesSeeds, getCount, countHelperGetInstanceParameters, getCountParametersNoSeeds);
                            StringBuilder html = new StringBuilder();
                            if (countList.Count() == 0)
                            {
                                //We werent able to get the playcount result, because the platform didnt have the resource. so it return "". Exception comes when we dont get the data from the response.
                                Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                                Console.WriteLine();
                                continue;
                            }
                            html.AppendLine(@"<div class='row'>&nbsp;</div>");
                            html.AppendLine(@"<div class='row'>&nbsp;</div>");
                            html.AppendLine(@"<div class='row'>&nbsp;</div>");
                            html.AppendLine(@"<div class='row'>");
                            html.AppendLine(@"<div class='col-xs-1'></div>");
                            html.AppendLine(@"<button class='col-xs-10 btn btn-success btn-lg active'>");
                            html.AppendLine(nameSwitcher.NameSwitch(platform) + "&nbsp;&nbsp;&nbsp;&nbsp;" + series + "&nbsp;&nbsp;&nbsp;&nbsp;播放量(亿)</button>");
                            html.AppendLine(@"<div class='col-xs-1'></div>");
                            html.AppendLine(" </div>");

                            foreach (var countItem in countList)
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
                            Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                            Console.WriteLine();
                            sw.WriteLine(html.ToString());
                        }
                        sw.WriteLine("</div>");
                        string navButton = "<button class='col-xs-2 btn btn-warning btn-lg' data-series='" + "series" + count + "'>" + series + "</button>";
                        string addNavButtonScript = "<script>$('.nav .container').append(\"" + navButton + "\")</script>";
                        sw.WriteLine(addNavButtonScript);
                        count++;
                    }
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");
                    sw.WriteLine(@"<div class='row'>&nbsp;</div>");

                    sw.WriteLine("</body>");
                    string clipboard = "var clipboard = new Clipboard('.btn-info');";
                    string clipboardSingal = "$('.btn-info').click(function(){$(this).prev().addClass('btn-danger');$(this).prev().click(function(){$(this).removeClass('btn-danger');});});";
                    string ButtonClass = "$('button').addClass('notextflow');";
                    string navButtonClick = "$('.nav .container button').click(function(){ var seriesToken = $(this).get(0).dataset.series;$('.series').addClass('hidden');$('.' + seriesToken).addClass('show');$('.' + seriesToken).removeClass('hidden');$('body').animate({scrollTop:'0'},500);});";
                    string initSeriesVisible = "$('.series').addClass('hidden');$('.series0').removeClass('hidden');$('.series0').addClass('show');";
                    sw.WriteLine("<script>" + clipboard + clipboardSingal + ButtonClass + initSeriesVisible + navButtonClick + "</script>");
                    sw.WriteLine("</html>");
                    sw.Write(System.Environment.NewLine);
                    sw.Write(System.Environment.NewLine);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    log.AppendLine("error: " + ex.Message);
                    log.AppendLine("streamWriter error");
                }
            }
            return log;
        }
    }
}
