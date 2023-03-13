using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLySpare.Models;

namespace WebQuanLySpare.Controllers
{
    public class HomeController : Controller
    {
        WebSparePartEntities db = new WebSparePartEntities();
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_admin ad)
        {
            tbl_admin a = db.tbl_admin.Where(x => x.ad_name == ad.ad_name && x.ad_pass == ad.ad_pass).SingleOrDefault();
            if (a != null)
            {
                int idla = a.ad_id;
                Session["ad_id"] = a.ad_id;
                return RedirectToAction("Index","danhsachlinhkiens");
            }
            else
            {
                ViewBag.msg = "Sai tên đăng nhập hoặc mật khẩu";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }

        public ActionResult danhsachlinhkien()
        {
            
            return View();
        }
        

        public ActionResult About()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}