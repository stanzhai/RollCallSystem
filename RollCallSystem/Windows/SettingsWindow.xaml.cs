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
using Microsoft.Win32;

using DotNetSpeech;
namespace RollCallSystem.Windows
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        // TTS控制对象
        private SpVoice voice = new SpVoice();
        // Linq 数据库操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();

        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载所有课程
        /// </summary>
        private void loadCourses()
        {
            int index = this.listBoxCourses.SelectedIndex;
            this.listBoxCourses.Items.Clear();
            foreach (Codes.Course course in from i in dc.Course select i)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = course.CourseName;
                item.Tag = course.ID;
                item.Style = FindResource("ListBoxItemStyle") as Style;
                this.listBoxCourses.Items.Add(item);
            }
            if (index < this.listBoxCourses.Items.Count)
            {
                this.listBoxCourses.SelectedIndex = index;
            }
        }

        /// <summary>
        /// 验证用户输入的信息是否合法
        /// </summary>
        /// <returns></returns>
        private bool validateInfo()
        {
            if (this.textBoxAdmin.Text.Trim() == "")
            {
                MessageBox.Show("必须输入点名负责人", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.textBoxAdminPhone.Text.Trim() == "")
            {
                MessageBox.Show("必须输入负责人联系电话", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.textBoxClass.Text.Trim() == "")
            {
                MessageBox.Show("必须输入班级名", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.textBoxServer.Text.Trim() == "")
            {
                MessageBox.Show("必须输入服务器地址", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.passwordBox.Password == "")
            {
                MessageBox.Show("必须输入管理员密码", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        // 窗口加载时初始化所有已设置的数据
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = System.Windows.Visibility.Hidden;
            if (dc.Settings.Where(t => t.Tag == "RollCall").Count() != 0)
            {
                Codes.Settings settings = dc.Settings.First();
                // 初始化基本信息设置
                this.textBoxAdmin.Text = settings.Admin;
                this.textBoxAdminPhone.Text = settings.Phone;
                this.textBoxClass.Text = settings.ClassName;
                this.textBoxServer.Text = settings.ServerPath;
                this.passwordBox.Password = settings.Password;
                this.textBoxClass.IsReadOnly = settings.ClassID == null ? false : true;
                // 初始化语音设置
                short tts = settings.TTS ?? 3;
                switch (tts)
                {
                    case 1:
                        this.radioBtnTTS.IsChecked = true;
                        break;
                    case 2:
                        this.radioBtnFile.IsChecked = true;
                        break;
                    case 3:
                        this.radioBtnDisable.IsChecked = true;
                        break;
                    default:
                        break;
                }
                this.sliderRate.Value = settings.TtsrAte ?? 1;
            }
            // 加载课程信息
            loadCourses();
            this.dataGridStudent.ItemsSource = from i in dc.Student select i;
        }

        // 新建课程
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Windows.NewCourseWindow newCourseWindow = new NewCourseWindow();
            newCourseWindow.Title = "新建课程";
            if (newCourseWindow.ShowDialog() == true)
            {
                Codes.Course course = new Codes.Course()
                {
                    ID = Guid.NewGuid(),
                    CourseName = newCourseWindow.CourseName
                };
                dc.Course.InsertOnSubmit(course);
                dc.SubmitChanges();
                loadCourses();
            }
        }

        // 修改课程
        private void btnFix_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)this.listBoxCourses.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("您还没有选择要操作的课程！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Windows.NewCourseWindow newCourseWindow = new NewCourseWindow();
            newCourseWindow.CourseName = item.Content.ToString();
            newCourseWindow.Title = "修改课程";
            if (newCourseWindow.ShowDialog() == true)
            {
                Codes.Course course = dc.Course.First(t => t.ID == (Guid)item.Tag);
                course.CourseName = newCourseWindow.CourseName;
                // 保存修改记录
                Codes.ChangeSet cs = new Codes.ChangeSet() { TableName = "Course", GuidID = (Guid)item.Tag, Type = 0 };
                dc.ChangeSet.InsertOnSubmit(cs);

                dc.SubmitChanges();
                loadCourses();
            }
        }

        // 删除
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)this.listBoxCourses.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("您还没有选择要操作的课程！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("您确定要删除 “" + item.Content.ToString() + "” 这门课吗，和此课程相关的其他信息也将删除？", "操作提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var recordIndex = from i in dc.RecordIndex where i.CourseID == (Guid)item.Tag select i;
                foreach (Codes.RecordIndex ri in recordIndex)
                {
                    // 删除和课程相关的记录
                    dc.Record.DeleteAllOnSubmit(dc.Record.Where(t => t.IndexID == ri.ID));
                }
                // 删除和课程相关的索引
                dc.RecordIndex.DeleteAllOnSubmit(recordIndex);
                dc.Course.DeleteOnSubmit(dc.Course.First(t => t.ID == (Guid)item.Tag));

                // 保存修改记录
                Codes.ChangeSet cs = new Codes.ChangeSet() { TableName = "Course", GuidID = (Guid)item.Tag, Type = 1 };
                dc.ChangeSet.InsertOnSubmit(cs);

                dc.SubmitChanges();
                loadCourses();
            }
        }

        // 保存基本信息
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateInfo())
            {
                // 保存数据的更改
                if (dc.Settings.Where(a => a.Tag == "RollCall").Count() != 0)
                {
                    Codes.Settings settings = dc.Settings.First();
                    settings.Admin = this.textBoxAdmin.Text;
                    settings.ClassName = this.textBoxClass.Text;
                    settings.Password = this.passwordBox.Password;
                    settings.Phone = this.textBoxAdminPhone.Text;
                    settings.ServerPath = this.textBoxServer.Text;

                    // 保存修改记录
                    Codes.ChangeSet cs = new Codes.ChangeSet() { TableName = "Settings", Type = 0 };
                    dc.ChangeSet.InsertOnSubmit(cs);
                }
                else
                {
                    Codes.Settings settings = new Codes.Settings()
                    {
                        Tag = "RollCall",
                        Admin = this.textBoxAdmin.Text,
                        ClassName = this.textBoxClass.Text,
                        Password = this.passwordBox.Password,
                        Phone = this.textBoxAdminPhone.Text,
                        ServerPath = this.textBoxServer.Text
                    };
                    dc.Settings.InsertOnSubmit(settings);
                }
                dc.SubmitChanges();
                MessageBox.Show("信息已被成功保存！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // 批量导入学生信息
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "导入CSV文件";
            fileDialog.Filter = "CSV文件|*.csv";
            if (fileDialog.ShowDialog() == true)
            {
                List<Codes.Student> students = new List<Codes.Student>();
                // 分析数据
                string[] infos = System.IO.File.ReadAllLines(fileDialog.FileName, Encoding.Default);
                int count = 0, valid = 0;
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Contains(","))
                    {
                        string[] temp = infos[i].Split(',');
                        if (Codes.GlobalMethod.isNum(temp[0]))
                        {
                            int no = Convert.ToInt32(temp[0]);
                            if (dc.Student.Where(t => t.No == no).Count() == 0)
                            {
                                Codes.Student student = new Codes.Student() { No = no, Name = temp[1] };
                                students.Add(student);
                                count++;
                            }
                            valid++;
                        }
                    }
                }

                // 导入时的分析
                if (valid == 0)
                {
                    MessageBox.Show("没有检测到有效信息，文件格式错误，请以（学号，姓名）的形式导入！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (count == 0)
                {
                    MessageBox.Show("这些信息已被添加过，不允许重复添加！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (MessageBox.Show("检测到文件中包含" + count + "条有效信息，是否导入这些信息！", "操作提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dc.Student.InsertAllOnSubmit(students);
                    dc.SubmitChanges();
                    this.dataGridStudent.ItemsSource = from i in dc.Student select i;
                }
            }
        }

        // 添加一个学生信息
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            NewStudentWindow student = new NewStudentWindow();
            if (student.ShowDialog() == true)
            {
                int no = Convert.ToInt32(student.No);
                if (dc.Student.Where(t => t.No == no).Count() == 0)
                {
                    Codes.Student s = new Codes.Student() { No = no, Name = student.StudentName };
                    dc.Student.InsertOnSubmit(s);
                    dc.SubmitChanges();
                    // 更新控件的数据
                    this.dataGridStudent.ItemsSource = from i in dc.Student select i;
                }
                else
                {
                    MessageBox.Show("此学号为：" + student.No + "的同学已经存在！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnTTS_Click(object sender, RoutedEventArgs e)
        {
            voice.Rate = (int)this.sliderRate.Value;
            voice.Speak("测试成功", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        // 导出音频文件
        private void btnExportFile_Click(object sender, RoutedEventArgs e)
        {
            this.btnExportFile.IsEnabled = false;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (System.IO.Directory.Exists(path + "Sounds"))
            {
                string[] files = System.IO.Directory.GetFiles(path + "Sounds");
                foreach (string file in files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(path + "Sounds");
            }

            SpFileStream filestream = new SpFileStream();
            this.progressBar.Visibility = System.Windows.Visibility.Visible;
            this.progressBar.Maximum = dc.Student.Count();
            this.progressBar.Value = 0;
            foreach (Codes.Student student in dc.Student.ToList())
            {
                // wpf中没有DoEvents()不过幸好一个外国人帮忙写了类似的函数
                App.DoEvents();
                filestream.Open(path + "Sounds\\" + student.No + student.Name + ".wav", SpeechStreamFileMode.SSFMCreateForWrite, false);
                voice.AudioOutputStream = filestream;
                voice.Speak(student.Name.Trim(), SpeechVoiceSpeakFlags.SVSFlagsAsync);
                voice.WaitUntilDone(1000);
                filestream.Close();
                this.progressBar.Value++;
            }
            this.progressBar.Visibility = System.Windows.Visibility.Hidden;
            this.btnExportFile.IsEnabled = true;
            MessageBox.Show("音频文件导出成功", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 保存语音配置
        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            int tts;
            if (this.radioBtnTTS.IsChecked == true)
            {
                tts = 1;    // 使用TTS
            }
            else if (this.radioBtnFile.IsChecked == true)
            {
                tts = 2;    // 使用File
            }
            else
            {
                tts = 3;    // 禁用语音
            }

            if (dc.Settings.Where(a => a.Tag == "RollCall").Count() != 0)
            {
                Codes.Settings settings = dc.Settings.First();
                settings.TTS = (short)tts;
                settings.TtsrAte = (short)this.sliderRate.Value;
            }
            else
            {
                Codes.Settings settings = new Codes.Settings();
                settings.TTS = (short)tts;
                settings.TtsrAte = (short)this.sliderRate.Value;
                dc.Settings.InsertOnSubmit(settings);
            }
            dc.SubmitChanges();
            MessageBox.Show("信息已被成功保存！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
