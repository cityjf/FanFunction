using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace FanFunction.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 得到本周第一天(以星期一为第一天)
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime day)
        {
            //星期几(周日到周六：0，1，2，3，4，5，6)
            int weeknow = (int)day.DayOfWeek;
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            return day.AddDays(daydiff).Date;
        }
        /// <summary>
        /// 得到本周最后一天(以星期天为最后一天)
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(this DateTime day)
        {
            int weeknow = (int)day.DayOfWeek;
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            return day.AddDays(daydiff).Date;
        }
        /// <summary>
        /// 当前日期是当月的第几周（星期一/星期天）
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int WeekOfMonth(this DateTime day)
        {
            DateTime FirstDayofMonth = new DateTime(day.Date.Year, day.Date.Month, 1);
            int week = (int)FirstDayofMonth.DayOfWeek;

            if (week == 0)
                week = 7;

            return (day.Date.Day + week - 2) / 7 + 1;
        }
        /// <summary>
        /// 当前日期是当年的第几周（星期一/星期天）
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime day)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(day, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
        /// <summary>
        /// 获取日期为周几，返回数字 1 2 3 4 5 6 7 
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static int DayOfWeekNumber(this DateTime dt)
        {
            int weekNumber = Convert.ToInt32(dt.DayOfWeek);
            if (weekNumber == 0) return 7;
            return weekNumber;
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type">要格式化的类型
        /// <para>1返回yyyy-MM-dd HH:mm:ss</para>
        /// <para>2返回yyyy-MM-dd</para>
        /// <para>3返回yyyy-MM-dd HH:mm</para>
        /// <para>4返回yyyyMMdd</para>
        /// <para>5返回yyyy-MM-dd HH:mm:ss.fff</para>
        /// </param>
        /// <returns></returns>
        public static string ToString(this DateTime dt, int type)
        {
            switch (type)
            {
                case 1:
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return dt.ToString("yyyy-MM-dd");
                case 3:
                    return dt.ToString("yyyy-MM-dd HH:mm");
                case 4:
                    return dt.ToString("yyyyMMdd");
                case 5:
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                default:
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
