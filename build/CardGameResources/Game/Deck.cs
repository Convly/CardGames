using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Game
{
    public class Deck
    {
        private List<Card> array;

        public Deck()
        {
        }

        public Deck(List<Card> array_)
        {
            this.Array = array_;
        }

        public bool Add(Card card_)
        {
            this.Array.Add(card_);
            return true;
        }

        public bool Remove(int index)
        {
            if (index >= this.Array.Count())
            {
                return false;
            }
            this.Array.RemoveAt(index);
            return true;
        }

        public bool Remove(Card card)
        {
            this.Array.Remove(card);
            return true;
        }

        public bool Replace(int index, Card card)
        {
            return this.Remove(index) && this.Add(card);
        }

        public bool Replace(Card old, Card dst)
        {
            return this.Remove(old) && this.Add(dst);
        }

        public List<Card> Array { get => array; set => array = value; }
    }
}
