using System.Linq;
using System.Collections.Generic;

namespace DailyHealthReportZju
{
    public enum DriverTypes
    {
        Chrome,
        // not tested
        Firefox,
    }

    public static class GlobalSettings
    {
        public static bool IsFullAutoMode { get; } = true;
        // Do NOT try Firefox that is not currently working.
        public static DriverTypes DriverTypes { get; } = DriverTypes.Chrome;
        public static string FirefoxBrowserPath { get; } = "";
        public static string Url { get; } = "https://healthreport.zju.edu.cn/ncov/wap/default/index";
        public static int InitiationTimeoutInSeconds { get; } = 60;
        public static int ElementDiscoveryTimeoutInSeconds { get; } = 20;
        public static string Username { get; } = "";
        public static string Password { get; } = "";

        public static List<(string, string)> GetKeyWords()
        {
            List<(string, string)> keywords = new List<(string, string)>();
            keywords.Add(("今日是否因发热请假未到岗", "否"));
            keywords.Add(("今日是否因发热外的其他原因请假未到岗", "否"));
            keywords.Add(("你是否做过核酸检测", "否"));
            keywords.Add(("是否已经申领校区所在地健康码", "是"));
            keywords.Add(("今日申领校区所在地健康码的颜色", "绿码"));
            keywords.Add(("今日是否在校", "否"));
            keywords.Add(("你是否从以下地区返回浙江", "否"));
            keywords.Add(("本人家庭成员(包括其他密切接触人员)是否有近14日入境或近14日拟入境的情况", "否"));
            // bug: found but not checkbox not checked when clicking div
            keywords.Add(("", "上述信息真实准确"));
            return keywords;
        }
    }
}
