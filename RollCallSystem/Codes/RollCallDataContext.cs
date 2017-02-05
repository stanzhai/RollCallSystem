#region Auto-generated classes for RollCallDataContext database on 2010-10-16 22:19:17Z

//
//  ____  _     __  __      _        _
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from RollCallDataContext on 2010-10-16 22:19:17Z
// Please visit http://linq.to/db for more information

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DbLinq.Data.Linq;

namespace RollCallSystem.Codes
{
    public partial class RollCallDataContext : DbLinq.Data.Linq.DataContext
    {
        public RollCallDataContext(System.Data.IDbConnection connection)
            : base(connection, new DbLinq.Sqlite.SqliteVendor())
        {
        }

        public RollCallDataContext(System.Data.IDbConnection connection, DbLinq.Vendor.IVendor vendor)
            : base(connection, vendor)
        {
        }

        public Table<ChangeSet> ChangeSet { get { return GetTable<ChangeSet>(); } }
        public Table<Course> Course { get { return GetTable<Course>(); } }
        public Table<Record> Record { get { return GetTable<Record>(); } }
        public Table<RecordIndex> RecordIndex { get { return GetTable<RecordIndex>(); } }
        public Table<Settings> Settings { get { return GetTable<Settings>(); } }
        public Table<Student> Student { get { return GetTable<Student>(); } }

    }

	[Table(Name = "main.ChangeSet")]
	public partial class ChangeSet
	{
		#region  GuidID

		private Guid _guidID;
		[DebuggerNonUserCode]
		[Column(Storage = "_guidID", Name = "GuidID", DbType = "guid")]
		public Guid GuidID
		{
			get
			{
				return _guidID;
			}
			set
			{
				if (value != _guidID)
				{
					_guidID = value;
				}
			}
		}

		#endregion

		#region int ID

		private int _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "ID", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
				}
			}
		}

		#endregion

		#region int? IntID

		private int? _intID;
		[DebuggerNonUserCode]
		[Column(Storage = "_intID", Name = "intID", DbType = "int")]
		public int? IntID
		{
			get
			{
				return _intID;
			}
			set
			{
				if (value != _intID)
				{
					_intID = value;
				}
			}
		}

		#endregion

		#region string TableName

		private string _tableName;
		[DebuggerNonUserCode]
		[Column(Storage = "_tableName", Name = "TableName", DbType = "nvarchar(32)", CanBeNull = false)]
		public string TableName
		{
			get
			{
				return _tableName;
			}
			set
			{
				if (value != _tableName)
				{
					_tableName = value;
				}
			}
		}

		#endregion

		#region short Type

		private short _type;
		[DebuggerNonUserCode]
		[Column(Storage = "_type", Name = "Type", DbType = "smallint", CanBeNull = false)]
		public short Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (value != _type)
				{
					_type = value;
				}
			}
		}

		#endregion

	}

    [Table(Name = "main.Course")]
    public partial class Course
    {
        #region string CourseName

        private string _courseName;
        [DebuggerNonUserCode]
        [Column(Storage = "_courseName", Name = "CourseName", DbType = "nvarchar(32)", CanBeNull = false)]
        public string CourseName
        {
            get
            {
                return _courseName;
            }
            set
            {
                if (value != _courseName)
                {
                    _courseName = value;
                }
            }
        }

        #endregion

        #region  ID

        private Guid _id;
        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "ID", DbType = "guid", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                }
            }
        }

        #endregion

    }

    [Table(Name = "main.Record")]
    public partial class Record
    {
        #region string Contents

        private string _contents;
        [DebuggerNonUserCode]
        [Column(Storage = "_contents", Name = "Record", DbType = "nvarchar(2)", CanBeNull = false)]
        public string Contents
        {
            get
            {
                return _contents;
            }
            set
            {
                if (value != _contents)
                {
                    _contents = value;
                }
            }
        }

        #endregion

        #region  ID

        private Guid _id;
        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "ID", DbType = "guid", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                }
            }
        }

        #endregion

        #region  IndexID

        private Guid _indexID;
        [DebuggerNonUserCode]
        [Column(Storage = "_indexID", Name = "IndexID", DbType = "guid", CanBeNull = false)]
        public Guid IndexID
        {
            get
            {
                return _indexID;
            }
            set
            {
                if (value != _indexID)
                {
                    _indexID = value;
                }
            }
        }

        #endregion

        #region string Remark

        private string _remark;
        [DebuggerNonUserCode]
        [Column(Storage = "_remark", Name = "Remark", DbType = "nvarchar(32)")]
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (value != _remark)
                {
                    _remark = value;
                }
            }
        }

        #endregion

        #region int StudentNo

        private int _studentNo;
        [DebuggerNonUserCode]
        [Column(Storage = "_studentNo", Name = "StudentNo", DbType = "integer", CanBeNull = false)]
        public int StudentNo
        {
            get
            {
                return _studentNo;
            }
            set
            {
                if (value != _studentNo)
                {
                    _studentNo = value;
                }
            }
        }

        #endregion

    }

    [Table(Name = "main.RecordIndex")]
    public partial class RecordIndex
    {
        #region  CourseID

        private Guid _courseID;
        [DebuggerNonUserCode]
        [Column(Storage = "_courseID", Name = "CourseID", DbType = "guid", CanBeNull = false)]
        public Guid CourseID
        {
            get
            {
                return _courseID;
            }
            set
            {
                if (value != _courseID)
                {
                    _courseID = value;
                }
            }
        }

        #endregion

        #region System.DateTime Date

        private System.DateTime _date;
        [DebuggerNonUserCode]
        [Column(Storage = "_date", Name = "\"Date\"", DbType = "datetime", CanBeNull = false)]
        public System.DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                }
            }
        }

        #endregion

        #region  ID

        private Guid _id;
        [DebuggerNonUserCode]
        [Column(Storage = "_id", Name = "ID", DbType = "guid", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                }
            }
        }

        #endregion

    }

    [Table(Name = "main.Settings")]
    public partial class Settings
    {
        #region string Admin

        private string _admin;
        [DebuggerNonUserCode]
        [Column(Storage = "_admin", Name = "Admin", DbType = "nvarchar(32)")]
        public string Admin
        {
            get
            {
                return _admin;
            }
            set
            {
                if (value != _admin)
                {
                    _admin = value;
                }
            }
        }

        #endregion

        #region int? ClassID

        private int? _classID;
        [DebuggerNonUserCode]
        [Column(Storage = "_classID", Name = "ClassID", DbType = "int")]
        public int? ClassID
        {
            get
            {
                return _classID;
            }
            set
            {
                if (value != _classID)
                {
                    _classID = value;
                }
            }
        }

        #endregion

        #region string ClassName

        private string _className;
        [DebuggerNonUserCode]
        [Column(Storage = "_className", Name = "ClassName", DbType = "nvarchar(32)")]
        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                if (value != _className)
                {
                    _className = value;
                }
            }
        }

        #endregion

        #region string Password

        private string _password;
        [DebuggerNonUserCode]
        [Column(Storage = "_password", Name = "Password", DbType = "nvarchar(32)")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                }
            }
        }

        #endregion

        #region string Phone

        private string _phone;
        [DebuggerNonUserCode]
        [Column(Storage = "_phone", Name = "Phone", DbType = "nvarchar(16)")]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                }
            }
        }

        #endregion

        #region string ServerPath

        private string _serverPath;
        [DebuggerNonUserCode]
        [Column(Storage = "_serverPath", Name = "ServerPath", DbType = "nvarchar(32)")]
        public string ServerPath
        {
            get
            {
                return _serverPath;
            }
            set
            {
                if (value != _serverPath)
                {
                    _serverPath = value;
                }
            }
        }

        #endregion

        #region string Tag

        private string _tag;
        [DebuggerNonUserCode]
        [Column(Storage = "_tag", Name = "Tag", DbType = "nvarchar(4)", IsPrimaryKey = true, CanBeNull = false)]
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (value != _tag)
                {
                    _tag = value;
                }
            }
        }

        #endregion

        #region short? TTS

        private short? _tts;
        [DebuggerNonUserCode]
        [Column(Storage = "_tts", Name = "TTS", DbType = "smallint")]
        public short? TTS
        {
            get
            {
                return _tts;
            }
            set
            {
                if (value != _tts)
                {
                    _tts = value;
                }
            }
        }

        #endregion

        #region short? TtsrAte

        private short? _ttsrAte;
        [DebuggerNonUserCode]
        [Column(Storage = "_ttsrAte", Name = "TTSRate", DbType = "smallint")]
        public short? TtsrAte
        {
            get
            {
                return _ttsrAte;
            }
            set
            {
                if (value != _ttsrAte)
                {
                    _ttsrAte = value;
                }
            }
        }

        #endregion

    }


    [Table(Name = "main.Student")]
    public partial class Student
    {
        #region string Name

        private string _name;
        [DebuggerNonUserCode]
        [Column(Storage = "_name", Name = "Name", DbType = "nvarchar(4)", CanBeNull = false)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
            }
        }

        #endregion

        #region int No

        private int _no;
        [DebuggerNonUserCode]
        [Column(Storage = "_no", Name = "No", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int No
        {
            get
            {
                return _no;
            }
            set
            {
                if (value != _no)
                {
                    _no = value;
                }
            }
        }

        #endregion

    }
}
