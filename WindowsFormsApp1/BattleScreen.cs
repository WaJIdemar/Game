using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1;
using Движение.Controllers;
using Движение.Entites;
using static Движение.Controllers.BattleButtons;

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
        Timer battleTimer;
        ProgressBar heroHealth;
        ProgressBar monsterHealth;
        ICharacter hero;
        ICharacter monster;
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
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Size = new Size(1920, 1080);
            hero = character1;
            monster = character2;
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
            monsterHealth = new ProgressBar
            {
                Location = new Point(Width - map.health.Size.Width, map.health.Location.Y),
                Size = map.health.Size,
                Maximum = character2.Health,
                BackColor = map.health.BackColor,
                Value = character2.Health,
                ForeColor = map.health.ForeColor,
                Step = map.health.Step,
                Style = map.health.Style
            };
            var heroFace = new PictureBox
            {
                Image = map.heroFace.Image,
                Location = map.heroFace.Location,
                Size = map.heroFace.Size,
                SizeMode = map.heroFace.SizeMode,
            };
            var monsterFace = new PictureBox
            {
                Image = character2.SpriteFace,
                Location = new Point(Width - map.heroFace.Width, map.heroFace.Location.Y),
                Size = map.heroFace.Size,
                SizeMode = map.heroFace.SizeMode,
            };
            Controls.Add(monsterHealth);
            Controls.Add(heroHealth);
            Controls.Add(heroFace);
            Controls.Add(monsterFace);
            fontsProjects();
            BackgroundImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Ринг.jpg"));

            //Заголовок
            var titleLabel = new Label
            {
                Font = new Font(fonts.Families[2], 80),
                ForeColor = Color.Red,
                Size = new Size(400, 100),
                Location = new Point(ClientSize.Width / 2 - 120, 1),
                Text = "БОЙ",
                BackColor = Color.Transparent,
            };
            titleLabel.BringToFront();
            Controls.Add(titleLabel);

            //Кубы
            var die1 = new PictureBox
            {
                Size = new Size(135, 135),
                Location = new Point(ButtonWidth, Height - 5 * ButtonHeight),
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
            battleTimer = new Timer();
            battleTimer.Interval = 100;
            battleTimer.Tick += new EventHandler(Update);

            //Блок для анимации героя
            var picHero = new PictureBox
            {
                Size = die1.Size,
                Location = new Point(die1.Location.X,
                die1.Location.Y - ButtonHeight - die1.Size.Height),
                Image = hero.SpriteForBattle,
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
                Image = character2.SpriteForBattle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };
            Controls.Add(picMon);
            InitData(ButtonWidth, ButtonHeight, this, fonts, dies,
                map, hero, character2, battleLog, die1, die2);
            var buttonSword = CreateButtonSword();
            var buttonBow = CreateButtonBow(buttonSword);
            var buttonItem = CreateButtonItem(buttonBow);
            var buttonBless = CreateButtonBless(buttonItem);
            var buttonEscape = CreateButtonEscape(buttonBless);
            Controls.Add(buttonSword);
            Controls.Add(buttonBow);
            Controls.Add(buttonItem);
            Controls.Add(buttonBless);
            Controls.Add(buttonEscape);
            battleTimer.Start();
            Invalidate();
        }
        #endregion

        public void Update(object sender, EventArgs e)
        {
            heroHealth.Value = hero.Health;
            monsterHealth.Value = monster.Health;
            if (heroHealth.Value == 0)
            {
                EndBattle("Это была последняя капля...\n Вы УБИТЫ!", false);
                battleTimer.Stop();
            }
            Invalidate();
        }
    }
}
