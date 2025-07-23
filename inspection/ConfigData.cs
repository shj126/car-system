using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inspection
{
    public class ConfigData
    {

        private static string path = Path.Combine(Application.StartupPath + "\\config\\");

        public static String readData(string key)
        {
            try
            {
                if (File.Exists(path + key + ".txt"))
                {
                    StreamReader sr = new StreamReader(path + key + ".txt", Encoding.Default);
                    String line;
                    String data = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        data += line;
                        // Console.WriteLine(line.ToString());
                    }

                    return data;
                }
            } catch(Exception e)
            {
                Console.WriteLine("读取文件时出错");
                Console.WriteLine(e.Message);
            }
            

            return null;
        }

        public static void writeData(string key, string value)
        {
            try
            {
                if (!File.Exists(path + key + ".txt"))
                {
                    // 如果文件不存在就创建
                    FileStream fs = new FileStream(path + key + ".txt", FileMode.CreateNew);
                    fs.Close();
                    Console.WriteLine("已创建文件：" + path + key + ".txt");
                }

                FileStream fs2 = new FileStream(path + key + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs2);
                sw.Write(value);
                sw.Flush();
                sw.Close();
                fs2.Close();
                Console.WriteLine("已写入至文件：" + path + key + ".txt");
            } catch(Exception e)
            {
                Console.WriteLine("写入文件时出错");
                Console.WriteLine(e.Message);
            }
            

        }
    }
}
