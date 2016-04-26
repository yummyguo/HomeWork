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
        #region 1.0 帳本列表 + ActionResult Index(int id)
        /// <summary>
        /// 1.0 帳本列表
        /// </summary>
        /// <returns>View(list)</returns>
        [HttpGet]
        public ActionResult Index(int id = 1)
        {
            return View(this.GetList(id, 5));
        }
        #endregion
        #region 2.0 獲取分頁Bar +  ActionResult PageList(string id)
        /// <summary>
        /// 2.0 獲取分頁Bar
        /// </summary>
        /// <param name="pageIndex">當前頁碼</param>
        /// <returns>分頁Bar</returns>
        [HttpGet]
        [ChildActionOnly]
        public ActionResult PageList(int pageIndex)
        {
            var pageSize = Request.Params["pageSize"] == null ? 10 : Convert.ToInt32(Request.Params["pageSize"]);      
            var totalCount = this.GetListCount() / pageSize;
            var prevIndex = pageIndex == 1 ? 1 : pageIndex - 1;
            var nextIndex = pageIndex == totalCount ? totalCount : pageIndex + 1;
            var pageList = this.GetList(pageIndex, pageSize);
            return PartialView(new PageModel() {
                Models = pageList,
                PageBarSize = 5,
                PrevIndex = prevIndex,
                NextIndex = nextIndex,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }
        #endregion
        #region 3.0 刪除單筆AccountBook + ActionResult Del(string id)
        /// <summary>
        /// 3.0 刪除單筆AccountBook
        /// </summary>
        /// <param name="id">guid + pageIndex</param>
        /// <returns>RedirectToAction("Index")</returns>
        [HttpDelete]
        public ActionResult Del(string id,int pageIndex)
        {
            if (string.IsNullOrEmpty(id)) return Json(new {
                uccess = false,
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray()}); 
            try
            {
                Instance.AccountBooks.RemoveRange(listBooks.Where(s => s.Id == Guid.Parse(id)));
                Instance.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbEntityValidationException ex)
            {
                //錯誤僅For 內部看
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                return Json(new { success = false });
            }
        }
        #endregion
        #region 4.0 取得單筆AccountBoo資料
        /// <summary>
        /// 4.0 取得單筆AccountBoo資料
        /// </summary>
        /// <param name="id">guid + pageIndex</param>
        /// <returns>AccountBook</returns>
        [HttpGet]
        public ActionResult Edit(string id,int pageIndex)
        {
            if (string.IsNullOrEmpty(id)) return Content("<script>alert('Id Error');</script>");
            var rGuid = Guid.Parse(id);
            var editModel = listBooks.Where(s => s.Id == rGuid).Select(s => new Models.AccountBookViewModel() {
                Id = s.Id.ToString(),
                Amounttt = s.Amounttt,
                Categoryyy = s.Categoryyy,
                Dateee = s.Dateee,
                Remarkkk = s.Remarkkk,
                PageIndex = pageIndex
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.AccountBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
               return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                               .Select(m => m.ErrorMessage).ToArray()
                });
            }
            if(string.IsNullOrEmpty(model.Id))
            {
                return Json(new
                {
                    success = false,
                    errors = "Id不可為空"
                });
            }
            var rid = Guid.Parse(model.Id.Split('_')[0]);
            try
            {
                var eModel = listBooks.FirstOrDefault(s => s.Id == rid);
                eModel.Categoryyy = (int)model.Categoryyy;
                eModel.Amounttt = (int)model.Amounttt;
                eModel.Dateee = (DateTime)model.Dateee;
                eModel.Remarkkk = model.Remarkkk;
                Instance.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbEntityValidationException ex)
            {
                //錯誤僅For 內部看
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                return Json(new { success = false });
            }
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
        [ValidateAntiForgeryToken]
        public ActionResult Add(AccountBookViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                    .Select(m => m.ErrorMessage).ToArray()
                    });
                }
                var addModel = new AccountBook() { Amounttt = (int)model.Amounttt, Categoryyy = (int)model.Categoryyy, Dateee = (DateTime)model.Dateee, Remarkkk = model.Remarkkk, Id = Guid.NewGuid() };
                Instance.AccountBooks.Add(addModel);
                Instance.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbEntityValidationException ex)
            {
                //錯誤僅For 內部看
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                return Json(new { success = false });
            }
        }
        #endregion
    }
}
