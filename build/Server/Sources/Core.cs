using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Sources
{
    /// <summary>
    /// Core class of the server
    /// </summary>
    class Core
    {
        public static Core instance = null;

        /// <summary>
        /// Getter and Setter of the singleton instance of the <see cref="Core"/>
        /// </summary>
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

        /// <summary>
        /// Getter and Setter for the locker state of the <see cref="Core"/>
        /// </summary>
        public static bool Locker { get => locker; set => locker = value; }

        /// <summary>
        /// Default constructor of the <see cref="Core"/>
        /// </summary>
        public Core()
        {

        }

        /// <summary>
        /// Start the server's dependencies like the Network and loop infinitly the execution.
        /// </summary>
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
