using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class CountResult: ICountResult
    {
        public string Platform { get; set; }
        public string Series { get; set; }
        public IEnumerable<ICountItem> CountList { get; set; }
        public CountResult() { }
        public CountResult(string platform, string series)
        {
            Platform = platform;
            Series = series;
        }

    }
}
