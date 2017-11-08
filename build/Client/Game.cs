using CardGameResources.Net;
using Client.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Client
{
    class GameClient
    {
        private bool connected = false;
        private string name = null;
        private List<string> usersList;
        private int scoreTeam1 = 0;
        private int scoreTeam2 = 0;
        private string userWhoPlay;

        public string Name { get => name; set => name = value; }
        public List<string> UsersList { get => usersList; set => usersList = value; }

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

        public int EntryPoint(Object data)
        {
            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(delegate ()
            {
                Packet p = JsonConvert.DeserializeObject<Packet>(data.ToString());
                if (!connected && p.Type != PacketType.SYS)
                {
                    return;
                }
                switch (p.Type)
                {
                    case PacketType.ERR:
                        ErrorEntryPoint(p);
                        break;
                    case PacketType.ENV:
                        EnvEntryPoint(p);
                        break;
                    case PacketType.GAME:
                        GameEntryPoint(p);
                        break;
                    case PacketType.SYS:
                        SysEntryPoint(p);
                        break;
                    default:
                        break;
                }
                Console.WriteLine(MainWindow.Instance.name_txtbox.Text);
            }));
            return 0;
        }

        public void ErrorEntryPoint(Packet data)
        {

        }
        public void EnvEntryPoint(Packet data)
        {
            Envcall ev = JsonConvert.DeserializeObject<Envcall>(data.Data.ToString());
            switch (ev.Type)
            {
                case EnvInfos.S_USER_LIST:
                    usersList = JsonConvert.DeserializeObject<List<string>>(ev.Data.ToString());
                    Lobby.Instance.progress.Value = 25 * usersList.Count;
                    Lobby.Instance.waitingMessage.Content = "Waiting for players... (" + usersList.Count + "/4)";
                    break;
                case EnvInfos.S_SCORES:
                    List<int> score = JsonConvert.DeserializeObject<List<int>>(ev.Data.ToString());
                    scoreTeam1 = score.ElementAt(0);
                    scoreTeam2 = score.ElementAt(1);
                    break;
                case EnvInfos.S_SET_TOUR:
                    userWhoPlay = JsonConvert.DeserializeObject<string>(ev.Data.ToString());
                    break;
                case EnvInfos.S_SET_REMAINING_TIME:

                    break;
            }

        }
        public void GameEntryPoint(Packet data)
        {

        }
        public void SysEntryPoint(Packet data)
        {
            Syscall sys = JsonConvert.DeserializeObject<Syscall>(data.Data.ToString());
            if (!connected && sys.Command != SysCommand.S_CONNECTED)
            {
                return;
            }
            switch (sys.Command)
            {
                case SysCommand.S_DISCONNECTED:
                    connected = false;
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case SysCommand.S_CONNECTED:
                    connected = true;
                    Lobby lobby = new Lobby();
                    App.Current.MainWindow = lobby;
                    MainWindow.Instance.Close();
                    lobby.Show();
                    break;
                case SysCommand.S_START_GAME:
                    GameBoard board = new GameBoard();
                    App.Current.MainWindow = board;
                    Lobby.Instance.Close();
                    board.Show();
                    break;
            }
        }
    }
}
