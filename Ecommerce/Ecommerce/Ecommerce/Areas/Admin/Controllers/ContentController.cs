
using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        private EcommerceDbContext db = new EcommerceDbContext();

        // GET: Admin/Content
        public ActionResult Index(string searchStringContent, int page = 1, int pageSize = 5)
        {
            var dao = new ContentDao();
            var model = dao.ListAllContent(searchStringContent, page, pageSize);
            ViewBag.SearchStringContent = searchStringContent;
            return View(model);
        }        
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ContentDao().Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(long id)
        {
            var dao = new EcommerceDao();
            var content = dao.GetByID(id);
            SetViewBag(content.Content_Category_ID);
            return View();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,Description,Detail,Image,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescription,Content_Category_ID,Status")]Content model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();
                
                var a = new Content();
                var path = "";
                var filename = "";
                if (image != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                    path = Path.Combine(Server.MapPath("~/Image"), filename);
                    image.SaveAs(path);
                    model.Image = filename;
                }
                //else
                //{

                //    content.Image = "~/Image/logo.png";
                //}
                model.Name = model.Name;
                model.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                model.MetaTitle = StringHelper.ToUnsignString(model.Name);
                model.Description = model.Description;
                model.Detail = model.Detail;
                model.Content_Category_ID = model.Content_Category_ID;
                model.Status = Convert.ToBoolean(true);
                var result = dao.Update(model);
                if (result)
                {
                    SetAlert("Sửa thành công", "success");
                    ViewBag.Success = "Cập nhật thành công";
                    model = new Content();
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật ko thành công");
                }

            }
            SetViewBag(model.Content_Category_ID);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Description,Detail,Image,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescription,Content_Category_ID,Status")]Content content, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new ContentDao();
                    var filename = "";
                    var path = "";
                    if (image != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                        path = Path.Combine(Server.MapPath("~/Image"), filename);
                        image.SaveAs(path);
                        content.Image = filename;
                    }
                    //else
                    //{

                    //    content.Image = "~/Image/logo.png";
                    //}
                    content.Name = content.Name;
                    content.CreateDate = Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                    content.MetaTitle = StringHelper.ToUnsignString(content.Name);
                    content.Description = content.Description;
                    content.Detail = content.Detail;
                    content.Content_Category_ID = content.Content_Category_ID;
                    content.Status = Convert.ToBoolean(true);
                    var id = dao.Insert(content);
                    if (id > 0)
                    {
                        SetAlert("Thêm mới thành thành công", "success");
                        ViewBag.Success = "Thêm thành công";
                        content = new Content();
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
            return View(content);
        }
        public ActionResult Detail(int? id)
        {
            var dao = new ContentDao();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }
        public void SetViewBag(long? seletedID = null)
        {
            var dao = new Model.Dao.ContentDao();
            ViewBag.Content_Category_ID = new SelectList(dao.ListAllContentCategory(), "ID", "Name", seletedID);

        }
    }
}