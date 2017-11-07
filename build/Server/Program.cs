using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using Network;
using Newtonsoft.Json;
using Servers.Sources;

namespace ServerApplication
{
    public class Test
    {
        public string name;
        public string toto;
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Referee referee = new Referee();
            string addr = Server.Instance.Start(referee.EntryPoint);
            Console.WriteLine("Server running on:" + addr);
            Console.ReadLine();
        }
    }
}