using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
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
    /// ele.xaml 的交互逻辑
    /// </summary>
    public partial class ele : Page
    {
        public static String Mydocument = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public ele()
        {
            InitializeComponent();
            cef.Address = "http://ele.car.com";
            cef.Height = 935;
            /*MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);*/
            /*cef.Address = AppDomain.CurrentDomain.BaseDirectory + @"\\dist\\index.html";*/
            //（bound为前端被调用的对象名称.如：bound.login();BoundObject为C#被暴露的Class对象，对应的js调用的方法就是BoundObject.Login()）
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            cef.JavascriptObjectRepository.Register("bound", new CallbackObjectForJs(), isAsync: false, options: BindingOptions.DefaultBinder);

        }
      /*  public static void alentStatus()
        {
            string command = "uploadstatus(\"" + "ok" + "\")";
            web cef = new
            cef.ExecuteScriptAsync(command);
        }*/

/*
        public static void ResponesStatus()
        {
            alentStatus();
        }
*/
        public class CallbackObjectForJs
        {
            public static string targetPath = null;
            public String showtest(string msg)
            {
                MessageBox.Show(msg);
                return "111";
            }
          /*  public void upload()
            {
               
                DownFIle();     
                *//* e.cef.ExecuteScriptAsync("uploadfile('"+ ResFile.url + "')");*//*
            } */
            public string uploadfile(string ele_id,string sid)
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
                        var client = new RestClient(Api.Url+"/subFiles/");
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

                    if(!Directory.Exists(@"D:\\doc"))
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
        }
       /* private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            cef.ExecuteScriptAsync("callJsFunction('我是c#')");
        }*/

        private void cef_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void cef_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (JsStatus.Ele_status == false)
            {
                string hostname = Environment.GetEnvironmentVariable("computername");
                string command = "initmqtt('" + hostname + "')";
                Console.WriteLine(command);
                cef.ExecuteScriptAsync(command);
                JsStatus.Ele_status = true;
                Console.WriteLine("JSok");
            }
        }
    }

    
}
