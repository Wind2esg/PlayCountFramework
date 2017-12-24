using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DependenceLib
{
    public static class DependenceLib
    {
        public static IDictionary<string, string> Lib { get; set; }
        public static void Init(IDictionary<string, string> dict = null)
        {
            if(dict != null)
            {
                Lib = dict;
            }
            else
            {
                Lib = new Dictionary<string, string>();
                string libDir = @"d:\Users\Adol\Documents\visual studio 2017\Projects\CShapReview\TextRecorder\bin\Debug\lib\";
                AddToLib("CountItem", libDir);
                AddToLib("CountResult", libDir);
                AddToLib("ICountItem", libDir);
                AddToLib("ICountResult", libDir);
                AddToLib("ISeekItem", libDir);
                AddToLib("SeekItem", libDir);
                AddToLib("DefaultDb", libDir);
                AddToLib("DefaultPresentHelper", libDir);
                AddToLib("IqiyiDefaultCountHelper", libDir);
                AddToLib("SohuDefaultCountHelper", libDir);

            }
        }
        public static void AddToLib(KeyValuePair<string, string> keyValuePair)
        {
            Lib.Add(keyValuePair);
        }
        public static void AddToLib(string dpName, string libDir)
        {
            Lib.Add(dpName, libDir + dpName + @"\");
        }
        public static string ShowLib()
        {
            var queryList = new List<QueryDependenceItem>();
            foreach(var kvPair in Lib)
            {
                queryList.Add(new QueryDependenceItem(kvPair.Key, kvPair.Value));
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(queryList);
        }
        
        public static string SeekLib(string dp)
        {
            if (Lib.ContainsKey(dp))
            {
                return GetDir(dp);
            }
            else
            {
                return String.Format("There's no {0} in Lib", dp);
            }
        }
        public static string GetDir(string dp)
        {
            if(Lib.ContainsKey(dp) != false)
            {
                string dir = "";
                Lib.TryGetValue(dp, out dir);
                return dir;
            }
            else
            {
                return "no dir";
            }
        }
        public static string QueryDir(string requestString)
        {
            var dpList = requestString.Split('^');
            QueryDependenceItem queryDependenceItem = null;
            var queryList = new List<QueryDependenceItem>();
            foreach(string dp in dpList)
            {
                queryDependenceItem = new QueryDependenceItem();
                queryDependenceItem.Dependence = dp;
                queryDependenceItem.LibDir = GetDir(dp);
                queryList.Add(queryDependenceItem);
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(queryList);
        }
    }
}
