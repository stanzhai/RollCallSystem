using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using RollCallWebPage.Models;
using ChineseIndex;

namespace RollCallWebPage.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            UserValidate uv = new UserValidate();
            return View(uv);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOn(UserValidate uv)
        {
            if (!ModelState.IsValid)
            {
                return View(uv);
            }
            else
            {
                RollCallDataContext dc = new RollCallDataContext();
                // 验证用户身份
                if (uv.Identity == "Student")
                {
                    var student = dc.Student.SingleOrDefault(t => t.No.ToString() == uv.UserName);
                    if (student != null && WordsIndex.getWordsIndex(student.Name).ToLower() == uv.Password)
                    {
                        // 学生身份验证通过
                        addCookie(student.Name + "同学", "Student", uv.RememberMe);
                        Session["StudentNo"] = uv.UserName;
                        return RedirectToAction("MyClasses", "RollCall", new { id = uv.UserName });
                    }
                }
                else if (uv.Identity == "Teacher")
                {
                    foreach (ClassInfo ci in dc.ClassInfo)
                    {
                        if (WordsIndex.getWordsIndex(ci.ClassName).ToLower() == uv.UserName)
                        {
                            foreach (Course course in dc.Course.Where(t => t.ClassID == ci.ID))
                            {
                                if (WordsIndex.getWordsIndex(course.CourseName).ToLower() == uv.Password)
                                {
                                    // 教师身份登录
                                    addCookie(ci.ClassName + "的" + course.CourseName + "老师", "Teacher", uv.RememberMe);
                                    Session["ClassID"] = ci.ID;
                                    Session["CourseID"] = course.ID;
                                    return RedirectToAction("Index", "Teacher", new { id = ci.ID, id2 = course.ID });
                                }
                            }
                        }
                        break;
                    }
                }
                else if (uv.Identity == "Admin")
                {
                    var admin = dc.ClassInfo.SingleOrDefault(t => t.Phone == uv.UserName);
                    if (admin != null && admin.Password == uv.Password)
                    {
                        // 班级管理员身份登录
                        addCookie("点名负责人" + admin.Admin, "Admin", uv.RememberMe);
                        Session["ClassID"] = admin.ID;
                        return RedirectToAction("ListInfo", "RollCall", new { id = admin.ID });
                    }
                    else
                    {
                        if (uv.UserName == "admin" && uv.Password == "nimda")
                        {
                            // 超级管理员身份
                            addCookie("超级管理员", "SuperAdmin", uv.RememberMe);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "您输入的用户名或密码不正确");
                return View(uv);
            }
        }

        private void addCookie(string userName, string roles, bool remember)
        {
            // 创建票据
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                remember,
                roles,
                FormsAuthentication.FormsCookiePath
                );
            // 将票据加密到cookie
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket));
            // 返回cookies
            Response.Cookies.Add(cookie);
        }
    }
}
