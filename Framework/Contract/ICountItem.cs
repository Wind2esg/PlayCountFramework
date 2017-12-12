using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface ICountItem
    {
        string Title { get; }
        string Count { get;}
        //for test
        string Key { get; }
    }
}
