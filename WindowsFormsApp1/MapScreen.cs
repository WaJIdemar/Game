using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Движение.Controllers;
using Движение.Entites;
using Движение.Models;
using Движение;

namespace WindowsFormsApp1
{
    public partial class MapScreen : Form
    {
        public Hero player;
        public ProgressBar health;
        public PictureBox heroFace;
        public bool battleCheck = false;
        public int sqSize = 50;

        public MapScreen()
        {
            InitializeComponent();
            health = new ProgressBar
            {
                Location = new Point(0, 50),
                Size = new Size(200, 30)
            };
            heroFace = new PictureBox
            {
                Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Face.png")),
                Location = new Point(0, 0),
                Size = new Size(50, 50),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(heroFace);
            Controls.Add(health);
            timer1.Interval = 60;
            timer1.Tick += new EventHandler(Update);

            KeyUp += new KeyEventHandler(MoveController.OnKeyUp);
            KeyDown += new KeyEventHandler(MoveController.OnPress);
            Init();
        }

        public void Init()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            AllItems.Init();

            player = new Hero(MapController.mapWidth / 2 * sqSize, MapController.mapHeight / 2 * sqSize,
                HeroModels.idleFrames, HeroModels.runFrames, sqSize);
            MapController.Init(player);
            Width = MapController.GetWidth();
            Height = MapController.GetHeight();
            MoveController.AddPlayer(player);
            health.Maximum = 25;
            health.BackColor = Color.Black;
            health.Value = player.Health;
            health.ForeColor = Color.Red;
            health.Step = 1;
            health.Style = ProgressBarStyle.Continuous;
            timer1.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.isInBattle)
            {
                MiniMapController.Init(player);
                var battleScreen = new BattleScreen(this, player, player.whoInBattle);
                Hide();
                battleScreen.Show();
                timer1.Stop();
            }
            if (player.isMoving && PhysicsController.IsCollide(player, MapController.stop))
            {
                player.Move();
                health.Value = player.Health;
            }
            else
            {
                player.SetAnimationConfiguration(0);
                health.Value = player.Health;
            }
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
            MapController.DrawMap(g, player);
            if (player.isAlive)
                player.PlayAnimation(g, player.posX, player.posY, player.Size);
        }
    }
}