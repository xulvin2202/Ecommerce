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
    public class QandAController : BaseController
    {
        EcommerceDbContext db = new EcommerceDbContext();
        // GET: Admin/QandA
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include ="ID,Question,Answer,Image,Status,CreateDate")]QandA qandA,HttpPostedFileBase image)
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
    }
}