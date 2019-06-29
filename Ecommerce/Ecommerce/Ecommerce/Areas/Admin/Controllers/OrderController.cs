using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        EcommerceDbContext db = new EcommerceDbContext();
        // GET: Admin/Order
        public ActionResult Index(int page=1,int pageSize=10)
        {
            var dao = new OrderDao();
            var model = dao.ListOrder(page,pageSize);
            return View(model);
        }
    }
}