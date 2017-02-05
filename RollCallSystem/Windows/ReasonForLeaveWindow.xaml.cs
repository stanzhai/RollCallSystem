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
    /// ReasonForLeaveWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReasonForLeaveWindow : Window
    {
        // 设置提示信息
        public string Info
        {
            set { this.textBlockReason.Text = value + "同学请假了，请输入TA的请假理由："; }
        }

        /// <summary>
        /// 请假原因
        /// </summary>
        public string Reason
        {
            set { this.textBoxReason.Text = value; }
            get { return this.textBoxReason.Text; }
        }

        public ReasonForLeaveWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBlockReason.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBoxReason.Text.Trim() == "")
            {
                this.textBoxReason.Text = "未说明";
            }
            this.DialogResult = true;
        }


        private void btnIll_Click(object sender, RoutedEventArgs e)
        {
            this.textBoxReason.Text = "生病";
            this.DialogResult = true;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.textBoxReason.Text = "回家";
            this.DialogResult = true;
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
