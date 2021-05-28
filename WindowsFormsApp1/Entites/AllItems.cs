using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Движение.Entites
{
    public static class AllItems
    {
        private static List<Item> allItems = new List<Item>();
        public static void Init()
        {
            StreamReader f = new StreamReader(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Iventory.txt"));
            var itemsDescription = new List<string>();
            while (!f.EndOfStream)
                itemsDescription.Add(f.ReadLine());
            f.Close();
            foreach(var item in itemsDescription)
            {
                var parametrsItem = item.Split('|');
                switch (parametrsItem[1])
                {
                    case "damage":
                        allItems.Add(new Item(parametrsItem[0].Replace('_', ' '), new Tuple<bool, int>(false, 0),
                            new Tuple<bool, int>(true, Convert.ToInt32(parametrsItem[2])), new Tuple<bool, int>(false, 0),
                            parametrsItem[3]));
                        break;
                    case "heal":
                        allItems.Add(new Item(parametrsItem[0].Replace('_', ' '), new Tuple<bool, int>(true, Convert.ToInt32(parametrsItem[2])),
                            new Tuple<bool, int>(false, 0), new Tuple<bool, int>(false, 0),
                            parametrsItem[3]));
                        break;
                    case "dice":
                        allItems.Add(new Item(parametrsItem[0].Replace('_', ' '), new Tuple<bool, int>(false, 0),
                             new Tuple<bool, int>(false, 0), new Tuple<bool, int>(true, Convert.ToInt32(parametrsItem[2])),
                            parametrsItem[3]));
                        break;
                }
            }
        }

        public static Item GetRandomItem()
        {
            var rnd = new Random();
            return allItems[rnd.Next(0, allItems.Count)];
        }
    }
}
