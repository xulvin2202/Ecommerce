using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        public ActionResult Index( int page = 1, int pageSize = 5)
        {
            var dao = new ContactDao();
            var model = dao.ListAllFeedback( page, pageSize);
            return View(model);
        }
    }
}