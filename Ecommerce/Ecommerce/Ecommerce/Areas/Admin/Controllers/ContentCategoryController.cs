using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class ContentCategoryController : BaseController
    {
        private EcommerceDbContext db = new EcommerceDbContext();
        // GET: Admin/ContentCategory
        public ActionResult Index()
        {
            var dao = new ContentCategoryDao();
            var model = dao.ListAllContentCategory();
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ContentCategoryDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ContentCategory contentCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new ContentCategoryDao();
                    contentCategory.Name = contentCategory.Name;
                    contentCategory.MetaTitle = contentCategory.MetaTitle;
                    contentCategory.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                    contentCategory.Status = Convert.ToBoolean(true);
                    contentCategory.ParentID = contentCategory.ParentID;
                    contentCategory.SeoTitle = contentCategory.SeoTitle;
                    var id = dao.Insert(contentCategory);
                    if (id > 0)
                    {
                        SetAlert("Thêm mới thành thành công", "success");
                        ViewBag.Success = "Thêm thành công";
                        contentCategory = new ContentCategory();
                        return RedirectToAction("Index", "Content");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm mới ko thành công");
                    }

                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(contentCategory);
        }
        


    }
}