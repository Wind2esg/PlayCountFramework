using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface ISeekItem
    {
        string Series { get; set; }
        string Title { get; set; }
        string Key { get; set; }
    }
}
