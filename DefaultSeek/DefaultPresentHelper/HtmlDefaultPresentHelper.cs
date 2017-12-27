using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    class HtmlDefaultPresentHelper : PresentHelper
    {
        private static HtmlDefaultPresentHelper helper = null;
        private HtmlDefaultPresentHelper() { }
        public static HtmlDefaultPresentHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new HtmlDefaultPresentHelper();
            }
            return helper;
        }
        protected override object Present(object countObj, NameSwitcher nameSwitcher)
        {
            var countResult = countObj as ICountResult;
            CountResult defaultCountResult = new CountResult();
            defaultCountResult.Platform = countResult.Platform;
            defaultCountResult.Series = countResult.Series;
            defaultCountResult.CountList = countResult.CountList;

            Assembly assembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == "Present")
                {
                    assembly = asm;
                }
            }
            Type helperType = assembly.GetType("Present.HtmlPresentHelper");
            object helperInstance = helperType.GetMethod("GetInstance").Invoke(null, null);
            return helperType.GetMethod("GetPresent").Invoke(helperInstance, new object[2] { defaultCountResult, nameSwitcher });
        }
    }
}
