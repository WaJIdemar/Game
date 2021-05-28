using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Движение.Entites
{
    class Boss : ICharacter
    {
        public int Health { get; set; }
        public int Size { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public Point LocationMap;
        public Image SpriteForBattle { get; set; }
        public Image SpriteFace { get; set; }
        public int AttackPower { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Image spriteBoss;

        public Boss(int posX, int posY, int idleFrames, Point locationMap, int size)
        {
            this.posX = posX;
            this.posY = posY;
            Size = size;
            LocationMap = locationMap;
            spriteBoss = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Boss\B.png"));
            SpriteForBattle = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Boss\BR.png"));
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation(Graphics g, int posX, int posY, int size)
        {
            g.DrawImage(spriteBoss, new Rectangle(new Point(posX, posY), new Size(size, size)));
        }

        public void ResetMove()
        {
            throw new NotImplementedException();
        }

        public void SetAnimationConfiguration(int currentAnimation)
        {
            throw new NotImplementedException();
        }
    }
}
