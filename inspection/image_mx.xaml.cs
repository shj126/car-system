using CefSharp;
using System;
using System.Collections.Generic;
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

namespace inspection
{
    /// <summary>
    /// image_mx.xaml 的交互逻辑
    /// </summary>
    public partial class image_mx : Page
    {
        public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "setting.json";
        public image_mx()
        {
            InitializeComponent();
            cef.Address = AppDomain.CurrentDomain.BaseDirectory + @"\\fault\\index.html";

            // cef.Address = "http://localhost:8080/";
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            cef.JavascriptObjectRepository.Register("bound", new CallbackObjectForJs(), isAsync: false, options: BindingOptions.DefaultBinder);

        }

        private void cef_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void cef_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine(JsStatus.Image_status);
            if (JsStatus.Image_status==false)
            {
                string hostname = Environment.GetEnvironmentVariable("computername");
                string command = "initmqtt(\"" + hostname + "\")";
                Console.WriteLine(command);
                cef.ExecuteScriptAsync(command);
                JsStatus.Image_status = true;
                Console.WriteLine("JSok");
            }
            if (File.Exists(FilePath))
            {
               
                StreamReader sr = File.OpenText(FilePath);
                string data=sr.ReadToEnd();
                Console.WriteLine(data);
                string command = "initdata('" + data + "')";
                Console.WriteLine(command);
                cef.ExecuteScriptAsync(command);
            }
            else
            {
                string command = "initdata(\"\")";
                Console.WriteLine(command);
                cef.ExecuteScriptAsync(command);
            }
       
           
            
        }
       
        public class CallbackObjectForJs
        {
            public void savedata(string data)
            {
                StreamWriter sw = File.CreateText(FilePath);
                sw.Write(data);
                sw.Close();

            }

        }
    }
}
