using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Game
{
    public class Deck
    {
        private int size;
        private List<Card> array;

        public Deck(int size_)
        {
            this.Size = size_;
        }

        public Deck(int size_, List<Card> array_)
        {
            this.Size = size_;
            this.Array = array_;
        }

        public bool Add(Card card_)
        {
            if (this.Array.Count() >= this.Size)
            {
                return false;
            }
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

        public int Size { get => size; set => size = value; }
        public List<Card> Array { get => array; set => array = value; }
    }
}
