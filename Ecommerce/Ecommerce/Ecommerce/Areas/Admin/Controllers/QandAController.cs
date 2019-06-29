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
    public class QandAController : BaseController
    {
        EcommerceDbContext db = new EcommerceDbContext();
        // GET: Admin/QandA
        public ActionResult Index(int page = 1, int pageSize = 8)
        {
            var dao = new QandADao();
            var model = dao.ListQ(page,pageSize);
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(/*[Bind(Include ="ID,Question,Answer,Image,Status,CreateDate")]*/QandA qandA,HttpPostedFileBase image)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var dao = new QandADao();
                    var path = "";
                    var filename = "";
                    if(image != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                        path = Path.Combine(Server.MapPath("~/Image"), filename);
                        image.SaveAs(path);
                        qandA.Image = filename;
                    }
                    qandA.Question = qandA.Question;
                    qandA.Answer = qandA.Answer;
                    qandA.MetaTitle = StringHelper.ToUnsignString(qandA.Question);
                    qandA.CreateDate= Convert.ToDateTime(DateTime.UtcNow.ToLocalTime());
                    qandA.Status = Convert.ToBoolean(true);
                    var id = dao.Insert(qandA);
                    if (id > 0)
                    {
                        SetAlert("Thêm mới thành thành công", "success");
                        ViewBag.Success = "Thêm thành công";
                        qandA = new QandA();
                        return RedirectToAction("Index", "QandA");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm mới ko thành công");
                    }
                }
            }
            catch(DbEntityValidationException e)
            {
                throw e;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View(qandA);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new QandADao();
            var qanda = dao.GetByID(id);
            return View();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Question,Answer,MetaTitle,CreateDate,Status")]QandA qandA)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new QandADao();
                    qandA.Question = qandA.Question;
                    qandA.Answer = qandA.Answer;
                    qandA.MetaTitle = StringHelper.ToUnsignString(qandA.Question);
                    qandA.Status = Convert.ToBoolean(true);
                    var result = dao.Update(qandA);
                    if (result)
                    {
                        SetAlert("Sửa thành công", "success");
                        ViewBag.Success = "Cập nhật thành công";
                        qandA = new QandA();
                        return RedirectToAction("Index", "QandA");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật ko thành công");
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
            return View(qandA);
        }
        public ActionResult Detail(int? id)
        {
            var dao = new QandADao();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QandA qandA = db.QandAs.Find(id);
            if (qandA == null)
            {
                return HttpNotFound();
            }
            return View(qandA);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new QandADao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}