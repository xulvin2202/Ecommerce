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
    public class BrandController : BaseController
    {
        EcommerceDbContext db = new EcommerceDbContext();
        // GET: Admin/Brand
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var dao = new BrandDao();
            var model = dao.ListBrand(page, pageSize);
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Image,CreateDate,Status,Category_ID")]Brand brand, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new BrandDao();
                    var filename = "";
                    var path = "";
                    if (image != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                        path = Path.Combine(Server.MapPath("~/Image"), filename);
                        image.SaveAs(path);
                        brand.Image = filename;
                    }
                    //else
                    //{

                    //    content.Image = "~/Image/logo.png";
                    //}
                    brand.Name = brand.Name;
                    brand.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                    brand.MetaTitle = StringHelper.ToUnsignString(brand.Name);
                    brand.Status = Convert.ToBoolean(true);
                    var id = dao.Insert(brand);
                    if (id > 0)
                    {
                        SetAlert("Thêm mới thành thành công", "success");
                        ViewBag.Success = "Thêm thành công";
                        brand = new Brand();
                        return RedirectToAction("Index", "Brand");
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
            return View(brand);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Name,Image,Metatitle,CreateDate,Status,Category_ID")]Brand brand, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                var dao = new BrandDao();
                var filename = "";
                var path = "";
                if (image != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                    path = Path.Combine(Server.MapPath("~/Image"), filename);
                    image.SaveAs(path);
                    brand.Image = filename;
                }
                //else
                //{
                //    content.Image = "~/Image/logo.png";
                //}
                brand.Name = brand.Name;
                brand.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                brand.MetaTitle = StringHelper.ToUnsignString(brand.Name);
                brand.Link = brand.Link;
                brand.Status = Convert.ToBoolean(true);
                var result = dao.Update(brand);
                if (result)
                {
                    SetAlert("Sửa thành công", "success");
                    ViewBag.Success = "Cập nhật thành công";
                    brand = new Brand();
                    return RedirectToAction("Index", "Brand");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật ko thành công");
                }

            }

            SetViewBag(brand.Category_ID);
            return View(brand);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new BrandDao().Delete(id);
            return RedirectToAction("Index");
        }

        public void SetViewBag(long? seletedID = null)
        {
            var dao = new Model.Dao.CategoryDao();
            ViewBag.Category_ID = new SelectList(dao.ListCategory(), "ID", "Name", seletedID);

        }
    }
}