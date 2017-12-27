using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;

namespace DefaultSeek
{
    public static class DefaultDb
    {
        public static List<ISeekItem> iqiyi = new List<ISeekItem>();
        public static List<ISeekItem> tencent = new List<ISeekItem>();
        public static List<ISeekItem> youku = new List<ISeekItem>();
        public static List<ISeekItem> sohu = new List<ISeekItem>();
        public static List<ISeekItem> pptv = new List<ISeekItem>();
        public static List<ISeekItem> letv = new List<ISeekItem>();

        public static void Add(string platform, ISeekItem seekItem)
        {
            FieldInfo fieldInfo = typeof(DefaultDb).GetField(platform, BindingFlags.Static | BindingFlags.Public);

            if(fieldInfo != null)
            {
                (fieldInfo.GetValue(null) as List<ISeekItem>).Add(seekItem);
            }
        }
        public static IEnumerable<ISeekItem> GetDefaultRepo(string platform, string series)
        {
            string upperPlatform = platform.Substring(0, 1).ToUpper() + platform.Substring(1);
            string platformSeries = upperPlatform + DefaultDb.SeriesEnParse(series);
            MethodInfo methodInfo = typeof(DefaultDb).GetMethod(platformSeries + "Setup", new Type[2] { typeof(string), typeof(string) });
            if(methodInfo == null)
            {
                Console.WriteLine("该平台没有 {0} 的资源!", series);
                return new List<ISeekItem>();
            }
            methodInfo.Invoke(null, new object[2] { platform, series });
            IEnumerable<ISeekItem> defaultRepo = typeof(DefaultDb).GetField(platform, BindingFlags.Static | BindingFlags.Public).GetValue(null) as IEnumerable<ISeekItem>;
            Clear(platform);
            return defaultRepo;
        }
        public static void Clear(string platform)
        {
            typeof(DefaultDb).GetField(platform, BindingFlags.Static | BindingFlags.Public).SetValue(null, new List<ISeekItem>());
        }

        public static string SeriesEnParse(string seriesZh)
        {
            switch (seriesZh)
            {
                case "海底小纵队":
                    return "Octonauts";
                case "小猪佩奇":
                    return "PeppaPig";
                case "汪汪队立大功":
                    return "PawPatrol";
                case "小马宝莉":
                    return "MyLittlePony";
                case "嗨道奇":
                    return "HeyDuggee";
                case "全球探险冲冲冲":
                    return "GoJetters";

            }
            return "wrong series Name";
        }

        public static void IqiyiOctonautsSetup(string platform = "iqiyi", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "全集", "265353700"));
            Add(platform, new SeekItem(series, "第一季", "354806800"));
            Add(platform, new SeekItem(series, "第二季", "355335400"));
            Add(platform, new SeekItem(series, "第三季", "355366300"));
            Add(platform, new SeekItem(series, "第四季", "604765000"));
            Add(platform, new SeekItem(series, "特别版", "271232100"));
            Add(platform, new SeekItem(series, "儿歌", "3738677509"));
            Add(platform, new SeekItem(series, "英文版", "390691100"));
        }

        //Octonauts
        public static void TencentOctonautsSetup(string platform = "tencent", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "全集", "fmy3srlfpa5wr53"));
            Add(platform, new SeekItem(series, "第一季", "yorxdeslttv9pr4"));
            Add(platform, new SeekItem(series, "第二季", "fde7e13b1g4ruju"));
            Add(platform, new SeekItem(series, "第三季", "6yndrrg2uutt9gz"));
            Add(platform, new SeekItem(series, "第四季", "6i41a79w3vv8cfl"));
            Add(platform, new SeekItem(series, "特别版", "53d7u0fnozntwv1"));
            Add(platform, new SeekItem(series, "儿歌", "5v8je3p4zr2qjx7"));
        }
        public static void YoukuOctonautsSetup(string platform = "youku", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "第一季", "za1b4108813c711e4b8b7"));
            Add(platform, new SeekItem(series, "第二季", "z71efbfbd2343efbfbdef"));
            Add(platform, new SeekItem(series, "第三季", "zefbfbd024d5eefbfbdef"));
            Add(platform, new SeekItem(series, "第四季", "z0aefbfbd6dcf81efbfbd"));
            Add(platform, new SeekItem(series, "特别版", "z005eeb5e448c11e4b2ad"));
        }
        public static void SohuOctonautsSetup(string platform = "sohu", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "全集", "s2014/hdxzd/"));
            Add(platform, new SeekItem(series, "第四季", "s2017/dhphdxzd4/"));
            Add(platform, new SeekItem(series, "特别版", "s2014/hdxzdzhyeg/"));
            Add(platform, new SeekItem(series, "儿歌", "s2014/hdxzdtbpian/"));
        }
        public static void PptvOctonautsSetup(string platform = "pptv", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "全集", "uZp29FzCMnDTUbk"));
            Add(platform, new SeekItem(series, "第一季", "dcCsKpL4aKYJhib8"));
            Add(platform, new SeekItem(series, "第二季", "ECYSkPhezgxv7VU"));
            Add(platform, new SeekItem(series, "第三季", "EScRjicddzQtu7FQ"));
            Add(platform, new SeekItem(series, "第四季", "naONC3PZSYfqaNA"));
            Add(platform, new SeekItem(series, "特别版", "EykDgelPvic1g3kY"));
            Add(platform, new SeekItem(series, "儿歌", "up139V3DM3HUUro"));
        }
        public static void LetvOctonautsSetup(string platform = "letv", string series = "海底小纵队")
        {
            Add(platform, new SeekItem(series, "全集", "10003312"));
            Add(platform, new SeekItem(series, "第一季", "10034658"));
            Add(platform, new SeekItem(series, "第二季", "10034661"));
            Add(platform, new SeekItem(series, "第三季", "10034663"));
            Add(platform, new SeekItem(series, "第四季", "10003312"));
            Add(platform, new SeekItem(series, "特别版", "10003897"));
            Add(platform, new SeekItem(series, "儿歌", "10003878"));
        }

        //peppa pig
        public static void IqiyiPeppaPigSetup(string platform = "iqiyi", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "全集", "438724200"));
            Add(platform, new SeekItem(series, "第一季", "405216600"));
            Add(platform, new SeekItem(series, "第二季", "422636600"));
            Add(platform, new SeekItem(series, "第三季", "439141200"));
            Add(platform, new SeekItem(series, "第四季", "439205500"));
        }
        public static void TencentPeppaPigSetup(string platform = "tencent", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "全集", "bzfkv5se8qaqel2"));
            Add(platform, new SeekItem(series, "第一季", "e3t4g6wgd3z8uq0"));
            Add(platform, new SeekItem(series, "第二季", "z8njmlc1opwgxoa"));
            Add(platform, new SeekItem(series, "第三季", "zbxd3sjpfcnfwzi"));
            Add(platform, new SeekItem(series, "第四季", "uqt7am9f9dmvoss"));
        }
        public static void YoukuPeppaPigSetup(string platform = "youku", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "第一季", "z60db2a18fba811e0a046"));
            Add(platform, new SeekItem(series, "第二季", "zefbfbd3e76c78e2411ef"));
            Add(platform, new SeekItem(series, "第三季", "zefbfbdefbfbd44c48e26"));
            Add(platform, new SeekItem(series, "第四季", "z0c2f626aefbfbd2711ef"));
            Add(platform, new SeekItem(series, "第五季", "zefbfbd7aefbfbd56efbf"));
        }
        public static void SohuPeppaPigSetup(string platform = "sohu", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "全集", "s2015/fhzxm/"));
            Add(platform, new SeekItem(series, "第一季", "s2016/dhpfhzxm1/"));
            Add(platform, new SeekItem(series, "第二季", "s2016/dhpfhzxm2/"));
            Add(platform, new SeekItem(series, "第三季", "s2016/fhzxm3/"));
            Add(platform, new SeekItem(series, "第四季", "s2016/fhzxm4/"));
        }
        public static void PptvPeppaPigSetup(string platform = "pptv", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "全集", "fOfRT7cdjcsurBQ"));
            Add(platform, new SeekItem(series, "第一季", "Cz0Xlf1j0xF08lo"));
            Add(platform, new SeekItem(series, "第二季", "DDYiaoAhu3hxicicWU"));
            Add(platform, new SeekItem(series, "第三季", "DTchnwdt3RtibicGQ"));
            Add(platform, new SeekItem(series, "第四季", "DjgUkvpg0A5x71c"));
        }
        public static void LetvPeppaPigSetup(string platform = "letv", string series = "小猪佩奇")
        {
            Add(platform, new SeekItem(series, "全集", "94075"));
            Add(platform, new SeekItem(series, "第一季", "10018250"));
            Add(platform, new SeekItem(series, "第二季", "10018251"));
            Add(platform, new SeekItem(series, "第三季", "10018252"));
            Add(platform, new SeekItem(series, "第四季", "10018976"));
        }

        //paw patrol
        public static void IqiyiPawPatrolSetup(string platform = "iqiyi", string series = "汪汪队立大功")
        {
            Add(platform, new SeekItem(series, "全集", "825701700"));
            Add(platform, new SeekItem(series, "第一季", "788615300"));
            Add(platform, new SeekItem(series, "第二季", "788615300"));
            Add(platform, new SeekItem(series, "第三季", "768156700"));
            Add(platform, new SeekItem(series, "第四季", "821505900"));
            Add(platform, new SeekItem(series, "第四季英文版", "821549400"));
        }
        public static void TencentPawPatrolSetup(string platform = "tencent", string series = "汪汪队立大功")
        {
            Add(platform, new SeekItem(series, "全集", "zlrjvoan5acz3gs"));
            Add(platform, new SeekItem(series, "第一季", "ca1k6ja4k81h8ov"));
        }
        public static void YoukuPawPatrolSetup(string platform = "youku", string series = "汪汪队立大功")
        {
            Add(platform, new SeekItem(series, "第一季", "z21943d702a9f11e59e2a"));
            Add(platform, new SeekItem(series, "第二季", "z229b815c182311e6a080"));
        }
        public static void PptvPawPatrolSetup(string platform = "pptv", string series = "汪汪队立大功")
        {
            Add(platform, new SeekItem(series, "全集", "3G1HxS2TA0GkIoo"));
        }
        public static void LetvPawPatrolSetup(string platform = "letv", string series = "汪汪队立大功")
        {
            Add(platform, new SeekItem(series, "全集", "10030239"));
            Add(platform, new SeekItem(series, "第一季", "10023490"));
        }

        //My Little Pony
        public static void IqiyiMyLittlePonySetup(string platform = "iqiyi", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "全集一到七", "207260500"));
            Add(platform, new SeekItem(series, "第一季", "254873000"));
            Add(platform, new SeekItem(series, "第二季", "254975300"));
            Add(platform, new SeekItem(series, "第三季", "255004500"));
            Add(platform, new SeekItem(series, "第四季", "252260700"));
            Add(platform, new SeekItem(series, "第五季", "379665400"));
            Add(platform, new SeekItem(series, "第六季", "511584600"));
            Add(platform, new SeekItem(series, "第七季", "692815700"));
        }
        public static void TencentMyLittlePonySetup(string platform = "tencent", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "全集65", "e26cdiurkbuwwj8"));
            Add(platform, new SeekItem(series, "全集一到七", "3x7rf42yye8fpif"));
            Add(platform, new SeekItem(series, "第七季", "8wzvvuy5l8cbg51"));
        }
        public static void YoukuMyLittlePonySetup(string platform = "youku", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "全集65", "z421d7cf8e7a911de97c0"));
            Add(platform, new SeekItem(series, "第一季", "z4d1387f6a0dd11e296da"));
            Add(platform, new SeekItem(series, "第二季", "z85d82fe6a0de11e2b2ac"));
            Add(platform, new SeekItem(series, "第三季", "z8c58160ea0e011e2b2ac"));
            Add(platform, new SeekItem(series, "第四季", "zd0900e7a002b11e4b2ad"));
            Add(platform, new SeekItem(series, "第五季", "z1b3a7e761fd211e5b5ce"));
            Add(platform, new SeekItem(series, "第六季", "z289c519a05cb11e69e2a"));
        }
        public static void SohuMyLittlePonySetup(string platform = "sohu", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "全集一到七", "s2014/xmbl/"));
            Add(platform, new SeekItem(series, "全集英文版", "s2015/xbmlywb/"));
            Add(platform, new SeekItem(series, "第一季", "s2014/wdxmjyydmf1/"));
            Add(platform, new SeekItem(series, "第二季", "s2014/wdxmjyydmf2/"));
            Add(platform, new SeekItem(series, "第三季", "s2014/wdxmjyydmf3/"));
            Add(platform, new SeekItem(series, "第四季", "s2014/wdxmjyydmf4/"));
            Add(platform, new SeekItem(series, "第五季", "s2015/dhpxmbl5/"));
            Add(platform, new SeekItem(series, "第六季", "s2017/xmbaoli6/"));
            Add(platform, new SeekItem(series, "第七季", "s2017/xmbaoli7/"));
        }
        public static void PptvMyLittlePonySetup(string platform = "pptv", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "第一季", "9ldBvyeNicTueHIQ"));
            Add(platform, new SeekItem(series, "第二季", "91ZCwCiaOicjyfHYU"));
            Add(platform, new SeekItem(series, "第三季", "ibVRAviaaMicDqdG4M"));
            Add(platform, new SeekItem(series, "第四季", "icVA8uiaKIibDaZF38"));
            Add(platform, new SeekItem(series, "第五季", "jaCMCnLYSIbpZ88"));
            Add(platform, new SeekItem(series, "第六季", "NgDsatI4qOZJxy8"));

        }
        public static void LetvMyLittlePonySetup(string platform = "letv", string series = "小马宝莉")
        {
            Add(platform, new SeekItem(series, "全集一到七", "10002188"));
            Add(platform, new SeekItem(series, "全集英文版", "10016446"));
            Add(platform, new SeekItem(series, "第四季", "10002907"));
            Add(platform, new SeekItem(series, "第五季", "10010779"));
            Add(platform, new SeekItem(series, "第五季英文版", "10011401"));
        }

        //Hey Duggee
        public static void IqiyiHeyDuggeeSetup(string platform = "iqiyi", string series = "嗨 道奇")
        {
            Add(platform, new SeekItem(series, "第一季", "651041400"));
            Add(platform, new SeekItem(series, "第一季英文版", "684205500"));
            Add(platform, new SeekItem(series, "第二季", "651825200"));
            Add(platform, new SeekItem(series, "第二季英文版", "684205500"));
            
        }
        public static void TencentHeyDuggeeSetup(string platform = "tencent", string series = "嗨道奇")
        {
            Add(platform, new SeekItem(series, "全集", "trgu7nxbpfcahxy"));
            Add(platform, new SeekItem(series, "第一季", "m442pmf9z9wjmk1"));
            Add(platform, new SeekItem(series, "第一季英文版", "663hdu9rpw7kumk"));
            Add(platform, new SeekItem(series, "第二季", "zz88qjv1wcbtqc7"));
            Add(platform, new SeekItem(series, "第二季英文版", "jzhfi4ihfeee781"));

        }

        //go jetters
        public static void IqiyiGoJettersSetup(string platform = "iqiyi", string series = "全球探险冲冲冲")
        {
            Add(platform, new SeekItem(series, "第一季", "663283000"));
            Add(platform, new SeekItem(series, "第一季英文版", "665616600"));
            Add(platform, new SeekItem(series, "第二季", "665869600"));
            Add(platform, new SeekItem(series, "第二季英文版", "665916600"));

        }
        public static void TencentGoJettersSetup(string platform = "tencent", string series = "全球探险冲冲冲")
        {
            Add(platform, new SeekItem(series, "全集", "4diuqk1bn78rhnu"));
            Add(platform, new SeekItem(series, "第一季", "yy43go8ta4zuddk"));
            Add(platform, new SeekItem(series, "第一季英文版", "tje5de09vigf44s"));
            Add(platform, new SeekItem(series, "第二季", "zhgilo6k2tlcvk8"));
            Add(platform, new SeekItem(series, "第二季英文版", "y9c0clf4nf68eo3"));

        }
    }
}
