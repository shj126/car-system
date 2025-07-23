using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace inspection
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)　　　　　//该重写函数实现在程序退出时关闭某个进程
        {
            Process[] myProgress;
            myProgress = Process.GetProcesses();　　　　　　　　　　//获取当前启动的所有进程
            foreach (Process p in myProgress)　　　　　　　　　　　　//关闭当前启动的Excel进程
            {
                if (p.ProcessName == "mqttdemo")　　　　　　　　　　//通过进程名来寻找
                {
                    p.Kill();
                    return;
                }
            }
            base.OnExit(e);
        }
    }
}
