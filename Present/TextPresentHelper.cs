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
        protected override object Present(ICountResult countResult, NameSwitcher nameSwitcher)
        {
            string platformZh = nameSwitcher.NameSwitch(countResult.Platform);
            if (countResult.CountList.Count() == 0)
            {
                return true;
                //return String.Format("error:  {1} {0}!", countResult.Series, platformZh);
            }
            string fileName = countResult.Series + "_" + platformZh + "_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".txt";
            using (StreamWriter sw = File.CreateText(fileName))
            {
                try
                {
                    sw.Write(platformZh + "  ");
                    sw.WriteLine(countResult.Series + "  播放量（亿）:");

                    foreach(var countItem in countResult.CountList)
                    {
                        sw.Write(System.Environment.NewLine);
                        string title = countItem.Title != "" ? countItem.Title : "全集";
                        sw.Write(title + " : ");
                        if (countItem.Count != "0" | countItem.Count != "" | countItem.Count != null)
                        {
                            string viewCount = (Math.Round((double.Parse(countItem.Count) / 1000000), MidpointRounding.AwayFromZero) /100).ToString();
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error: {0}", ex.Message);
                    Console.WriteLine("streamWriter error");
                    return false;
                }
            }
            return true;
        }
    }
}
