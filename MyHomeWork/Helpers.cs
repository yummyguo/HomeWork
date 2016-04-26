namespace MyHomeWork
{
    using System;
    /// <summary>
    /// 專案共用工具類 
    /// </summary>
    public static class Helpers
    {
        #region 1.0 檢查字串是否可以轉為數字
        /// <summary>
        /// 1.0 檢查字串是否可以轉為數字
        /// </summary>
        /// <param name="str">要轉換的字串</param>
        /// <returns>字串轉數字</returns>
        public static int CheckIdIsInt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            int rid;
            if (string.IsNullOrEmpty(str) || !int.TryParse(str, out rid)) return -1;
            return rid;
        }
        #endregion
        #region 2.0 驗證日期，false 表示大於當前日期時間
        /// <summary>
        /// 驗證日期，false 表示大於當前日期時間
        /// </summary>
        /// <param name="dateTime">填寫的日期</param>
        /// <returns>boo</returns>
        public static bool DateDiff(this DateTime? dateTime, DateTime time)
        {
            TimeSpan sTime = new TimeSpan(time.Ticks);
            TimeSpan eTime = new TimeSpan(DateTime.Now.Ticks);
            var ts = eTime.Subtract(sTime).Ticks;
            return eTime.Subtract(sTime).Ticks > 0;// false 表示大於當前日期時間
        } 
        #endregion
    }
}