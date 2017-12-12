using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface ICountResult
    {
        string Platform { get; }
        string Series { get; }
        IEnumerable<ICountItem> CountList { get; set; }
    }
}
