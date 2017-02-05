using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
namespace RollCallSystem.Codes
{
    class DataOperation
    {
        /// <summary>
        /// 获取DataContext对象
        /// </summary>
        /// <returns></returns>
        public static RollCallDataContext getDataContext()
        {
            // SQLite数据库连接字符串
            bool exit = System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "RollCall.db");
            string connStr = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "RollCall.db;password=jazzdan325";
            RollCallDataContext dc =  new RollCallDataContext(new System.Data.SQLite.SQLiteConnection(connStr));
            return dc;
        }
    }
}
