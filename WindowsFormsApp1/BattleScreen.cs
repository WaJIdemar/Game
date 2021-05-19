using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;
using Движение.Controllers;
using Движение.Entites;

namespace Движение
{
    public static class WinFormsExtensions
    {
        public static void AppendLine(this TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
    }


    public partial class BattleScreen : Form
    {
        const int ButtonWidth = 384;
        const int ButtonHeight = 54;
        bool closeFlag = false;
        ProgressBar heroHealth;
        ICharacter hero;
        MapScreen mapS;
        PrivateFontCollection fonts;
        readonly Image[] dies = new Image[6] { new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die2.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die3.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die4.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die5.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die6.png")) };

        public static int RollCheckSender(TextBox battleLog, PictureBox die1, PictureBox die2, Image[] dies)
        {
            var die = new Die();
            var dieValue1 = die.Roll();
            var dieValue2 = die.Roll();
            die1.Image = dies[dieValue1];
            die2.Image = dies[dieValue2];
            return dieValue1 + dieValue2 + 2;
        }

        public void EndBattle(string message, bool isWin)
        {
            Controls.Clear();
            var endLabel = new Label
            {
                Size = new Size(ButtonWidth, 2 * ButtonHeight),
                Location = new Point(Width / 2 - ButtonWidth / 2, Height / 2 - ButtonHeight),
                Text = message,
                ForeColor = Color.Black,
                Font = new Font(fonts.Families[0], 15),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var endButton = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(Width/2-ButtonWidth/2, endLabel.Bottom),
                Text = isWin ? "Собрать добычу." : "Уйти на покой.",
                ForeColor = Color.Black,
                Font = new Font(fonts.Families[0], 15),
            };
            endButton.Click += (sender, args) =>
            {
                if (isWin)
                {
                    MapController.map[mapS.player.LocationMap.Y, mapS.player.LocationMap.X] = '0';
                    mapS.Show();
                    mapS.player.isInBattle = false;
                    mapS.timer1.Start();
                    Close();
                }
                else
                    mapS.Close();
            };
            Controls.Add(endLabel);
            Controls.Add(endButton);
        }

        private void fontsProjects()
        {
            fonts = new PrivateFontCollection();
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\galaxyGirl.ttf"));
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\Consolas.ttf"));
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\PostModernOne.ttf"));
        }
        #region Инициализация экрана
        public BattleScreen(MapScreen map, ICharacter character1, ICharacter character2)
        {
            mapS = map;
            hero = character1;
            heroHealth = new ProgressBar
            {
                Location = map.health.Location,
                Size = map.health.Size,
                Maximum = map.health.Maximum,
                BackColor = map.health.BackColor,
                Value = map.health.Value,
                ForeColor = map.health.ForeColor,
                Step = map.health.Step,
                Style = map.health.Style
            };
            Controls.Add(heroHealth);
            fontsProjects();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Size = new Size(1920, 1080);
            BackgroundImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Ринг.jpg"));
            var label = new Label();
            label.Font = new Font(fonts.Families[2], 80);
            label.ForeColor = Color.Red;
            label.Size = new Size(400, 100);
            label.Location = new Point(ClientSize.Width / 2 - 120, 1);
            label.Text = "БОЙ";
            label.BringToFront();
            label.BackColor = Color.Transparent;
            Controls.Add(label);

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (sender, args) =>
            {
                if (closeFlag) Close();
            };
            timer.Start();
            // Атака мечем
            var buttonSword = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(0, Height - ButtonHeight),
                Text = "АТАКА МЕЧЕМ",
                Font = new Font(fonts.Families[1], 20),
            };
            Controls.Add(buttonSword);
            //Атака луком
            var buttonBow = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonSword.Location.X, buttonSword.Location.Y - ButtonHeight),
                Text = "АТАКА ЛУКОМ",
                Font = new Font(fonts.Families[1], 20),
            };
            Controls.Add(buttonBow);
            //Расходуемый предмет
            var buttonItem = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonBow.Location.X, buttonBow.Location.Y - ButtonHeight),
                Text = "РАСХОДУЕМЫЙ ПРЕДМЕТ",
                Font = new Font(fonts.Families[1], 20),
            };
            Controls.Add(buttonItem);
            //Благословение
            var buttonBless = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonItem.Location.X, buttonItem.Location.Y - ButtonHeight),
                Text = "БЛАГОСЛОВЕНИЕ",
                Font = new Font(fonts.Families[1], 20),
            };
            Controls.Add(buttonBless);
            //Отступление
            var buttonGo = new Button
            {
                Size = new Size(ButtonWidth, ButtonHeight),
                Location = new Point(buttonBless.Location.X, buttonBless.Location.Y - ButtonHeight),
                Text = "ОТСТУПЛЕНИЕ",
                ForeColor = Color.Red,
                Font = new Font(fonts.Families[1], 20),
            };
            Controls.Add(buttonGo);
            //Кубы
            var die1 = new PictureBox
            {
                Size = new Size(135, 135),
                Location = new Point(buttonGo.Location.X + buttonGo.Size.Width, buttonGo.Location.Y),
                Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die6.png")),
            };
            Controls.Add(die1);
            var die2 = new PictureBox
            {
                Size = new Size(135, 135),
                Location = new Point(die1.Location.X, die1.Location.Y + die1.Size.Height),
                Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\die\die6.png")),
            };
            Controls.Add(die2);
            //Миникарта
            var miniMap = new PictureBox
            {
                Size = new Size(ButtonHeight * 5, ButtonHeight * 5),
                Location = new Point(Width - ButtonHeight * 5, Height - ButtonHeight * 5),
                Image = MiniMapController.GetMiniMap(),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(miniMap);
            //Лог боя
            var battleLog = new TextBox
            {
                Size = new Size(3 * ButtonWidth / 2, ButtonHeight * 4),
                Location = new Point(die1.Location.X + die1.Width, die1.Location.Y + ButtonHeight),
                Font = new Font(fonts.Families[0], 15),
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Multiline = true,
            };
            Controls.Add(battleLog);
            //Надпись о логе боя
            var logLabel = new Label
            {
                Size = new Size(3 * ButtonWidth / 2, ButtonHeight),
                Location = new Point(die1.Location.X + die1.Width, die1.Location.Y),
                Font = new Font(fonts.Families[2], 30), //Здесь курят
                Text = "Лог боя"
            };
            Controls.Add(logLabel);
            //Таймер обновления анимаций
            var battleTimer = new Timer();
            battleTimer.Interval = 100;
            //Блок для анимации героя
            var picHero = new PictureBox
            {
                Size = die1.Size,
                Location = new Point(die1.Location.X,
                die1.Location.Y - ButtonHeight - die1.Size.Height),
                Image = hero.nowSprite,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };
            Controls.Add(picHero);
            //Блок для анимации монстра
            var picMon = new PictureBox
            {
                Size = die1.Size,
                Location = new Point(die1.Location.X + +die1.Size.Width + 2 * ButtonWidth,
                die1.Location.Y - ButtonHeight - die1.Size.Height),
                Image = character2.nowSprite,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };
            Controls.Add(picMon);
            buttonSword.Click += (sender, args) =>
            {
                var result = RollCheckSender(battleLog, die1, die2, dies);
                if (result > 6)
                {
                    if (result < 10)
                    {
                        character1.Health--;
                        EndBattle("Вы зарубили его, но на последок получили 1 урона!\nОсталось только собрать лут...", true);
                    }
                    EndBattle("Вы зарубили его!\nОсталось только собрать лут...", true);
                }
                else
                {
                    character1.Health--;
                    battleLog.AppendLine("При попытке нанести удар мечем вы выкинули " + result.ToString()
                        + " и получили 1 урон! Монстр отбил атаку и контратаковал.");
                }
            };

            buttonBow.Click += (sender, args) =>
            {
                var result = RollCheckSender(battleLog, die1, die2, dies);
                if (result > 6)
                {
                    EndBattle("Вы застрелили его!\nОсталось только собрать лут...", true);
                }
                else
                {
                    character1.Health--;
                    battleLog.AppendLine("При попытке выстрела из лука вы выкинули " + result.ToString()
                        + " и получили 1 урон! Монстр был более ловок.");
                }
            };

            buttonGo.Click += (sender, args) =>
            {
                var result = RollCheckSender(battleLog, die1, die2, dies);
                if (result > 6)
                {
                    if (result < 10)
                        character1.Health--;
                    map.Show();
                    MapController.BackStep();
                    map.player.isInBattle = false;
                    map.timer1.Start();
                    Close();
                }
                else
                {
                    character1.Health--;
                    battleLog.AppendLine("При попытке побега вы выкинули " + result.ToString()
                        + " и получили 1 урон! Монстр обрезал тот путь.");
                }
            };

            buttonBless.Click += (sender, args) =>
            {
                var result = RollCheckSender(battleLog, die1, die2, dies);
            };
            battleTimer.Tick += new EventHandler(Update);
            battleTimer.Start();
            Invalidate();
        }
        #endregion

        public void Update(object sender, EventArgs e)
        {
            heroHealth.Value = hero.Health;
            if (heroHealth.Value == 0)
            {
                EndBattle("Это была последняя капля...\n Вы УБИТЫ!", false);
            }
            Invalidate();
        }
    }
}
