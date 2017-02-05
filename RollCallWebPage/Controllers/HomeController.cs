using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RollCallWebPage.Models;
using ChineseIndex;
using System.Web.Security;
namespace RollCallWebPage.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string fileName = Server.MapPath("~") + "/index.content";
            if (System.IO.File.Exists(fileName))
            {
                ViewData["Content"] = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default);
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //[Authorize(Roles="SuperAdmin")]
        public ActionResult EditIndex()
        {
            string fileName = Server.MapPath("~") + "/index.content";
            if (System.IO.File.Exists(fileName))
            {
                ViewData["Content"] = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default);
            }
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditIndex(FormCollection collection)
        {
            string fileName = Server.MapPath("~") + "/index.content";
            System.IO.File.WriteAllText(fileName, collection["Content"], System.Text.Encoding.Default);
            return RedirectToAction("Index");
        }
    }
}
