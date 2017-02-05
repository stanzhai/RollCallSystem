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

using DotNetSpeech;
using System.Windows.Interop;
namespace RollCallSystem.Windows
{
    /// <summary>
    /// RollCallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RollCallWindow : Window
    {
        // 定义用于显示和控制的控件
        private TextBlock[] names;
        private ComboBox[] infos;

        // 数据库操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
        // 定义状态
        private string[] info = new string[] { "已到", "迟到", "旷课", "请假", "假中", "早退" };
        // 定义不同的状态所使用的颜色
        private SolidColorBrush[] colors = new SolidColorBrush[] { 
            new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            new SolidColorBrush(Color.FromRgb(255, 0, 0)),
            new SolidColorBrush(Color.FromRgb(0, 255, 255)),
            new SolidColorBrush(Color.FromRgb(255, 0, 255)),
            new SolidColorBrush(Color.FromRgb(0, 255, 0)),
            new SolidColorBrush(Color.FromRgb(255, 255, 0)),};

        // TTS控制对象
        private SpVoice voice = new SpVoice();
        // 音频文件播放对象
        private System.Media.SoundPlayer sound;

        #region 重新点名时会用到的一些变量

        private Guid recordIndexID;
        public Guid RecordIndexID
        {
            set { this.recordIndexID = value; }
            get { return recordIndexID; }
        }

        private bool isNew = false;
        public bool IsNew
        {
            set { this.isNew = value; }
            get { return isNew; }
        }

        private Guid[] recordIDs;
        private string[] remarks;
        // 用户记录XX同学的记录是否被改动，为了提高效率减少数据冗余
        private string[] oldContent;
        // 用于记录每一个同学姓名是否被语音播放过
        private bool[] played;
        // 使用何种语音播放方式，1：TTS，2：file，3：None
        private int tts;

        #endregion

        public RollCallWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 语音点名
        /// </summary>
        /// <param name="index">同学列表索引</param>
        private void speakName(int index)
        {
            if (played[index] == false && (index == 0 || (index != 0 && played[index - 1])))
            {
                if (this.tts == 1)
                {
                    // 先停止以前的播放
                    voice.Speak(string.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                    voice.Speak(names[index].Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                }
                else if (this.tts == 2)
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + "Sounds\\" + names[index].Tag + names[index].Text + ".wav";
                    sound = new System.Media.SoundPlayer(file);
                    sound.Play();
                }
                played[index] = true;
            }
        }

        /// <summary>
        /// 将数据保存到数据库
        /// </summary>
        private void saveToDataBase()
        {
            if (IsNew)
            {
                Windows.SelectCourseWindow course = new Windows.SelectCourseWindow();
                course.ShowDialog();
                Guid id = course.ID;
                Guid recordID = RecordIndexID;
                // 保存记录索引
                Codes.RecordIndex index = new Codes.RecordIndex()
                {
                    ID = recordID,
                    CourseID = id,
                    Date = DateTime.Now
                };
                dc.RecordIndex.InsertOnSubmit(index);
            }

            // 保存所有学生记录
            for (int i = 0; i < names.Length; i++)
            {
                // 一个学生的迟到情况
                string contents = info[infos[i].SelectedIndex];
                contents = contents == "已到" ? "" : contents;
                if (contents == "请假" && remarks[i] == "")
                {
                    ReasonForLeaveWindow reason = new ReasonForLeaveWindow();
                    reason.Info = names[i].Text;
                    reason.ShowDialog();
                    remarks[i] = reason.Reason;
                }
                // 判断是新键数据，还是修改数据
                if (IsNew)
                {
                    Codes.Record record = new Codes.Record()
                    {
                        ID = Guid.NewGuid(),
                        IndexID = RecordIndexID,
                        Contents = contents,
                        Remark = remarks[i],
                        StudentNo = Convert.ToInt32(names[i].Tag)
                    };
                    dc.Record.InsertOnSubmit(record);
                }
                else
                {
                    // 修改数据，本来有更简单的修改方法的，使用这种方法实属无奈，貌似实现Linq To SQLite 的Dblinq有问题
                    foreach (Codes.Record record in dc.Record.ToList())
                    {
                        // 只有记录被修改才保存
                        if (record.ID == recordIDs[i] && oldContent[i] != contents)
                        {
                            record.Contents = contents;
                            record.Remark = remarks[i];

                            // 保存修改记录
                            Codes.ChangeSet cs = new Codes.ChangeSet() { TableName = "Record", GuidID = record.ID, Type = 0 };
                            dc.ChangeSet.InsertOnSubmit(cs);
                        }
                    }
                }
            }
            // 提交数据更改
            dc.SubmitChanges();
        }

        private void loadAsNew()
        {
            // 获取数据库学生信息
            var students = from i in dc.Student orderby i.No select i;
            int count = students.Count();
            if (count == 0)
            {
                MessageBox.Show("没有学生信息，请先完善信息！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
                SettingsWindow settings = new SettingsWindow();
                settings.Show();
                this.Close();
            }
            else
            {
                remarks = new string[count];
                // 初始化控件数组
                names = new TextBlock[count];
                infos = new ComboBox[count];
                played = new bool[count];
                // 实例化每一个控件
                int i = 0;

                foreach (Codes.Student student in students)
                {
                    played[i] = false;
                    remarks[i] = "";
                    // 实例化TextBlock控件
                    names[i] = new TextBlock();
                    names[i].Style = FindResource("TextBlockStyle") as Style;

                    names[i].Text = student.Name;
                    names[i].Tag = student.No;

                    this.mainCanvas.Children.Add(names[i]);
                    Canvas.SetLeft(names[i], 10 + 150 * (i / 25));
                    Canvas.SetTop(names[i], 8 + 27 * (i % 25));
                    // 实例化Combox控件
                    infos[i] = new ComboBox();
                    infos[i].Tag = i;
                    infos[i].Width = 60;
                    infos[i].ItemsSource = info;
                    infos[i].SelectedIndex = 0;
                    infos[i].Style = FindResource("ComboBoxStyle") as Style;
                    infos[i].MouseEnter += new MouseEventHandler(ComboBoxInfo_MouseEnter);
                    infos[i].SelectionChanged += new SelectionChangedEventHandler(ComboBoxInfo_SelectionChanged);
                    this.mainCanvas.Children.Add(infos[i]);
                    Canvas.SetLeft(infos[i], 70 + 150 * (i / 25));
                    Canvas.SetTop(infos[i], 9 + 27 * (i % 25));
                    i++;
                }
            }
        }

        private void loadAsOld()
        {
            // 重新点名时按记录初始化控件
            var oldRecords = from t in dc.Record where t.IndexID == RecordIndexID orderby t.StudentNo select t;
            int count = oldRecords.Count();
            // 初始化控件数组
            names = new TextBlock[count];
            infos = new ComboBox[count];
            // 保存原有点名相关信息
            recordIDs = new Guid[count];
            remarks = new string[count];
            oldContent = new string[count];
            played = new bool[count];

            int i = 0;
            foreach (Codes.Record record in oldRecords)
            {
                recordIDs[i] = record.ID;
                remarks[i] = record.Remark;
                oldContent[i] = record.Contents;
                played[i] = false;

                // 实例化TextBlock控件
                names[i] = new TextBlock();
                names[i].Style = FindResource("TextBlockStyle") as Style;
                names[i].Text = dc.Student.First(t => t.No == record.StudentNo).Name;
                names[i].Tag = record.StudentNo;

                this.mainCanvas.Children.Add(names[i]);
                Canvas.SetLeft(names[i], 10 + 150 * (i / 25));
                Canvas.SetTop(names[i], 8 + 27 * (i % 25));
                // 实例化Combox控件
                infos[i] = new ComboBox();
                infos[i].Tag = i;
                infos[i].Width = 60;
                infos[i].ItemsSource = info;
                infos[i].Text = record.Contents == "" ? "已到" : record.Contents;
                infos[i].Style = FindResource("ComboBoxStyle") as Style;
                infos[i].MouseEnter += new MouseEventHandler(ComboBoxInfo_MouseEnter);
                infos[i].SelectionChanged += new SelectionChangedEventHandler(ComboBoxInfo_SelectionChanged);
                this.mainCanvas.Children.Add(infos[i]);
                Canvas.SetLeft(infos[i], 70 + 150 * (i / 25));
                Canvas.SetTop(infos[i], 9 + 27 * (i % 25));
                ComboBoxInfo_SelectionChanged(infos[i], EventArgs.Empty as SelectionChangedEventArgs);
                i++;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "点名系统 - " + dc.Settings.First().ClassName + " - 点名负责人：" + dc.Settings.First().Admin;
            if (IsNew)
            {
                loadAsNew();
            }
            else
            {
                loadAsOld();
            }
            Codes.Settings settings = dc.Settings.First(t => t.Tag == "RollCall");
            this.tts = settings.TTS ?? 3;
            // TTS的播放速率
            this.voice.Rate = settings.TtsrAte ?? 1;
        }

        // 鼠标经过时让控件得到焦点
        void ComboBoxInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            c.Focus();
            speakName(Convert.ToInt32(c.Tag));
        }

        // 当迟到状态改变时，更改不同的颜色
        void ComboBoxInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox s = sender as ComboBox;
            names[Convert.ToInt32(s.Tag)].Foreground = colors[s.SelectedIndex];
            s.Foreground = colors[s.SelectedIndex];
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 退出窗口时提示保存信息
            switch (MessageBox.Show("是否保存本次的点名记录？", "操作提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Yes:
                    saveToDataBase();
                    break;
                default:
                    break;
            }
        }
    }
}
