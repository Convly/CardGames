using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer webServer;

            webServer = WebServer.Instance;
            Console.ReadLine();
        }
    }
}
