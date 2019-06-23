using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Price,PromotionPrice,Description,Detail,Image,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescription,Category_ID,Brand_ID,Status")]Product product, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new ProductDao();
                    var filename = "";
                    var path = "";
                    if (image != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                        path = Path.Combine(Server.MapPath("~/Image"), filename);
                        image.SaveAs(path);
                        product.Image = filename;
                    }
                    //else
                    //{

                    //    content.Image = "~/Image/logo.png";
                    //}
                    product.Name = product.Name;
                    product.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                    product.MetaTitle = StringHelper.ToUnsignString(product.Name);
                    product.Description = product.Description;
                    product.Detail = product.Detail;
                    product.Price = product.Price;
                    product.PromotionPrice = product.PromotionPrice;
                    product.Category_ID = product.Category_ID;
                    product.Brand_ID = product.Brand_ID;
                    product.Status = Convert.ToBoolean(true);
                    var id = dao.Insert(product);
                    if (id > 0)
                    {
                        SetAlert("Thêm mới thành thành công", "success");
                        ViewBag.Success = "Thêm thành công";
                        product = new Product();
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
            SetViewBag();
            return View(product);
        }
        public void SetViewBag(long? seletedID = null)
        {
            var dao = new Model.Dao.ProductDao();
            ViewBag.Category_ID = new SelectList(dao.ListAllCategory(), "ID", "Name", seletedID);
            ViewBag.Brand_ID = new SelectList(dao.ListAllBrand(), "ID", "Name", seletedID);
        }
    }
}