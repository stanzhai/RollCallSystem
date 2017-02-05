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
    /// RollCallOptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RollCallOptionWindow : Window
    {
        // 数据库操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
        // 记录索引的ID
        public Guid RecordIndexID
        {
            get
            {
                ComboBoxItem item = this.comboBoxRecordIndex.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    return (Guid)(item).Tag;
                }
                else
                {
                    return Guid.NewGuid();
                }
            }
        }
        // 判断是否为新建的课程
        public bool IsNew
        {
            get
            {
                if (this.radiobtnNew.IsChecked ?? false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public RollCallOptionWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化记录索引
            var indexes = from i in dc.RecordIndex orderby i.Date descending select i;
            foreach (Codes.RecordIndex index in indexes)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = index.Date.ToString("F") + "    " + dc.Course.First(t => t.ID == index.CourseID).CourseName;
                item.Tag = index.ID;
                this.comboBoxRecordIndex.Items.Add(item);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNew && this.comboBoxRecordIndex.SelectedIndex == -1)
            {
                MessageBox.Show("必须选定一次点名记录！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
        }

        private void radiobtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.comboBoxRecordIndex != null)
            {
                this.comboBoxRecordIndex.IsEnabled = this.radiobtnOld.IsChecked ?? false;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
