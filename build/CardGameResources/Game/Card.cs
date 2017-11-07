using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Game
{
    public class Card
    {
        private char value;
        private string color;
        public Card(char v, string c)
        {
            this.Value = v;
            this.Color = c;
        }

        public char Value { get => value; set => this.value = value; }
        public string Color { get => color; set => color = value; }
    }
}
