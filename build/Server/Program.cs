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
    /// <summary>
    /// Entry point of the server project
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            Core core = new Core();
            core.Start();
        }
    }
}