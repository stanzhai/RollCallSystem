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
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Reflection;
using Forms = System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Animation;


namespace RollCallSystem.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 用于获取桌面背景路径的API
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private const int SPI_GETDESKWALLPAPER = 0x0073;
        // 记录屏幕分辨率
        int screenWidth, screenHeight;
        bool screenChanged = false;

        private bool logined = false;
        private DispatcherTimer timer = new DispatcherTimer();

        // 要启动的窗口标识
        private enum ShowWindow
        {
            LoginWindow,
            RollCallWindow,
            SettingsWindow,
            DataWindow,
            FeedbackWindow,
            AboutWindow
        }

        public MainWindow()
        {
            InitializeComponent();
            // 以桌面作为应用程序背景
            StringBuilder s = new StringBuilder(200);
            // 获取Window 桌面背景图片地址，使用缓冲区
            SystemParametersInfo(SPI_GETDESKWALLPAPER, 200, s, 0);
            // 缓冲区中字符进行转换
            string wallpaper_path = s.ToString();
            // 当桌面背景文件存在时，以桌面背景作为窗体背景
            if (System.IO.File.Exists(wallpaper_path))
            {
                // 设置桌面背景为应用程序的背景
                ImageBrush image = new ImageBrush();
                ImageSourceConverter converter = new ImageSourceConverter();
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(wallpaper_path);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                image.ImageSource = (ImageSource)converter.ConvertFrom(ms);
                this.Background = image;
            }

            // 获取屏幕宽度和高度
            this.screenWidth = Forms.Screen.PrimaryScreen.Bounds.Width;
            this.screenHeight = Forms.Screen.PrimaryScreen.Bounds.Height;
            // 如果屏幕分辨率过小，则调整屏幕分辨率
            if (this.screenWidth < 1024 || this.screenHeight < 768)
            {
                Codes.ScreenResolve.ChangeScreen(1024, 768, 60);
                screenChanged = true;
            }

        }

        private bool checkPermission()
        {
            if (!logined)
            {
                Codes.RollCallDataContext dc = Codes.DataOperation.getDataContext();
                if (dc.Settings.Where(t => t.Tag == "RollCall").Count() == 0)
                {
                    // 进行必要的应用程序设置
                    MessageBox.Show("信息尚未初始化，接下来请完应用程序配置任务！", "点名系统", MessageBoxButton.OK, MessageBoxImage.Information);
                    Windows.SettingsWindow sw = new Windows.SettingsWindow();
                    sw.ShowDialog();
                    return false;
                }
                else
                {
                    // ?? 为结合运算符，当可空类型为空时，logined=false
                    logined = new LoginWindow().ShowDialog() ?? false;
                }
            }
            return logined;
        }

        /// <summary>
        /// 利用反射机制，根据类名启动相应窗口，确保单实例
        /// </summary>
        /// <param name="sw">要显示的窗口标识</param>
        /// <param name="checkLogin">显示前是否验证使用者身份</param>
        private void showWindow(ShowWindow sw, bool checkLogin)
        {
            if (checkLogin == false || checkPermission())
            {
                bool shown = false;
                // 获取当前进程的程序集信息
                Assembly ass = Assembly.GetExecutingAssembly();
                string typeName = "RollCallSystem.Windows." + sw.ToString();
                // 判断窗口是否已经在运行
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == ass.GetType(typeName))
                    {
                        window.WindowState = System.Windows.WindowState.Normal;
                        window.Activate();
                        shown = true;
                    }
                }
                // 单独处理逻辑较为复杂的点名窗口
                if (sw.ToString() == "RollCallWindow")
                {
                    showRollCallWindow();
                    return;
                }

                // 没有启动则，利用反射机制实例化要启动的窗口
                if (!shown)
                {
                    Window window = ass.CreateInstance(typeName) as Window;
                    window.Topmost = true;
                    window.Show();
                    addStatusBarIcon(window);
                }
            }
        }

        // 窗体初始化
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // 显示时间
            this.textBlockTime.Text = DateTime.Now.ToString("F");
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        // 实时时间显示
        void timer_Tick(object sender, EventArgs e)
        {
            this.textBlockTime.Text = DateTime.Now.ToString("F");
        }


        /// <summary>
        /// 向模拟状态栏添加图标
        /// </summary>
        /// <param name="window"></param>
        private void addStatusBarIcon(Window window)
        {
            // 用于显示窗口缩略图内容
            Rectangle rectangle = new Rectangle()
            {
                Style = FindResource("ToolTipRectangleStyle") as Style,
                Fill = new VisualBrush()
                {
                    Visual = window
                },
            };

            // ToolTip边框，用于存放缩略图
            Border toolTipBorder = new Border()
            {
                Style = FindResource("ToolTipBorderStyle") as Style,
                Child = rectangle,
            };

            // 任务栏图标要现实的ToolTip
            ToolTip visualToolTip = new ToolTip
            {
                Content = toolTipBorder,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Placement = PlacementMode.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HasDropShadow = false,
                VerticalOffset = 3
            };

            // 任务栏图像
            Image thumbnail = new Image()
            {
                Width = 40,
                Height = 40,
                Margin = new Thickness(2),
                ToolTip = visualToolTip,
                Stretch = Stretch.Uniform,
                Source = window.Icon
            };

            thumbnail.ToolTipOpening += delegate(object sender, ToolTipEventArgs e)
            {
                // 定义窗口缩略图的大小
                double height = 160;
                double width = window.Width * height / window.Height;
                rectangle.Width = width - 8;
                rectangle.Height = height - 8;
                toolTipBorder.Width = width;
                toolTipBorder.Height = height;
            };

            // 控制缩略图延迟显示
            ToolTipService.SetInitialShowDelay(thumbnail, 20);
            // 任务栏图像边框
            Border hostBorder = new Border()
            {
                Style = FindResource("StatusBarItemStyle") as Style,
                Child = thumbnail
            };

            hostBorder.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (window.WindowState == System.Windows.WindowState.Normal)
                {
                    window.WindowState = System.Windows.WindowState.Minimized;
                }
                else
                {
                    window.WindowState = System.Windows.WindowState.Normal;
                }
            };

            window.MaxHeight = Forms.Screen.PrimaryScreen.Bounds.Height - 40;

            window.Closed += delegate(object sender, EventArgs e)
            {
                this.statusBar.Children.Remove(hostBorder);
            };

            // 将组合后的控件添加到任务栏
            this.statusBar.Children.Add(hostBorder);
        }

        /// <summary>
        /// 由于点名窗口的逻辑有些复杂，这里单独处理点名窗口
        /// </summary>
        private void showRollCallWindow()
        {
            RollCallOptionWindow option = new RollCallOptionWindow();
            if (option.ShowDialog() == true)
            {
                RollCallWindow rollcall = new RollCallWindow();
                rollcall.IsNew = option.IsNew;
                rollcall.RecordIndexID = option.RecordIndexID;
                rollcall.Show();
                addStatusBarIcon(rollcall);
            }
        }

        // 退出
        private void stackPanelExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Application.Current.Shutdown();
            }
        }

        // 点名
        private void stackPanelRollCall_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                showWindow(ShowWindow.RollCallWindow, true);
            }
        }

        // 设置
        private void stackPanelSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                showWindow(ShowWindow.SettingsWindow, true);
            }
        }

        // 数据
        private void stackPanelData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                showWindow(ShowWindow.DataWindow, true);
            }
        }

        // 反馈
        private void stackPanelFeddBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                showWindow(ShowWindow.FeedbackWindow, true);
            }
        }

        // 关于
        private void stackPanelAbout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                showWindow(ShowWindow.AboutWindow, false);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 屏幕分辨率发生变化时，还原为原来的分辨率
            if (this.screenChanged)
            {
                Codes.ScreenResolve.ChangeScreen(this.screenWidth, this.screenHeight, 60);
            }
        }

        // 模拟开始菜单
        private void start_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double left = -100;
            double top = this.screenHeight - 168 - 45;
            Canvas.SetLeft(menu, left);
            Canvas.SetTop(menu, top);
            if (this.menu.Visibility == System.Windows.Visibility.Hidden)
            {
                this.menu.Visibility = System.Windows.Visibility.Visible;
                Storyboard s = FindResource("StartMenu") as Storyboard;
                s.Begin();
            }
            else
            {
                this.menu.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void mainScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
        }

        // 模拟桌面右击
        private void mainScreen_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(this.mainScreen).X;
            double y = e.GetPosition(this.mainScreen).Y;
            Canvas.SetLeft(this.menu, x);
            Canvas.SetTop(this.menu, y);
            this.menu.Visibility = System.Windows.Visibility.Visible;
            Storyboard s = FindResource("popupMenu") as Storyboard;
            s.Begin();
        }

        #region 菜单事件处理

        private void menuRollCall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
            showWindow(ShowWindow.RollCallWindow, true);
        }

        private void menuSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
            showWindow(ShowWindow.SettingsWindow, true);
        }

        private void menuData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
            showWindow(ShowWindow.DataWindow, true);
        }

        private void menuFeedback_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
            showWindow(ShowWindow.FeedbackWindow, true);
        }

        private void menuAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.menu.Visibility = System.Windows.Visibility.Hidden;
            showWindow(ShowWindow.AboutWindow, false);
        }

        private void menuExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
