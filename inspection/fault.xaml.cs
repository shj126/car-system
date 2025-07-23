using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
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
    /// fault.xaml 的交互逻辑
    /// </summary>
    public partial class fault : Page
    
    {
        public ObservableCollection<Info> LeftList { get; set; }

        public ObservableCollection<Info> RightList { get; set; }
        public SerialPort serialPort;
        public fault()
        {
            InitializeComponent();
            //getport();
            
            get_fault();

            if (InitCOM(getport()))
            {
           
                //SendCommand("asdasdasd");//发送字符
            }
            /***
        RightList = new ObservableCollection<Info>();
        
            LeftList = new ObservableCollection<Info>()
            {
                 new Info(){ ID=1, Name="1", Card=1, Address="2", Status=1},
                 new Info(){ ID=2, Name="2", Card=1, Address="3", Status=0},
                 new Info(){ ID=3, Name="3", Card=1, Address="5", Status=0},
                 new Info(){ ID=4, Name="第四个", Card=1, Address="4", Status=0},
                 new Info(){ ID=5, Name="5", Card=1, Address="5", Status=0},
                 new Info(){ ID=6, Name="6", Card=1, Address="6", Status=0},
                 new Info(){ ID=7, Name="7", Card=1, Address="7", Status=0},
                 new Info(){ ID=8, Name="8", Card=1, Address="8", Status=0},
                 new Info(){ ID=9, Name="9", Card=1, Address="9", Status=0},
                 new Info(){ ID=10, Name="哈哈哈", Card=1, Address="10", Status=0},
                 new Info(){ ID=11, Name="11", Card=1, Address="11", Status=0},
                 new Info(){ ID=12, Name="12", Card=1, Address="12", Status=0},
                 new Info(){ ID=13, Name="13", Card=1, Address="13", Status=0},
                       new Info(){ ID=8, Name="8", Card=1, Address="8", Status=0},
                 new Info(){ ID=9, Name="9", Card=1, Address="9", Status=0},
                 new Info(){ ID=10, Name="哈哈哈", Card=1, Address="10", Status=0},
                 new Info(){ ID=11, Name="11", Card=1, Address="11", Status=0},
                 new Info(){ ID=12, Name="12", Card=1, Address="12", Status=0},
                 new Info(){ ID=13, Name="13", Card=1, Address="13", Status=0},
            };
        ListView1.ItemsSource = LeftList;
        RightList.Add(LeftList.Where(c => c.Status == 1).FirstOrDefault());
        ListView2.ItemsSource = RightList;
            ***/
        }
    /// <summary>
    /// 列表二还原事件，点中哪行删除哪行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void restore_Click_1(object sender, RoutedEventArgs e)
    {
        var selectItem = this.ListView2.SelectedItem as Info;
        if (selectItem != null)
        {
            //从列表中删除
            RightList.Remove(selectItem);
            SendSer(selectItem.Card, selectItem.Address, 0);
            alterStatus(selectItem.ID,0);
                MessageBox.Show("删除数据成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
            MessageBox.Show("没有选中需要删除的行", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// 列表一还原事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListViewReStore_Click_1(object sender, RoutedEventArgs e)
    {
        var selectItem = this.ListView1.SelectedItem as Info;
        if (selectItem != null)
        {
                //从列表中删除
                // LeftList.Remove(selectItem);
                var isExists = RightList.Where(c => c.ID == selectItem.ID).FirstOrDefault();
                if (isExists == null)
                {
                    RightList.Remove(RightList.Where(c => c.ID == selectItem.ID).FirstOrDefault());
                }
                else
                {
                    
                    RightList.Remove(RightList.Where(c => c.ID == selectItem.ID).FirstOrDefault());
                    
                }
                    SendSer(selectItem.Card, selectItem.Address, 0);
                    alterStatus(selectItem.ID, 0);
                MessageBox.Show("删除数据成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
            MessageBox.Show("没有选中需要删除的行", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// 故障事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Error_Click_1(object sender, RoutedEventArgs e)
    {
        var selectItem = this.ListView1.SelectedItem as Info;
        if (selectItem != null)
        {
            selectItem.Status = 1;
            //检查右边列表是否已经存在了这个编号为故障的
            var isExists = RightList.Where(c => c.ID == selectItem.ID).FirstOrDefault();
            if (isExists == null)
            {
                RightList.Add(selectItem);
                SendSer(selectItem.Card,selectItem.Address,1);
                alterStatus(selectItem.ID, 1);
                }
            else
            {
                MessageBox.Show(string.Format("列表二已经存在编号为{0}的记录", selectItem.ID), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("没有选中需要设置状态为故障的行", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// 正常状态
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    private void OK_Click_1(object sender, RoutedEventArgs e)
    {
        var selectItem = this.ListView1.SelectedItem as Info;
        if (selectItem != null)
        {
            selectItem.Status = 2;
            //检查右边列表是否已经存在了这个编号为故障的
            var isExists = RightList.Where(c => c.ID == selectItem.ID).FirstOrDefault();
            if (isExists == null)
            {
                RightList.Add(selectItem);
                    //MessageBox.Show(selectItem.Card.ToString());
                    SendSer(selectItem.Card, selectItem.Address, 2);
                    alterStatus(selectItem.ID, 2);
                }
            else
            {
                MessageBox.Show(string.Format("列表二已经存在编号为{0}的记录", selectItem.ID), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("没有选中需要设置状态为故障的行", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

        //int转换
        public static int bytesToInt(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        public static byte[] intToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }


        public bool InitCOM(string PortName)
        {
            serialPort = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);//DataReceived事件委托
            serialPort.ReceivedBytesThreshold = 1;
            serialPort.RtsEnable = true;
            return OpenPort();//串口打开
        }
        public bool OpenPort()
        {
            try//这里写成异常处理的形式以免串口打不开程序崩溃
            {
                serialPort.Close();
                serialPort.Open();
            }
            catch { }
            if (serialPort.IsOpen)
            {
                return true;
            }
            else
            {
                MessageBox.Show("串口打开失败!");
                return false;
            }
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Thread.Sleep(2000);
            byte[] readBuffer = new byte[serialPort.ReadBufferSize];
            serialPort.Read(readBuffer, 0, readBuffer.Length);
            string str = System.Text.Encoding.Default.GetString(readBuffer);
            MessageBox.Show(str);
        }

        //串口发送
        public void SendCommand(string CommandString)
        {
           
            //byte[] WriteBuffer = Encoding.ASCII.GetBytes(CommandString);
            Byte[] TxData = { 11, 2, 3, 4, 5, 6, 7, 8 };
            String s = "19";
            int a = int.Parse(s);

            TxData[0] = intToBytes(a)[0];
            //serialPort.Write(WriteBuffer, 0, WriteBuffer.Length);
            serialPort.Write(TxData, 0, 8);
        }
        public bool SendSer(int card,int address,int status)
        {
            Byte[] TxData = { 01, 06, 00, 02, 00, 01, 233, 202 };
            TxData[0] = intToBytes(card)[0];
            TxData[3] = intToBytes(address)[0];
            TxData[5] = intToBytes(status)[0];
            Byte[] crctmp = { TxData[0], TxData[1], TxData[2], TxData[3], TxData[4], TxData[5] };
            Byte[] CRC = GetModbusCrc16(crctmp);
            TxData[6] = CRC[0];
            TxData[7] = CRC[1];
            try
            {
                serialPort.Write(TxData, 0, 8);
                return true;
            }
            catch
            {
                   
                MessageBox.Show("串口打开失败,请切换窗口或重新启动软件");
                return false;
            }

            
        }
        public static byte[] GetModbusCrc16(byte[] bytes)
        {
            byte crcRegister_H = 0xFF, crcRegister_L = 0xFF;// 预置一个值为 0xFFFF 的 16 位寄存器

            byte polynomialCode_H = 0xA0, polynomialCode_L = 0x01;// 多项式码 0xA001

            for (int i = 0; i < bytes.Length; i++)
            {
                crcRegister_L = (byte)(crcRegister_L ^ bytes[i]);

                for (int j = 0; j < 8; j++)
                {
                    byte tempCRC_H = crcRegister_H;
                    byte tempCRC_L = crcRegister_L;

                    crcRegister_H = (byte)(crcRegister_H >> 1);
                    crcRegister_L = (byte)(crcRegister_L >> 1);
                    // 高位右移前最后 1 位应该是低位右移后的第 1 位：如果高位最后一位为 1 则低位右移后前面补 1
                    if ((tempCRC_H & 0x01) == 0x01)
                    {
                        crcRegister_L = (byte)(crcRegister_L | 0x80);
                    }

                    if ((tempCRC_L & 0x01) == 0x01)
                    {
                        crcRegister_H = (byte)(crcRegister_H ^ polynomialCode_H);
                        crcRegister_L = (byte)(crcRegister_L ^ polynomialCode_L);
                    }
                }
            }

            return new byte[] { crcRegister_L, crcRegister_H };
        }
        //获取故障列表
        private void get_fault()
        {
            var client = new RestClient("http://jyhf-api.mcpanl.cn/get_fault_list/?name=默认");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            String jsstr = response.Content;
            JArray ja = (JArray)JsonConvert.DeserializeObject(jsstr);
           // List<Info> fault_s = new List<Info>();
            RightList = new ObservableCollection<Info>();
            LeftList = new ObservableCollection<Info>();
            foreach (JObject j in ja)
            {
               
                LeftList.Add(new Info() { ID = int.Parse(j["id"].ToString()),Name=j["name"].ToString(),Card=int.Parse(j["card"].ToString()),Address= int.Parse(j["address"].ToString()),Status= int.Parse(j["status"].ToString()) }) ;

            }
            ListView1.ItemsSource = LeftList;
            var rightData = LeftList.Where(c => c.Status == 1 || c.Status == 2).ToList();
            foreach (var item in rightData)
            {
                RightList.Add(item);
            }
            ListView2.ItemsSource = RightList;
        }
        private String getport()
        {
            var client = new RestClient("http://jyhf-api.mcpanl.cn/getport/");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            JObject p = (JObject)JsonConvert.DeserializeObject(response.Content);
            return "COM"+p["fault"].ToString();
        }
        private bool setall()
        {
            var client = new RestClient("http://jyhf-api.mcpanl.cn/set_all_status/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("class", "默认");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content == "\"ok\"")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool alterStatus(int id,int status)
        {
            var client = new RestClient("http://jyhf-api.mcpanl.cn/alter_status/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("id", id.ToString());
            request.AddParameter("status", status.ToString());
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content == "\"ok\"")
            {

                return true;

            }
            else
            {
                return false;
            }
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            serialPort.Close();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            get_fault();
        }

        private void setalllist_Click(object sender, RoutedEventArgs e)
        {
            if (setall())
            {
                if (SendSer(0, 0, 0))
                {
                    MessageBox.Show("重置成功");
                    get_fault();
                }
                else
                {
                    MessageBox.Show("已重置数据，但命令发送失败，请重启软件后，重新重置");
                }
                
            }
            else
            {
                MessageBox.Show("重置失败");
            }
        }
    }
}
