using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using DependenceLib;

namespace DependenceManager
{
    public static class DependenceManger
    {
        public static IDictionary<string, string> LocalList { get; set; }
        public static List<string> Log { get; set; }
        public static void Init()
        {
            LocalList = new Dictionary<string, string>();
            Log = new List<string>();
            DependenceLib.DependenceLib.Init();
        }
        public static void PrintLog()
        {
            if(Log.Count != 0)
            {
                foreach (var LocalItem in LocalList)
                {
                    Console.WriteLine(LocalItem.Key + "   " + LocalItem.Value);
                }
                foreach (var logItem in Log)
                {
                    Console.WriteLine(logItem);
                }
            }
        }
        public static List<LocalDependenceItem> CheckDependence(string path)
        {
            try
            {
                string jsonString = "";
                using (StreamReader sr = File.OpenText(path + "dependence.json"))
                {
                    jsonString = sr.ReadToEnd();
                    sr.Close();
                }
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<LocalDependenceItem> dpList = javaScriptSerializer.Deserialize<List<LocalDependenceItem>>(jsonString);
                return dpList;
            }
            catch (Exception ex)
            {
                Log.Add(String.Format("path: {0} +  erroe: {1}", path, ex.Message));
                return null;
            } 
        }
        //Currently we hard code this for test. Replace this by a web api, wcf or something else.
        public static IEnumerable<QueryDependenceItem> GetDownloadPath(IEnumerable<LocalDependenceItem> dpList)
        {
            StringBuilder requstString = new StringBuilder();
            foreach(var dpItem in dpList)
            {
                requstString.Append(dpItem.Dependence + "^");
            }
            requstString.Remove(requstString.Length - 1, 1);
            try
            {
                string ResponseJsonString = DependenceLib.DependenceLib.QueryDir(requstString.ToString());
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Deserialize<List<QueryDependenceItem>>(ResponseJsonString);
            }
            catch (Exception ex)
            {
                Log.Add(String.Format("GetDownLoadPath error: {0}", ex.Message));
                return null;
            }

        }
        public static bool DownloadFile(string dpName, string downloadSourceDir, string downloadTargetDir)
        {
            Console.WriteLine("{0} starts to download", dpName);

            string targetFile = "";
            string sourceFile = "";
            //Currently we download file from local.
            try
            {
                DirectoryInfo folder = new DirectoryInfo(downloadSourceDir);
                Directory.CreateDirectory(downloadTargetDir);
                var files = folder.GetFiles("*.cs");
                foreach(var fileInfo in files)
                {
                    sourceFile = fileInfo.FullName;
                    targetFile = downloadTargetDir + fileInfo.Name;
                    File.Copy(sourceFile, targetFile, true);
                }
            }
            catch (Exception ex)
            {
                Log.Add(String.Format("download .cs file {0}  fail. error:  {1}", sourceFile, ex.Message));
                return false;
            }
            try
            {
                sourceFile = downloadSourceDir + "dependence.json";
                Directory.CreateDirectory(downloadTargetDir);
                targetFile = downloadTargetDir + "dependence.json";
                File.Copy(sourceFile, targetFile, true);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("{0} has no dependence. {0} downloaded", dpName);
                return true;
            }
            catch (Exception ex)
            {
                Log.Add(String.Format("download .json file {0} fail. error:  {1}", sourceFile, ex.Message));
                return false;
            }

            Console.WriteLine("{0} downloaded", dpName);

            return true;
        }
        public static bool CompileFile(string checkDpDir, string compileSourceRootDir, string compileTargetRootDir, IEnumerable<LocalDependenceItem> dpList = null)
        {
            DirectoryInfo folder = new DirectoryInfo(checkDpDir);
            string dpName = folder.Name;
            Console.WriteLine("{0} starts to compile", dpName);

            FileInfo[] files = folder.GetFiles("*.cs");
            string[] filesName = new string[files.Count()];
            for(int i = 0; i < files.Count(); i++)
            {
                filesName[i] = files[i].FullName;
            }
            CompilerResults result = null;
            using (var provider = new CSharpCodeProvider())
            {
                var options = new CompilerParameters();
                string dllPath = compileTargetRootDir + dpName + @"\" + dpName + ".dll";
                options.OutputAssembly = dllPath;
                string localDllPath = "";
                if (dpList != null)
                {
                    foreach (var dp in dpList)
                    {
                        LocalList.TryGetValue(dp.Dependence, out localDllPath);
                        Console.WriteLine("{0} depends on {1}", dpName, dp.Dependence);
                        options.ReferencedAssemblies.Add(localDllPath);
                    }
                }
                options.CoreAssemblyFileName = @"mscorlib.dll";
                options.ReferencedAssemblies.Add(@"System.Linq.dll");
                try
                {
                    result = provider.CompileAssemblyFromFile(options, filesName);
                    if (result.Output.Count != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        foreach (var outPut in result.Output)
                        {
                            Console.WriteLine(outPut);
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }

                }
                catch (Exception ex)
                {
                    
                    Log.Add(String.Format("{0}  compile error:  {1}", dpName, ex.Message));
                    Log.Add(ex.StackTrace);
                    return false;
                }

                provider.Dispose();
            }

            Console.WriteLine("{0} compiled", dpName);
            LocalList.Add(new KeyValuePair<string, string>(dpName, result.PathToAssembly));
            //Assembly.LoadFile(result.PathToAssembly);
            return true;
        }
        public static bool DownloadDependenceFileAndCompile(string checkDpDir, string downloadTargetRootDir, string compileTargetRootDir, bool compileCurrent = true)
        {
            DirectoryInfo folder = new DirectoryInfo(checkDpDir);
            if (!File.Exists(checkDpDir + "dependence.json"))
            {
                CompileFile(checkDpDir, downloadTargetRootDir, compileTargetRootDir);
                return true;
            }

            List<string> downloadLog = new List<string>();
            var myDpList = CheckDependence(checkDpDir);

            if(myDpList == null | myDpList.Count == 0)
            {
                return false;
            }
            List<LocalDependenceItem> compiledList = new List<LocalDependenceItem>();
            foreach(var item in myDpList)
            {
                if (LocalList.ContainsKey(item.Dependence))
                {
                    compiledList.Add(item);
                }
            }
            if(compiledList.Count != 0)
            {
                foreach(var item in compiledList)
                {
                    myDpList.Remove(item);
                }
            }
            if(myDpList.Count == 0)
            {
                Console.WriteLine("{0}'s dependences already have been compiled", folder.Name);
                CompileFile(checkDpDir, downloadTargetRootDir, compileTargetRootDir, compiledList);
                return true;
            }

            var myDpPathList = GetDownloadPath(myDpList);
            if(myDpPathList == null | myDpPathList.Count() == 0)
            {
                return false;
            }
            foreach (var queryItem in myDpPathList)
            {
                if (LocalList.ContainsKey(queryItem.Dependence))
                {
                    return true;
                }
                if (queryItem.LibDir == "no dir")
                {
                    Log.Add(String.Format("there's no {0} file on the Lib at path {1}", queryItem.Dependence, queryItem.LibDir));
                    return false;
                }
                bool downloadState = DownloadFile(queryItem.Dependence, queryItem.LibDir, downloadTargetRootDir + queryItem.Dependence + @"\");
                if (downloadState == false)
                {
                    Log.Add(String.Format("{0} download from the Lib failed at path {1}", queryItem.Dependence, queryItem.LibDir));
                    return false;
                }
                bool state = DownloadDependenceFileAndCompile(downloadTargetRootDir + queryItem.Dependence + @"\", downloadTargetRootDir, compileTargetRootDir);
                if(state == false)
                {
                    return false;
                }
            }
            Console.WriteLine("{0}'s dependences compiled", folder.Name);
            if(compileCurrent == false)
            {
                Console.WriteLine("just compiled all {0}'s dependences", folder.Name);
                return true;
            }
            var compileState = CompileFile(checkDpDir, downloadTargetRootDir, compileTargetRootDir, myDpList);
            if (compileState == false)
            {
                return false;
            }
            return true;
        }
        public static void GetDlls(string currentDir)
        {
            foreach(var item in LocalList)
            {
                File.Copy(item.Value, currentDir + @"\" + item.Key + ".dll", true);
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please input dir as instructed");
                Console.WriteLine("currentFile dir");
                var checkDpDir = Console.ReadLine();
                Console.WriteLine("downloadTargetRootDir dir");
                var downloadTargetRootDir = Console.ReadLine();
                Console.WriteLine("compileTargetRootDir dir");
                var compileTargetRootDir = Console.ReadLine();
                Console.WriteLine("if compile currentFile, press 'yes'");
                var compileCurrent = Console.ReadLine();
                bool state;
                if(compileCurrent == "yes")
                {
                    state = DownloadDependenceFileAndCompile(checkDpDir, downloadTargetRootDir, compileTargetRootDir);
                }
                else
                {
                    state = DownloadDependenceFileAndCompile(checkDpDir, downloadTargetRootDir, compileTargetRootDir, true);
                }
                if (state == true)
                {
                    Console.WriteLine("succeed");
                }
                else
                {
                    Console.WriteLine("fail, please input again");
                }
            }
        }
    }
}
