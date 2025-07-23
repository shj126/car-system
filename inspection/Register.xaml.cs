using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;

namespace inspection
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : System.Windows.Window
    {
        public Register()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            getDe();
           
        }

        public void getDe()
        {
            var client = new RestClient(Api.Url+"/getDepartment/");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            try
            {
                JArray list = (JArray)JsonConvert.DeserializeObject(response.Content);
                List<Department> dicItem = new List<Department>();
                if(list == null)
                {
                    MessageBox.Error("获取院系列表出错，请检查网络或后端程序后重新打开本页面");
                   // this.Close();
                }
                else
                {
                    foreach (JObject l in list)
                    {
                        dicItem.Add(new Department { ID = l["id"].ToString(), Name = l["name"].ToString() });
                    }
                    comb.ItemsSource = dicItem;
                    comb.DisplayMemberPath = "Name";
                    comb.SelectedValuePath = "ID";
                }
                
            }
            catch
            {
                MessageBox.Error("获取院系列表出错，请检查网络或后端程序后重新打开本页面");
            }
           


        }
       public void registerTe()
        {
            string sex = null;
            if (sex_man.IsChecked ==true)
            {
                sex = "男";
            }else if (sex_woman.IsChecked == true)
            {
                sex = "女";
            }
            string depart = comb.SelectedValue.ToString();
            var client = new RestClient(Api.Url+"/register/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("tid", ed_tid.Text);
            request.AddParameter("name", ed_name.Text);
            request.AddParameter("sex",sex);
            request.AddParameter("depart", depart);
            request.AddParameter("password", ed_password.Password);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content == "\"error\"")
            {
                MessageBox.Error("注册失败");
            }else if (response.Content == "\"same\"")
            {
                MessageBox.Warning("此账号已存在");
            }else if (response.Content == "\"success\"")
            {
                MessageBox.Success("注册成功");
                this.Close();
            }
            else
            {
                MessageBox.Error("获取院系列表出错，请检查网络或后端程序后重新打开本页面");
            }

        }

        private void btn_rigister_user_Click(object sender, RoutedEventArgs e)
        {
            if (ed_name.Text == "")
            {
                MessageBox.Warning("名字不能为空");
            }else if (ed_password.Password == "")
            {
                MessageBox.Warning("密码不能为空");
            }else if (ed_tid.Text == "")
            {
                MessageBox.Warning("教工号不能为空");
            }else if (comb.SelectedIndex==-1)
            {
                MessageBox.Warning("院系不能为空");
            }
            else if (ed_password.Password.Length < 5)
            {
                MessageBox.Warning("密码长度过低");
            }
            else
            {
                registerTe();
            }
        }
    }
}
