using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;
using System.IO;

namespace TextRecorder
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
            object[] presentHelperGetInstanceParameters= null, 
            object[] getCountParametersNoSeeds = null)
        {
            NameSwitcher nameSwitcher = new NameSwitcher();
            Console.WriteLine("开始搜索 {0} 平台上 {1} 的播放量", nameSwitcher.NameSwitch(platform), series);
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
        public static void Show(string[] platforms, string[] seriesList, string[] countHelperDependences, string[] presentHelperDependences, string presentHelperType = "TextDefaultPresentHelper")
        {
            NameSwitcher nameSwitcher = new NameSwitcher();
            string upperPlatform;
            foreach(var platform in platforms)
            {
                upperPlatform = platform.Substring(0, 1).ToUpper() + platform.Substring(1);
                foreach(var series in seriesList)
                {
                    var result = TellMeResult(
                        platform, series, 
                        "DefaultSeek." + upperPlatform + "DefaultCountHelper", 
                        countHelperDependences, "DefaultSeek." + presentHelperType, 
                        presentHelperDependences);
                    if((bool)result == true)
                    {
                        Console.WriteLine("{0} {1} 播放量数据搜索成功，已记录在文件中!", nameSwitcher.NameSwitch(platform), series);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("{0} {1} 播放量数据搜索失败!", nameSwitcher.NameSwitch(platform), series);
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("数据搜索完毕，按回车退出");
        }
        public static void Main(string[] args)
        {
            string[] platforms = { "iqiyi", "youku" , "sohu", "pptv", "letv" };
            string[] seriesList = { "海底小纵队", "小猪佩奇", "汪汪队立大功", "小马宝莉", "嗨道奇", "全球探险冲冲冲" };

            string[] platforms2 = { "tencent" };
            string[] seriesList2 = { "海底小纵队", "小猪佩奇", "汪汪队立大功", "小马宝莉", "嗨道奇", "全球探险冲冲冲" };

            while (true)
            {
                Console.WriteLine("请输入操作，选择要抓取的平台 1 爱奇艺，优酷，搜狐，PPTV，乐视 2 腾讯 9 退出");
                ConsoleKeyInfo input = Console.ReadKey();
                Console.WriteLine();
                switch (input.KeyChar.ToString())
                {
                    case "1":
                        Console.WriteLine("选择抓取爱奇艺，优酷，搜狐，PPTV，乐视的数据");
                        Show(platforms, seriesList, new string[2] { "Seek", "DefaultSeek" }, new string[2] { "Present", "DefaultSeek" });
                        break;
                    case "2":
                        Console.WriteLine("选择抓取腾讯的数据");

                        Show(platforms2, seriesList2, new string[2] { "Seek", "DefaultSeek" }, new string[2] { "Present", "DefaultSeek" });
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("请输入正确的操作");
                        break;
                }

            }
            //

            //DependenceManger.Init();
            //DependenceManger.DownloadDependenceFileAndCompile(
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\PlayCountFramework\TextRecorder\bin\Debug\",
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\PlayCountFramework\TextRecorder\bin\Debug\vendor\",
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\PlayCountFramework\TextRecorder\bin\Debug\vendor\",
            //    false);

            //DependenceManger.PrintLog();
        }
    }
}
