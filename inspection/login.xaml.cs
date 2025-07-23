using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HandyControl.Controls;
using RestSharp;
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
using System.Windows.Shapes;
using System.Configuration;
using HandyControl.Tools;
using MessageBox = HandyControl.Controls.MessageBox;

namespace inspection
{
    /// <summary>
    /// login.xaml 的交互逻辑
    /// </summary>
    public partial class login : System.Windows.Window
    {
        public login()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ConfigHelper.Instance.SetWindowDefaultStyle();
            ConfigHelper.Instance.SetNavigationWindowDefaultStyle();
            if (GetSettingString("isRemember") == "true")
            {
                ckbRemember.IsChecked = true;
                ed_user.Text = GetSettingString("userName");
                ed_password.Password = GetSettingString("password");
            }
            else
            {
                ckbRemember.IsChecked = false;
            }

        }
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

            private void btn_login_Click(object sender, RoutedEventArgs e)
            {

            if(true)
            {
                loginSuccess();
                return;
            }

            Console.WriteLine(ed_user.Text);
            if (ed_user.Text == "")
            {
                MessageBox.Warning("账号不能为空");
            }else if(ed_password.Password == "")
            {
                MessageBox.Warning("密码不能为空");
            }
            else
            {
                if (Identity.status)
                {
                    loginstu();
                }
                else
                {
                    loginuser();
                }
                
            }

        }
        private void loginSuccess()
        {
            if (ckbRemember.IsChecked==true)
            {
                UpdateSettingString("userName", ed_user.Text);
                UpdateSettingString("password", ed_password.Password);
                UpdateSettingString("isRemember", "true");
            }
            else
            {
                UpdateSettingString("userName", "");
                UpdateSettingString("password", "");
                UpdateSettingString("isRemember", "false");
            }
            MessageBox.Success("登录成功");
            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            this.Close();
        }
        private void loginuser()
        {
            var client = new RestClient(Api.Url+"/login/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("username", ed_user.Text);
            request.AddParameter("password", ed_password.Password);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content == "\"pwderr\"")
            {
                MessageBox.Warning("密码错误");
            }else if (response.Content == "\"None\"")
            {
                MessageBox.Warning("账号不存在");
            }
            else
            {
                try
                {
                    JObject te = (JObject)JsonConvert.DeserializeObject(response.Content);
                    Teacher.Sex = te["sex"].ToString();
                    Teacher.Name = te["name"].ToString();
                    Teacher.Depart = te["department"].ToString();
                    Console.WriteLine("22222");
                    loginSuccess();
                }
                catch
                {
                    //Console.WriteLine("6666666");
                    MessageBox.Error("登录失败，请检测网络或后端程序，或用户未启用");
                }
             }
        }
        
        private void loginstu()
        {
            var client = new RestClient(Api.Url+"/stulogin/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("sid", ed_user.Text);
            request.AddParameter("password", ed_password.Password);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content.Length>10)
            {
                try
                {
                    JObject Stu = (JObject)JsonConvert.DeserializeObject(response.Content);
                    
                    Student.Sid= int.Parse(Stu["sid"].ToString());
                    /* MessageBox.Show(Student.Sid.ToString());*/
                    loginSuccess();
                }
                catch
                {
                    MessageBox.Error("登录失败，请检测网络或后端程序");
                }
                

            }
            else if (response.Content == "\"pwderror\"")
            {
                MessageBox.Warning("密码错误");
            }
            else if (response.Content == "\"None\"")
            {
                MessageBox.Warning("账号不存在");
            }
            else
            {
                MessageBox.Error("登录失败，请检测网络或后端程序");
            }
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            register.Show();
        
        }
    }
}
