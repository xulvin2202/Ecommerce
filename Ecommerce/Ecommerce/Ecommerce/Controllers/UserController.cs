using BotDetect.Web.UI.Mvc;
using Common;
using Ecommerce.Common;
using Ecommerce.Models;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class UserController : Controller
    {
        EcommerceDbContext db = new EcommerceDbContext();
        //public ActionResult Acc()
        //{
        //    Session[Constants.USER_ID] = true;
        //    return Redirect(Request.UrlReferrer.ToString());
        //}
        //public ActionResult SigninSuccess()
        //{
        //    if (Session[Constants.USER_ID] == null)
        //    {
        //        return PartialView("SigninNull");
        //    }
        //    else
        //    {
        //        var item = from t in db.Users
        //                   select t;
        //        return PartialView(item.SingleOrDefault());
        //    }
        //}


        //public ActionResult SigninNull()
        //{
        //    return PartialView();
        //}
        ////public ActionResult SigninSuccess()
        ////{

        ////    var acc = from t in db.Users
        ////              select t;
        ////    return PartialView(acc.SingleOrDefault());
        ////}

        //public bool isLoginUser()
        //{
        //    if (Session[Constants.USER_ID] == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public bool isLogined()
        //{
        //    if (Session[Constants.USER_ID] == null || Session[Constants.USER_PASSWORD] == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        //public bool isLoginSucess(string id, string password)
        //{
        //    //Check user's id and password
        //    foreach (User a in db.Users)
        //    {
        //        if (a.UserName.Replace(" ", "") == id)
        //        {
        //            if (a.Password.Replace(" ", "") == password)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckUserEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.GroupID = Convert.ToString("MEMBER");
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = int.Parse(model.DistrictID);
                    }

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}