using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Framework
{
    public static class PlayCountFramework
    {

        public static object GetSeriesSeeds(Type helperType, object helperInstance, string getSeriesSeeds, object[] parameters)
        {
            return helperType.GetMethod(getSeriesSeeds).Invoke(helperInstance, parameters);
        }
        public static object GetPlayCount(Type helperType, object helperInstance, string getCount, object[] parameters)
        {
            return helperType.GetMethod(getCount).Invoke(helperInstance, parameters);
        }
        public static Type GetHelperType(string helperTypeString, string[] helperDirTokens)
        {
            //recode if using DependenceManager
            string[] helperDir = GetHelperDir(helperDirTokens);
            Assembly assembly = Assembly.LoadFile(helperDir[helperDir.Count() - 1] + helperDirTokens[helperDir.Count() - 1] + ".dll");
            Type helperType = assembly.GetType(helperTypeString);
            for (int i = 0; i < helperDir.Count() - 1 ; i++)
            {
                Assembly.LoadFile(helperDir[i] + helperDirTokens[i] + ".dll");
            }
            return helperType;
        }
        public static object GetHelperInstance(Type helperType, string getInstance, object[] parameters)
        {
            return helperType.GetMethod(getInstance).Invoke(null, parameters);
        }
        //it should be DependenceManager's duty to get the proper helper .cs file compiled and then return the .dll's dir.
        public static string[] GetHelperDir(string[] helperDirTokens)
        {
            string[] helperDir = new string[helperDirTokens.Count()];
            for(int i = 0;  i < helperDirTokens.Count(); i++ )
            {
                //currently put all dlls in the current dir
                helperDir[i] = System.Environment.CurrentDirectory + @"\";
                //@"d:\Users\Adol\Documents\visual studio 2017\Projects\PlayCountFramework\HtmlRecorder\bin\Debug\"+ helperDirTokens[i] + @"\";
            }
            return helperDir;
        }
        public static object GetResult(string countHelperTypeString, string[] countHelperDirTokens, object[] getSeriesSeedsParameters, string getInstance = "GetInstance", object[] getInstanceParameters = null, string getSeriesSeeds = "GetSeriesSeeds", string getCount = "GetCount", object[] getCountParametersNoSeeds = null)
        {
            var helperType = GetHelperType(countHelperTypeString, countHelperDirTokens);
            var helperInstance = GetHelperInstance(helperType, getInstance, getInstanceParameters);
            var seriesSeeds = GetSeriesSeeds(helperType, helperInstance, getSeriesSeeds, getSeriesSeedsParameters);
            object[] getCountParameters;
            if (getCountParametersNoSeeds == null)
            {
                getCountParameters = new object[1] { seriesSeeds };
            }
            else
            {
                getCountParameters = new object[getCountParametersNoSeeds.Count() + 1];
                getCountParameters[0] = seriesSeeds;
                for(int i = 0; i < getCountParametersNoSeeds.Count(); i++)
                {
                    getCountParameters[i + 1] = getCountParametersNoSeeds[i];
                }
            }
            return GetPlayCount(helperType, helperInstance, getCount, getCountParameters);
        }
        //choose present style
        public static object GetPresent(string presentHelperTypeString, string[] presentHelperDirTokens, object[] getPresentParameters, string getInstance = "GetInstance", object[] getInstanceParameters = null, string getPresent = "GetPresent")
        {
            var helperType = GetHelperType(presentHelperTypeString, presentHelperDirTokens);
            var helperInstance = GetHelperInstance(helperType, getInstance, getInstanceParameters);
            return helperType.GetMethod(getPresent).Invoke(helperInstance, getPresentParameters);
        }
    }
}
