using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Движение.Entites
{
    class Knight : ICharacter
    {
        public int posX { get; set; }
        public int AttackPower { get; set; }
        public int posY { get; set; }
        public Point LocationMap;
        public string pathToSprites;
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Size { get; set; }
        public Image SpriteForBattle { get; set; }
        public Image SpriteFace { get; set; }

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;

        public int countFrame = 0;

        public Knight(int posX, int posY, int idleFrames, Point locationMap, int size)
        {
            this.posX = posX;
            this.posY = posY;
            this.idleFrames = idleFrames / 10;
            pathToSprites = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Knight\Stand\");
            Size = size;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames / 10;
            LocationMap = locationMap;
            SpriteFace = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Knight\Face.png"));
            SpriteForBattle = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Knight\BattleModel.png"));
            Health = 15;
            MaxHealth = 10;
            AttackPower = 3;
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation(Graphics g, int posX, int posY, int size)
        {
            if (countFrame == 10)
            {
                if (currentFrame < currentLimit - 1)
                    currentFrame++;
                else currentFrame = 0;
                countFrame = 0;
            }
            else
                countFrame++;


            var currentPathToSprite = new StringBuilder(pathToSprites);
            currentPathToSprite.Append(currentFrame.ToString() + ".png");
            var spriteOrangeMonster =
                new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                currentPathToSprite.ToString()));
            g.DrawImage(spriteOrangeMonster, new Rectangle(new Point(posX, posY), new Size(size, size)));
        }

        public void ResetMove()
        {
            throw new NotImplementedException();
        }

        public void SetAnimationConfiguration(int currentAnimation)
        {
            this.currentAnimation = currentAnimation;
            switch (currentAnimation)
            {
                case 0:
                    currentLimit = idleFrames;
                    break;
            }
        }
    }
}
