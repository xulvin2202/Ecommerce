﻿using System;
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
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        //{
        //    int totalRecord = 0;
        //    var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

        //    ViewBag.Total = totalRecord;
        //    ViewBag.Page = page;
        //    ViewBag.Keyword = keyword;
        //    int maxPage = 5;
        //    int totalPage = 0;

        //    totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
        //    ViewBag.TotalPage = totalPage;
        //    ViewBag.MaxPage = maxPage;
        //    ViewBag.First = 1;
        //    ViewBag.Last = totalPage;
        //    ViewBag.Next = page + 1;
        //    ViewBag.Prev = page - 1;

        //    return View(model);
        //}
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.Category = new CategoryDao().ViewDetail(product.Category_ID.Value);
          
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
            return View(product);
        }
    }
}