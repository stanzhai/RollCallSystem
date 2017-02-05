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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RollCallSystem.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.passwordBox.Focus();
        }

        // 验证用户
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
            if (dc.Settings.First().Password == this.passwordBox.Password)
            {
                this.DialogResult = true;
            }
            else
            {
                this.passwordBox.Password = "";
                this.passwordBox.Focus();
                MessageBox.Show("对不起，您输入的管理员密码有错误，请重新输入！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.btnOK_Click(this.btnOK, RoutedEventArgs.Empty as RoutedEventArgs);
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
