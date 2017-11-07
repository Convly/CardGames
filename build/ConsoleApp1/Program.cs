using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Test
    {
        public string name = "titi";
    }

    class Program
    {
        static void Main(string[] args)
        {
            string ip = Console.ReadLine();
            int port = int.Parse(Console.ReadLine());
            Client.Instance.Start(ip, port);
            Test test = new Test();
            Client.Instance.SendToServer(test);
            Console.ReadLine();
        }
    }
}
