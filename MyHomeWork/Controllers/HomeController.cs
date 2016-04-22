namespace MyHomeWork.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Text;

    public class HomeController : MyController
    { 
        #region 1.0 帳本列表 + ActionResult Index(string id)
        /// <summary>
        /// 1.0 帳本列表
        /// </summary>
        /// <returns>View(list)</returns>
        [HttpGet]
        public ActionResult Index(string id)
        {
            var rid = Helpers.CheckIdIsInt(id);
            if (rid == 0 || rid == -1) rid = 1;
            var list = this.GetList(rid, 5);
            return View(list);
        }
        #endregion
        #region 2.0 獲取分頁Bar +  ActionResult PageList(string id)
        /// <summary>
        /// 2.0 獲取分頁Bar
        /// </summary>
        /// <param name="id">當前頁碼</param>
        /// <returns>分頁Bar</returns>
        [HttpGet]
        [ChildActionOnly]
        public ActionResult PageList(string id)
        {
            var pageIndex = id.CheckIdIsInt();
            if (pageIndex == 0 || pageIndex == -1) pageIndex = 1;
            var pageSize = Request.Params["pageSize"] == null ? 10 : Convert.ToInt32(Request.Params["pageSize"]);
            var list = GetList(pageIndex, pageSize);
            var totalCount = this.GetListCount() / pageSize;
            var pageBarSize = 5;
            var prevIndex = pageIndex == 1 ? 1 : pageIndex - 1;
            var nextIndex = pageIndex == totalCount ? totalCount : pageIndex + 1;
            var pageList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            StringBuilder sb = new StringBuilder(1000);
            sb.Append("<div class='black'>");
            sb.Append("<a href ='/Home/Index/1'>第一頁</a>");
            sb.Append("<a href ='/Home/Index/" + prevIndex + "'>上一頁</a>");
            if (pageIndex == totalCount) sb.Append("<a href ='/Home/Index/" + pageIndex + "'> " + pageIndex + " </a>");
            else
            {
                var step = 1;
                var skipCount = (pageIndex + pageBarSize) > totalCount ? totalCount+1 : (pageIndex + pageBarSize);
                for (var i = pageIndex; i < skipCount; i++)
                {
                    if (step == 1) sb.Append("<a class='highlight' href ='/Home/Index/" + i + "'> " + i + " </a>");
                    else sb.Append("<a href ='/Home/Index/" + i + "'> " + i + " </a>");
                    ++step;
                }
            }
            sb.Append("<a href ='/Home/Index/" + nextIndex + "'> 下一頁</a>");
            sb.Append("<a href ='/Home/Index/" + totalCount + "'>最末頁</a>");
            sb.Append("</div>");
            return Content(sb.ToString());
        }
        #endregion
        #region 3.0 刪除單筆AccountBook + ActionResult Del(string id)
        /// <summary>
        /// 3.0 刪除單筆AccountBook
        /// </summary>
        /// <param name="id">guid + pageIndex</param>
        /// <returns>RedirectToAction("Index")</returns>
        [HttpGet]
        public ActionResult Del(string id)
        {
            if (string.IsNullOrEmpty(id)) return Content("<script>alert('Id Error');</script>");
            var splitStr = id.ToString().Split('_');
            try
            {
                Instance.AccountBooks.RemoveRange(listBooks.Where(s => s.Id == Guid.Parse(splitStr[0])));
                Instance.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //錯誤僅For 內部看
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
            }
            return RedirectToAction("Index",new { id = splitStr[1].ToLower().Equals("index") ? "" : splitStr[1] });
        }
        #endregion
        #region 4.0 取得單筆AccountBoo資料
        /// <summary>
        /// 4.0 取得單筆AccountBoo資料
        /// </summary>
        /// <param name="id">guid + pageIndex</param>
        /// <returns>AccountBook</returns>
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return Content("<script>alert('Id Error');</script>");
            var splitStr = id.ToString().Split('_');
            var rGuid = Guid.Parse(splitStr[0]);
            var pageIndex = -1;
            var tempPageIndex =splitStr[1].ToLower();//switch()
            switch (tempPageIndex)
            {
                case "":
                    pageIndex = 1;
                    break;
                case "index":
                    pageIndex = 1;
                    break;
                default:
                    pageIndex = Convert.ToInt32(tempPageIndex);
                    break;
            }
            var editModel = listBooks.Where(s => s.Id == rGuid).Select(s => new Models.AccountBookViewModel() {
                Id = s.Id.ToString(),Amounttt = s.Amounttt,Categoryyy = s.Categoryyy,Dateee = s.Dateee,Remarkkk = s.Remarkkk,PageIndex = pageIndex
            }).FirstOrDefault();
            return View(editModel);
        }
        #endregion
        #region 5.0 更新單筆資料 + ActionResult Edit(Models.AccountBookViewModel model)
        /// <summary>
        /// 5.0 更新單筆資料
        /// </summary>
        /// <param name="model">要修改的實體</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Models.AccountBookViewModel model)
        {
            var rid = Guid.Parse(model.Id.Split('_')[0]);
            try
            {
                var eModel = listBooks.FirstOrDefault(s => s.Id == rid);
                eModel.Categoryyy = (int)model.Categoryyy;
                eModel.Amounttt = (int)model.Amounttt;
                eModel.Dateee = (DateTime)model.Dateee;
                eModel.Remarkkk = model.Remarkkk;
                Instance.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //錯誤僅For 內部看
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);

            }
            return RedirectToAction("Index", new { id = model.PageIndex });
        }
        #endregion
        #region 6.0 新增頁面Get + ActionResult Add(string id)
        /// <summary>
        /// 6.0 新增頁面Get
        /// </summary>
        /// <param name="id">pageIndex</param>
        /// <returns>View(AccountBookViewModel)</returns>
        [HttpGet]
        public ActionResult Add(string id)
        {
            var pageIndex = Helpers.CheckIdIsInt(id);
            if (pageIndex == 0 || pageIndex == -1) pageIndex = 1;
            return View(new AccountBookViewModel() { PageIndex = pageIndex });
        }
        #endregion
        #region 7.0 新增頁面Post + ActionResult Add(AccountBookViewModel model)
        /// <summary>
        /// 7.0 新增頁面Post
        /// </summary>
        /// <param name="model">要新增的實體</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(AccountBookViewModel model)
        {
            if (!ModelState.IsValid) return View(new AccountBookViewModel() { PageIndex = model.PageIndex });
            var addModel = new AccountBook() { Amounttt = (int)model.Amounttt, Categoryyy = (int)model.Categoryyy, Dateee = (DateTime)model.Dateee, Remarkkk = model.Remarkkk, Id = Guid.NewGuid() };

            Instance.AccountBooks.Add(addModel);
            Instance.SaveChanges();
            return RedirectToAction("Index", new { id = model.PageIndex });
        }
        #endregion
    }
}
