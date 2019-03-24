using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class AdminChatController : Controller
    {
        // GET: AdminChat
        public ActionResult Index(int id)
        {
            if (id != 0)
            {
                HttpCookie cookie = new HttpCookie("userid", id + "");
                cookie.Expires = DateTime.Now.AddDays(10);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            return View();
        }
    }
}