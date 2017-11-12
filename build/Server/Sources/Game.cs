using CardGameResources.Game;
using CardGameResources.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Servers.Sources
{
    /// <summary>
    /// Game Logic for the Belote card Game used by <see cref="Referee"/>
    /// </summary>
    class Game
    {
        /// <summary>
        /// Default constructor for <see cref="Game"/>
        /// </summary>
        public Game()
        {
        }

        /// <summary>
        /// Entry point of the game. Called by the referee when 4 players are connected
        /// </summary>
        public void StartGame()
        {
            this.GameLaunched = true;
            this.Init();
            this.Run();
            this.Reset();
            Core.Locker = false;
        }

        /// <summary>
        /// This method will randomly fill the <see cref="Deck"/> of all the connected users with a limit of 8 <see cref="Card"/> per user.
        /// </summary>
        private void FillUserDeck()
        {
            Random random = new Random();
            foreach (string username in this.Users)
            {
                while (this.UsersDeck[username].Array.Count() < 8 && MasterCopy.Array.Count() > 0)
                {
                    int index = random.Next(0, MasterCopy.Array.Count());
                    Card card = MasterCopy.Array.ElementAt(index);
                    this.UsersDeck[username].Add(card);
                    Console.WriteLine("_> Adding new card in the " + username + "'s deck: " + card.Value + ":" + card.Color + ". (" + MasterCopy.Array.Count() + " left in the deck)");
                    MasterCopy.Remove(index);
                }
                this.Send(username, PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[username]));
            }
        }

        /// <summary>
        /// This method will chose a card for the trump randomly and will then notify all the clients about it
        /// </summary>
        private void TrumpDecision()
        {
            Console.WriteLine("_> About to choose what'll be the potential trump...");
            Random random = new Random();
            int index = random.Next(0, this.MasterCopy.Array.Count());
            this.TrumpInfos = new TrumpInfos(this.MasterCopy.Array.ElementAt(index));
            this.MasterCopy.Remove(index);
            Console.WriteLine("_> The trump is set to " + this.TrumpInfos.Card.Value + ":" + this.TrumpInfos.Card.Color);
            this.Send(PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, new Deck(new List<Card> { this.TrumpInfos.Card })));
        }

        /// <summary>
        /// This method distribute randomly 5 <see cref="Card"/> per user
        /// </summary>
        private void GiveCards()
        {
            this.InitMasterDeck();
            this.InitUsersDeck();
        }

        /// <summary>
        /// Return the real value for the paramaeter <paramref name="card"/>.
        /// This method allow the user to do some arithmetics and comparaison with the cards.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>The value of the <paramref name="card"/> as an integer</returns>
        private int GetCardValue(Card card)
        {
            if (card == null)
                return 0;
            return (card.Color == this.TrumpInfos.RealColor) ? TrumpCardValues[card.Value] : BasicCardValues[card.Value];
        }

        /// <summary>
        /// Get the amount of point for a specific <see cref="Card"/>. This method take in consideration the actual trump.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>The amount of point for <paramref name="card"/></returns>
        private int GetCardPoints(Card card)
        {
            if (card == null)
                return 0;
            return (card.Color == this.TrumpInfos.RealColor) ? TrumpCardPoints[card.Value] : BasicCardPoints[card.Value];
        }

        /// <summary>
        /// This method check who's the winner for the current round, update the scores and notify the users about it
        /// </summary>
        /// <returns>The name of the winner</returns>
        private string CheckDeckWinner()
        {
            string mainColor = this.LastRound[this.CurrentPlayerName].Color;
            string tcolor = this.TrumpInfos.RealColor;
            KeyValuePair<string, Card> maxItem = new KeyValuePair<string, Card>("", this.TrumpInfos.Card);
            int score = 0;
            foreach (var item in this.LastRound)
            {
                if (maxItem.Key == "" || (
                    (item.Value.Color == maxItem.Value.Color && this.GetCardValue(item.Value) > this.GetCardValue(maxItem.Value))
                    || (item.Value.Color == tcolor && maxItem.Value.Color != tcolor)
                    || (item.Value.Color == tcolor && maxItem.Value.Color == tcolor && this.GetCardValue(item.Value) > this.GetCardValue(maxItem.Value))))
                {
                    maxItem = item;
                }
                score += this.GetCardPoints(item.Value);
            }
            int team = this.Teams[maxItem.Key];
            this.Scores[team] += score;

            if (this.RemainingCards == 0)
            {
                this.Scores[team] = (this.Scores[(team + 1) % 2] == 0)? 252: this.Scores[team] + 10;
            }

            this.Send(PacketType.ENV, new Envcall(EnvInfos.S_SCORES, this.Scores));
            Console.WriteLine("_> " + maxItem.Key + " won this lap with " + score + " points");
            Console.WriteLine("_> Scores:    Team 1 : " + this.Scores.ElementAt(0) + " - " + this.Scores.ElementAt(1) + " : Team 2");
            return maxItem.Key;
        }

        /// <summary>
        /// Generate a new list of user which indicate the order of the plays
        /// </summary>
        /// <param name="begin">The username which will play first</param>
        /// <returns>The ordered list of players</returns>
        private List<string> GetUserListFrom(string begin)
        {
            Console.WriteLine("_> Generating new user list where root is " + begin);
            List<string> list = new List<string>();
            List<string> tpUser = this.Users;
            tpUser.Reverse();
            int beginIndex = tpUser.IndexOf(begin);
            for (int i = 0; i < 4; ++i)
            {
                list.Add(tpUser.ElementAt((beginIndex + i) % 4));
                Console.WriteLine("   -> " + list.ElementAt(i));
            }
            this.CurrentRoundOrder = list;
            return list;
        }

        /// <summary>
        /// This method will check if there is at least one <see cref="Card"/> with the color <paramref name="color"/> in the <paramref name="deck"/>
        /// </summary>
        /// <param name="deck">The <see cref="Deck"/> of card which will be the object of the search.</param>
        /// <param name="color">The color which will be used for the search.</param>
        /// <returns>The number of <see cref="Card"/> which match the <paramref name="color"/></returns>
        private int IsColorInDeck(Deck deck, string color)
        {
            int k = 0;
            foreach (var card in deck.Array)
            {
                if (card.Color == color)
                    k++;
            }
            return k;
        }

        /// <summary>
        /// This method will initialize each component of the game logic
        /// </summary>
        private void Init()
        {
            Console.WriteLine("_> Starting game...");
            this.Send(PacketType.SYS, new Syscall(SysCommand.S_START_GAME, null));

            Console.WriteLine("_> Setting teams randomly...");
            int x = 0;
            foreach (var username in this.Users)
            {
                this.Teams.Add(username, x % 2);
                Console.WriteLine("_> Team " + this.Teams[username].ToString() + " has been assigned to " + username + ".");
                x++;
            }
            this.Send(PacketType.ENV, new Envcall(EnvInfos.S_SET_TEAM, this.Teams));
            this.GiveCards();
            this.TrumpDecision();
            this.Send(PacketType.ENV, new Envcall(EnvInfos.S_SCORES, this.Scores));
        }

        /// <summary>
        /// Initialize the master deck, which is a constant <see cref="Deck"/> of all the 32 avalaible cards.
        /// </summary>
        private void InitMasterDeck()
        {
            foreach (string color in this.Colors)
            {
                foreach (char c in "789tjqka")
                {
                    this.MasterDeck.Add(new Card(c, color));
                }
            }
            this.MasterCopy = this.MasterDeck;
        }

        /// <summary>
        /// Initialize the user <see cref="Deck"/>s with 5 <see cref="Card"/>
        /// </summary>
        private void InitUsersDeck()
        {
            Random random = new Random();
            foreach (string username in this.Users)
            {
                this.UsersDeck.Add(username, new Deck());
                for (int x = 0; x < 5; ++x)
                {
                    int index = random.Next(0, MasterCopy.Array.Count());
                    Card card = MasterCopy.Array.ElementAt(index);
                    this.UsersDeck[username].Add(card);
                    Console.WriteLine("_> Adding new card in the " + username + "'s deck: " + card.Value + ":" + card.Color + ". (" + MasterCopy.Array.Count() + " left in the deck)");
                    MasterCopy.Remove(index);
                }
                this.Send(username, PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[username]));
            }
        }

        
        /// <summary>
        /// Start the GamePlay phases: TrumpPhase, PlayPhase ; then end the game
        /// </summary>
        private void Run()
        {
            if (this.TrumpPhase())
            {
                this.PlayPhase();
            }
            this.Send(PacketType.SYS, new Syscall(SysCommand.S_END_GAME, null));
        }

        /// <summary>
        /// Manage the trump phase: init, request, chose, assign and notify
        /// </summary>
        /// <returns></returns>
        private bool TrumpPhase()
        {
            this.TrumpPhase_lock = true;
            int phase = 1;
            Console.WriteLine("_> Trump Phase");
            while (TrumpPhase_lock && phase <= 2)
            {
                Console.WriteLine("_> Turn " + phase);
                foreach (var user in this.Users)
                {
                    Console.WriteLine("_> New turn for " + user);
                    this.TrumpPhaseInitLock(phase);
                    this.CurrentPlayerName = user;
                    this.Send(PacketType.ENV, new Envcall(EnvInfos.S_SET_TOUR, user));
                    this.Send(user, PacketType.GAME, new Gamecall(GameAction.S_REQUEST_TRUMP_FROM, new KeyValuePair<int, string>(phase, user)));
                    this.TrumpPhaseWait(phase);
                    if (this.TrumpInfos.Owner != null)
                        break;
                }
                phase++;
            }
            Console.WriteLine("_> Exiting Trump phase at turn " + (phase - 1));
            if (this.TrumpPhase_lock)
            {
                Console.WriteLine("_> No trump has been chosed, exiting...");
                return false;
            }
            Console.WriteLine("_> Launch Game!");
            this.Send(PacketType.GAME, new Gamecall(GameAction.S_SET_TRUMP, this.TrumpInfos));
            return true;
        }

        /// <summary>
        /// Manage the locks for the trump phase
        /// </summary>
        /// <param name="phase"></param>
        private void TrumpPhaseInitLock(int phase)
        {
            if (phase == 1)
                this.TakeTrump_lock = true;
            else
                this.TakeTrumpAs_lock = true;
        }

        /// <summary>
        /// Lock the actions for trump phase
        /// </summary>
        /// <param name="phase"></param>
        private void TrumpPhaseWait(int phase)
        {
            if (phase == 1)
                while (this.TakeTrump_lock) ;
            else
                while (this.TakeTrumpAs_lock) ;
        }

        /// <summary>
        /// Assign a <paramref name="name"/> and a <paramref name="color"/> to the <see cref="TrumpInfos"/>
        /// </summary>
        /// <param name="name">The name of the owner</param>
        /// <param name="color">The name of the real color of the tump</param>
        private void AssignTrump(string name, string color)
        {
            this.TrumpInfos.Owner = name;
            this.TrumpInfos.RealColor = color;
            this.UsersDeck[name].Add(this.TrumpInfos.Card);
        }

        /// <summary>
        /// Callback method when the server receive a <see cref="GameAction.C_TAKE_TRUMP"/> request
        /// </summary>
        /// <param name="name">The name of the user who sent the request</param>
        /// <param name="ans">The answer of the user</param>
        public void TakeTrump_callback(string name, bool ans)
        {
            Console.WriteLine("_> Got Take trump: " + ans + " for " + name);

            if (name != this.CurrentPlayerName)
                return;

            if (ans)
            {
                this.AssignTrump(name, this.TrumpInfos.Card.Color);
                this.TrumpPhase_lock = false;
                Console.WriteLine("_> Trump updated, exiting phase...");
            }

            this.TakeTrump_lock = false;
        }

        /// <summary>
        /// Callback method when the server receive a <see cref="GameAction.C_TAKE_TRUMP_AS"/> request
        /// </summary>
        /// <param name="name">The name of the user who sent the request</param>
        /// <param name="color">A string containing the color of the trump, or nothing</param>
        public void TakeTrumpAs_callback(string name, string color)
        {
            Console.WriteLine("_> Got Take trump as : " + color + " for " + name);

            if (name != this.CurrentPlayerName)
                return;

            if (color != null && this.Colors.Contains(color))
            {
                this.AssignTrump(name, color);
                this.TrumpPhase_lock = false;
                Console.WriteLine("_> Trump updated, exiting phase...");
            }

            this.TakeTrumpAs_lock = false;
        }

        /// <summary>
        /// Entry point for the main gameplay phase. Manage the core of the game.
        /// </summary>
        private bool PlayPhase()
        {
            this.FillUserDeck();
            Console.WriteLine("_> Game Phase");
            this.PlayPhase_lock = true;
            this.CurrentPlayerName = this.Users.ElementAt(0);
            while (this.PlayPhase_lock && this.RemainingCards > 0)
            {
                this.LastRound.Clear();
                this.BoardDeck.Clear();
                this.Send(PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, this.BoardDeck));
                List<string> roundUserList = this.GetUserListFrom(this.CurrentPlayerName);
                foreach (var user in roundUserList)
                {
                    Console.WriteLine("_> New turn for " + user);
                    this.GamePlayTurn_lock = true;
                    this.CurrentPlayerName = user;
                    this.Send(PacketType.ENV, new Envcall(EnvInfos.S_SET_TOUR, user));
                    while (this.GamePlayTurn_lock) ;
                }
                this.CurrentPlayerName = this.CheckDeckWinner();
                this.Send(PacketType.GAME, new Gamecall(GameAction.S_SET_LASTROUND_DECK, this.LastRound));
            }
            return true;
        }

        /// <summary>
        /// Utility method mainly used by <see cref="PlayCard_callback(string, Card)"/> to check if a user can play a specific <see cref="Card"/>
        /// </summary>
        /// <param name="userDeck">The deck from which "<paramref name="playedCard"/>" is issued</param>
        /// <param name="playedCard">The <see cref="Card"/> played</param>
        /// <param name="board">The current <see cref="Deck"/> on the board</param>
        /// <param name="color">The current board main color</param>
        /// <param name="powerCheck">If set to true, the method will also check if the user have a better card in he's <see cref="Deck"/></param>
        /// <returns></returns>
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

                    if (this.GetCardValue(maxColorCard) < this.GetCardValue(c) && c.Color == color)
                    {
                        maxColorCard = c;
                    }
                }
                foreach (var c in colorDeck.Array)
                {
                    if ((maxColorCard != null && this.GetCardValue(c) > this.GetCardValue(maxColorCard)) && this.GetCardValue(playedCard) < this.GetCardValue(maxColorCard))
                        return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// This function will play a specific <see cref="Card"/> for <paramref name="name"/>.
        /// It'll also update all the needed variables and trigger all the changes to the users.
        /// </summary>
        /// <param name="name">The name of the user who sent the command</param>
        /// <param name="card">The card that the user want to play</param>
        private void PlayCard(string name, Card card)
        {
            this.RemainingCards -= 1;
            this.UsersDeck[name].Remove(card);
            this.BoardDeck.Add(card);
            this.LastRound.Add(name, card);
            this.Send(PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, this.BoardDeck));
            this.Send(name, PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[name]));
            Console.WriteLine("_> " + name + " played " + card.Value + ":" + card.Color);
            this.GamePlayTurn_lock = false;
        }

        /// <summary>
        /// Entry point of the main rules method. It will be called when the server receive a request of type <see cref="GameAction.C_PLAY_CARD"/>
        /// This function act also as a referee by checking if the user movement does not break any rules.
        /// </summary>
        /// <param name="name">The name of the user who sent the command</param>
        /// <param name="card">The card that the user want to play</param>
        public void PlayCard_callback(string name, Card card)
        {
            if (name != this.CurrentPlayerName)
                return;

            if (this.UsersDeck[name].Contains(card))
            {
                //Get main color if it exists
                string color = card.Color;
                if (this.BoardDeck.Array.Count() > 0)
                    color = BoardDeck.Array.ElementAt(0).Color;
                int colorCheck = (color != this.TrumpInfos.RealColor)? this.GamePlayCheckMove(this.UsersDeck[name], card, BoardDeck, color, false): 4;
                switch (colorCheck)
                {
                    case 0:
                        this.PlayCard(name, card);
                        break;
                    case 3:
                        this.Send(name, PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "Main color is " + color + " and you have some in your deck. Please play a valid card"));
                        break;
                    case 4:
                        int trumpCheck = this.GamePlayCheckMove(this.UsersDeck[name], card, BoardDeck, this.TrumpInfos.RealColor, true);
                        switch (trumpCheck)
                        {
                            case 0:
                                this.PlayCard(name, card);
                                break;
                            case 1:
                                this.Send(name, PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "You have a better card than a " + card.Value + " of " + card.Color + " (trump color) to play!"));
                                break;
                            case 3:
                                this.Send(name, PacketType.ERR, new Errcall(Err.FORBIDDEN_CARD, "You have some " + this.TrumpInfos.RealColor + " to play! (which is trump color)"));
                                break;
                            case 4:
                                this.PlayCard(name, card);
                                break;
                        }
                        break;
                }
            }
       }

        /// <summary>
        /// This method is designed to totally reset the game logic.
        /// </summary>
        public void Reset()
        {
            this.TrumpInfos = null;
            // Useful decks and cards containers
            this.MasterDeck = new Deck();
            this.MasterCopy = new Deck();
            this.BoardDeck = new Deck();
            this.UsersDeck = new Dictionary<string, Deck> { };
            this.LastRound = new Dictionary<string, Card>();
            this.CurrentRoundOrder = new List<string>();
            this.CurrentPlayerName = "";
            // Utils definitions
            this.RemainingCards = 32;
            this.Teams = new Dictionary<string, int> { };
            this.Users = new List<string> { };
            // Points counting tools
            this.Scores = new List<int> { 0, 0 };
            this.Points = new List<Deck> { new Deck(), new Deck() };
            // States
            this.GameLaunched = false;
            this.TakeTrump_lock = false;
            this.TakeTrumpAs_lock = false;
            this.GamePlayTurn_lock = false;
            this.TrumpPhase_lock = false;
            this.PlayPhase_lock = false;
        }

        /// <summary>
        /// Reconnect a player who's been previously disconnect from the server.
        /// </summary>
        /// <param name="name">The name of the player who just registered</param>
        public void Reconnect(string name)
        {
            Console.WriteLine("_> Reconnect player " + name);
            this.Send(name, PacketType.SYS, new Syscall(SysCommand.S_START_GAME, null));
            this.Send(name, PacketType.GAME, new Gamecall(GameAction.S_SET_BOARD_DECK, this.BoardDeck));
            this.Send(name, PacketType.GAME, new Gamecall(GameAction.S_SET_USER_DECK, this.UsersDeck[name]));
            this.Send(name, PacketType.GAME, new Gamecall(GameAction.S_SET_LASTROUND_DECK, this.LastRound));
            this.Send(name, PacketType.ENV, new Envcall(EnvInfos.S_SCORES, this.Scores));
            this.Send(name, PacketType.ENV, new Envcall(EnvInfos.S_SET_TEAM, this.Teams));
            this.Send(name, PacketType.GAME, new Gamecall(GameAction.S_SET_TRUMP, this.TrumpInfos));
            this.Send(name, PacketType.ENV, new Envcall(EnvInfos.S_SET_TOUR, this.CurrentPlayerName));
        }

        /// <summary>
        /// Send some <paramref name="data"/> of type "<paramref name="type"/>" to <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The username who will receive the request</param>
        /// <param name="type">The type of the <paramref name="data"/> which will be send</param>
        /// <param name="data">The data which will be send</param>
        public bool Send(string name, PacketType type, Object data)
        {
            Packet p = new Packet("root", type, data);
            try
            {
               Network.Server.Instance.SendDataToClient(name, p);
            }
            catch (Exception)
            {
                this.AIEntryPoint(name, p);
            }
            return true;
        }

        /// <summary>
        /// Send some <paramref name="data"/> of type "<paramref name="type"/>" to all registered users.
        /// </summary>
        /// <param name="type">The type of <paramref name="data"/> which will be send</param>
        /// <param name="data">The data which will be send</param>
        /// <returns></returns>
        public bool Send(PacketType type, Object data)
        {
            Packet p = new Packet("root", type, data);
            string cuser = null;
            List<string> brokenLinks = new List<string>();
            foreach (var username in this.Users)
            {
                cuser = username;
                try
                {
                    Network.Server.Instance.SendDataToClient(cuser, p);
                }
                catch (Exception err)
                {
                    Console.Error.WriteLine(err.Message);
                    brokenLinks.Add(cuser);
                }
            }

            foreach (var username in brokenLinks)
            {
                this.AIEntryPoint(username, p);
            }
            return true;
        }

        /// <summary>
        /// Main entry point for all the AI actions
        /// </summary>
        /// <param name="name">Define the name of the player which the AI will play for.</param>
        /// <param name="p">The packet transmission that the AI will use</param>
        private void AIEntryPoint(string name, Packet p)
        {
            switch (p.Type)
            {
                case PacketType.ENV:
                    Envcall e_evt = p.Data as Envcall;
                    switch (e_evt.Type)
                    {
                        case EnvInfos.S_SET_TOUR:
                            this.AIPlayCard(name);
                            break;
                    }
                    break;
                case PacketType.GAME:
                    Gamecall g_evt = p.Data as Gamecall;
                    switch (g_evt.Action)
                    {
                        case GameAction.S_REQUEST_TRUMP_FROM:
                            this.AITakeTrump((KeyValuePair<int, string>)g_evt.Data);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Logic for AI on <see cref="GameAction.S_REQUEST_TRUMP_FROM"/> event.
        /// </summary>
        /// <param name="data">A pair of value which contains the lap number and the name of the player who must play</param>
        private void AITakeTrump(KeyValuePair<int, string> data)
        {
            string name = data.Value;
            int lap = data.Key;

            Console.WriteLine("_> AI act for trump");

            Deck uDeck = this.UsersDeck[name];
            Card trump = this.TrumpInfos.Card;

            Dictionary<string, int> points = new Dictionary<string, int>();
            foreach (var color in this.Colors)
                points.Add(color, 0);

            KeyValuePair<string, int> max = new KeyValuePair<string, int>("hearts", 0);
            foreach (var card in uDeck.Array)
            {
                points[card.Color] += this.TrumpCardPoints[card.Value];
                if (points[card.Color] > max.Value)
                    max = new KeyValuePair<string, int>(card.Color, points[card.Color]);
            }

            Thread.Sleep(this.AILatency);

            if (lap == 1)
            {
                Referee.Instance.EntryPoint(JsonConvert.SerializeObject(new Packet(name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP, (max.Key == trump.Color)))));
            }
            else if (lap == 2)
            {
                Referee.Instance.EntryPoint(JsonConvert.SerializeObject(new Packet(name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP_AS, ((max.Value > 30) ? max.Key : "")))));
            }
        }

        /// <summary>
        /// Logic for AI on <see cref="EnvInfos.S_SET_TOUR"/> event.
        /// </summary>
        /// <param name="name">The name of the player who must play</param>
        private void AIPlayCard(string name)
        {
            if (name != this.CurrentPlayerName || this.TrumpPhase_lock)
                return;

            Console.WriteLine("_> AI playing a card for " + name);

            Deck uDeck = this.UsersDeck[name];
            Dictionary<string, Card> board = this.LastRound;
            string color = (board.Count() == 0) ? null : board[this.CurrentRoundOrder.ElementAt(0)].Color;

            Deck colorDeck = new Deck();
            Deck trumpDeck = new Deck();
            Deck restDeck = new Deck();
            Card maxItem = uDeck.Array.ElementAt(0);

            foreach (var item in uDeck.Array)
            {
                if (item.Color == color)
                {
                    if (this.GetCardValue(item) > this.GetCardValue(maxItem) || maxItem.Color != color)
                    {
                        maxItem = item;
                    }
                    colorDeck.Add(item);

                } else if (item.Color == this.TrumpInfos.RealColor)
                {
                    if (this.IsColorInDeck(uDeck, color) == 0 && (this.GetCardValue(item) > this.GetCardValue(maxItem) || maxItem.Color != this.TrumpInfos.RealColor))
                    {
                        maxItem = item;
                    }
                    trumpDeck.Add(item);
                } else
                {
                    if (this.IsColorInDeck(uDeck, color) == 0 && this.IsColorInDeck(uDeck, this.TrumpInfos.RealColor) == 0 && this.GetCardValue(item) > this.GetCardValue(maxItem))
                    {
                        maxItem = item;
                    }
                    restDeck.Add(item);
                }
            }

            Thread.Sleep(this.AILatency);

            Console.WriteLine("_> " + maxItem.Value + ":" + maxItem.Color + " will be play for " + name);
            Referee.Instance.EntryPoint(JsonConvert.SerializeObject(new Packet(name, PacketType.GAME, new Gamecall(GameAction.C_PLAY_CARD, maxItem))));
        }

        // Trump infos
        private TrumpInfos trumpInfos = null;
        // Useful decks and cards containers
        private Deck masterDeck = new Deck();
        private Deck masterCopy = new Deck();
        private Deck boardDeck = new Deck();
        private Dictionary<string, Deck> usersDeck = new Dictionary<string, Deck> { };
        private Dictionary<string, Card> lastRound = new Dictionary<string, Card>();
        private List<string> currentRoundOrder = new List<string>();
        private string currentPlayerName = "";
        // Utils definitions
        private int aILatency = 1000;
        private int remainingCards = 32;
        private Dictionary<string, int> teams = new Dictionary<string, int> { };
        private List<string> users = new List<string> { };
        private List<String> colors = new List<string> { "clubs", "diamond", "hearts", "spades" };
        // Points counting tools
        private List<int> scores = new List<int> { 0, 0 };
        private List<Deck> points = new List<Deck> { new Deck(), new Deck() };
        // Cards values
        private Dictionary<char, int> basicCardValues = new Dictionary<char, int> { { '7', 1 }, { '8', 2 }, { '9', 3 }, { 'j', 4 }, { 'q', 5 }, { 'k', 6 }, { 't', 7 }, { 'a', 8 } };
        private Dictionary<char, int> trumpCardValues = new Dictionary<char, int> { { '7', 1 }, { '8', 2 }, { 'q', 3 }, { 'k', 4 }, { 't', 5 }, { 'a', 6 }, { '9', 7 }, { 'j', 8 } };
        // Cards Points
        private Dictionary<char, int> basicCardPoints = new Dictionary<char, int> { { '7', 0 }, { '8', 0 }, { '9', 0 }, { 'j', 2 }, { 'q', 3 }, { 'k', 4 }, { 't', 10 }, { 'a', 11 } };
        private Dictionary<char, int> trumpCardPoints = new Dictionary<char, int> { { '7', 0 }, { '8', 0 }, { 'q', 3 }, { 'k', 4 }, { 't', 10 }, { 'a', 11 }, { '9', 14 }, { 'j', 20 } };
        // States
        private bool gameLaunched = false;
        private bool takeTrump_lock = false;
        private bool takeTrumpAs_lock = false;
        private bool gamePlayTurn_lock = false;
        private bool trumpPhase_lock = false;
        private bool playPhase_lock = false;

        public TrumpInfos TrumpInfos { get => trumpInfos; set => trumpInfos = value; }
        public Deck MasterDeck { get => masterDeck; set => masterDeck = value; }
        public Deck MasterCopy { get => masterCopy; set => masterCopy = value; }
        public Deck BoardDeck { get => boardDeck; set => boardDeck = value; }
        public Dictionary<string, Deck> UsersDeck { get => usersDeck; set => usersDeck = value; }
        public Dictionary<string, Card> LastRound { get => lastRound; set => lastRound = value; }
        public List<string> CurrentRoundOrder { get => currentRoundOrder; set => currentRoundOrder = value; }
        public string CurrentPlayerName { get => currentPlayerName; set => currentPlayerName = value; }
        public int RemainingCards { get => remainingCards; set => remainingCards = value; }
        public Dictionary<string, int> Teams { get => teams; set => teams = value; }
        public List<string> Users { get => users; set => users = value; }
        public List<string> Colors { get => colors; set => colors = value; }
        public List<int> Scores { get => scores; set => scores = value; }
        public List<Deck> Points { get => points; set => points = value; }
        public Dictionary<char, int> BasicCardValues { get => basicCardValues; set => basicCardValues = value; }
        public Dictionary<char, int> TrumpCardValues { get => trumpCardValues; set => trumpCardValues = value; }
        public Dictionary<char, int> BasicCardPoints { get => basicCardPoints; set => basicCardPoints = value; }
        public Dictionary<char, int> TrumpCardPoints { get => trumpCardPoints; set => trumpCardPoints = value; }
        public bool GameLaunched { get => gameLaunched; set => gameLaunched = value; }
        public bool TakeTrump_lock { get => takeTrump_lock; set => takeTrump_lock = value; }
        public bool TakeTrumpAs_lock { get => takeTrumpAs_lock; set => takeTrumpAs_lock = value; }
        public bool GamePlayTurn_lock { get => gamePlayTurn_lock; set => gamePlayTurn_lock = value; }
        public bool TrumpPhase_lock { get => trumpPhase_lock; set => trumpPhase_lock = value; }
        public bool PlayPhase_lock { get => playPhase_lock; set => playPhase_lock = value; }
        public int AILatency { get => aILatency; set => aILatency = value; }
    }
}
