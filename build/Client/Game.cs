using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class GameClient
    {
        private static GameClient instance = null;
        public static GameClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameClient();
                }
                return instance;
            }
        }

        private string name = null;
        public string Name { get => name; set => name = value; }

        public int EntryPoint(Object data)
        {
            return 0;
        }
    }
}
