using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class CountItem: ICountItem
    {
        public string Title { get; set; }
        public string Count { get; set; }
        public string Key { get; set; }
    }
}
