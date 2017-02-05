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
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace RollCallSystem.Windows
{
    /// <summary>
    /// FeedbackWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FeedbackWindow : Window
    {
        // 更新进度条的事件委托
        public delegate void UpdateProgressBar();
        // WCF服务对象
        private RollCallServiceClient service = new RollCallServiceClient();
        // 数据操作对象
        private Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();

        private string author, admin, adminEmail;

        private void startBackWork(bool toAuthor)
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
                    if (toAuthor)
                    {
                        feedbackToAuthor();
                    }
                    else
                    {
                        feedbackToAdmin();
                    }
                }
                catch (Exception up)
                {
                    MessageBox.Show(up.ToString(), "上传错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        public void update()
        {
            this.upload.Visibility = System.Windows.Visibility.Visible;
        }

        private void feedbackToAuthor()
        {
            // 反馈信息给作者
            if (this.author != "")
            {
                Codes.Settings s = dc.Settings.FirstOrDefault(t => t.Tag == "RollCall");

                service.sendEmail("jazzdan325@163.com", "点名系统——信息反馈",
                    String.Format("{0}的{1}({2})向您反馈如下信息：<br />{3}",
                    s.ClassName, s.Admin, s.Phone, this.author.Replace("\r\n", "<br />")));

                MessageBox.Show("您反馈的信息已发送到作者邮箱，谢谢！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("必须输入有效的反馈信息！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void feedbackToAdmin()
        {
            // 反馈信息给管理员
            Regex r = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            Match m = r.Match(this.adminEmail);
            if (!m.Success)
            {
                MessageBox.Show("您输入了错误的邮箱", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.admin == "")
            {
                MessageBox.Show("必须输入有效的反馈信息！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Codes.Settings s = dc.Settings.FirstOrDefault(t => t.Tag == "RollCall");

                service.sendEmail(adminEmail, "点名系统——信息反馈",
                    String.Format("{0}的{1}({2})向您反馈如下信息：<br />{3}",
                    s.ClassName, s.Admin, s.Phone, this.admin.Replace("\r\n", "<br />")));
                MessageBox.Show("您反馈的信息已发送到指定邮箱，谢谢！", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 初始化服务接入点
        /// </summary>
        private void initService()
        {
            Codes.Settings s = dc.Settings.FirstOrDefault(t => t.Tag == "RollCall");
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(s.ServerPath);
        }

        public FeedbackWindow()
        {
            InitializeComponent();
        }

        private void btnAuthor_Click(object sender, RoutedEventArgs e)
        {
            startBackWork(true);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            startBackWork(false);
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gif.BitmapImage = Properties.Resources.loading;
        }

        private void textBoxAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.author = this.textBoxAuthor.Text.Trim();
        }

        private void textBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.admin = this.textBoxContent.Text.Trim();
        }

        private void textBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.adminEmail = this.textBoxEmail.Text;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
