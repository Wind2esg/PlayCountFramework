using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class SeekItem: ISeekItem
    {
        public string Series { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public SeekItem()
        {

        }
        public SeekItem(string series, string title, string key)
        {
            Series = series;
            Title = title;
            Key = key;
        }
    }
}
