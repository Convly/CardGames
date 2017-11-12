namespace CardGameResources.Game
{
    /// <summary>
    /// Class used to defined a Card object.
    /// </summary>
    public class Card
    {
        private char value;
        private string color;
        
        /// <summary>
        /// Main constructor for a <see cref="Card"/>
        /// </summary>
        /// <param name="v">The value of the card</param>
        /// <param name="c">The color of the card</param>
        public Card(char v, string c)
        {
            this.Value = v;
            this.Color = c;
        }
        
        /// <summary>
        /// Getter and Setter for the value of the <see cref="Card"/>.
        /// </summary>
        public char Value { get => value; set => this.value = value; }

        /// <summary>
        /// Getter and Setter for the color of the <see cref="Card"/>
        /// </summary>
        public string Color { get => color; set => color = value; }
    }
}
