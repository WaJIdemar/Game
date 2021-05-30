using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Движение.Entites
{
    public class Item
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

    public class Inventory
    {
        private List<Item> items = new List<Item>();
        public int Count
        {
            get
            {
                return items.Count;
            }
            set { }
        }
        public bool AddItem(Item item)
        {
            if (items.Count < 5)
            {
                items.Add(item);
                return true;
            }
            return false;
        }

        public Item UseItem(string name)
        {
            var item = items.Find(x => x.Name == name);
            items.Remove(item);
            return item;
        }

        public void ReplaceItem(Item oldItem, Item newItem)
        {
            var index = items.IndexOf(oldItem);
            items[index] = newItem;
        }
        public IEnumerator<Item> GetEnumerator() => items.GetEnumerator();

        public Item this[int index]
        {
            get
            {
                return items[index];
            }
            set { }
        }
    }
}