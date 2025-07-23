using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;

namespace inspection
{
    /// <summary>
    /// tools.xaml 的交互逻辑
    /// </summary>
    public partial class tools : Page
    {
        public tools()
        {
            InitializeComponent();
        }

        private void btn_sb_Click(object sender, RoutedEventArgs e)
        {
            String filepath = @"C:\OSC9822 V2.8.8 汽修示波器\\OSC9822.exe";
            try
            {
                System.Diagnostics.Process.Start(filepath);
            }
            catch
            {
                MessageBox.Error("打开失败");
            }
            
        }

        private void btn_wy_Click(object sender, RoutedEventArgs e)
        {
            String filepath = @"C:\Program Files (x86)\\Hantek365\\Hantek365.exe";
            try
            {
                System.Diagnostics.Process.Start(filepath);
            }
            catch
            {
                MessageBox.Error("打开失败");
            }
        }

        private void btn_dec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\decoder\\EOBD.exe");  
            }
            catch
            {
                MessageBox.Error("打开失败");
            }
        }
    }
}
