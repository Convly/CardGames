using CardGameResources.Game;
using CardGameResources.Net;
using Client.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        private Deck userDeck;
        private Deck boardDeck;

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
            Errcall er = JsonConvert.DeserializeObject<Errcall>(data.Data.ToString());
            MessageBox.Show(er.Message);
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
            Gamecall game = JsonConvert.DeserializeObject<Gamecall>(data.Data.ToString());
            switch (game.Action)
            {
                case GameAction.S_SET_USER_DECK:
                    userDeck = JsonConvert.DeserializeObject<Deck>(game.Data.ToString());
                    for (int i = 0; i < userDeck.Array.Count() && i < 8; ++i)
                    {
                        Image image = GameBoard.Instance.userDeck_panel.Children[i] as Image;
                        image.Source = new BitmapImage(new Uri(String.Format("/Client;component/Img/cards/" + userDeck.Array.ElementAt(i).Color + "/" + userDeck.Array.ElementAt(i).Value.ToString() + ".png"), UriKind.Relative));
                    }
                    break;
                case GameAction.S_SET_BOARD_DECK:

                    break;
                case GameAction.S_SET_LASTROUND_DECK:

                    break;
                case GameAction.S_SET_TRUMP:

                    break;
                case GameAction.S_REQUEST_TRUMP_FROM:

                    break;
            }
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
