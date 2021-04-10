﻿using System;
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

//TODO: 3. Сделать генератор карты (ручной пока что) и проверку на выход за границы

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Image gladiatorSheetRight;
        public Image gladiatorSheetLeft;
        public Entity player;
        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer1.Tick += new EventHandler(Update);

            KeyUp += new KeyEventHandler(OnKeyUp);
            KeyDown += new KeyEventHandler(OnPress);
            Init();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            player.pressButtonMove = false;
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            if (!player.pressButtonMove && !player.isMoving)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        player.dirY = -3;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.S:
                        player.dirY = 3;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.A:
                        player.dirX = -3;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.D:
                        player.dirX = 3;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                }
                player.pressButtonMove = true;
            }
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

            player = new Entity(100, 100, Hero.idleFrames, Hero.runFrames, Hero.attackFrames,
                Hero.deathFrames, gladiatorSheetLeft, gladiatorSheetRight);
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
//public void StepOn(int dirX, int dirY, int repeats)
//{
//    player.dirX = dirX;
//    player.dirY = dirY;
//    timer1.Interval = 50;
//    player.isMoving = true;
//    player.SetAnimationConfiguration(1);
//    for (int i = 0; i < repeats; i++)
//    {
//        Invalidate();
//    }
//    timer1.Interval = 100;
//}