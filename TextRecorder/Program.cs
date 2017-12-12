using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;
using DependenceManager;

using System.Web.Script.Serialization;
using System.IO;

using Microsoft.CSharp;
using System.CodeDom.Compiler;

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
                        Console.WriteLine("{0} {1} 播放量数据搜索成功，数记录在文件中!", nameSwitcher.NameSwitch(platform), series);
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
            string[] platforms = { "iqiyi", "tencent", "youku", "sohu", "pptv", "letv" };
            string[] seriesList = { "海底小纵队", "小猪佩奇", "汪汪队立大功", "小马宝莉", "嗨道奇", "全球探险冲冲冲" };

            string[] platforms2 = { "iqiyi" };
            string[] seriesList2 = { "海底小纵队" };

            Show(platforms, seriesList, new string[2] { "Seek", "DefaultSeek" }, new string[2] { "Present", "DefaultSeek" });

            //DependenceManger.Init();
            //DependenceManger.DownloadDependenceFileAndCompile(
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\CShapReview\TextRecorder\bin\Debug\",
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\CShapReview\TextRecorder\bin\Debug\vendor\",
            //    @"d:\Users\Adol\Documents\visual studio 2017\Projects\CShapReview\TextRecorder\bin\Debug\vendor\",
            //    false);

            //DependenceManger.PrintLog();

            Console.ReadLine();
        }
    }
}
