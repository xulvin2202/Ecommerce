﻿using Common;
using Ecommerce.Models;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        EcommerceDbContext db = new EcommerceDbContext();

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
        private const string CartSession = "CartSession";
        // GET: Cart
        
        public ActionResult Index()
        {
            //if (isLogined() == false)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            //if (isLogined() == false)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        [HttpGet]
        public ActionResult Payment()
        {

            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.Total = Total();
            return View(list);
        }
        private decimal? Total()
        {
            decimal? iTotal = 0;
            List<CartItem> lstCart = Session["CartItem"] as List<CartItem>;
            if (lstCart != null )
            {
                iTotal = lstCart.Sum(x => x.TotalPrice);
            }
            return iTotal;
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {

            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new Model.Dao.OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    if(item.Product.PromotionPrice != null)
                    {
                        total += (item.Product.PromotionPrice.GetValueOrDefault(0) * item.Quantity);
                    }
                    else
                    {
                        total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                    }
                    //total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới", content);
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            Session[CartSession] = null;
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult ErrorPayment()
        {
            return View();
        }
    }
}