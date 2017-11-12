using System;
using System.Collections.Generic;
using System.IO;
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
        public void Start(string[] args)
        {
            int localNbGame = 0;
            Referee referee = Referee.Instance;
            while (true)
            {
                this.SetLog(localNbGame);
                Core.Locker = true;
                string addr = Network.Server.Instance.Start(referee.EntryPoint);
                Console.WriteLine("Server running on:" + addr);
                if (args != null)
                {
                    foreach (var item in args)
                    {
                        referee.AddAi(item);
                    }
                }
                while (Core.Locker) ;
                Network.Server.Instance.Stop();
                Console.Clear();
                localNbGame++;
            }
        }

        /// <summary>
        /// Bind the standard output of the server to a specific log file
        /// </summary>
        /// <param name="nbGame">The amount of game that has been played for this run of the server</param>
        private void SetLog(int nbGame)
        {
            Random rd = new Random();
            string outName = "game_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "_" + nbGame + "_" + rd.Next().ToString() + ".txt";
            FileStream filestream = new FileStream(outName, FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
        }
    }
}
