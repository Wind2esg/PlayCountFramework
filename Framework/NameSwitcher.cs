using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class NameSwitcher
    {
        protected IDictionary<string, string> NameDict { get; set; }
        public NameSwitcher()
        {
            Dictionary<string, string> nameDict = new Dictionary<string, string>();
            nameDict.Add("iqiyiDefault", "爱奇艺");
            nameDict.Add("tencentDefault", "腾讯");
            nameDict.Add("youkuDefault", "优酷");
            nameDict.Add("sohuDefault", "搜狐");
            nameDict.Add("pptvDefault", "PPTV");
            nameDict.Add("letvDefault", "乐视");
            nameDict.Add("iqiyi", "爱奇艺");
            nameDict.Add("tencent", "腾讯");
            nameDict.Add("youku", "优酷");
            nameDict.Add("sohu", "搜狐");
            nameDict.Add("pptv", "PPTV");
            nameDict.Add("letv", "乐视");
            nameDict.Add("爱奇艺", "iqiyi");
            nameDict.Add("腾讯", "tencent");
            nameDict.Add("优酷", "youku");
            nameDict.Add("搜狐", "sohu");
            nameDict.Add("Pptv", "pptv");
            nameDict.Add("乐视", "letv");
            NameDict = nameDict;
        }
        public NameSwitcher(IDictionary<string, string> nameDict )
        {
            NameDict = nameDict;
        }
        public string NameSwitch(string platformName)
        {
            if (NameDict.ContainsKey(platformName))
            {
                string switchedName = null;
                NameDict.TryGetValue(platformName, out switchedName);
                return switchedName;
            }
            Console.WriteLine("no paired platform name in NameSwitcher");
            return platformName;
        }
    }
}
