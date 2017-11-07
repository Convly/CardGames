using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum GameAction
    {
        PLAY_CARD = 0,
        TAKE_TRUMP,
        TAKE_TRUMP_AS,
        SHOT_BELOT,
        SHOT_REBELOT
    }

    public class Gamecall
    {
        private GameAction action;
        private List<string> args;

        public void GameCall(GameAction action_, List<string> args_)
        {
            this.Action = action_;
            this.Args = args_;
        }

        public GameAction Action { get => action; set => action = value; }
        public List<string> Args { get => args; set => args = value; }
    }
}
