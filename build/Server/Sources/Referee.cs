using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameResources.Net;
using Network;

namespace Servers.Sources
{
    public class Referee
    {
        private Game game;

        internal Game Game { get => game; set => game = value; }

        public Referee()
        {
            this.Game = new Game();
        }

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
                    break;
                case PacketType.API:
                    break;
                default:
                    break;
            }
            return 0;
        }

        /**
         * GAME FUNCTIONS
         */

        private void GameEntryPoint(Packet p)
        {
            Gamecall evt = JsonConvert.DeserializeObject<Gamecall>(p.Data.ToString());

            Console.WriteLine("Got GameCall -> " + evt.Action);

            switch (evt.Action)
            {
                case GameAction.PLAY_CARD:
                    break;
                case GameAction.TAKE_TRUMP:
                    break;
                case GameAction.TAKE_TRUMP_AS:
                    break;
                case GameAction.SHOT_BELOT:
                    break;
                case GameAction.SHOT_REBELOT:
                    break;
                default:
                    break;
            }

        }

        /**
         * SYS FUNCTIONS
         */
        private void SysEntryPoint(Packet p)
        {
            Syscall evt = JsonConvert.DeserializeObject<Syscall>(p.Data.ToString());

            Console.WriteLine("Got Syscall -> " + evt.Command);

            switch (evt.Command)
            {
                case SysCommand.REGISTER:
                    this.Register(evt.Args);
                    break;
                case SysCommand.QUIT:
                    break;
                default:
                    break;
            }
        }

        private bool CheckRegisterValidity(List<string> args)
        {
            if (args.Count() != 1)
            {
                return false;
            }

            if (Game.Users.Count() >= 4)
            {
                Network.Server.Instance.sendDataToClient(args.ElementAt(0), new Packet("ROOT", PacketType.ERR, new Errcall(Err.SERVER_FULL, "The server is already full. Please, try again later.")));
                return false;
            }
            return true;
        }

        private bool Register(List<string> args)
        {
            if (!this.CheckRegisterValidity(args))
                return false;

            this.Game.Users.Add(args.ElementAt(0));

            List<string> clientList = new List<string> { };
            foreach (var user in Network.Server.Instance.Clients)
            {
                clientList.Add(user.Key);
            }

            Network.Server.Instance.SendToAllClient(new Packet("ROOT", PacketType.ENV, new Envcall(EnvInfos.USER_LIST, clientList)));

            if (Game.Users.Count() == 4)
            {
                Console.WriteLine("Lets the hunger games begin!");
                this.Game.StartGame();
            }
            return true;
        }
    }
}
