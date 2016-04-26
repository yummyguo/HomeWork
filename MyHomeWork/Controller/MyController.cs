namespace MyHomeWork
{
    using System;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    public class MyController:Controller
    { 
        private static readonly Lazy<SkillTree> lazy = 
            new Lazy<SkillTree>(() => new SkillTree()); 

        public static SkillTree Instance { get { return lazy.Value; } }

        protected readonly List<AccountBook> listBooks = Instance.AccountBooks.Where(s => true).ToList();
        #region 1.0 獲取頁面資訊 - List<AccountBook> GetList(int pageIndex, int pageSize)
        /// <summary>
        /// 1.0 獲取頁面資訊
        /// </summary>
        /// <param name="pageIndex">當前頁碼</param>
        /// <param name="pageSize">當前頁面總數</param>
        /// <returns>List<AccountBook></returns>
        protected List<AccountBookViewModel> GetList(int pageIndex, int pageSize)
        {
            return listBooks.Select(s => s.ToViewModel(pageIndex))
                .OrderByDescending(s => s.Dateee)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        #endregion
        #region 2.0 獲取AccountBook總數 - List<AccountBook> GetListCount()
        /// <summary>
        /// 2.0 獲取頁面資訊
        /// </summary>
        /// <returns>listBooks總數</returns>
        protected int GetListCount()
        {
            return listBooks.Count;
        }
        #endregion
    }
}