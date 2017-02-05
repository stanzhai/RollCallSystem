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
    /// SelectCourseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCourseWindow : Window
    {
        // 定义数据库操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
        
        public Guid ID
        {
            get { return (Guid)(this.comboBoxCourse.SelectedItem as ComboBoxItem).Tag; }
        }

        public SelectCourseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 加载课程数据
            var courses = from i in dc.Course select i;
            foreach (Codes.Course course in courses)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = course.CourseName;
                item.Tag = course.ID;
                this.comboBoxCourse.Items.Add(item);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.comboBoxCourse.SelectedIndex == -1)
            {
                // 必须选择课程才能退出本窗口
                MessageBox.Show("必须选择课程！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
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
