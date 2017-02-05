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
    /// NewStudentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewStudentWindow : Window
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string No
        {
            get { return this.textBoxNo.Text; }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string StudentName
        {
            get { return this.textBoxName.Text; }
        }

        public NewStudentWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!Codes.GlobalMethod.isNum(this.textBoxNo.Text))
            {
                this.textBoxNo.Focus();
                MessageBox.Show("抱歉，学号格式错误！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (this.textBoxName.Text.Trim() == "")
            {
                this.textBoxNo.Focus();
                MessageBox.Show("抱歉，姓名不能为空！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.btnOK_Click(this.btnOK, EventArgs.Empty as RoutedEventArgs);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxNo.Focus();
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
