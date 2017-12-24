using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependenceLib
{
    public class QueryDependenceItem: IQueryDependenceItem
    {
        public string Dependence { get; set; }
        public string LibDir { get; set; }
        public QueryDependenceItem() { }
        public QueryDependenceItem(string dp, string libDir)
        {
            Dependence = dp;
            LibDir = libDir;
        }
    }
}
