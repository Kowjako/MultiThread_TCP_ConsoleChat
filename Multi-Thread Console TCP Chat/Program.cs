using System;
using System.Threading;

namespace Multi_Thread_Console_TCP_Chat
{
    class Program
    {
        static ServerObject server;
        static Thread listenThread;
        static void Main()
        {
            server = new ServerObject();
            listenThread = new Thread(new ThreadStart(server.Listen));
            listenThread.Start();
        }
    }
}
