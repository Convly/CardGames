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
        private Game game;

        internal Game Game { get => game; set => game = value; }

        /// <summary>
        /// Default constructor for Referee Object
        /// </summary>
        public Referee()
        {
            this.Game = new Game();
        }

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

            Console.WriteLine("Got GameCall -> " + evt.Action);

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
                case GameAction.C_SHOT_BELOT:
                    break;
                case GameAction.C_SHOT_REBELOT:
                    break;
                default:
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

            Console.WriteLine("Got Syscall -> " + evt.Command);

            switch (evt.Command)
            {
                case SysCommand.C_REGISTER:
                    this.Register(p.Name, evt);
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
            if (Game.Users.Count() >= 4)
            {
                Network.Server.Instance.sendDataToClient(name, new Packet("ROOT", PacketType.ERR, new Errcall(Err.SERVER_FULL, "The server is already full. Please, try again later.")));
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

            this.Game.Users.Add(name);
            Network.Server.Instance.sendDataToClient(name, new Packet("root", PacketType.SYS, new Syscall(SysCommand.S_CONNECTED, null)));

            Thread.Sleep(500);

            List<string> clientList = new List<string> { };
            foreach (var user in Network.Server.Instance.Clients)
            {
                clientList.Add(user.Key);
            }

            this.Game.Send(PacketType.ENV, new Envcall(EnvInfos.S_USER_LIST, clientList));

            if (Game.Users.Count() == 4)
            {
                Console.WriteLine("Lets the hunger games begin!");
                this.Game.StartGame();
            }
            return true;
        }
    }
}
