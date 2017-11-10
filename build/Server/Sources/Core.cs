using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Sources
{
    class Core
    {
        public static Core instance = null;

        public Core Instance
        {
            get
            {
                if (Core.instance == null)
                {
                    Core.instance = new Core();
                }
                return Core.instance;
            }
        }

        private static bool locker = false;

        public static bool Locker { get => locker; set => locker = value; }

        public Core()
        {

        }

        public void Start()
        {
            Referee referee = Referee.Instance;
            while (true)
            {
                Core.Locker = true;
                string addr = Network.Server.Instance.Start(referee.EntryPoint);
                Console.WriteLine("Server running on:" + addr);
                while (Core.Locker) ;
                Network.Server.Instance.Stop();
            }
        }
    }
}
