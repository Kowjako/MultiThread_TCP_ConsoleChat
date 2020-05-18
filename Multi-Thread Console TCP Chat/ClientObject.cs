using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Multi_Thread_Console_TCP_Chat
{
    class ClientObject
    {
        protected internal string Id { get; set; }
        protected internal NetworkStream stream { get; set; }
        string username;
        TcpClient client;
        ServerObject server;
        public ClientObject(TcpClient x, ServerObject y)
        {
            server = y;
            client = x;
            Id = Guid.NewGuid().ToString();
            y.AddConnection(this);
        }
        public void Process()
        {
            try
            {
                stream = client.GetStream();
                string msg = GetMessage();
                username = msg;
                msg = username + "connected to chat";
                server.BroadcastMessage(msg, this.Id);
                Console.WriteLine(msg);
                while(true)
                {
                    try
                    {
                        msg = GetMessage();
                        msg = String.Format("{0}: {1}", username, msg);
                        Console.WriteLine(msg);
                        server.BroadcastMessage(msg, Id);
                    }
                    catch
                    {
                        msg = String.Format("{0}: leave chat", username);
                        Console.WriteLine(msg);
                        server.BroadcastMessage(msg,this.Id);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(Id);
                Close();
            }
        }
        public string GetMessage()
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
            return sb.ToString();
        }
        protected internal void Close()
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
        }
    }
}
