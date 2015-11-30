using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FanFunction
{
    /// <summary>
    /// 关于DateTime的一系列函数
    /// </summary>
    public static class clsDateTime
    {
        /// <summary>
        /// 获取数据库服务器当前时间（如：2009-10-22 14:13:17.700）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeByDB()
        {
            //return DateTime.Parse(FanFunction.db.DBHelper.GetDataTable("Select CONVERT(varchar(100), GETDATE(), 21)").Rows[0][0].ToString());//此种方式位数更长
            return Convert.ToDateTime(FanFunction.db.DBHelper.GetScalar("Select GetDate()"));
        }
        /// <summary>
        /// 转换yyyy-MM-dd格式为yyyyMMdd
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static string ChangeTo8(string strDate)
        {
            return System.Text.RegularExpressions.Regex.Replace(strDate, @"[^0-9.]", string.Empty);
        }
        /// <summary>
        /// 转换yyyyMMdd为yyyy-MM-dd
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime Change8To10(string strDate)
        {
            string tempDate = strDate.Insert(4, "-");
            tempDate = tempDate.Insert(7, "-");
            DateTime newDate = Convert.ToDateTime(tempDate);
            return newDate;
        }
        /// <summary>
        /// 计算2个日期之间的天数
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>2个日期之间的天数</returns>
        public static int GetDaysFormBeginAndEnd(DateTime beginDate, DateTime endDate)
        {
            return endDate.Subtract(beginDate).Days;
        }
        /// <summary>
        /// 根据开始日期、当前日期计算是第几周
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="theDate">当前日期</param>
        /// <returns>当前是第几周</returns>
        private static int GetWeakByDate(DateTime beginDate, DateTime theDate)
        {
            double partDays = (double)GetDaysFormBeginAndEnd(beginDate, theDate) + 0.0000000001;//所有数都加上个无限小数，便与计算
            double partWeaks = partDays / 7;//获取部分周数
            int theWeaks = (int)Math.Ceiling(partWeaks);//返回大于或等于指定的十进制数的最小整数
            return theWeaks;
        }
        /// <summary>获取此示例所表示的日期是星期几（返回1,2,3...7）</summary>
        /// <param name="y">年</param> 
        /// <param name="m">月</param> 
        /// <param name="d">日</param> 
        /// <returns>星期几，1代表星期一,7代表星期日</returns>
        public static int GetWeekDayByDate(DateTime date)
        {
            int weekNumber = Convert.ToInt32(date.DayOfWeek);
            if (weekNumber == 0) return 7;
            return weekNumber;
        }
        /// <summary>
        /// 获取此示例所表示的日期是星期几（返回 星期一，星期二...）
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <returns>返回 星期一，星期二...</returns>
        public static string GetWeekDayDescByDate(DateTime date)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[Convert.ToInt16(date.DayOfWeek)];
        }
        /// <summary>
        /// 根据星期描述获取星期数字
        /// </summary>
        /// <param name="weekDayDesc">星期描述，如星期一，星期六，星期日</param>
        /// <returns></returns>
        public static int SetWeekDayByWeekDayDesc(string weekDayDesc)
        {
            int weekDay = 1;
            switch (weekDayDesc)
            {
                case "星期一":
                    weekDay = 1;
                    break;
                case "星期二":
                    weekDay = 2;
                    break;
                case "星期三":
                    weekDay = 3;
                    break;
                case "星期四":
                    weekDay = 4;
                    break;
                case "星期五":
                    weekDay = 5;
                    break;
                case "星期六":
                    weekDay = 6;
                    break;
                case "星期日":
                    weekDay = 0;
                    break;
            }
            return weekDay;
        }
        /// <summary>
        /// 获取时间段内的有效日期
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="weeks">指定需要查询的星期,为null则查询星期1-7</param>
        /// <returns></returns>
        public static List<DateTime> GetDates(DateTime startDate, DateTime endDate, List<int> weeks = null)
        {
            var tspan = endDate.Date - startDate.Date;
            if (tspan < TimeSpan.Zero)
                throw new Exception("结束日期不能小于开始日期");
            var spanDays = tspan.Days + 1;
            var dates = Enumerable.Range(0, spanDays).Select(i => startDate.AddDays(i)).ToList();
            if (weeks != null)
            {
                int[] changeWeeks = weeks.Select(w => (w == 7 ? 0 : w)).ToArray();
                dates = dates.Where(w => changeWeeks.Contains(Convert.ToInt32(w.DayOfWeek))).ToList();
            }
            return dates;
        }
    }
}