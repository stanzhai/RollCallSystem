using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RollCallWebPage.Models;
namespace RollCallWebPage.Controllers
{
    public class RollCallController : Controller
    {
        private RollCallRepository rollCall = new RollCallRepository();

        //
        // GET: /ClassInfo/
        [Authorize(Roles="SuperAdmin")]
        public ActionResult Index()
        {
            ViewData["Title"] = "已上传的班级";
            return View(rollCall.GetAllClasses());
        }

        [Authorize(Roles="SuperAdmin,Admin")]
        public ActionResult ListInfo(int id)
        {
            ViewData["Title"] = rollCall.GetClassName(id) + " 的点名记录";
            ViewData["ClassID"] = id;
            var model = rollCall.GetListInfo(id, null);
            if (model.Count == 0)
            {
                return View("NotFound");
            }
            else
            {
                string fileName = Server.MapPath("~") + "/temp.csv";
                Codes.CreateCSV.CreateListCSV(model, fileName);
                return View(model);
            }
        }

        [Authorize(Roles="Student")]
        public ActionResult MyClasses(int id)
        {
            ViewData["Title"] = "我所在的班级列表";
            ViewData["StudentNo"] = id;
            return View(rollCall.GetMyClasses(id));
        }

        // 通用，查看某同学的详细记录
        [Authorize]
        public ActionResult DetailInfo(int id, int id2)
        {
            ViewData["Title"] = rollCall.GetStudentName(id2) + " 同学的详细记录";
            ViewData["ClassID"] = id;
            ViewData["StudentNo"] = id2;
            string fileName = Server.MapPath("~") + "/temp.csv";
            var model = rollCall.GetDetailInfo(id, id2, null);
            Codes.CreateCSV.CreateDetailCSV(model, fileName);
            return View(model);
        }

        [Authorize]
        public FilePathResult DownloadListInfo(int id)
        {
            string fileName = Server.MapPath("~") + "/temp.csv";
            // 下载文件
            return File(fileName, "text/plain", rollCall.GetClassName(id) + ".csv");
        }

        [Authorize]
        public FilePathResult DownloadDetailInfo(int id)
        {
            string fileName = Server.MapPath("~") + "/temp.csv";
            // 下载文件
            return File(fileName, "text/plain", rollCall.GetStudentName(id) + ".csv");
        }
    }
}
