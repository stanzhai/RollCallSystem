using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollCallSystem.Codes
{
    class GlobalMethod
    {
        /// <summary>
        /// 判断学号格式是否正确
        /// </summary>
        /// <param name="scr"></param>
        /// <returns></returns>
        public static bool isNum(string scr)
        {
            try
            {
                Convert.ToInt32(scr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
