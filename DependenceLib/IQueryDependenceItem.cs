using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependenceLib
{
    public interface IQueryDependenceItem
    {
        string Dependence { get; set; }
        string LibDir { get; set; }
    }
}
