using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.ComponentModel;

namespace inspection
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
      
        system_set system_set;
        fault_list fault_List;
        stu stu;
        tools tools;
        image_mx image_mx;
        ele ele;
        Process pro;
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            if (Identity.status)
            {
                //fault_expan.Visibility = Visibility.Collapsed;
                //btn_setting.Visibility = Visibility.Collapsed;
            }
            try
            {
                string pathURL = AppDomain.CurrentDomain.BaseDirectory + "\\mqtt\\mqttdemo.exe";
                // System.Diagnostics.Process.Start(@"C:\Users\刘雨顺同志\source\repos\mqttdemo\mqttdemo\bin\Release\net461\mqttdemo.exe");    //调用该命令，打开mqtt
                /*string pathURL = @"C:\Users\刘雨顺同志\source\repos\mqttdemo\mqttdemo\bin\Release\net461\mqttdemo.exe"; */                    //需要打开的exe的路径，根据实际填写
                pro = new Process();
                pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     //把窗口隐藏，使其在后台运行
                pro.StartInfo.FileName = pathURL;       //设置要打开的exe程序的路径
                pro.Start();       //启动exe程序 
            }
            catch
            {
                // HandyControl.Controls.MessageBox.Error("打开故障设置程序失败");
            }
                

            /*MessageBox.Show(Student.Sid.ToString());*/
            this.WindowState = WindowState.Maximized;
            //cef.ZoomLevelIncrement = 0.5;

            stu = new stu();
            ContentControl.Content = new Frame()
            {
                Content = stu
            };
        }



        private void Window_Closed(object sender, EventArgs e)
        {

            Console.WriteLine("233 CLosed");

            HandyControl.Controls.MessageBox.Error("Closed");

            try
            {
                string[] appList = new string[] { "mqttdemo", "Hantek365", "系统设置", "EOBD", "DPSApp", "Meter", "PWM", "OSC9822", "_cache_mqttdemo", "._cache_系统设置", "._cache_EOBD", "._cache_DPSApp", "._cache_Meter", "._cache_PWM", "._cache_OSC980" , "._cache_Hantek365" };

                for (int x = 0; x < appList.Length; x++)
                {
                    Process[] proc = Process.GetProcessesByName(appList[x]);

                    Console.WriteLine(proc);

                    for (int i = 0; i < proc.Length; i++)
                    {
                        proc[i].Kill();  //逐个结束进程.
                    }
                }

                Environment.Exit(0); // 结束进程

            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
         
            system_set = new system_set();
            ContentControl.Content = new Frame()
            {
                Content = system_set
            };

        }


        private void btn_list_Click_1(object sender, RoutedEventArgs e)
        {

            fault_List = new fault_list();
            ContentControl.Content = new Frame()
            {
                Content = fault_List
            };


        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            system_set = new system_set();
            ContentControl.Content = new Frame()
            {
                Content = system_set
            };
        }

    /*    private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            cef.Visibility = Visibility.Visible;
            cef.Address = "http://127.0.0.1:9000/admin/";
            cef.Height = 1000;
        }
*/
    /*    private void RadioButton_Click_3(object sender, RoutedEventArgs e)
        {
            cef.Visibility = Visibility.Collapsed;
            fault = new fault();
            ContentControl.Content = new Frame()
            {
                Content = fault
            };
        }*/

   /*     private void RadioButton_Click_4(object sender, RoutedEventArgs e)
        {
            cef.Visibility = Visibility.Visible;
            //string html = File.ReadAllText(@"F:/index.html");
            //cef.LoadHtml(html);
            cef.Address = "http://43.248.186.198:8802/";
            //cef.Address = "http://192.168.31.144:8081";
            //cef.Address = "http://html5test.com/";
            //cef.Address = "https://v.qq.com/x/page/p3270350w9n.html";
            cef.Height = 800;
        }
*/
        private void btn_sut_Click(object sender, RoutedEventArgs e)
        {
            stu = new stu();
            ContentControl.Content = new Frame()
            {
                Content = stu
            };
        }

        private void btn_tools_Click(object sender, RoutedEventArgs e)
        {
            tools = new tools();
            ContentControl.Content = new Frame()
            {
                Content = tools
            };
        }

        private void image_man_Click(object sender, RoutedEventArgs e)
        {
            image_mx = new image_mx();
            ContentControl.Content = new Frame()
            {
                Content = image_mx
            };
        }

        private void ele_button_Click(object sender, RoutedEventArgs e)
        {
            ele = new ele();
            ContentControl.Content = new Frame()
            {
                Content = ele
            };
        }

        private void btn_setting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\setting\\系统设置.exe");    //调用该命令，打开系统设置
            }
            catch
            {
                HandyControl.Controls.MessageBox.Error("打开系统设置失败");
            }
        }

        private void btn_decs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\decoder\\EOBD.exe");
            }
            catch
            {
                HandyControl.Controls.MessageBox.Error("打开系统设置失败");
            }
        }
    }
}
