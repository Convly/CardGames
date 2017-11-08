using CardGameResources.Net;
using Client.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(delegate ()
            {
                Packet p = JsonConvert.DeserializeObject<Packet>(data.ToString());

                switch (p.Type)
                {
                    case PacketType.ERR:
                        break;
                    case PacketType.ENV:
                        Envcall ev = JsonConvert.DeserializeObject<Envcall>(p.Data.ToString());
                        List<string> userList = JsonConvert.DeserializeObject<List<string>>(ev.Data.ToString());
                        Lobby.Instance.progress.Value = 25 * userList.Count;
                        Lobby.Instance.waitingMessage.Content = "Waiting for players... (" + userList.Count + "/4)";
                        break;
                    case PacketType.GAME:
                        break;
                    case PacketType.API:
                        break;
                    default:
                        break;
                }
                Console.WriteLine(MainWindow.Instance.name_txtbox.Text);
            }));
            return 0;
        }
    }
}
