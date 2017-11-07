﻿using Newtonsoft.Json;
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
                case GameAction.C_PLAY_CARD:
                    break;
                case GameAction.C_TAKE_TRUMP:
                    break;
                case GameAction.C_TAKE_TRUMP_AS:
                    break;
                case GameAction.C_SHOT_BELOT:
                    break;
                case GameAction.C_SHOT_REBELOT:
                    break;
                case GameAction.S_SET_USER_DECK:
                    break;
                case GameAction.S_SET_BOARD_DECK:
                    break;
                case GameAction.S_SET_LASTROUND_DECK:
                    break;
                case GameAction.S_SET_TRUMP:
                    break;
                case GameAction.S_REQUEST_TRUMP_FROM:
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
                case SysCommand.C_REGISTER:
                    this.Register(p.Name, evt);
                    break;
                case SysCommand.C_QUIT:
                    break;
                case SysCommand.S_DISCONNECTED:
                    break;
                case SysCommand.S_CONNECTED:
                    break;
                default:
                    break;
            }
        }

        private bool CheckRegisterValidity(string name)
        {
            if (Game.Users.Count() >= 4)
            {
                Network.Server.Instance.sendDataToClient(name, new Packet("ROOT", PacketType.ERR, new Errcall(Err.SERVER_FULL, "The server is already full. Please, try again later.")));
                return false;
            }
            return true;
        }

        private bool Register(string name, Syscall evt)
        {
            if (!this.CheckRegisterValidity(name))
                return false;

            this.Game.Users.Add(name);

            List<string> clientList = new List<string> { };
            foreach (var user in Network.Server.Instance.Clients)
            {
                clientList.Add(user.Key);
            }

            Network.Server.Instance.SendToAllClient(new Packet("ROOT", PacketType.ENV, new Envcall(EnvInfos.S_USER_LIST, clientList)));

            if (Game.Users.Count() == 4)
            {
                Console.WriteLine("Lets the hunger games begin!");
                this.Game.StartGame();
            }
            return true;
        }
    }
}
