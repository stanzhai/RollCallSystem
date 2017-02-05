using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Mail;

namespace RollCallService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    public class RollCallService : IRollCallService
    {
        private RollCallDataContext dc = new RollCallDataContext();

        #region IRollCallService 成员

        public bool sendEmail(string sendTo, string subject, string content)
        {
            bool ret = false;
            MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(sendTo); //收件人

            //发件人信息
            msg.From = new MailAddress("jazzdan@jazzdan.co.cc", "点名系统", System.Text.Encoding.UTF8);
            msg.Subject = subject;   //邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8;    //标题编码
            msg.Body = content; //邮件主体
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;  //是否HTML
            msg.Priority = MailPriority.High;   //优先级

            SmtpClient client = new SmtpClient();
            //设置GMail邮箱和密码 
            client.Credentials = new System.Net.NetworkCredential("jazzdan@jazzdan.co.cc", "zsd325");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            object userState = msg;
            try
            {
                client.Send(msg);
                ret = true;
            }
            catch (Exception)
            {

            }
            return ret;
        }

        #endregion

        #region IRollCallService 成员

        public bool checkUser(string userName, string password, int permission)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 添加数据操作
        public int addClass(string className, string admin, string password, string phone)
        {
            int ret = -1;
            if (dc.ClassInfo.Where(t => t.ClassName == className).Count() == 0)
            {
                ClassInfo ci = new ClassInfo() { ClassName = className, Admin = admin, Password = password, Phone = phone };
                dc.ClassInfo.InsertOnSubmit(ci);
                dc.SubmitChanges();
                ret = ci.ID;
            }
            return ret;
        }

        public void addStudent(int no, string name, int classID)
        {
            try
            {
                if (dc.Student.Where(t => t.No == no).Count() == 0)
                {
                    Student s = new Student() { No = no, Name = name };
                    dc.Student.InsertOnSubmit(s);
                }
                if (dc.Student_Class.Where(t => t.ClassID == classID && t.StudentNo == no).Count() == 0)
                {
                    Student_Class sc = new Student_Class() { StudentNo = no, ClassID = classID };
                    dc.Student_Class.InsertOnSubmit(sc);
                }
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void addCourse(Guid courseID, string courseName, int classID)
        {
            try
            {
                if (dc.Course.Where(t => t.ID == courseID).Count() == 0)
                {
                    Course c = new Course() { ID = courseID, CourseName = courseName, ClassID = classID };
                    dc.Course.InsertOnSubmit(c);
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void addRecordIndex(Guid indexID, DateTime date, Guid courseID, int classID)
        {
            try
            {
                if (dc.RecordIndex.Where(t => t.ID == indexID).Count() == 0)
                {
                    RecordIndex ri = new RecordIndex() { ID = indexID, Date = date, CourseID = courseID, ClassID = classID };
                    dc.RecordIndex.InsertOnSubmit(ri);
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void addRecord(Guid recordID, int studentNo, string record, string remark, Guid recordIndex)
        {
            try
            {
                if (dc.Record.Where(t => t.ID == recordID).Count() == 0)
                {
                    Record r = new Record() { ID = recordID, StudentNo = studentNo, Record1 = record, Remark = remark, IndexID = recordIndex };
                    dc.Record.InsertOnSubmit(r);
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 数据修改操作

        public void updateClassInfo(int classID, string admin, string password, string phone)
        {
            try
            {
                ClassInfo ci = dc.ClassInfo.FirstOrDefault(t => t.ID == classID);
                if (ci != null)
                {
                    ci.Admin = admin;
                    ci.Password = password;
                    ci.Phone = phone;
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void updateStudent(int no, string name)
        {
            try
            {
                Student s = dc.Student.FirstOrDefault(t => t.No == no);
                if (s != null)
                {
                    s.Name = name;
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void updateCourse(Guid courseID, string courseName)
        {
            try
            {
                Course c = dc.Course.FirstOrDefault(t => t.ID == courseID);
                if (c != null)
                {
                    c.CourseName = courseName;
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void updateRecord(Guid recordID, string record, string remark)
        {
            try
            {
                Record r = dc.Record.FirstOrDefault(t => t.ID == recordID);
                if (r != null)
                {
                    r.Record1 = record;
                    r.Remark = remark;
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 删除操作
        public void deleteStudent(int no)
        {
            try
            {
                dc.Student_Class.DeleteAllOnSubmit(dc.Student_Class.Where(t => t.StudentNo == no));
                dc.Record.DeleteAllOnSubmit(dc.Record.Where(t => t.StudentNo == no));
                dc.Student.DeleteAllOnSubmit(dc.Student.Where(t => t.No == no));
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void deleteCourse(Guid id)
        {
            try
            {
                var indexes = dc.RecordIndex.Where(t => t.CourseID == id);
                foreach (RecordIndex r in indexes)
                {
                    dc.Record.DeleteAllOnSubmit(dc.Record.Where(t => t.IndexID == r.ID));
                }
                dc.RecordIndex.DeleteAllOnSubmit(indexes);
                dc.Course.DeleteAllOnSubmit(dc.Course.Where(t => t.ID == id));
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void deleteRecordIndex(Guid id)
        {
            try
            {
                var indexes = dc.RecordIndex.Where(t => t.ID == id);
                foreach (RecordIndex r in indexes)
                {
                    dc.Record.DeleteAllOnSubmit(dc.Record.Where(t => t.IndexID == r.ID));
                }
                dc.RecordIndex.DeleteAllOnSubmit(indexes);
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        // 判断一个班级是否存在
        public bool isClassExist(int classID)
        {
            return !(dc.ClassInfo.FirstOrDefault(t => t.ID == classID) == null);
        }
    }
}
