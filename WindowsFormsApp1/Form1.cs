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
        public Size oldClientSize;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer1.Tick += new EventHandler(Update);

            KeyUp += new KeyEventHandler(MoveController.OnKeyUp);
            KeyDown += new KeyEventHandler(MoveController.OnPress);
            Init();
        }

        public void Init()
        {
            MapController.Init();
            this.Width = MapController.GetWidth();
            this.Height = MapController.GetHeight();

            gladiatorSheetRight = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Gladiator_Right.png"));
            gladiatorSheetLeft = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Gladiator_Left.png"));

            player = new Entity((this.Width - MapController.cellSize * 2) / 2 + 7, (this.Height - MapController.cellSize * 2) / 2 - 7,
                Hero.idleFrames, Hero.runFrames, Hero.attackFrames,
                Hero.deathFrames, gladiatorSheetLeft, gladiatorSheetRight);

            MoveController.AddPlayer(player);

            timer1.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.isMoving && PhysicsController.IsCollide(player))
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
           
            MapController.DrawMap(g);
            player.PlayAnimation(g);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace BardDungeon
//{
//    public partial class Form1 : Form
//    {
//        //protected override void OnPaint(PaintEventArgs e)
//        //{
//        //          var graphics = e.Graphics;
//        //              var image = Image.FromFile(@"C:\Users\Lenovo\source\repos\BardDungeon\BardDungeon\мозг.png");
//        //          graphics.ScaleTransform(2f, 2f);
//        //          graphics.DrawImage(image, new Point(200,0));
//        //}

//        public Form1()
//        {
//            var fSizeX = 600f;
//            var fSizeY = 600f;
//            ClientSize = new Size(600, 600);
//            var centerX = ClientSize.Width / 2;
//            var centerY = ClientSize.Height / 2;
//            var timer = new Timer();
//            var t = 0;
//            timer.Interval = 40;
//            timer.Tick += (sender, args) =>
//            {
//                var graphics = CreateGraphics();
//                fSizeY = ClientSize.Height;
//                fSizeX = ClientSize.Width;
//                graphics.ScaleTransform(fSizeX / 600, fSizeY / 600);
//                var fileName = new StringBuilder(@"Sprites\Snowman\Snowman");
//                fileName.Append((t % 8).ToString() + ".png");
//                var i = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
//                .Parent.Parent.FullName.ToString(), fileName.ToString()));
//                graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
//                graphics.DrawImage(i, new PointF(centerX, centerY));
//                t++;
//            };
//            timer.Start();
//        }
//    }
//}
