using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Game
{
    public class TrumpInfos
    {
        private Card card;
        private string owner = null;
        private string realColor = null;

        public TrumpInfos(Card card_)
        {
            this.Card = card_;
        }

        public TrumpInfos(Card card_, string owner_, string realColor_)
        {
            this.Card = card_;
            this.Owner = owner_;
            this.RealColor = realColor_;
        }

        public Card Card { get => card; set => card = value; }
        public string Owner { get => owner; set => owner = value; }
        public string RealColor { get => realColor; set => realColor = value; }
    }
}
