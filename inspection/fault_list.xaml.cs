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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace inspection
{
    /// <summary>
    /// fault_list.xaml 的交互逻辑
    /// </summary>
    public partial class fault_list : Page
    {
        public fault_list()
        {
            InitializeComponent();
            var client = new RestClient("http://127.0.0.1:8000/get_fault_list/?name=默认");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            String jsstr = response.Content;

            JArray ja = (JArray)JsonConvert.DeserializeObject(jsstr);
            foreach(JObject j in ja)
            {
                var item = new Fault_cla { Id = j["id"].ToString(), Name = j["name"].ToString(),Status=j["status"].ToString() };
                DataContext = item;

                list1.Items.Add(item);
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

        }
        public class Fault_cla
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
        }
    }
}
