using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multi_Thread_Console_TCP_Chat
{
    class ServerObject
    {
        static TcpListener listener;
        List<ClientObject> clients = new List<ClientObject>();
        protected internal void AddConnection(ClientObject x)
        {
            clients.Add(x);
        }
        protected internal void BroadcastMessage(string msg, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            foreach(ClientObject x in clients)
            {
                if (x.Id != id) x.stream.Write(data, 0, data.Length);
            }
        }
        protected internal void RemoveConnection(string id)
        {
            ClientObject tmp = clients.FirstOrDefault(c => c.Id == id);
            if (tmp != null) clients.Remove(tmp);
        }
        protected internal void Disconnect()
        {
            listener.Stop();
            foreach (ClientObject t in clients) t.Close();
            Environment.Exit(0);
        }
        protected internal void Listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                Console.WriteLine("Server is started");
                while(true)
                {
                    TcpClient tmp = listener.AcceptTcpClient();
                    ClientObject cobject = new ClientObject(tmp, this);
                    Thread t = new Thread(new ThreadStart(cobject.Process));
                    t.Start();
                }
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
    }
}
