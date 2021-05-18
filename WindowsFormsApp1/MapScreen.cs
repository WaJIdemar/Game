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
        public string gladiatorSheetRight;
        public string gladiatorSheetLeft;
        public Hero player;
        public Point delta;
        public ProgressBar health;
        public bool battleCheck = false;

        public MapScreen()
        {
            InitializeComponent();
            health = new ProgressBar
            {
                Location = new Point(0, 0),
                Size = new Size(90, 30)
            };
            Controls.Add(health);
            timer1.Interval = 1;
            timer2.Interval = 200;
            timer1.Tick += new EventHandler(Update);

            KeyUp += new KeyEventHandler(MoveController.OnKeyUp);
            KeyDown += new KeyEventHandler(MoveController.OnPress);
            Init();
        }

        public void Init()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            MapController.Init();
            Width = MapController.GetWidth();
            Height = MapController.GetHeight();

            gladiatorSheetRight = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Right\");
            gladiatorSheetLeft = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Left\");
            player = new Hero(MapController.cellSize * MapController.mapWidth / 2 - 14, MapController.cellSize * MapController.mapHeight / 2 - 15,
                HeroModels.idleFrames, HeroModels.runFrames, HeroModels.attackFrames,
                HeroModels.deathFrames, gladiatorSheetLeft, gladiatorSheetRight);
            MoveController.AddPlayer(player);
            health.Maximum = 4;
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
                var battleScreen = new BattleScreen(this);
                Hide();
                battleScreen.Show();
                timer1.Stop();
            }
            if (player.isMoving && PhysicsController.IsCollide(player, MapController.stop))
            {
                timer1.Interval = 33;
                player.Move();
                health.Value = player.Health;
            }
            else
            {
                timer1.Interval = 500;
                player.SetAnimationConfiguration(0);
            }
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
            MapController.DrawMap(g, player);
            if (player.isAlive)
                player.PlayAnimation(g, player.posX, player.posY, player.size);
        }
    }
}