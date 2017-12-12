using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class PresentHelper
    {   
        protected abstract string Present(ICountResult countResult, NameSwitcher nameSwitcher);
        public string GetPresent(ICountResult countResult, NameSwitcher nameSwitcher)
        {
            return Present(countResult, nameSwitcher);
        }
    }
}
