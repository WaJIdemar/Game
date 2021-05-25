using System;
using System.Collections.Generic;
using System.Text;

namespace Движение.Entites
{
    public class Dice
    {
        private int num_sides;
        private Random rnd;
        public Dice(int num_sides = 6)
        {
            this.num_sides = num_sides;
            rnd = new Random();
        }

        public int Roll()
        {
            return rnd.Next(num_sides - 1);
        }
    }
}
