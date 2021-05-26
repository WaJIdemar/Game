using System;
using System.Collections.Generic;
using System.Text;

namespace Движение.Entites
{
    class Item
    {
        public string Name;
        public Tuple<bool, int> IsHeal;
        public Tuple<bool, int> IsAddValueToDiceCheck;
        public Tuple<bool, int> IsAddDamage;
        public string Description;

        public Item(string name, Tuple<bool, int> isHeal, Tuple<bool, int> isAddDamage,
            Tuple<bool, int> isAddValueToDiceCheck, string description = "")
        {
            Name = name;
            Description = description;
            IsAddDamage = isAddDamage;
            IsHeal = isHeal;
            IsAddValueToDiceCheck = isAddValueToDiceCheck;
        }
    }

    class Inventory
    {
        private List<Item> items = new List<Item>();
        public void AddItem(Item item)
        {
            if (items.Count < 5)
                items.Add(item);
        }

        public Item UseItem(string name)
        {
            var item = items.Find(x => x.Name == name);
            items.Remove(item);
            return item;
        }

        public IEnumerator<Item> GetEnumerator() => items.GetEnumerator();
    }
}