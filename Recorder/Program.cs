using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.IO;
namespace Recorder
{
    public class Program
    {
        public static StringBuilder Show(string[] platforms, string[] seriesList, string presentHelperType)
        {
            return (StringBuilder)PlayCountFramework.GetPresent(presentHelperType, new string[1] { "Present" }, new object[11] { platforms, seriesList, "DefaultCountHelper", "DefaultSeek", new string[1] { "DefaultSeek" }, new NameSwitcher(), "GetInstance", "GetSeriesSeeds", "GetCount", null, null });
        }
        static void Main(string[] args)
        {
            string[] platforms = { "iqiyi", "tencent", "youku", "sohu", "pptv", "letv" };
            string[] seriesList = { "海底小纵队", "小猪佩奇", "汪汪队立大功", "小马宝莉", "嗨道奇", "全球探险冲冲冲" };

            string[] seriesList2 = { "海底小纵队"};

            Console.WriteLine("本程序支持产生三种结果文件格式，请输入想要得到的文件格式:");
            Console.WriteLine();
            Console.WriteLine("1 txt: 某平台某专辑的播放数据记录在独立的txt中，并且能查看搜索key");
            Console.WriteLine("2 html: 所有数据整合在一个html文件中，提供点击即复制数据");
            Console.WriteLine("3 xlsx: 将数据记录在xlsx文件中");
            Console.WriteLine("9 退出");
            while (true)
            {
                StringBuilder log = new StringBuilder();
                Console.WriteLine();
                Console.WriteLine("请输入相应的数字:(1 xlsx, 2 html, 3 txt, 9 退出)");
                var input = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                switch (input.KeyChar.ToString())
                {
                    case "1":
                        log = Show(platforms, seriesList, "Present.XlsxPresentHelper");
                        break;
                    case "2":
                        log = Show(platforms, seriesList, "Present.HtmlPresentHelper");
                        break;
                    case "3":
                        log = Show(platforms, seriesList, "Present.TextPresentHelper");
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("请输入正确的操作!");
                        continue;
                }
                Console.WriteLine();
                if (log.Length == 0)
                {
                    Console.WriteLine("数据搜索完毕，按回车退出");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(log);
                    Console.WriteLine();
                    try
                    {
                        using (StreamWriter sw = File.CreateText("./log/" + "Log_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".txt"))
                        {
                            sw.WriteLine(log.ToString());
                            sw.Close();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("生成日志失败");
                    }
                }
            }
        }
    }
}
