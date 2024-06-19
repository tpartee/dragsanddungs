using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsAndDungeonsCharSheet
{
    public class Dice
    {
        public int Number { get; }
        public int Sides { get;}

        private Random rng;

        public Dice(int number, int sides) {
            Number = number;
            Sides = sides;
            rng = new Random();
        }

        public int Roll()
        {
            return rng.Next(Number, Number * Sides + 1);  // Generate a number from Number (of dice) to Number * Sides
        }
    }
}
