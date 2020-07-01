using System.Collections.Generic;

namespace DailyHealthReportZju
{
    public static class GlobalSettings
    {
        public static bool IsFullAutoMode = true;

        // init selector paths
        public static List<string> GetSelectors()
        {
            List<string> selectors = new List<string>();
            // absent due to fever? -> No
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(4) > div > div > div:nth-child(2)");
            // absent anyway? -> No
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(5) > div > div > div:nth-child(2)");
            // have not do the test
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(14) > div > div > div:nth-child(2)");
            // has collected health code
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(16) > div > div > div:nth-child(1)");
            // health code is green code
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(17) > div > div > div:nth-child(1)");
            // not in school
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(18) > div > div > div:nth-child(2)");
            // have not pass through any pandamic zone
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > div:nth-child(5)");
            // no bad family member
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(25) > div > div > div:nth-child(2)");

            // promise
            selectors.Add("body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(37) > div > div > div");

            return selectors;
        }

        public static List<(string, string)> GetKeyWords()
        {
            List<(string, string)> keywords = new List<(string, string)>();
            keywords.Add(("今日是否因发热请假未到岗", "否"));
            keywords.Add(("今日是否因发热外的其他原因请假未到岗", "否"));
            keywords.Add(("你是否做过核酸检测", "否"));
            keywords.Add(("是否已经申领校区所在地健康码", "是"));
            keywords.Add(("今日申领校区所在地健康码的颜色", "绿码"));
            keywords.Add(("今日是否在校", "否"));
            keywords.Add(("你是否5月30日后从下列地区返回浙江", "否"));
            keywords.Add(("本人家庭成员(包括其他密切接触人员)是否有近14日入境或近14日拟入境的情况", "否"));
            // bug: found but not checkbox not checked when clicking div
            keywords.Add(("", "上述信息真实准确"));
            return keywords;
        }
    }
}
