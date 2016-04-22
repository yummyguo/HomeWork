namespace MyHomeWork
{
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
            rid = int.Parse(str);
            return rid;
        } 
        #endregion
    }
}