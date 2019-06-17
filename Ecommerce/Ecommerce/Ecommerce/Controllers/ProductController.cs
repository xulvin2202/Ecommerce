using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using PagedList;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new CategoryDao().ListAllCategory();
            return PartialView(model);
        }
        public ActionResult Category(long cateid, int page = 1/*, int pageSize = 2*/)
        {
            var category = new CategoryDao().ViewDetail(cateid);
            ViewBag.Category = category;

            var model = new ProductDao().ListByCategoryId(cateid);


            return View(model.ToPagedList(page, 8));
        }
        
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.Category = new CategoryDao().ViewDetail(product.Category_ID.Value);
          
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
            return View(product);
        }
    }
}