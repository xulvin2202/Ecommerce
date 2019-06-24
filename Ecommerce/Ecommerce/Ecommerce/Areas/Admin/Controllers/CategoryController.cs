using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            var dao = new CategoryDao();
            var model = dao.ListCategory();
            return View(model);
        }
        public ActionResult SubCategory()
        {
            var dao = new CategoryDao();
            var model = dao.ListSubCategory();
            return View(model);
        }
    }
}