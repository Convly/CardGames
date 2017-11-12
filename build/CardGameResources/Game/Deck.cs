using System.Collections.Generic;
using System.Linq;

namespace CardGameResources.Game
{
    /// <summary>
    /// Class used to store and manipulate one or multiples <see cref="Card"/>
    /// </summary>
    public class Deck
    {
        private List<Card> array = new List<Card>();

        /// <summary>
        /// Default constructor for <see cref="Deck"/>
        /// </summary>
        public Deck()
        {
        }

        /// <summary>
        /// Second constructor for <see cref="Deck"/>. It allow to fill directly the <see cref="Deck"/> with some <see cref="Card"/>.
        /// </summary>
        /// <param name="array_">The List of <see cref="Card"/> which will be copied into the <see cref="Deck"/></param>
        public Deck(List<Card> array_)
        {
            this.Array = array_;
        }
        
        /// <summary>
        /// Add a <see cref="Card"/> into the <see cref="Deck"/>
        /// </summary>
        /// <param name="card_">The <see cref="Card"/> which will be append to thee <see cref="Deck"/></param>
        /// <returns>This method always return true</returns>
        public bool Add(Card card_)
        {
            this.Array.Add(card_);
            return true;
        }

        /// <summary>
        /// Remove all the <see cref="Card"/> objects in the <see cref="Deck"/>
        /// </summary>
        public void Clear()
        {
            this.Array.Clear();
        }

        /// <summary>
        /// Remove a <see cref="Card"/> by its index in the <see cref="Deck"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="Card"/></param>
        /// <returns>False if the index is greater than the number of cards in the <see cref="Deck"/>, true otherwise.</returns>
        public bool Remove(int index)
        {
            if (index >= this.Array.Count())
            {
                return false;
            }
            this.Array.RemoveAt(index);
            return true;
        }
        
        /// <summary>
        /// Check if a <see cref="Card"/> is in the <see cref="Deck"/>
        /// </summary>
        /// <param name="card">The <see cref="Card"/> used for the search</param>
        /// <returns>True if the <see cref="Card"/> is contained in the <see cref="Deck"/>, false otherwise.</returns>
        public bool Contains(Card card)
        {
            foreach (var c in this.Array)
            {
                if (c.Color == card.Color && c.Value == card.Value)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Remove a specific <see cref="Card"/> from the <see cref="Deck"/>
        /// </summary>
        /// <param name="card">The <see cref="Card"/> which will be removed</param>
        public bool Remove(Card card)
        {
            int x = 0;
            foreach (var c in this.Array)
            {
                if (c.Color == card.Color && c.Value == card.Value)
                {
                    this.Array.RemoveAt(x);
                    break;
                }
                ++x;
            }
            this.Array.Remove(card);
            return true;
        }

        /// <summary>
        /// Getter and Setter for the List of <see cref="Card"/> contained in the <see cref="Deck"/>
        /// </summary>
        public List<Card> Array { get => array; set => array = value; }
    }
}
