using CardGameResources.Game;
using CardGameResources.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servers.Sources
{
    class Game
    {
        private List<string> users = new List<string> { };
        private Dictionary<string, int> teams = new Dictionary<string, int> { };
        private Deck masterDeck = new Deck();
        private Deck masterCopy = new Deck();
        private Dictionary<string, Deck> usersDeck = new Dictionary<string, Deck> { };
        private TrumpInfos trumpInfos = null;
        private Dictionary<string, Card> lastRound = new Dictionary<string, Card>();
        private string currentPlayerName = "";

        public List<string> Users { get => users; set => users = value; }
        public Dictionary<string, int> Teams { get => teams; set => teams = value; }
        public Dictionary<string, Deck> UsersDeck { get => usersDeck; set => usersDeck = value; }
        public TrumpInfos TrumpInfos { get => trumpInfos; set => trumpInfos = value; }
        public string CurrentPlayerName { get => currentPlayerName; set => currentPlayerName = value; }
        public Dictionary<string, Card> LastRound { get => lastRound; set => lastRound = value; }

        public Game()
        {
        }

        private void InitMasterDeck()
        {
            foreach (string color in new List<String> { "clubs", "diamond", "hearts", "spades" })
            {
                foreach (char c in "789tjqka")
                {
                    this.masterDeck.Add(new Card(c, color));
                    Console.WriteLine("New Card: /" + color + "/" + c + ".png");
                }
            }
            this.masterCopy = this.masterDeck;
        }

        private void InitUsersDeck()
        {
            Random random = new Random();
            foreach (string username in this.Users)
            {
                this.UsersDeck.Add(username, new Deck());
                for (int x = 0; x < 5; ++x)
                {
                    int index = random.Next(0, masterCopy.Array.Count());
                    Card card = masterCopy.Array.ElementAt(index);
                    this.UsersDeck[username].Add(card);
                    Console.WriteLine("Adding new card in the " + username + "'s deck: " + card.Value + ":" + card.Color + ". (" + masterCopy.Array.Count() + " left in the deck)");
                    masterCopy.Remove(index);
                }
                Network.Server.Instance.sendDataToClient(username, new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[username])));
            }
        }

        private void TrumpDecision()
        {
            Console.WriteLine("About to choose what'll be the potential trump...");
            Random random = new Random();
            int index = random.Next(0, this.masterCopy.Array.Count());
            this.TrumpInfos = new TrumpInfos(this.masterCopy.Array.ElementAt(index));
            this.masterCopy.Remove(index);
            Console.WriteLine("The trump is set to " + this.TrumpInfos.Card.Value + ":" + this.TrumpInfos.Card.Color);
            Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, new Deck(new List<Card> { this.TrumpInfos.Card }))));
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
            Console.WriteLine("Starting game...");
            Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.SYS, new Syscall(SysCommand.S_START_GAME, null)));

            Thread.Sleep(500);

            Console.WriteLine("Setting teams randomly...");
            int x = 0;
            foreach (var username in this.Users)
            {
                this.Teams.Add(username, x % 2);
                Console.WriteLine("Team " + this.Teams[username].ToString() + " has been assigned to " + username + ".");
                x++;
            }
            Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.ENV, new Envcall(EnvInfos.S_SET_TEAM, this.Teams)));
            this.GiveCards();
            this.TrumpDecision();
        }

        private void GiveCards()
        {
            Console.WriteLine("About to give some cards...");
            this.InitMasterDeck();
            this.InitUsersDeck();
        }

        /**
         * 
         * SYS 
         * 
         */

        //private void SetRemainingTime(Func<Object, int> callback, int duration)
        //{
        //    Timer t = new Timer(new TimerCallback(callback));
        //    t.Start();
        //}

        /**
         * 
         * RULES
         * 
         */
    }
}
