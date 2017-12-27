using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Framework;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace Present
{
    public class XlsxPresentHelper: PresentHelper
    {
        private static XlsxPresentHelper helper = null;
        private XlsxPresentHelper() { }
        public static XlsxPresentHelper GetInstance()
        {
            if (helper == null)
            {
                helper = new XlsxPresentHelper();
            }
            return helper;
        }
        protected Dictionary<string, string[]> GetTitles()
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            dict.Add("海底小纵队", new string[]{ "全集", "第一季", "第二季", "第三季", "第四季", "特别版", "儿歌", "英文版" });
            dict.Add("小猪佩奇", new string[] { "全集", "第一季", "第二季", "第三季", "第四季", "第五季" });
            dict.Add("汪汪队立大功", new string[] { "全集", "第一季", "第二季", "第三季", "第四季", "第四季英文版" });
            dict.Add("小马宝莉", new string[] { "全集65", "全集一到七", "全集英文版", "第一季", "第二季", "第三季", "第四季", "第五季", "第五季英文版", "第六季", "第七季" });
            dict.Add("嗨道奇", new string[] { "全集", "第一季", "第一季英文版", "第二季", "第二季英文版"});
            dict.Add("全球探险冲冲冲", new string[] { "全集", "第一季", "第一季英文版", "第二季", "第二季英文版" });
            return dict;
        }
         protected override object Present(string[] platforms, string[] seriesList,
            string countHelperTypeStringSuffix, string countHelperTypeNameSpaceString,
            string[] countHelperDependences, NameSwitcher nameSwitcher,
            string countHelperGetInstance,
            string getSeriesSeeds,
            string getCount,
            object[] countHelperGetInstanceParameters,
            object[] getCountParametersNoSeeds)
        {
            StringBuilder log = new StringBuilder();
            int mode = 0;
            while(mode == 0)
            {
                Console.WriteLine("已选择xlsx类型的操作，请选择模式");
                Console.WriteLine("1 PlayCount.xlsx内容追加 2 创建新的xlsx文件 3 自定义xlsx文件内容追加 9 退出");
                var input = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                switch (input.KeyChar.ToString())
                {
                    case "1":
                        mode = 1;
                        break;
                    case "2":
                        mode = 2;
                        break;
                    case "3":
                        mode = 3;
                        break;
                    case "9":
                        log.AppendLine("未进行操作");
                        return log;
                    default:
                        Console.WriteLine("请输入正确的操作!");
                        break;
                }
            }
            
            string upperPlatform;
            IEnumerable<ICountItem> countList;
            Dictionary<string, string[]> dict = GetTitles();
            string xlsxFileName = "PlayCount";
            string tempXlsxPath = System.Environment.CurrentDirectory + @"\xlsx\temp" + new Random().Next().GetHashCode() + new Random().Next() + ".xlsx";
            string xlsxInputPath = string.Empty;

            IWorkbook workbook;
            if (mode == 2)
            {
                //when create a xlsx file. you just create a XSSFWorkbook object then write it into a filestream.
                workbook = new XSSFWorkbook();
            }
            else
            {
                if(mode == 1)
                {
                    //add content to exist .xlsx file by default file path: ./xlsx/PlayCount.xlsx
                    xlsxInputPath = System.Environment.CurrentDirectory + @"\xlsx\" + xlsxFileName + ".xlsx";
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("请输入要追加内容的文件名(不包括.xlsx),回车确认输入，输入#退出");
                        xlsxFileName = Console.ReadLine();
                        Console.WriteLine();

                        if (xlsxFileName == "#")
                        {
                            log.AppendLine("未进行操作");
                            return log;
                        }
                        xlsxInputPath = System.Environment.CurrentDirectory + @"\xlsx\" + xlsxFileName + ".xlsx";
                        if (File.Exists(xlsxInputPath))
                        {
                            break;
                        }
                        Console.WriteLine("未能找到文件:{0}", xlsxInputPath);
                    }
                }
                File.Copy(xlsxInputPath, tempXlsxPath);
                workbook = new XSSFWorkbook(tempXlsxPath);
            }

            foreach (var series in seriesList)
            {
                string[] titles;
                if (!dict.TryGetValue(series, out titles))
                {
                    log.AppendLine("there is no titles for " + series);
                    continue;
                }

                int rowCount;

                ISheet sheet;
                if(mode == 2)
                {
                    sheet = workbook.CreateSheet(series);
                    rowCount = 0;
                }
                else
                {
                    sheet = workbook.GetSheet(series);
                    if(sheet == null)
                    {
                        Console.WriteLine("不存在名为{0}的sheet", series);
                        continue;
                    }
                    //the count in NPOI is start from 0 but excel from 1. the row after the last row is where we begin to fill the data
                    rowCount = sheet.LastRowNum + 1;
                }

                int column = 1;

                if (mode == 2)
                {
                    sheet.CreateRow(rowCount).CreateCell(column).SetCellValue(series + "播放量: 亿");
                    rowCount++;
                    sheet.CreateRow(rowCount).CreateCell(column).SetCellValue("日期|平台");
                    rowCount++;
                }
                sheet.CreateRow(rowCount).CreateCell(column).SetCellValue(DateTime.Now.ToLongDateString());
                rowCount++;
                Dictionary<string, int> titleDict = new Dictionary<string, int>();
                foreach (var title in titles)
                {
                    sheet.CreateRow(rowCount).CreateCell(column).SetCellValue(title);
                    titleDict.Add(title, rowCount);
                    rowCount++;
                }
                
                foreach (var platform in platforms)
                {
                    column++;
                    if(mode == 2)
                    {
                        sheet.GetRow(1).CreateCell(column).SetCellValue(platform);

                    }
                    upperPlatform = platform.Substring(0, 1).ToUpper() + platform.Substring(1);
                    countList = (IEnumerable<ICountItem>)SeekResult(platform, series, countHelperTypeNameSpaceString + "." + upperPlatform + countHelperTypeStringSuffix, countHelperDependences, nameSwitcher, countHelperGetInstance, getSeriesSeeds, getCount, countHelperGetInstanceParameters, getCountParametersNoSeeds);
                    if (countList.Count() == 0)
                    {
                        //We werent able to get the playcount result, because the platform didnt have the resource. so it return "". Exception comes when we dont get the data from the response.
                        Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                        Console.WriteLine();
                        continue;
                    }
                    foreach (var countItem in countList)
                    {
                        int rowNum;
                        if (!titleDict.ContainsKey(countItem.Title))
                        {
                            rowNum = sheet.LastRowNum + 1;
                            titleDict.Add(countItem.Title, rowNum);
                        }
                        titleDict.TryGetValue(countItem.Title, out rowNum);
                        double viewCount = Math.Round((double.Parse(countItem.Count) / 1000000), MidpointRounding.AwayFromZero) / 100;
                        sheet.GetRow(rowNum).CreateCell(column).SetCellValue(viewCount);
                    }
                    Console.WriteLine("{0} {1} 播放量数据搜索完毕!", nameSwitcher.NameSwitch(platform), series);
                    Console.WriteLine();
                }
                column++;
                if (mode == 2)
                {
                    sheet.GetRow(1).CreateCell(column).SetCellValue("总计");
                }
            }

            if(mode == 2)
            {
                string xlsxOutputFilePath = System.Environment.CurrentDirectory + @"\xlsx\" + "PlayCount_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "时") + ".xlsx";
                //when create a xlsx, you have to save the XSSFWorkbook object from memory into a file
                try
                {
                    using (FileStream fs = File.Open(xlsxOutputFilePath, FileMode.OpenOrCreate))
                    {
                        workbook.Write(fs);
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.AppendLine("error: " + ex.Message);
                    log.AppendLine("无法创建文件");
                }
            }
            else
            {
                File.Delete(xlsxInputPath);
                string xlsxOutputFilePath = System.Environment.CurrentDirectory + @"\xlsx\" + xlsxFileName + ".xlsx";
                try
                {
                    using (FileStream fs = File.Open(xlsxOutputFilePath, FileMode.OpenOrCreate))
                    {
                        workbook.Write(fs);
                        workbook.Close();
                        //File.Delete(tempXlsxPath);
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.AppendLine("error: " + ex.Message);
                    log.AppendLine("");
                }
            }
            return log;
         }

    }
}
