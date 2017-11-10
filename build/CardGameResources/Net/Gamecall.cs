using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum GameAction
    {
        C_PLAY_CARD, // Card
        C_TAKE_TRUMP, // bool
        C_TAKE_TRUMP_AS, // string
        S_SET_USER_DECK, // Deck
        S_SET_BOARD_DECK, // Deck
        S_SET_LASTROUND_DECK, // Dictionary<string, Card> (size: 4)
        S_SET_TRUMP, // TrumpInfos
        S_REQUEST_TRUMP_FROM // KeyValuePair<int, string>
    }

    public class Gamecall
    {
        private GameAction action;
        private Object data;

        public Gamecall(GameAction action_, Object data_)
        {
            this.Action = action_;
            this.Data = data_;
        }

        public GameAction Action { get => action; set => action = value; }
        public object Data { get => data; set => data = value; }
    }
}
