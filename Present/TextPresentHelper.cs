using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Framework;

namespace Present
{
    public class TextPresentHelper: PresentHelper
    {
        private static TextPresentHelper helper = null;
        private TextPresentHelper() { }
        public static TextPresentHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new TextPresentHelper();
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
            foreach (var series in seriesList)
            {
                foreach(var platform in platforms)
                {
                    upperPlatform = platform.Substring(0, 1).ToUpper() + platform.Substring(1);
                    countList = (IEnumerable<ICountItem>)SeekResult(platform, series, countHelperTypeNameSpaceString + "." + upperPlatform + countHelperTypeStringSuffix, countHelperDependences, nameSwitcher, countHelperGetInstance, getSeriesSeeds, getCount, countHelperGetInstanceParameters, getCountParametersNoSeeds);
                    if (countList.Count() == 0)
                    {
                        //there is no series on that platform.
                        Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                        Console.WriteLine();
                        continue;
                    }
                    string fileName = System.Environment.CurrentDirectory + @"\txt\" + series + "_" + nameSwitcher.NameSwitch(platform) + "_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".txt";
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        try
                        {
                            sw.Write(nameSwitcher.NameSwitch(platform) + "  ");
                            sw.WriteLine(series + "  播放量（亿）:");

                            foreach (var countItem in countList)
                            {
                                sw.Write(System.Environment.NewLine);
                                string title = countItem.Title != "" ? countItem.Title : "全集";
                                sw.Write(title + " : ");
                                if (countItem.Count != "0" | countItem.Count != "" | countItem.Count != null)
                                {
                                    string viewCount = (Math.Round((double.Parse(countItem.Count) / 1000000), MidpointRounding.AwayFromZero) / 100).ToString();
                                    sw.Write(viewCount);
                                }
                                else
                                {
                                    sw.Write(0);
                                }
                                sw.WriteLine("          key: " + countItem.Key);
                            }
                            sw.Write(System.Environment.NewLine);
                            sw.Write(System.Environment.NewLine);
                            sw.Close();
                            Console.WriteLine("{0} {1} 播放量数据搜索成功，已记录在文件中!", nameSwitcher.NameSwitch(platform), series);
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            log.AppendLine("error: " + ex.Message);
                            log.AppendLine(platform + "  " + series + "  streamWriter error");
                        }
                    }
                }
            }
            return log;
        }
    }
}
