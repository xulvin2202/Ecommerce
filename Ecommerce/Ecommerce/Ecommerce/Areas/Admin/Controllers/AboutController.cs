using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        // GET: Admin/About
        public ActionResult Index(long id)
        {
            var dao = new AboutDao();           
            var model = dao.LstAbout(id);       
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new AboutDao();
            var content = dao.GetByID(id);
            return View();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Detail")]About model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AboutDao();
                model.Detail = model.Detail;               
                var result = dao.Update(model);
                if (result)
                {                    
                    ViewBag.Success = "Cập nhật thành công";
                    model = new About();
                    return RedirectToAction("Index", "About");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật ko thành công");
                }

            }
            return View();
        }
    }
}