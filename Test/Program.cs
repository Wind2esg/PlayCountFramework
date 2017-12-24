using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;

namespace Test
{
    class Program
    {
        public static string TestPptvCountPattern(string count, string pattern)
        {
            Match match = Regex.Match(count, pattern);
            int unit;
            if (match.Groups["unit"].ToString() != "")
            {
                unit = match.Groups["unit"].ToString() == "亿" ? 100000000 : 10000;
            }
            else
            {
                unit = 1;
            }
            return (double.Parse(match.Groups["digit"].ToString()) * unit).ToString();
        }
        static void Main(string[] args)
        {
            var request = HttpWebRequest.CreateHttp("https://www.zhihu.com/question/21060036");
            request.Headers.Add("Accept-Encoding: br");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                System.Diagnostics.Debugger.Break();
            }

                    //string countPattern = @"(?<digit>[\d\.]+)(?<unit>.*)";
                    //string count = "4.02亿";
                    //string count2 = "1119.91万";
                    //string count3 = "4321";
                    //Console.WriteLine(TestPptvCountPattern(count, countPattern));
                    //Console.WriteLine(TestPptvCountPattern(count2, countPattern));
                    //Console.WriteLine(TestPptvCountPattern(count3, countPattern));

                    //string a = @"< class=""j-baidu-a"" title=""海底小纵队 全集版"" href=""http://www.le.com/comic/10003312.html"" >";
                    //string b = @"< li class=""list_item"" data-widget-searchlist-tvname=""海底小纵队 第3季"" data-widget-searchlist-tvid=""355366300"" data-widget-searchlist-albumid=""202573801"" data-widget-searchlist-catageory=""少儿"" >";

                    //string regPattern = @"href=""http://www.le.com/[^/]+/(?<key>\d+).html";
                    //string regSeries = "海底小纵队" + @"(?<title>[^""]*)""[^>]*";


                    //var match = Regex.Match(a,  regSeries + regPattern, RegexOptions.RightToLeft);
                    //Console.WriteLine(match.Groups["title"]);
                    //Console.WriteLine(match.Groups["key"]);

                    //string regPattern2 = @"\s+?data-widget-searchlist-tvid=""(?<key>\d+)";
                    //string regSeries2 = "海底小纵队" + @"(?<title>[^""]*?)""[^<]*";
                    //match = Regex.Match(b, regSeries2 + regPattern2 , RegexOptions.RightToLeft);
                    //Console.WriteLine(match.Groups["title"]);
                    //Console.WriteLine(match.Groups["key"]);

                }
    }
}
