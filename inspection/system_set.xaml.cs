using CefSharp.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace inspection
{
    /// <summary>
    /// system_set.xaml 的交互逻辑
    /// </summary>
    public partial class system_set : Page
    {
        public system_set()
        {
            InitializeComponent();
            // MessageBox.Show(system_set1.Height.ToString());
            //csf_web.Height = system_set1.Height;
            //csf_web.Width = system_set1.Width;

            //  SetFeatures(6000);
            //SetWebBrowserSilent(csf_web as WebBrowser, false);

            csf_web.Address = "http://localhost:8081/#/";
            //csf_web.Navigate(new Uri("http://www.baidu.com/"));
            //csf_web.Navigate(new Uri("http://127.0.0.1:8000/admin/"));


        }
        public static void SetFeatures(UInt32 ieMode)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }
        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            try
            {
                FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    object browser = fi.GetValue(webBrowser);
                    if (browser != null)
                        browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
                }
            }
            catch (Exception)
            {
                //LogInfo.saveLog("设置浏览器不弹错误提示框异常：" + ex);
                return;
            }
        }




    }
}
