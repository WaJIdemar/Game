using System;
using System.Collections.Generic;
using System.Text;

namespace Движение.Entites
{
    class Inventory
    {
        public Tuple<bool, int> isHeal;
        public Tuple<bool, int> isAddValueToDiceCheck;
        public Tuple<bool, int> isAddDamage;
        public string Description;
    }
}
