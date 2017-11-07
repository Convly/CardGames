using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Sources
{
    class Game
    {
        private List<string> users;
        private bool locker;

        public Game()
        {
            this.Users = new List<string> { };
            this.Locker = false;
        }

        public void StartGame()
        {
            this.Init();
            this.Run();
            this.End();
        }

        private void End()
        {
            Console.WriteLine("End");
        }

        private void Run()
        {
            Console.WriteLine("Run");
        }

        private void Init()
        {
            Console.WriteLine("Init");
        }

        public List<string> Users { get => users; set => users = value; }
        public bool Locker { get => locker; set => locker = value; }
    }
}
