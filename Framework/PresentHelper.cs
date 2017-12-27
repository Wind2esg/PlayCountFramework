using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class PresentHelper
    {
        protected object SeekResult(string platform, string series,
            string countHelperTypeString, string[] countHelperDirTokens, NameSwitcher nameSwitcher,
            string countHelperGetInstance,
            string getSeriesSeeds,
            string getCount,
            object[] countHelperGetInstanceParameters,
            object[] getCountParametersNoSeeds)
        {
            Console.WriteLine("开始在 {0} 上搜索 {1} 的播放量", nameSwitcher.NameSwitch(platform), series);

            return PlayCountFramework.GetResult(
                countHelperTypeString,
                countHelperDirTokens,
                new object[1] { series },
                countHelperGetInstance,
                countHelperGetInstanceParameters,
                getSeriesSeeds,
                getCount,
                getCountParametersNoSeeds);
        }
        protected virtual object Present(string[] platforms, string[] seriesList, 
            string countHelperTypeString, string countHelperTypeNameSpaceString, 
            string[] countHelperDependences, NameSwitcher nameSwitcher,
            string countHelperGetInstance,
            string getSeriesSeeds,
            string getCount,
            object[] countHelperGetInstanceParameters,
            object[] getCountParametersNoSeeds)
        {
            return "Base";
        }
        public object GetPresent(string[] platforms, string[] seriesList,
            string countHelperTypeStringSuffix, string countHelperTypeNameSpaceString, 
            string[] countHelperDependences, NameSwitcher nameSwitcher,
            string countHelperGetInstance = "GetInstance",
            string getSeriesSeeds = "GetSeriesSeeds",
            string getCount = "GetCount",
            object[] countHelperGetInstanceParameters = null,
            object[] getCountParametersNoSeeds = null)
        {
            return Present(platforms, seriesList,
                countHelperTypeStringSuffix, countHelperTypeNameSpaceString, 
                countHelperDependences, nameSwitcher, 
                countHelperGetInstance,
                getSeriesSeeds,
                getCount,
                countHelperGetInstanceParameters,
                getCountParametersNoSeeds);
        }
    }
}
