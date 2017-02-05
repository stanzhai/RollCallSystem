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

namespace RollCallSystem.Windows
{
    /// <summary>
    /// NewCourseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewCourseWindow : Window
    {
        // 课程名
        private string courseName;

        // 保存课程名属性
        public string CourseName
        {
            set { courseName = value; }
            get { return this.textBoxCourse.Text;}
        }

        public NewCourseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 用户修改课程名时，时时显示要修改的课程名
            this.textBoxCourse.Text = courseName;
            this.textBoxCourse.Focus();
        }

        // 返回文本框中的值
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBoxCourse.Text.Trim() == "")
            {
                this.textBoxCourse.Text = "";
                this.textBoxCourse.Focus();
                MessageBox.Show("课程名不允许为空！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void textBoxCourse_KeyDown(object sender, KeyEventArgs e)
        {
            // 处理在文本框上的回车按键
            if (e.Key == Key.Return)
            {
                this.btnOK_Click(this.btnOK, null);
            }
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
    }
}
