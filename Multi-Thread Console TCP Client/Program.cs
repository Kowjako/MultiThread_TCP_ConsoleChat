using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multi_Thread_Console_TCP_Client
{
    class Program
    {
        static string username;
        private static int port = 8888;
        private static string ip = "127.0.0.1";
        static TcpClient client;
        static NetworkStream stream;
        static void Main()
        {
            Console.WriteLine("Write your name");
            username = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(ip, port);
                stream = client.GetStream();
                string msg = username;
                byte[] data = Encoding.Unicode.GetBytes(msg);
                stream.Write(data, 0, data.Length);
                Thread t = new Thread(new ThreadStart(ReceiveMessage));
                t.Start();
                Console.WriteLine($"Hello {username}");
                SendMessage();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        static void SendMessage()
        {
            Console.WriteLine("Write msg:");
            while(true)
            {
                string msg = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(msg);
                stream.Write(data, 0, data.Length);
            }
        }
        static void ReceiveMessage()
        {
            while(true)
            {
                try
                {
                    byte[] data = new byte[256];
                    StringBuilder sb = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        sb.Append(Encoding.Unicode.GetString(data));
                    }
                    while (stream.DataAvailable);
                    Console.WriteLine(sb.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Connection is close");
                    Disconnect();
                }
            }
        }
        static void Disconnect()
        {
            stream.Close();
            client.Close();
        }
    }
}
