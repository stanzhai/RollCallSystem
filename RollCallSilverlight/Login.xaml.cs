using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RollCallSilverlight
{
    public partial class Login : ChildWindow
    {
        // 用于判断是否验证通过
        private bool pased = false;

        public Login()
        {
            InitializeComponent();
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!pased)
            {
                e.Cancel = true;
            }
        }
    }
}

