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
        static Image[] Dies;
        static PrivateFontCollection Fonts;
        static int ButtonWidth;
        static int ButtonHeight;
        static Form BattleScreen;
        static MapScreen mapS;
        static ICharacter hero;
        static ICharacter monster;
        static TextBox BattleLog;
        static PictureBox Die1;
        static PictureBox Die2;

        public static void InitData(int buttonWidth, int buttonHeight, Form battleScreen,
            PrivateFontCollection fonts, Image[] dies, MapScreen map,
            ICharacter character1, ICharacter character2,
            TextBox battleLog, PictureBox die1, PictureBox die2)
        {
            Dies = dies;
            Fonts = fonts;
            ButtonWidth = buttonWidth;
            ButtonHeight = buttonHeight;
            BattleScreen = battleScreen;
            mapS = map;
            hero = character1;
            monster = character2;
            BattleLog = battleLog;
            Die1 = die1;
            Die2 = die2;
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
                }
                else
                    mapS.Close();
            };
            BattleScreen.Controls.Add(endLabel);
            BattleScreen.Controls.Add(endButton);
        }

        public static int RollCheckSender(PictureBox Die1, PictureBox Die2)
        {
            var die = new Die();
            var dieValue1 = die.Roll();
            var dieValue2 = die.Roll();
            Die1.Image = Dies[dieValue1];
            Die2.Image = Dies[dieValue2];
            return dieValue1 + dieValue2 + 2;
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
                var result = RollCheckSender(Die1, Die2);
                if (result > 6)
                {
                    monster.Health--;
                    if (result < 10)
                    {
                        hero.Health--;
                        BattleLog.AppendLine("Ваше попадание успешно, но из-за неаккуратности вы тоже получаете урон! Вы и монстр получили 1 ед. урона");
                        if (monster.Health == 0)
                            EndBattle("Вы зарубили его, но на последок получили 1 урона!\nОсталось только собрать лут...", true);
                    }
                    else
                    {
                        monster.Health--;
                        BattleLog.AppendLine("Точным выпадом вы атаковали монстра в слабое место! Монстр получил 2 ед. урона");
                        if (monster.Health == 0)
                            EndBattle("Вы зарубили его!\nОсталось только собрать лут...", true);
                    }
                }
                else
                {
                    hero.Health--;
                    BattleLog.AppendLine("Монстр отбил атаку и контратаковал. Вы получили 1 ед. урон!");
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
                var result = RollCheckSender(Die1, Die2);
                if (result > 9)
                {
                    monster.Health--;
                    BattleLog.AppendLine("Выстрел нашел свою цель! Монстр получил 1 ед. урона");
                    if (monster.Health == 0)
                        EndBattle("Вы застрелили его!\nОсталось только собрать лут...", true);
                }
                else if (result > 6)
                {
                    BattleLog.AppendLine("Вы успели уклониться после атаки монстра, но и его удар не достиг цели! Никто не получил урона");
                }
                else
                {
                    hero.Health--;
                    BattleLog.AppendLine("Монстр уклонился и резко сократил дистанцию. Вы получили 1 ед. урон!");
                }
            };
            return buttonBow;
        }

        public static Button CreateButtonItem(Button buttonBow)
        {
            var buttonItem = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonBow.Location.X, buttonBow.Location.Y - ButtonHeight),
                Text = "РАСХОДУЕМЫЙ ПРЕДМЕТ",
                Font = new Font(Fonts.Families[1], 20),
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
            buttonBless.Click += (sender, args) =>
            {
                var result = RollCheckSender(Die1, Die2);
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
                var result = RollCheckSender(Die1, Die2);
                if (result > 6)
                {
                    if (result < 10)
                        hero.Health--;
                    mapS.Show();
                    MapController.BackStep();
                    mapS.player.isInBattle = false;
                    mapS.timer1.Start();
                    BattleScreen.Close();
                }
                else
                {
                    hero.Health--;
                    BattleLog.AppendLine("При попытке побега вы выкинули " + result.ToString()
                        + " и получили 1 урон! Монстр обрезал тот путь.");
                }
            };
            return buttonEscape;
        }
    }
}
