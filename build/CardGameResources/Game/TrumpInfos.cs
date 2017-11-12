using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Game
{
    /// <summary>
    /// TrumpInfos is a class used to store informations about a trump in a card game.
    /// </summary>
    public class TrumpInfos
    {
        private Card card;
        private string owner = null;
        private string realColor = null;

        /// <summary>
        /// Default constructor for TrumpInfos
        /// </summary>
        public TrumpInfos()
        {

        }

        /// <summary>
        /// Constructor which take a Card in parameter
        /// </summary>
        /// <param name="card_">The <see cref="Card"/> associated to the trump, the color of the card may differs from the real trump color</param>
        public TrumpInfos(Card card_)
        {
            this.Card = card_;
        }

        /// <summary>
        /// Complete constructor for TrumpInfos
        /// </summary>
        /// <param name="card_">The card stored as the trump</param>
        /// <param name="owner_">The name of the player which took the Trump</param>
        /// <param name="realColor_">The real color of the Trump. It can differs from the Card color</param>
        public TrumpInfos(Card card_, string owner_, string realColor_)
        {
            this.Card = card_;
            this.Owner = owner_;
            this.RealColor = realColor_;
        }

        /// <summary>
        /// Getter and Setter for the Card
        /// </summary>
        public Card Card { get => card; set => card = value; }
        /// <summary>
        /// Getter and Setter for the Owner of the trump Card
        /// </summary>
        public string Owner { get => owner; set => owner = value; }
        /// <summary>
        /// Getter and Setter for the trump real color
        /// </summary>
        public string RealColor { get => realColor; set => realColor = value; }
    }
}
