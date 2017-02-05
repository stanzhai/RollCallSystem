using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RollCallWebPage.Models;

namespace RollCallWebPage.Controllers
{
    public class TeacherController : Controller
    {
        private RollCallRepository rollCall = new RollCallRepository();

        //
        // GET: /Teacher/
        [Authorize(Roles = "Teacher")]
        public ActionResult Index(int id, Guid id2)
        {
            ViewData["Title"] = rollCall.GetClassName(id) + rollCall.GetCourseName(id2)+ " 的点名记录";
            ViewData["ClassID"] = id;
            var model = rollCall.GetListInfo(id, id2);
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

        // 给教师用，以查看这位同学在这门课的记录
        [Authorize(Roles = "Teacher")]
        public ActionResult DetailInfo(int id, int id2, Guid id3)
        {
            ViewData["Title"] = rollCall.GetStudentName(id2) + " 同学的详细记录";
            ViewData["ClassID"] = id;
            ViewData["StudentNo"] = id2;
            var model = rollCall.GetDetailInfo(id, id2, id3);
            if (model.Count() == 0)
            {
                return View("NotFound");
            }
            else
            {
                string fileName = Server.MapPath("~") + "/temp.csv";
                Codes.CreateCSV.CreateDetailCSV(model, fileName);
                return View(model);
            }
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
