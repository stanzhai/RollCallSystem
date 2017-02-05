using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RollCallSystem.RollCallServiceReference;
using System.ComponentModel;

namespace RollCallSystem.Windows
{
    /// <summary>
    /// DataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DataWindow : Window
    {
        // WCF服务对象
        private RollCallServiceClient service = new RollCallServiceClient();
        // 数据操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
        // 更新进度条的事件委托
        public delegate void UpdateProgressBar();

        public void update()
        {
            this.upload.Visibility = System.Windows.Visibility.Visible;
        }

        private void loadInfo()
        {
            this.comboBoxRecords.Items.Clear();
            // 添加所有记录索引
            foreach (Codes.RecordIndex index in dc.RecordIndex.ToList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = String.Format("{0}    {1}", index.Date.ToString("F"), dc.Course.First(t => t.ID == index.CourseID).CourseName);
                item.Tag = index.ID;
                this.comboBoxRecords.Items.Add(item);
            }
        }

        /// <summary>
        /// 初始化服务接入点
        /// </summary>
        private void initService()
        {
            Codes.Settings s = dc.Settings.FirstOrDefault(t => t.Tag == "RollCall");
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(s.ServerPath);
            //service.Endpoint.ListenUri = new Uri(s.ServerPath);
        }

        /// <summary>
        /// 上传数据到服务器
        /// </summary>
        private void uploadData()
        {
            
            // 根据班级ID判断，这是第一次上传数据，先上传班级信息
            Codes.Settings settings = dc.Settings.First(t => t.Tag == "RollCall");
            int classID = settings.ClassID ?? -1;
            if (service.isClassExist(classID) == false)
            {
                classID = service.addClass(settings.ClassName, settings.Admin, settings.Password, settings.Phone);
                if (classID == -1)
                {
                    MessageBox.Show("检测到服务器中已存在同名的班级，数据上传被终止！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                settings.ClassID = classID;
                dc.SubmitChanges();
            }
            // 上传学生信息
            foreach (Codes.Student student in dc.Student.ToList())
            {
                service.addStudent(student.No, student.Name, classID);
            }
            // 上传课程信息
            foreach (Codes.Course course in dc.Course.ToList())
            {
                service.addCourse(course.ID, course.CourseName, classID);
            }
            // 上传记录索引信息
            foreach (Codes.RecordIndex recordIndex in dc.RecordIndex.ToList())
            {
                service.addRecordIndex(recordIndex.ID, recordIndex.Date, recordIndex.CourseID, classID);
            }
            // 上传点名记录
            foreach (Codes.Record record in dc.Record.ToList())
            {
                service.addRecord(record.ID, record.StudentNo, record.Contents, record.Remark, record.IndexID);
            }
        }

        /// <summary>
        /// 提交本地的数据更改
        /// </summary>
        private void submitChanges()
        {
            foreach (Codes.ChangeSet cs in dc.ChangeSet.ToList())
            {
                // 对数据表的操作类型：0更改，1删除
                short type = cs.Type;
                if (cs.TableName == "Settings")
                {
                    if (type == 0)
                    {
                        Codes.Settings s = dc.Settings.FirstOrDefault(t => t.Tag == "RollCall");
                        service.updateClassInfo((int)s.ClassID, s.Admin, s.Password, s.Phone);
                    }
                }
                if (cs.TableName == "Student")
                {
                    if (type == 0)
                    {
                        Codes.Student student = dc.Student.FirstOrDefault(t => t.No == cs.IntID);
                        if (student != null)
                        {
                            service.updateStudent((int)cs.IntID, student.Name);
                        }
                    }
                    else
                    {
                        service.deleteStudent((int)cs.IntID);
                    }
                }
                if (cs.TableName == "Course")
                {
                    if (type == 0)
                    {
                        Codes.Course course = dc.Course.FirstOrDefault(t => t.ID == cs.GuidID);
                        if (course != null)
                        {
                            service.updateCourse(cs.GuidID, course.CourseName);
                        }
                    }
                    else
                    {
                        service.deleteCourse(cs.GuidID);
                    }
                }
                if (cs.TableName == "Record")
                {
                    if (type == 0)
                    {
                        Codes.Record record = dc.Record.FirstOrDefault(t => t.ID == cs.GuidID);
                        if (record != null)
                        {
                            service.updateRecord(cs.GuidID, record.Contents, record.Remark);
                        }
                    }
                }
                if (cs.TableName == "RecordIndex")
                {
                    if (type == 1)
                    {
                        service.deleteRecordIndex(cs.GuidID);
                    }
                }
                // 删除无用的变更记录
                dc.ChangeSet.DeleteOnSubmit(cs);
            }
            dc.SubmitChanges();
        }

        public DataWindow()
        {
            InitializeComponent();
            gif.BitmapImage = Properties.Resources.loading;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadInfo();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBoxRecords.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要删除的点名记录！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show(String.Format("您确定要删除“{0}”这条记录吗？", this.comboBoxRecords.Text), "操作提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    // 获取当前操作的课程id
                    Guid id = (Guid)(this.comboBoxRecords.SelectedItem as ComboBoxItem).Tag;
                    // 删除所有记录
                    dc.Record.DeleteAllOnSubmit(dc.Record.Where(t => t.IndexID == id));
                    // 删除记录索引
                    dc.RecordIndex.DeleteOnSubmit(dc.RecordIndex.First(t => t.ID == id));
                    // 记录删除操作
                    Codes.ChangeSet cs = new Codes.ChangeSet() { TableName = "RecordIndex", GuidID = id, Type = 1};
                    dc.ChangeSet.InsertOnSubmit(cs);
                    dc.SubmitChanges();
                    loadInfo();
                }

            }
        }

        // 同步数据到服务器
        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.Dispatcher dispatcher = this.upload.Dispatcher;
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs ex)
            {
                this.upload.Visibility = System.Windows.Visibility.Hidden;
            };
            // 在单独的线程中上传数据
            worker.DoWork += delegate(object s, DoWorkEventArgs ex)
            {
                try
                {
                    UpdateProgressBar up = new UpdateProgressBar(update);
                    dispatcher.BeginInvoke(up);
                    initService();
                    uploadData();
                    // 更新本地数据库更改到服务器
                    submitChanges();
                    MessageBox.Show("已将所有数据上传到服务器！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception up)
                {
                    MessageBox.Show(up.Message, "上传错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("您确定要清空所有数据吗？", "操作提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                dc.Record.DeleteAllOnSubmit(dc.Record.ToList());
                dc.RecordIndex.DeleteAllOnSubmit(dc.RecordIndex.ToList());
                dc.Course.DeleteAllOnSubmit(dc.Course.ToList());
                dc.Student.DeleteAllOnSubmit(dc.Student.ToList());
                Codes.Settings settings = dc.Settings.First(t => t.Tag == "RollCall");
                settings.ClassID = null;
                settings.ClassName = null;
                dc.SubmitChanges();
            }
        }

    }
}
