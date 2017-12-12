using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Reflection;

namespace DefaultSeek
{
    public class LetvDefaultCountHelper : CountHelper
    {

        private static LetvDefaultCountHelper helper = null;
        private LetvDefaultCountHelper()
        {
        }
        public static LetvDefaultCountHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new LetvDefaultCountHelper();
            }
            return helper;
        }
        protected override IEnumerable<ICountItem> SeekCount(IEnumerable<ISeekItem> seekItemList)
        {
            Assembly assembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == "Seek")
                {
                    assembly = asm;
                }
            }
            Type helperType = assembly.GetType("Seek.LetvCountHelper");
            object helperInstance = helperType.GetMethod("GetInstance").Invoke(null, null);
            return (IEnumerable<ICountItem>)helperType.GetMethod("GetCount").Invoke(helperInstance, new object[1] { seekItemList });
        }
        protected override IEnumerable<ISeekItem> SeekSeriesSeeds(string series)
        {
            return DefaultDb.GetDefaultRepo("letv", series);
        }
    }
}
