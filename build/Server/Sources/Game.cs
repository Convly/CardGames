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
        private Deck boardDeck = new Deck();
        private Dictionary<string, Deck> usersDeck = new Dictionary<string, Deck> { };
        private TrumpInfos trumpInfos = null;
        private Dictionary<string, Card> lastRound = new Dictionary<string, Card>();
        private string currentPlayerName = "";
        private List<String> colors = new List<string> { "clubs", "diamond", "hearts", "spades" };
        private List<int> scores = new List<int> { 0, 0 };
        private List<Deck> points = new List<Deck> { new Deck(), new Deck()};
        // States
        private bool takeTrump_lock = false;
        private bool takeTrumpAs_lock = false;
        private bool gamePlayTurn_lock = false;
        private bool trumpPhase_lock = false;
        private bool playPhase_lock = false;

        public List<string> Users { get => users; set => users = value; }
        public Dictionary<string, int> Teams { get => teams; set => teams = value; }
        public Dictionary<string, Deck> UsersDeck { get => usersDeck; set => usersDeck = value; }
        public TrumpInfos TrumpInfos { get => trumpInfos; set => trumpInfos = value; }
        public string CurrentPlayerName { get => currentPlayerName; set => currentPlayerName = value; }
        public Dictionary<string, Card> LastRound { get => lastRound; set => lastRound = value; }
        public bool TrumpPhase_lock { get => TrumpPhase_lock1; set => TrumpPhase_lock1 = value; }
        public List<string> Colors { get => colors; set => colors = value; }
        public List<int> Scores { get => scores; set => scores = value; }
        public List<Deck> Points { get => points; set => points = value; }
        public Deck BoardDeck { get => boardDeck; set => boardDeck = value; }
        public bool TakeTrump_lock { get => takeTrump_lock; set => takeTrump_lock = value; }
        public bool TakeTrumpAs_lock { get => takeTrumpAs_lock; set => takeTrumpAs_lock = value; }
        public bool GamePlayTurn_lock { get => gamePlayTurn_lock; set => gamePlayTurn_lock = value; }
        public bool TrumpPhase_lock1 { get => trumpPhase_lock; set => trumpPhase_lock = value; }
        public bool PlayPhase_lock { get => playPhase_lock; set => playPhase_lock = value; }

        public Game()
        {
        }

        private void InitMasterDeck()
        {
            foreach (string color in this.Colors)
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

        private void FillUserDeck()
        {
            Random random = new Random();
            foreach (string username in this.Users)
            {
                while (this.UsersDeck[username].Array.Count() < 8 && masterCopy.Array.Count() > 0)
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
            this.TrumpPhase();
            this.PlayPhase();
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
         * GAME 
         * 
         */

        private bool TrumpPhase()
        {
            this.TrumpPhase_lock = true;
            int phase = 1;
            Console.WriteLine("Trump Phase");
            while (TrumpPhase_lock1 && phase <= 2)
            {
                Console.WriteLine("Turn " + phase);
                foreach (var user in this.Users)
                {
                    Console.WriteLine("New turn for " + user);
                    this.TrumpPhaseInitLock(phase);
                    this.CurrentPlayerName = user;
                    Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.ENV, new Envcall(EnvInfos.S_SET_TOUR, user)));
                    Network.Server.Instance.sendDataToClient(user, new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_REQUEST_TRUMP_FROM, new KeyValuePair<int, string>(phase, user))));
                    this.TrumpPhaseWait(phase);
                    if (this.TrumpInfos.Owner != null)
                        break;
                }
                phase++;
            }
            Console.WriteLine("Exiting Trump phase at turn " + phase);
            if (this.TrumpPhase_lock)
            {
                Console.WriteLine("Abort Game");
                return false;
            }
            Console.WriteLine("Launch Game!");
            Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_TRUMP, this.TrumpInfos)));
            return true;
        }

        private void TrumpPhaseInitLock(int phase)
        {
            if (phase == 1)
                this.TakeTrump_lock = true;
            else
                this.TakeTrumpAs_lock = true;
        }

        private void TrumpPhaseWait(int phase)
        {
            if (phase == 1)
                while (this.TakeTrump_lock) ;
            else
                while (this.TakeTrumpAs_lock) ;
        }

        private void AssignTrump(string name, string color)
        {
            this.TrumpInfos.Owner = name;
            this.TrumpInfos.RealColor = color;
            this.UsersDeck[name].Add(this.TrumpInfos.Card);
        }

        public void TakeTrump_callback(string name, bool ans)
        {
            Console.WriteLine("Got Take trump: " + ans + " for " + name);

            if (name != this.CurrentPlayerName)
                return;

            if (ans)
            {
                this.AssignTrump(name, this.TrumpInfos.RealColor);
                this.TrumpPhase_lock = false;
                Console.WriteLine("Trump updated, exiting phase...");
            }

            this.TakeTrump_lock = false;
        }

        public void TakeTrumpAs_callback(string name, string color)
        {
            Console.WriteLine("Got Take trump as : " + color + " for " + name);

            if (name != this.CurrentPlayerName)
                return;

            if (color != null && this.Colors.Contains(color))
            {
                this.AssignTrump(name, color);
                this.TrumpPhase_lock = false;
                Console.WriteLine("Trump updated, exiting phase...");
            }

            this.TakeTrumpAs_lock = false;
        }

        private bool PlayPhase()
        {
            this.FillUserDeck();
            Console.WriteLine("Game Phase");
            this.PlayPhase_lock = true;
            while (this.PlayPhase_lock)
            {
                this.LastRound.Clear();
                this.BoardDeck.Clear();
                Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, this.BoardDeck)));
                foreach (var user in this.Users)
                {
                    Console.WriteLine("New turn for " + user);
                    this.GamePlayTurn_lock = true;
                    this.CurrentPlayerName = user;
                    Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.ENV, new Envcall(EnvInfos.S_SET_TOUR, user)));
                    while (this.GamePlayTurn_lock) ;
                }
                Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_LASTROUND_DECK, this.LastRound)));
            }
            return true;
        }

        private int GamePlayCheckMove(Deck userDeck, Card playedCard, Deck board, string color, bool powerCheck)
        {
            Deck colorDeck = new Deck();
            foreach (var c in userDeck.Array)
            {
                if (c.Color == color)
                    colorDeck.Add(c);
            }
            if (colorDeck.Array.Count() == 0)
                return 4;

            if (playedCard.Color != color)
                return 3;

            if (powerCheck)
            {
                Card maxColorCard = (board.Array.Count() > 0)? board.Array.ElementAt(0): null;
                foreach (var c in BoardDeck.Array)
                {
                    if (maxColorCard.Value < c.Value && c.Color == color)
                    {
                        maxColorCard = c;
                    }
                }
                bool haveGreater = false;
                foreach (var c in colorDeck.Array)
                {
                    if (c.Value > maxColorCard.Value)
                        haveGreater = true;
                }
                if (haveGreater && playedCard.Value < maxColorCard.Value)
                    return 1;
            }
            return 0;
        }

        private void PlayCard(string name, Card card)
        {
            this.UsersDeck[name].Remove(card);
            this.BoardDeck.Add(card);
            this.LastRound.Add(name, card);
            Network.Server.Instance.SendToAllClient(new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, this.BoardDeck)));
            Network.Server.Instance.sendDataToClient(name, new Packet("root", PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[name])));
            this.GamePlayTurn_lock = false;
        }

        public void PlayCard_callback(string name, Card card)
        {
            if (name != this.CurrentPlayerName)
                return;

            //if (this.UsersDeck[name].Array.Contains(card))
            //{
                // Get main color if it exists
                string color = card.Color;
                if (this.BoardDeck.Array.Count() > 0)
                    color = BoardDeck.Array.ElementAt(0).Color;
                int colorCheck = this.GamePlayCheckMove(this.UsersDeck[name], card, BoardDeck, color, false);
                switch (colorCheck)
                {
                    case 0:
                        this.PlayCard(name, card);
                        break;
                    case 3:
                        Network.Server.Instance.sendDataToClient(name, new Packet("root", PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "You have some " + color + " to play!")));
                        break;
                    case 4:
                        int trumpCheck = this.GamePlayCheckMove(this.UsersDeck[name], card, BoardDeck, this.TrumpInfos.RealColor, true);
                        switch (trumpCheck)
                        {
                            case 0:
                                this.PlayCard(name, card);
                                break;
                            case 1:
                                Network.Server.Instance.sendDataToClient(name, new Packet("root", PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "You have a better card than a " + card.Value + " of " + card.Color + "(trump) to play!")));
                                break;
                            case 3:
                                Network.Server.Instance.sendDataToClient(name, new Packet("root", PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "You have some " + TrumpInfos.RealColor + "(which is trump color) to play!")));
                                break;
                            case 4:
                                this.PlayCard(name, card);
                                break;
                        }
                        break;
                }
            }
       // }
    }
}
