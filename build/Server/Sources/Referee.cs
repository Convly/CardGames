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
using Network.Lock;

namespace Servers.Sources
{
    /// <summary>
    /// The mission of the referee is to regulate the flow of request handled by the <see cref="Server"/>. It also act as a router and can interact directly with the <see cref="Game"/>.
    /// </summary>
    public class Referee
    {
        private static Referee instance = null;

        /// <summary>
        /// A getter and a setter for the Referee singleton instance
        /// </summary>
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
        /// <param name="obj">The <see cref="Packet"/> object serialized as an <see cref="Object"/></param>
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
        /// This function is called by the main entry point when the received object is of type <see cref="PacketType.GAME"/>
        /// </summary>
        /// <param name="p">The <see cref="Packet"/> received from the server</param>
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
        /// This function is called by the main entry point when the received object is of type <see cref="PacketType.SYS"/>
        /// </summary>
        /// <param name="p">The <see cref="Packet"/> received from the server</param>
        private void SysEntryPoint(Packet p)
        {
            Syscall evt = JsonConvert.DeserializeObject<Syscall>(p.Data.ToString());

            Console.WriteLine("_> Syscall catched from " + p.Name + ":" + evt.Command);

            switch (evt.Command)
            {
                case SysCommand.C_REGISTER:
                    this.Register(p.Name, evt);
                    break;
                case SysCommand.S_POKE:
                    this.PokeHandling(p.Key, p.Name);
                    break;
                case SysCommand.C_QUIT:
                    Console.WriteLine("_> " + p.Name + " quit");
                    Network.Server.Instance.DeleteClient(p.Name);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if the client can register to the server
        /// </summary>
        /// <param name="name">The name of the client</param>
        private bool CheckRegisterValidity(string name)
        {
            if (Game.Users.Count >= 4 && !this.Game.Users.Exists(e => e.EndsWith(name)))
            {
                this.Game.Send(name, PacketType.ERR, new Errcall(Err.SERVER_FULL, "The server is already full. Please, try again later."));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to register a user into the server. If the server went full, it'll start the game.
        /// </summary>
        /// <param name="name">The name of the client who try to register</param>
        /// <param name="evt">The content of the <see cref="Syscall"/> event contained in the received <see cref="Packet"/></param>
        private bool Register(string name, Syscall evt)
        {
            if (!this.CheckRegisterValidity(name))
                return false;

            if (!this.Game.Users.Exists(e => e.EndsWith(name)))
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

        /// <summary>
        /// This method is used to handle synchronous events. It will unlock the <see cref="Locker"/> associated to the key of the <see cref="Packet"/>
        /// </summary>
        /// <param name="key">The key of the <see cref="Locker"/></param>
        /// <param name="name">The name of the client who try to unlock the <see cref="Locker"/></param>
        private void PokeHandling(uint key, string name)
        {
            if (name == "lock" && key != 0)
            {
                Network.Server.Instance.Lock_m.Unlock(key);
            }
        }

        /// <summary>
        /// Method called by the <see cref="Core"/> class to add a new AI in the game
        /// </summary>
        /// <param name="name">Name of the AI we want to add</param>
        public void AddAi(string name)
        {
            InfosClient ic = new InfosClient()
            {
                _ip = "",
                _port = -1
            };

            Server.Instance.Clients.Add(name, ic);
            this.Register(name, null);
        }
    }
}
