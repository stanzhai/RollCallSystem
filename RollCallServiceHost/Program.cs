using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using System.ServiceModel;
using RollCallService;
using System.ServiceModel.Description;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace RollCallServiceHost
{
    sealed class Host
    {
        private NotifyIcon notifyIcon;
        private ContextMenu notificationMenu;
        public ServiceHost serviceHost = new ServiceHost(typeof(RollCallService.RollCallService));
        private string port;

        #region 信息初始化
        public Host()
        {
            // 初始化托盘图标
            notifyIcon = new NotifyIcon();
            notificationMenu = new ContextMenu(InitializeMenu());
            notificationMenu.Popup += new EventHandler(notificationMenu_Popup);

            notifyIcon.DoubleClick += IconDoubleClick;
            notifyIcon.Icon = Properties.Resources.host;
            notifyIcon.ShowBalloonTip(200, "提示信息", "点名系统WCF宿主程序,已启动！", ToolTipIcon.Info);
            notifyIcon.Text = "点名系统WCF宿主程序";
            notifyIcon.ContextMenu = notificationMenu;

            loadPortSettings();
        }

        /// <summary>
        /// 加载端口号设置
        /// </summary>
        private void loadPortSettings()
        {
            string file = Application.StartupPath + "\\port.json";
            if (System.IO.File.Exists(file))
            {
                string content = System.IO.File.ReadAllText(file, System.Text.Encoding.Default);
                JObject json = JObject.Parse(content);
                this.port = (string)json["Port"];
                if (this.port != "")
                {
                    addServiceEndPoint(this.port);
                }
            }
        }

        /// <summary>
        /// 保存端口号设置
        /// </summary>
        private void savePortSettings()
        {
            string file = Application.StartupPath + "\\port.json";
            string content = JsonConvert.SerializeObject(new { Port = this.port });
            System.IO.File.WriteAllText(file, content);
        }

        private void notificationMenu_Popup(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey next = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (next.GetValue("RollCallServiceHost") == null)
            {
                notificationMenu.MenuItems[2].Checked = false;
            }
            else
            {
                notificationMenu.MenuItems[2].Checked = true;
            }
        }

        private MenuItem[] InitializeMenu()
        {
            MenuItem[] menu = new MenuItem[] {
				new MenuItem("关于", menuAboutClick),
                new MenuItem("-"),
                new MenuItem("随系统启动", menuAutoStartClick),
                new MenuItem("-"),
				new MenuItem("退出", menuExitClick)                
			};
            return menu;
        }

        // 添加服务终结点
        private void addServiceEndPoint(string port)
        {
            serviceHost.AddServiceEndpoint(
                typeof(RollCallService.IRollCallService), // 服务契约
                new System.ServiceModel.BasicHttpBinding(), // 绑定的协议类型
                String.Format("http://localhost:{0}/RollCallService", port)); // Uri
        }

        #endregion

        #region 应用程序入口

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool isFirstInstance;

            using (Mutex mtx = new Mutex(true, "RollCall", out isFirstInstance))
            {
                if (isFirstInstance)
                {

                    Host host = new Host();
                    try
                    {
                        host.notifyIcon.Visible = true;
                        host.notifyIcon.ShowBalloonTip(200, "提示信息", "点名系统WCF宿主程序已启动！", ToolTipIcon.Info);

                        // 打开服务，监听消息
                        host.serviceHost.Open();

                        Application.Run();
                        // 关闭WCF服务
                        host.serviceHost.Close();
                    }
                    catch (TimeoutException timeProblem)
                    {
                        MessageBox.Show(timeProblem.Message, "错误提示");
                    }
                    catch (CommunicationException commProblem)
                    {
                        MessageBox.Show(commProblem.Message, "错误提示");
                    }
                    finally
                    {
                        host.notifyIcon.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("宿主应用程序已在运行中！", "点名系统WCF宿主程序");
                }
            } // releases the Mutex
        }
        #endregion

        #region 菜单事件处理
        private void menuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("作者：翟士丹@曲阜师范大学 zYz Team 火山软件小组", "关于  点名系统WCF宿主程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuExitClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定退出本程序吗？退出后所有客户端将无法连接服务器！", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                savePortSettings();
                Application.Exit();
            }
        }

        private void menuAutoStartClick(object sender, EventArgs e)
        {
            if ((sender as MenuItem).Checked == true)
            {
                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey next = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                next.DeleteValue("RollCallServiceHost");
            }
            else
            {
                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey next = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                next.SetValue("RollCallServiceHost", Application.ExecutablePath);
            }
        }

        private void IconDoubleClick(object sender, EventArgs e)
        {
            Settings s = new Settings();
            s.Port = this.port;
            if (s.ShowDialog() == DialogResult.OK)
            {
                if (this.port != s.Port)
                {
                    // 使设置生效
                    addServiceEndPoint(s.Port);
                    this.port = s.Port;
                }
            }
        }

        #endregion
    }
}
