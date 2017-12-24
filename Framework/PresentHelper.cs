using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class PresentHelper
    {   
        protected abstract object Present(ICountResult countResult, NameSwitcher nameSwitcher);
        public object GetPresent(ICountResult countResult, NameSwitcher nameSwitcher)
        {
            return Present(countResult, nameSwitcher);
        }
    }
}
