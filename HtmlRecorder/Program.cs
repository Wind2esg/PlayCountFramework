using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;
using System.IO;

namespace HtmlRecorder
{
    public class Program
    {
        public static object TellMeResult(string platform, string series,
            string countHelperTypeString, string[] countHelperDirTokens,
            string presentHelperTypeString, string[] presentHelperDirTokens,
            string countHelperGetInstance = "GetInstance",
            string getSeriesSeeds = "GetSeriesSeeds",
            string getCount = "GetCount",
            string presentHelperGetInstance = "GetInstance",
            string getPresent = "GetPresent",
            object[] countHelperGetInstanceParameters = null,
            object[] presentHelperGetInstanceParameters = null,
            object[] getCountParametersNoSeeds = null)
        {
            NameSwitcher nameSwitcher = new NameSwitcher();
            Console.WriteLine("开始在 {0} 上搜索 {1} 的播放量", nameSwitcher.NameSwitch(platform), series);
            CountResult countResult = new CountResult();
            countResult.Platform = platform;
            countResult.Series = series;

            var countList = PlayCountFramework.GetResult(
                countHelperTypeString,
                countHelperDirTokens,
                countHelperGetInstance,
                countHelperGetInstanceParameters,
                getSeriesSeeds,
                new object[1] { series },
                getCount);

            countResult.CountList = (IEnumerable<ICountItem>)countList;
            var result = PlayCountFramework.GetPresent(
                presentHelperTypeString,
                presentHelperDirTokens,
                presentHelperGetInstance,
                presentHelperGetInstanceParameters,
                getPresent,
                new object[2] { countResult, nameSwitcher });
            return result;
        }
        public static void Show(string[] platforms, string[] seriesList, string[] countHelperDependences, string[] presentHelperDependences, string presentHelperType = "HtmlDefaultPresentHelper")
        {
            NameSwitcher nameSwitcher = new NameSwitcher();
            string upperPlatform;
            
            string fileName = "PlayCount_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".html";
            using (StreamWriter sw = File.CreateText(fileName))
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

                        var result = TellMeResult(
                            platform, series,
                            "DefaultSeek." + upperPlatform + "DefaultCountHelper",
                            countHelperDependences, "DefaultSeek." + presentHelperType,
                            presentHelperDependences);
                        Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                        Console.WriteLine();
                        sw.WriteLine(result);
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
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("数据搜索完毕，打开文件夹中开头为PlayCount的文件");
        }
        public static void Main(string[] args)
        {
            string[] platforms = { "iqiyi", "tencent",  "youku", "sohu", "pptv", "letv" };
            string[] seriesList = { "海底小纵队", "小猪佩奇", "汪汪队立大功", "小马宝莉", "嗨道奇", "全球探险冲冲冲" };
            string[] seriesList2 = { "海底小纵队" };
            string[] platforms2 = { "tencent" };
            string[] platforms3 = { "iqiyi", "tencent", "youku" };
            Show(platforms, seriesList, new string[1] { "DefaultSeek" }, new string[2] { "Present", "DefaultSeek" });
            Console.WriteLine("按回车退出");
            Console.Read();
        }
    }
}
