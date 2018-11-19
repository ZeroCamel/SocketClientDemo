using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace SocketClientDemo
{
    class Program
    {
        private static byte[] result = new byte[1024];
        static void Main(string[] args)
        {
            //1、Socket()
            //实例化地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //Socket包含的三层含义 参数：1、地址1 2、套接字类型 3、协议类型
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //2、Connect()
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885));
                Console.WriteLine("连接服务器成功！");
            }
            catch (Exception)
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }
            //3、Receive() 接收数据并接收至缓冲区
            int receiveLength = clientSocket.Receive(result);
            Console.WriteLine("接收服务器消息{0}", Encoding.ASCII.GetString(result, 0, receiveLength));

            //4、Send()
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    string sendMessage = "client send message help" + DateTime.Now;
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("向服务器发送消息：{0}"+sendMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;  
                }
            }

            Console.WriteLine("发送完毕，按回车退出");
            Console.ReadLine();
        }
    }
}
