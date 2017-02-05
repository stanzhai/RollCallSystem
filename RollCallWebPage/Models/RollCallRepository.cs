using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace RollCallWebPage.Models
{
    public class RollCallRepository
    {
        private RollCallDataContext dc = new RollCallDataContext();

        public IQueryable<ClassInfo> GetAllClasses()
        {
            return dc.ClassInfo;
        }

        public string GetClassName(int classID)
        {
            return (from i in dc.ClassInfo where i.ID == classID select i.ClassName).SingleOrDefault();
        }

        public string GetStudentName(int studentID)
        {
            return (from i in dc.Student where i.No == studentID select i.Name).SingleOrDefault();
        }

        public string GetCourseName(Guid courseID)
        {
            return (from i in dc.Course where i.ID == courseID select i.CourseName).SingleOrDefault();
        }

        /// <summary>
        /// 获取某个同学所在的班级
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public IQueryable<ClassInfo> GetMyClasses(int studentID)
        {
            return from i in dc.Student_Class where i.StudentNo == studentID select i.ClassInfo;
        }

        public List<ListInfoModel> GetListInfo(int classID, Guid? courseID)
        {
            List<ListInfoModel> li = new List<ListInfoModel>();
            // 获取一个班级的所有同学
            var students = dc.Student_Class.Where(t => t.ClassID == classID);
            foreach (Student_Class student in students)
            {
                ListInfoModel l = new ListInfoModel();
                l.StudentNo = student.StudentNo;
                l.Name = student.Student.Name;

                if (courseID.HasValue)
                {
                    l.ChiDaoCount = dc.Record.Where(t => t.RecordIndex.CourseID == courseID && t.StudentNo == student.StudentNo && t.Record1 == "迟到").Count();
                    l.KuangKeCount = dc.Record.Where(t => t.RecordIndex.CourseID == courseID && t.StudentNo == student.StudentNo && t.Record1 == "旷课").Count();
                    l.QingJiaCount = dc.Record.Where(t => t.RecordIndex.CourseID == courseID && t.StudentNo == student.StudentNo && t.Record1 == "请假").Count();
                    l.ZaoTuiCount = dc.Record.Where(t => t.RecordIndex.CourseID == courseID && t.StudentNo == student.StudentNo && t.Record1 == "早退").Count();

                }
                else
                {
                    l.ChiDaoCount = dc.Record.Where(t => t.RecordIndex.ClassID == classID && t.StudentNo == student.StudentNo && t.Record1 == "迟到").Count();
                    l.KuangKeCount = dc.Record.Where(t => t.RecordIndex.ClassID == classID && t.StudentNo == student.StudentNo && t.Record1 == "旷课").Count();
                    l.QingJiaCount = dc.Record.Where(t => t.RecordIndex.ClassID == classID && t.StudentNo == student.StudentNo && t.Record1 == "请假").Count();
                    l.ZaoTuiCount = dc.Record.Where(t => t.RecordIndex.ClassID == classID && t.StudentNo == student.StudentNo && t.Record1 == "早退").Count();

                }
                li.Add(l);
            }
            return li;
        }

        public IQueryable<DetailInfoModel> GetDetailInfo(int classID, int studentNo, Guid? courseID)
        {
            if (courseID.HasValue)
            {
                var t = from i in dc.Record
                        where i.StudentNo == studentNo && i.RecordIndex.ClassID == classID && i.RecordIndex.CourseID == courseID
                        select new DetailInfoModel()
                        {
                            Date = i.RecordIndex.Date,
                            Course = i.RecordIndex.Course.CourseName,
                            Record = i.Record1,
                            Remark = i.Remark
                        };
                return t;
            }
            else
            {
                var t = from i in dc.Record
                        where i.StudentNo == studentNo && i.RecordIndex.ClassID == classID
                        select new DetailInfoModel()
                        {
                            Date = i.RecordIndex.Date,
                            Course = i.RecordIndex.Course.CourseName,
                            Record = i.Record1,
                            Remark = i.Remark
                        };
                return t;
            }
        }
    }

    public class ListInfoModel
    {
        [DisplayName("学号")]
        public int StudentNo { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("迟到")]
        public int ChiDaoCount { get; set; }
        [DisplayName("旷课")]
        public int KuangKeCount { get; set; }
        [DisplayName("请假")]
        public int QingJiaCount { get; set; }
        [DisplayName("早退")]
        public int ZaoTuiCount { get; set; }
    }

    public class DetailInfoModel
    {
        public DateTime Date { get; set; }
        public string Course { get; set; }
        public string Record { get; set; }
        public string Remark { get; set; }
    }
}