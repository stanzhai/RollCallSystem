using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RollCallWebPage.Models;
namespace RollCallWebPage.Codes
{
    public class CreateCSV
    {
        public static void CreateListCSV(List<ListInfoModel> infos, string fileName)
        {
            string content = String.Format("{0},{1},{2},{3},{4},{5}\r\n", "学号", "姓名", "迟到", "旷课", "请假", "早退");
            foreach (ListInfoModel li in infos)
            {
                content += String.Format("{0},{1},{2},{3},{4},{5}\r\n", li.StudentNo, li.Name, li.ChiDaoCount, li.KuangKeCount, li.QingJiaCount, li.ZaoTuiCount);
            }
            // 写文件
            System.IO.File.WriteAllText(fileName, content, System.Text.Encoding.UTF8);
        }

        public static void CreateDetailCSV(IQueryable<DetailInfoModel> infos, string fileName)
        {
            string content = String.Format("{0},{1},{2},{3}\r\n", "日期", "课程", "记录", "备注");
            foreach (DetailInfoModel di in infos)
            {
                content += String.Format("{0},{1},{2},{3}\r\n", di.Date, di.Course, di.Record, di.Remark);
            }
            // 写文件
            System.IO.File.WriteAllText(fileName, content, System.Text.Encoding.UTF8);
        }
    }
}