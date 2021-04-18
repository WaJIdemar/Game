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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Image gladiatorSheetRight;
        public Image gladiatorSheetLeft;
        public Entity player;
        public Point delta;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer2.Interval = 200;
            timer1.Tick += new EventHandler(Update);

            KeyUp += new KeyEventHandler(MoveController.OnKeyUp);
            KeyDown += new KeyEventHandler(MoveController.OnPress);
            Init();
        }

        public void Init()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            MapController.Init();
            this.Width = MapController.GetWidth();
            this.Height = MapController.GetHeight();

            gladiatorSheetRight = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Gladiator_Right.png"));
            gladiatorSheetLeft = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Gladiator_Left.png"));
            player = new Entity((this.Width / 4 + 2 * MapController.cellSize + 14), (this.Height / 4 - MapController.cellSize - 11),
                Hero.idleFrames, Hero.runFrames, Hero.attackFrames,
                Hero.deathFrames, gladiatorSheetLeft, gladiatorSheetRight);
            MoveController.AddPlayer(player);
            
            timer1.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.isMoving && PhysicsController.IsCollide(player, MapController.stop))
            {
                timer1.Interval = 33;
                player.Move();
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
            g.ScaleTransform(3, 3);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));
            MapController.DrawMap(g, player.delta, this.Width, this.Height);
            player.PlayAnimation(g);
        }
    }
}