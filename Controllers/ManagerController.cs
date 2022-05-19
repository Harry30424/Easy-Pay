using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MidTest.Models;

namespace MidTest.Controllers
{
    public class ManagerController : Controller
    {
        DatabaseEntities2 db = new DatabaseEntities2();
        // GET: Manager
        public ActionResult Login()
        {
            ViewBag.count = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string account, string password,string password1, int count)
        {
            if(count>=3)
            {
                return RedirectToAction("Register");
            }
            ViewData["login"] = account;
            ViewData["password"] = password;
            ViewData["password1"] = password1;
            ViewBag.count = count + 1;
            var mb = db.Member.Where(x => x.account.Equals(account) && x.password.Equals(password)).FirstOrDefault();
            if (mb != null)
            {
                TempData["count"] = ViewBag.count;
                //Session["pass"] = true;
                return RedirectToAction("Create");
                
            }
            else
            {
                ViewData["flag"] = false;
            }
            //if (account.Equals("admin") && password.Equals("1234"))
            //{
            //    TempData["msg"] = "您已成功登入!";
            //    return RedirectToAction("Demo");
            //}
            //else
            //{
            //    ViewData["flag"] = false;
            //}
            TempData["msg1"] = "登入失敗次數過多，請重新註冊!";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string account, string password, string password1)
        {
            Member mb = new Member();
            //Member mb = db.Member.Where(x => x.account.Equals(account) && x.password.Equals(password) && x.password1.Equals(password1)).FirstOrDefault();
            mb.account = account;
            mb.password = password;
            mb.password1 = password1;
            if(account == "")
            {
                if(password != password1)
                {
                    return new EmptyResult();
                }
            }
            if(account != "")
            {
                if(password == password1)
                {
                    db.Member.Add(mb);
                    db.SaveChanges();
                    TempData["msg"] = "註冊成功!";
                    return RedirectToAction("Login");
                }
            }
            TempData["msg1"] = "註冊失敗!";
            return View();
        }

        public ActionResult Demo()
        {
            //if (Session["pass"] == null)
            //{
            //    return RedirectToAction("Login");
            //}
            var t = db.T.ToList();
            return View(t);
        }
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(string m_id, string m_name, string m_option,string m_pay,string m_dollar)
        {
            T rec = new T();
            rec.m_id = m_id;
            rec.m_name = m_name;
            rec.m_option = m_option;
            rec.m_pay = m_pay;
            rec.m_dollar = m_dollar;
            rec.m_created_date = DateTime.Now;
            //try
            //{
            //    db.T.Add(rec);
            //    db.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
            db.T.Add(rec);
            db.SaveChanges();
            TempData["msg"] = "您已完成線上預購!";

            return RedirectToAction("Demo");
            //var t = db.T.ToList();
            //return View("Demo",t);
        }

        [HttpPost]
        public ActionResult Edit(int id, string m_option, string update, string function)
        {
            T rec = db.T.Find(id);
            if (update != null)
            {
                if (update.Equals("修改"))
                {
                    rec.m_option = m_option;
                    TempData["msg"] = "您已完成線上預購修改!";
                }
            }
            if (function != null)
            {
                if (function.Equals("刪除"))
                {
                    db.T.Remove(rec);
                    TempData["msg"] = "您已完成線上預購刪除!";
                }
            }

            db.SaveChanges();



            return RedirectToAction("Demo");
            //var t = db.T.ToList();
            //return View("Demo",t);
        }
    }
}