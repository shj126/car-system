using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    /// stu.xaml 的交互逻辑
    /// </summary>
    public partial class stu : Page
    {

        ChromiumWebBrowser s = null;
        public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "setting.json";

        public stu()
        {
            InitializeComponent();

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;

            // 禁用右键菜单
            cef.MenuHandler = new MenuHandler();


            var cofj = new CallbackObjectForJs();
            cofj.stu = cef;

            cef.JavascriptObjectRepository.Register("bound", cofj, isAsync: false, options: BindingOptions.DefaultBinder);

            // cef.Address = "http://www.car.com/toPage/?username=admin&password=1qaz@WSX&url=http%3A%2F%2Fwww.car.com%2Fadmin%2Fcurriculum%2Fresources_views%2F&_t=1644468520070";

            // 电工电子
            cef.Address = "http://main.car.com/?_t=" + DateTime.Now;
            // cef.Address = "http://127.0.0.1:8123";
            // cef.Address = AppDomain.CurrentDomain.BaseDirectory + @"\\main\\index.html";

            // cef.Height = 500;
        }


        private void cef_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //string command = "win_to_js(\"i'm windows, send to js\")";
            //cef.ExecuteScriptAsyncWhenPageLoaded(command);

            // 获取配置文件，并向网页传递消息，决定工具箱显示哪些软件
            var my_decoder = ConfigData.readData("my_decoder");
            if (my_decoder != null && my_decoder.Equals("1"))
            {
                string command = "set_software_show(\"my_decoder\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);  
            }

            var my_setting = ConfigData.readData("my_setting");
            if (my_setting != null && my_setting.Equals("1"))
            {
                string command = "set_software_show(\"my_setting\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_pwm = ConfigData.readData("my_pwm");
            if (my_pwm != null && my_pwm.Equals("1"))
            {
                string command = "set_software_show(\"my_pwm\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_dps = ConfigData.readData("my_dps");
            if (my_dps != null && my_dps.Equals("1"))
            {
                string command = "set_software_show(\"my_dps\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_multimeter = ConfigData.readData("my_multimeter");
            if (my_multimeter != null && my_multimeter.Equals("1"))
            {
                string command = "set_software_show(\"my_multimeter\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_OSC980 = ConfigData.readData("my_OSC980");
            if (my_OSC980 != null && my_OSC980.Equals("1"))
            {
                string command = "set_software_show(\"my_OSC980\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_point = ConfigData.readData("my_point");
            if (my_point != null && my_point.Equals("1"))
            {
                string command = "set_software_show(\"my_point\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }

            var my_machine = ConfigData.readData("my_machine");
            if (my_machine != null && my_machine.Equals("1"))
            {
                string command = "set_software_show(\"my_machine\")";
                cef.ExecuteScriptAsyncWhenPageLoaded(command);
            }
        }

    


        public class CallbackObjectForJs
        {
            

            public ChromiumWebBrowser stu;

            public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "setting.json";

            public static string basePath = AppDomain.CurrentDomain.BaseDirectory;
            
            public static string userMenuSavePath = AppDomain.CurrentDomain.BaseDirectory + "\\data_user_menu\\";

            public static string targetPath = null;


            public string uploadfile(string ele_id, string sid)
            {
                Console.WriteLine(targetPath);
                if (targetPath == null)
                {
                    return "error";
                }
                else
                {
                    try
                    {
                        Console.WriteLine(ele_id);
                        Console.WriteLine(sid);
                        var client = new RestClient(Api.Url + "/subFiles/");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddParameter("ele_id", ele_id);
                        request.AddParameter("sid", sid);
                        request.AddFile("file", targetPath);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        //ele.ResponesStatus();
                        if (response.Content == "\"ok\"")
                        {
                            return "success";
                        }
                        else
                        {
                            return "error";
                        }
                    }
                    catch
                    {
                        return "error";
                    }


                }

            }
            public string openfile()
            {
                try
                {
                    return DownFIle();
                }
                catch
                {
                    return "error";
                }

            }
            public string DownFIle()
            {
                String Fileurl = GetFileUrl();
                if (Fileurl == "error")
                {
                    /*MessageBox.Show("获取地址错误");*/
                    return "error";
                }
                else
                {
                    int i = Fileurl.LastIndexOf('/');
                    String FIleName = Fileurl.Substring(i + 1);

                    if (!Directory.Exists(@"D:\\doc"))
                    {
                        Directory.CreateDirectory(@"D:\\doc");
                    }


                    targetPath = @"D:\\doc\\" + FIleName;
                    ResFile.url = targetPath;
                    if (File.Exists(targetPath))
                    {
                        System.Diagnostics.Process.Start(targetPath);
                        return "success";
                    }
                    else
                    {
                        Console.WriteLine(Fileurl);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Api.Url + "/upload/" + Fileurl);
                        WebResponse respone = request.GetResponse();
                        Stream netStream = respone.GetResponseStream();
                        using (Stream fileStream = new FileStream(targetPath, FileMode.Create))
                        {
                            byte[] read = new byte[1024];
                            int realReadLen = netStream.Read(read, 0, read.Length);
                            while (realReadLen > 0)
                            {
                                fileStream.Write(read, 0, realReadLen);
                                realReadLen = netStream.Read(read, 0, read.Length);
                            }
                            netStream.Close();
                            fileStream.Close();
                        }
                        System.Diagnostics.Process.Start(targetPath);
                        return "success";
                    }

                }

            }

            public string GetFileUrl()
            {
                var client = new RestClient(Api.Url + "/getEle/");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                try
                {
                    JObject Ele = (JObject)JsonConvert.DeserializeObject(response.Content);
                    return Ele["file"].ToString();
                }
                catch
                {
                    return "error";
                }

            }
      


        // 获取用户自定义菜单
        public string get_user_menu(string name)
            {
                if (!Directory.Exists(basePath + "\\data_user_menu"))
                {
                    Directory.CreateDirectory(basePath + "\\data_user_menu");
                }

                try
                {
                    if (File.Exists(userMenuSavePath + name + ".json"))
                    {
                        StreamReader sr = new StreamReader(userMenuSavePath + name + ".json", Encoding.UTF8);
                        String line;
                        String data = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            data += line;
                            // Console.WriteLine(line.ToString());
                        }

                        return data;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("读取文件时出错");
                    Console.WriteLine(e.Message);
                }

                return null;
            }

            // 保存用户自定义菜单
            public void save_user_menu(string name, string data)
            {
                if(!Directory.Exists(basePath + "\\data_user_menu"))
                {
                    Directory.CreateDirectory(basePath + "\\data_user_menu");
                }

                try
                {
                    if (!File.Exists(userMenuSavePath + name + ".json"))
                    {
                        // 如果文件不存在就创建
                        FileStream fs = new FileStream(userMenuSavePath + name + ".json", FileMode.CreateNew);
                        fs.Close();
                        Console.WriteLine("已创建文件：" + userMenuSavePath + name + ".json");
                    }

                    FileStream fs2 = new FileStream(userMenuSavePath + name + ".json", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs2, Encoding.UTF8);
                    sw.Write(data);
                    sw.Flush();
                    sw.Close();
                    fs2.Close();
                    Console.WriteLine("已写入至文件：" + userMenuSavePath + name + ".json");
                }
                catch (Exception e)
                {
                    Console.WriteLine("写入文件时出错");
                    Console.WriteLine(e.Message);
                }

            }


            // 打开指定软件
            public void openSoftware(string name)
            {
                if(name == "my_decoder")
                {
                    // 打开解码仪
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\decoder\\EOBD.exe");
                    }
                    catch
                    {
                        HandyControl.Controls.MessageBox.Error("打开失败");
                    }
                }

                if(name == "my_setting")
                {
                    // 打开系统设置
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\setting\\系统设置.exe");    //调用该命令，打开系统设置
                    }
                    catch
                    {
                        HandyControl.Controls.MessageBox.Error("打开系统设置失败");
                    }
                }

                if(name == "my_pwm")
                {
                    // 打开信号发生器
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\pwm\\PWM.exe");
                    }
                    catch
                    {
                        HandyControl.Controls.MessageBox.Error("打开信号发生器失败");
                    }
                }

                if (name == "my_dps")
                {
                    // 打开数控电源
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\dps\\DPSApp.exe");
                    }
                    catch
                    {
                        HandyControl.Controls.MessageBox.Error("打开数控电源失败");
                    }
                }

                if(name == "my_multimeter")
                {
                    // 打开万用表
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\multimeter\\Hantek365.exe");
                    }
                    catch(Exception e)
                    {
                        HandyControl.Controls.MessageBox.Error(e.ToString());
                        HandyControl.Controls.MessageBox.Error("打开万用表失败");
                    }
                }

                if (name == "my_OSC980")
                {
                    // 打开万用表
                    try
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\OSC980\\OSC9822.exe");
                    }
                    catch
                    {
                        HandyControl.Controls.MessageBox.Error("打开示波器失败");
                    }
                }
            }

            // 获取计算机名
            public string getComputerName()
            {
                // return "DESKTOP-BPDRN9S";
                string hostname = Environment.GetEnvironmentVariable("computername");
                return hostname;
            }

            // 保存点位数据
            public string savePointData(string pointData)
            {
                return "success";
            }

            // 获取点位数据
            public string getPointData()
            {

                // return FilePath;

                if (File.Exists(FilePath))
                {
                    Console.WriteLine(FilePath);
                    StreamReader sr = File.OpenText(FilePath);
                    string data = sr.ReadToEnd();
                    return data;
                }
                else
                {
                    return "";
                }
            }

            public void js_to_win(string data)
            {
                HandyControl.Controls.MessageBox.Success("来自网页的消息：" + data);
            
                string command = "win_to_js(\"i'm windows, send to js\")";
                stu.ExecuteScriptAsyncWhenPageLoaded(command);
            }

        }
    }
}
