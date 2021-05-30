using System;
using System.Collections.Generic;
using System.Drawing;
using Движение.Entites;
using Движение.Controllers;
using Движение.Models;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Text;
using WindowsFormsApp1;

namespace Движение.Controllers
{
    public static class BattleButtons
    {
        static bool isBlessesActive;
        static bool isInventoryActive;
        static PrivateFontCollection Fonts;

        static int ButtonWidth;
        static int ButtonHeight;

        static Form BattleScreen;
        static MapScreen mapS;
        static PictureBox MiniMap;

        static Hero hero;
        static ICharacter monster;

        static TextBox BattleLog;

        static Image[] Dices;
        static PictureBox Dice1;
        static PictureBox Dice2;

        static int escapeBless;
        static int hitBless;
        static int heroPowerBless;
        static int monsterPowerBless;
        static bool blessCheck;
        static bool itemCheck;
        static bool isDesOpen;
        static int totalMod3000;

        public static void InitData(int buttonWidth, int buttonHeight, Form battleScreen,
            PrivateFontCollection fonts, Image[] dices, MapScreen map,
            Hero character1, ICharacter character2,
            TextBox battleLog, PictureBox dice1, PictureBox dice2, PictureBox miniMap)
        {
            isBlessesActive = false;
            isInventoryActive = false;
            Dices = dices;
            Fonts = fonts;
            ButtonWidth = buttonWidth;
            ButtonHeight = buttonHeight;
            BattleScreen = battleScreen;
            mapS = map;
            hero = character1;
            monster = character2;
            BattleLog = battleLog;
            Dice1 = dice1;
            Dice2 = dice2;
            escapeBless = 0;
            hitBless = 0;
            totalMod3000 = 0;
            blessCheck = true;
            itemCheck = true;
            isDesOpen = false;
            MiniMap = miniMap;
        }
        
        public static void EndBattle(string message, bool isWin)
        {
            BattleScreen.Controls.Clear();
            var endLabel = new Label
            {
                Size = new Size(ButtonWidth, 2 * ButtonHeight),
                Location = new Point(BattleScreen.Width / 2 - ButtonWidth / 2, BattleScreen.Height / 2 - ButtonHeight),
                Text = message,
                ForeColor = Color.Black,
                Font = new Font(Fonts.Families[0], 15),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var endButton = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(BattleScreen.Width / 2 - ButtonWidth / 2, endLabel.Bottom),
                Text = isWin ? "Собрать добычу." : "Уйти на покой.",
                ForeColor = Color.Black,
                Font = new Font(Fonts.Families[0], 15),
            };
            endButton.Click += (sender, args) =>
            {
                if (isWin)
                {
                    MapController.map[mapS.player.LocationMap.Y, mapS.player.LocationMap.X] = '0';
                    mapS.Show();
                    mapS.player.isInBattle = false;
                    mapS.timer1.Start();
                    BattleScreen.Close();
                    hero.inventory.AddItem(AllItems.GetRandomItem());
                }
                else
                    mapS.Close();
            };
            BattleScreen.Controls.Add(endLabel);
            BattleScreen.Controls.Add(endButton);
        }

        public static int RollCheckSender(int mod = 0)
        {
            mod += totalMod3000;
            var dice = new Dice();
            var diceValue1 = dice.Roll();
            var diceValue2 = dice.Roll();
            if (mod < 0)
                if (diceValue1 >= Math.Abs(mod))
                    diceValue1 += mod;
                else if (diceValue2 >= Math.Abs(mod))
                    diceValue2 += mod;
                else if (diceValue1 + diceValue2 <= Math.Abs(mod))
                {
                    diceValue1 = 0;
                    diceValue2 = 0;
                }
                else
                {
                    mod += diceValue1;
                    diceValue1 = 0;
                    diceValue2 += mod;
                }
            else if (diceValue1 + mod < 6)
                diceValue1 += mod;
            else if (diceValue2 + mod < 6)
                diceValue2 += mod;
            else if (diceValue1 + diceValue2 + mod > 10)
            {
                diceValue1 = 5;
                diceValue2 = 5;
            }
            else
            {
                var k = 5 - diceValue1;
                mod -= k;
                diceValue1 += k;
                diceValue2 += mod;
            }
            Dice1.Image = Dices[diceValue1];
            Dice2.Image = Dices[diceValue2];
            return diceValue1 + diceValue2 + 2;
        }

        #region Blesses

        private static void MonsterPowerBlessCreate(Button bless)
        {
            bless.Click += (sender, args) =>
            {
                if (blessCheck)
                {
                    var result = RollCheckSender();
                    if (result > 6)
                    {
                        if (result < 10)
                        {
                            monsterPowerBless--;
                            BattleLog.AppendLine("Ваша музыка бьет по мостру. В следующий раз его атака будет чуть слабее!");
                        }
                        else
                        {
                            monsterPowerBless -= 2;
                            BattleLog.AppendLine("Музыка стала вашим спасением! Следующая атака монстра будет значительно слабее!");
                        }
                    }
                    else
                    {
                        monsterPowerBless++;
                        BattleLog.AppendLine("Ваша музыка вдохновила монстра вместо того, что бы ослабить. Следующая атака по вам будет сильнее...");
                    }
                    blessCheck = false;
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };
            BattleScreen.Controls.Add(bless);
        }

        private static void HitBlessCreate(Button bless)
        {
            bless.Click += (sender, args) =>
            {
                if (blessCheck)
                {
                    blessCheck = false;
                    var result = RollCheckSender();
                    if (result > 6)
                    {
                        if (result < 10)
                        {
                            hitBless++;
                            BattleLog.AppendLine("Ваша музыка вдохновляет вас на бой и вы уверены, что в этот раз ударите еще точнее!");
                        }
                        else
                        {
                            hitBless += 2;
                            BattleLog.AppendLine("Ваша музыка вдохновляет вас на бой и вы уверены, что в этот раз ударите намного точнее!");
                        }
                    }
                    else
                    {
                        BattleLog.AppendLine("Решив спеть, вы закашлялись. Неудача...");
                    }
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };
            BattleScreen.Controls.Add(bless);
        }

        private static void HeroPowerBlessCreate(Button bless)
        {
            bless.Click += (sender, args) =>
            {
                if (blessCheck)
                {
                    var result = RollCheckSender();
                    if (result > 6)
                    {
                        if (result < 10)
                        {
                            heroPowerBless++;
                            BattleLog.AppendLine("Ваше пение пробуждает в вас скрытую силу! Следующее попадание будет смертоноснее!");
                        }
                        else
                        {
                            heroPowerBless += 2;
                            BattleLog.AppendLine("Ваше пение пробуждает в вас мощь великанов! Следующее попадание будет еще смертоноснее!");
                        }
                    }
                    else
                    {
                        BattleLog.AppendLine("Хоть ваше тело и налилось энергией, но не может с ней справиться! Она незамедлительно покинула вас...");
                    }
                    blessCheck = false;
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };
            BattleScreen.Controls.Add(bless);
        }

        private static void GetHealthBlessCreate(Button bless)
        {
            bless.Click += (sender, args) =>
            {
                if (blessCheck)
                {
                    blessCheck = false;
                    var result = RollCheckSender();
                    if (result > 6)
                    {
                        if (result < 10)
                        {
                            hero.Health++;
                            BattleLog.AppendLine("Вы почувствоваль эманации жизни в воздухе и сковали их своей песней! 1 ед. здоровья восстановлена.");
                        }
                        else
                        {
                            hero.Health += 2;
                            BattleLog.AppendLine("Сильные эманации жизни пришли в ваш инструмент! 2 ед. здоровья восстановлены!");
                        }
                    }
                    else
                    {
                        BattleLog.AppendLine("Почувствовав толику жизни в воздухе вы попытались поймать её, но она улетучилась...");
                    }
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };
            BattleScreen.Controls.Add(bless);
        }

        private static void EscapeBlessCreate(Button bless)
        {
            bless.Click += (sender, args) =>
            {
                if (blessCheck)
                {
                    var result = RollCheckSender();
                    if (result > 6)
                    {
                        if (result < 10)
                        {
                            escapeBless++;
                            BattleLog.AppendLine("Ваши стихи придали лёгкость ногам. Сбежать будет легче!");
                        }
                        else
                        {
                            escapeBless += 2;
                            BattleLog.AppendLine("Ваши ноги вышли на новый уровень! Сбежать гораздо легче.");
                        }
                    }
                    else
                    {
                        BattleLog.AppendLine("Споткнувшись в конце предложения, вы не закончили стих. Неудача...");
                    }
                    blessCheck = false;
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };
            BattleScreen.Controls.Add(bless);
        }
        #endregion

        private static void ItemCreate(Button item, int indexOfItem)
        {
            item.Click += (sender, args) =>
            {
                if (itemCheck && item.Text != "Пусто")
                {
                    if (hero.inventory[indexOfItem].IsAddDamage.Item1)
                        heroPowerBless += hero.inventory[indexOfItem].IsAddDamage.Item2;
                    if (hero.inventory[indexOfItem].IsAddValueToDiceCheck.Item1)
                        totalMod3000 += hero.inventory[indexOfItem].IsAddValueToDiceCheck.Item2;
                    if (hero.inventory[indexOfItem].IsHeal.Item1)
                        hero.Health += hero.inventory[indexOfItem].IsHeal.Item2;
                    itemCheck = false;
                    BattleLog.AppendLine("Вы успешно использовали " + hero.inventory[indexOfItem].Name);
                    hero.inventory.UseItem(item.Text);
                    item.Text = "Пусто";
                }
                else if (item.Text == "Пусто")
                {
                    BattleLog.AppendLine("В этой ячейке инвентаря нет предмета!");
                }
                else
                {
                    BattleLog.AppendLine("Между другими действиями можно использовать благословение и предмет из инвентаря только один раз!");
                }
            };

            var description = new Button
            {
                Location = new Point(item.Location.X + item.Size.Width, item.Location.Y),
                Size = new Size(MiniMap.Location.X - item.Location.X - item.Size.Width, ButtonHeight),
                Text = "(?)",
                Font = new Font(Fonts.Families[0], 20)
            };
            description.Click += (sender, args) =>
            {
                if (!isDesOpen && item.Text != "Пусто")
                {
                    isDesOpen = true;
                    var endLabel = new Label
                    {
                        Size = new Size(ButtonWidth, 2 * ButtonHeight),
                        Location = new Point(BattleScreen.Width / 2 - ButtonWidth / 2, BattleScreen.Height / 2 - ButtonHeight),
                        Text = hero.inventory[indexOfItem].Description,
                        ForeColor = Color.Black,
                        Font = new Font(Fonts.Families[0], 15),
                        BorderStyle = BorderStyle.FixedSingle,
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    var endButton = new Button
                    {
                        Size = new Size(ButtonWidth, ButtonHeight),
                        Location = new Point(BattleScreen.Width / 2 - ButtonWidth / 2, endLabel.Bottom),
                        Text = "Закрыть",
                        ForeColor = Color.Black,
                        Font = new Font(Fonts.Families[0], 15),
                    };
                    endButton.Click += (sender, args) =>
                    {
                        BattleScreen.Controls.RemoveAt(BattleScreen.Controls.Count - 1);
                        BattleScreen.Controls.RemoveAt(BattleScreen.Controls.Count - 1);
                        isDesOpen = false;
                    };
                    BattleScreen.Controls.Add(endLabel);
                    BattleScreen.Controls.Add(endButton);
                }
            };
            BattleScreen.Controls.Add(description);
            BattleScreen.Controls.Add(item);
        }

        public static Button CreateButtonSword()
        {
            var buttonSword = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(0, BattleScreen.Height - ButtonHeight),
                Text = "АТАКА МЕЧЕМ",
                Font = new Font(Fonts.Families[1], 20),
            };
            buttonSword.Click += (sender, args) =>
            {
                var result = RollCheckSender(hitBless);
                var damage = monster.AttackPower + monsterPowerBless;
                hitBless = 0;
                blessCheck = true;
                itemCheck = true;
                if (result > 6)
                {
                    var attackDamage = hero.AttackPower + heroPowerBless;
                    if (result < 10)
                    {
                        hero.Health -= damage;
                        monsterPowerBless = 0;
                        BattleLog.AppendLine("Ваше попадание успешно, но из-за неаккуратности вы тоже получаете урон! Вы получили " +
                             damage.ToString() + ", а монстр " + attackDamage.ToString() + " ед. урона");
                        heroPowerBless = 0;
                        monster.Health -= attackDamage;
                        if (monster.Health <= 0)
                            EndBattle("Вы зарубили его, но на последок получили " + damage.ToString() + " ед. урона!\nОсталось только собрать лут...", true);
                    }
                    else
                    {
                        monster.Health -= attackDamage * 2;
                        BattleLog.AppendLine("Точным выпадом вы атаковали монстра в слабое место! Монстр получил " +
                            (2 * attackDamage).ToString() + " ед. урона");
                        heroPowerBless = 0;
                        if (monster.Health <= 0)
                            EndBattle("Вы зарубили его!\nОсталось только собрать лут...", true);
                    }
                }
                else
                {
                    hero.Health -= damage;
                    monsterPowerBless = 0;
                    BattleLog.AppendLine("Монстр отбил атаку и контратаковал. Вы получили " + damage.ToString() + " ед. урон!");
                }
            };
            return buttonSword;
        }

        public static Button CreateButtonBow(Button buttonSword)
        {
            var buttonBow = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonSword.Location.X, buttonSword.Location.Y - ButtonHeight),
                Text = "АТАКА ЛУКОМ",
                Font = new Font(Fonts.Families[1], 20),
            };
            buttonBow.Click += (sender, args) =>
            {
                blessCheck = true;
                itemCheck = true;
                var result = RollCheckSender(hitBless);
                hitBless = 0;
                var attackDamage = hero.AttackPower + heroPowerBless;
                if (result > 9)
                {
                    monster.Health -= attackDamage;
                    heroPowerBless = 0;
                    BattleLog.AppendLine("Выстрел нашел свою цель! Монстр получил " + attackDamage.ToString() + " ед. урона");
                }
                else if (result > 6)
                {
                    monster.Health -= attackDamage / 2;
                    heroPowerBless = 0;
                    BattleLog.AppendLine("Вы успели уклониться после атаки монстра, но ваш удар прошел по касательной! Монстр получил " + (attackDamage / 2).ToString() + " ед. урона.");
                }
                else
                {

                    var damage = monster.AttackPower + monsterPowerBless;
                    hero.Health -= damage;
                    monsterPowerBless = 0;
                    BattleLog.AppendLine("Монстр уклонился и резко сократил дистанцию. Вы получили " + damage.ToString() + " ед. урон!");
                }
                if (monster.Health <= 0)
                    EndBattle("Вы застрелили его!\nОсталось только собрать лут...", true);
            };
            return buttonBow;
        }

        public static Button CreateButtonItem(Button buttonBow)
        {
            var buttonItem = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonBow.Location.X, buttonBow.Location.Y - ButtonHeight),
                Text = "ИНВЕНТАРЬ",
                Font = new Font(Fonts.Families[1], 20),
            };

            buttonItem.Click += (sender, args) =>
            {
                if (!isInventoryActive)
                {
                    if (isBlessesActive)
                        for (int i = 0; i < 5; i++)
                            BattleScreen.Controls.RemoveAt(BattleScreen.Controls.Count - 1);
                    isInventoryActive = true;
                    isBlessesActive = false;
                    var itemList = new Button[5];
                    for (int i = 0; i < 5; i++)
                    {
                        itemList[i] = new Button
                        {
                            Size = new Size(ButtonWidth, ButtonHeight),
                            Location = i > 0 ? new Point(itemList[i - 1].Location.X, itemList[i - 1].Location.Y - ButtonHeight)
                            : new Point(BattleLog.Location.X + BattleLog.Size.Width, BattleScreen.Height - ButtonHeight),
                            Text = hero.inventory.Count > 4 - i ? hero.inventory[4 - i].Name
                            : "Пусто",
                            Font = new Font(Fonts.Families[1], 20),
                        };
                        ItemCreate(itemList[i], 4 - i);
                    }
                }
            };
            return buttonItem;
        }

        public static Button CreateButtonBless(Button buttonItem)
        {
            var buttonBless = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonItem.Location.X, buttonItem.Location.Y - ButtonHeight),
                Text = "БЛАГОСЛОВЕНИЕ",
                Font = new Font(Fonts.Families[1], 20),
            };
            var nameOfBlesses = new string[5] {
                "Слово слабости", "Слово сокола", "Слово великана", "Слово лечения", "Слово лани" };
            buttonBless.Click += (sender, args) =>
            {
                if (!isBlessesActive)
                {
                    if (isInventoryActive)
                        for (int i = 0; i < 10; i++)
                            BattleScreen.Controls.RemoveAt(BattleScreen.Controls.Count - 1);
                    isBlessesActive = true;
                    isInventoryActive = false;
                    var blessList = new Button[5];
                    for (int i = 0; i < 5; i++)
                    {
                        blessList[i] = new Button
                        {
                            Size = new Size(ButtonWidth, ButtonHeight),
                            Location = i > 0 ? new Point(blessList[i - 1].Location.X, blessList[i - 1].Location.Y - ButtonHeight)
                            : new Point(BattleLog.Location.X + BattleLog.Size.Width, BattleScreen.Height - ButtonHeight),
                            Text = nameOfBlesses[i],
                            Font = new Font(Fonts.Families[1], 20),
                        };
                    }
                    MonsterPowerBlessCreate(blessList[0]);
                    HitBlessCreate(blessList[1]);
                    HeroPowerBlessCreate(blessList[2]);
                    GetHealthBlessCreate(blessList[3]);
                    EscapeBlessCreate(blessList[4]);
                }
            };
            return buttonBless;
        }

        public static Button CreateButtonEscape(Button buttonBless)
        {
            var buttonEscape = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonBless.Location.X, buttonBless.Location.Y - ButtonHeight),
                Text = "ОТСТУПЛЕНИЕ",
                ForeColor = Color.Red,
                Font = new Font(Fonts.Families[1], 20),
            };
            buttonEscape.Click += (sender, args) =>
            {
                blessCheck = true;
                itemCheck = true;
                var result = RollCheckSender(escapeBless);
                escapeBless = 0;
                if (result > 6)
                {
                    if (result < 10)
                    {
                        hero.Health -= monster.AttackPower + monsterPowerBless;
                        monsterPowerBless = 0;
                    }
                    mapS.Show();
                    MapController.BackStep();
                    mapS.player.isInBattle = false;
                    mapS.timer1.Start();
                    BattleScreen.Close();
                }
                else
                {
                    hero.Health -= monster.AttackPower + monsterPowerBless;
                    var damage = monster.AttackPower + monsterPowerBless;
                    monsterPowerBless = 0;
                    BattleLog.AppendLine("При попытке побега вы выкинули " + result.ToString()
                        + " и получили " + damage.ToString() + " урон! Монстр обрезал тот путь.");
                }
            };
            return buttonEscape;
        }
    }
}
