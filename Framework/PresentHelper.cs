using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class PresentHelper
    {   
        protected abstract bool Present(ICountResult countResult, NameSwitcher nameSwitcher);
        public bool GetPresent(ICountResult countResult, NameSwitcher nameSwitcher)
        {
            return Present(countResult, nameSwitcher);
        }
    }
}
