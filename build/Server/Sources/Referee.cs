using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameResources.Net;
using Network;
using System.Threading;
using CardGameResources.Game;

namespace Servers.Sources
{
    public class Referee
    {
        private static Referee instance = null;

        public static Referee Instance
        {
            get
            {
                if (Referee.instance == null)
                {
                    Referee.instance = new Referee();
                }
                return Referee.instance;
            }
        }

        private Game game = new Game();

        internal Game Game { get => game; set => game = value; }

        /// <summary>
        /// This method is triggered when the server receive an object. It Will redirect the object trhough the different routes
        /// </summary>
        /// <param name="Obj"></param>
        public int EntryPoint(Object obj)
        {
            Packet p = JsonConvert.DeserializeObject<Packet>(obj.ToString());

            switch (p.Type)
            {
                case PacketType.ERR:
                    break;
                case PacketType.SYS:
                    this.SysEntryPoint(p);
                    break;
                case PacketType.GAME:
                    this.GameEntryPoint(p);
                    break;
                default:
                    break;
            }
            return 0;
        }

        /**
         * GAME FUNCTIONS
         */

        /// <summary>
        /// This function is cqlled by the main entry point when the received object is of type "GAME"
        /// </summary>
        /// <param name="p"></param>
        private void GameEntryPoint(Packet p)
        {
            Gamecall evt = JsonConvert.DeserializeObject<Gamecall>(p.Data.ToString());

            Console.WriteLine("_> Gamecall catched from " + p.Name + ":" + evt.Action);

            switch (evt.Action)
            {
                case GameAction.C_PLAY_CARD:
                    Card card = JsonConvert.DeserializeObject<Card>(evt.Data.ToString());
                    this.Game.PlayCard_callback(p.Name, card);
                    break;
                case GameAction.C_TAKE_TRUMP:
                    bool takeTrumpAnswer = Convert.ToBoolean(evt.Data.ToString());
                    this.Game.TakeTrump_callback(p.Name, takeTrumpAnswer);
                    break;
                case GameAction.C_TAKE_TRUMP_AS:
                    string takeTrumpAsAnswer = (evt.Data != null) ? evt.Data.ToString() : null;
                    this.Game.TakeTrumpAs_callback(p.Name, takeTrumpAsAnswer);
                    break;
            }

        }

        /**
         * SYS FUNCTIONS
         */

        /// <summary>
        /// This function is cqlled by the main entry point when the received object is of type "SYS"
        /// </summary>
        /// <param name="p"></param>
        private void SysEntryPoint(Packet p)
        {
            Syscall evt = JsonConvert.DeserializeObject<Syscall>(p.Data.ToString());

            Console.WriteLine("_> Syscall catched for " + evt.Command);

            switch (evt.Command)
            {
                case SysCommand.C_REGISTER:
                    this.Register(p.Name, evt);
                    break;
                case SysCommand.S_POKE:
                    this.PokeHandling(p.Key, p.Name);
                    break;
                case SysCommand.C_QUIT:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if the user can register to the server
        /// </summary>
        /// <param name="name"></param>
        private bool CheckRegisterValidity(string name)
        {
            if (Network.Server.Instance.Clients.Count() > 4)
            {
                this.Game.Send(name, PacketType.ERR, new Errcall(Err.SERVER_FULL, "The server is already full. Please, try again later."));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to register a user into the server. If the server is full, it'll start the game.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="evt"></param>
        private bool Register(string name, Syscall evt)
        {
            if (!this.CheckRegisterValidity(name))
                return false;

            if (!this.Game.Users.Contains(name))
            {
                this.Game.Users.Add(name);
            }

            this.Game.Send(name, PacketType.SYS, new Syscall(SysCommand.S_CONNECTED, null));

            List<string> clientList = new List<string> { };
            foreach (var user in this.Game.Users)
            {
                clientList.Add(user);
            }

            this.Game.Send(PacketType.ENV, new Envcall(EnvInfos.S_USER_LIST, clientList));

            if (Game.Users.Count() == 4 && !this.Game.GameLaunched)
            {
                this.Game.StartGame();
            }
            else if (this.Game.GameLaunched)
            {
                this.Game.Reconnect(name);
            }
            return true;
        }

        private void PokeHandling(uint key, string name)
        {
            if (name == "lock" && key != 0)
            {
                Network.Server.Instance.Lock_m.Unlock(key);
            }
        }
    }
}
