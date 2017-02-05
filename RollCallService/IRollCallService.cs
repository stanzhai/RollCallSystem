using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RollCallService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IRollCallService
    {
        [OperationContract]
        bool sendEmail(string sendTo, string subject, string content);

        [OperationContract]
        bool checkUser(string userName, string password, int permission);

        #region 添加信息
        [OperationContract]
        int addClass(string className, string admin, string password, string phone);

        [OperationContract]
        void addStudent(int no, string name, int classID);

        [OperationContract]
        void addCourse(Guid courseID, string courseName, int classID);

        [OperationContract]
        void addRecordIndex(Guid indexID, DateTime date, Guid courseID, int classID);

        [OperationContract]
        void addRecord(Guid recordID, int studentNo, string record, string remark, Guid recordIndex);
        #endregion

        [OperationContract]
        bool isClassExist(int classID);

        #region 修改信息

        [OperationContract]
        void updateClassInfo(int classID, string admin, string password, string phone);

        [OperationContract]
        void updateStudent(int no, string name);

        [OperationContract]
        void updateCourse(Guid courseID, string courseName);

        [OperationContract]
        void updateRecord(Guid recordID, string record, string remark);
        #endregion

        #region 删除信息
        [OperationContract]
        void deleteStudent(int no);

        [OperationContract]
        void deleteCourse(Guid id);

        [OperationContract]
        void deleteRecordIndex(Guid id);
        #endregion
    }


    //// 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
