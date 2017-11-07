using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using Network;
using Newtonsoft.Json;

namespace ServerApplication
{
    public class Test
    {
        public string name;
        public string toto;
    }

    class Program
    {
        public static int Test(Object str)
        {
            Test test = JsonConvert.DeserializeObject<Test>(str.ToString());
            return 2;
        }

        static void Main(string[] args)
        {
            string addr = Server.Instance.Start(Test);
            Console.ReadLine();
        }
    }
}