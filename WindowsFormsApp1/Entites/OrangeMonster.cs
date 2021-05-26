using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Движение.Entites
{
    public class OrangeMonster : ICharacter
    {
        public int posX { get; set; }
        public int AttackPower { get; set; }
        public int posY { get; set; }
        public int Health { get; set; }
        public Image SpriteForBattle { get; set; }
        public Image SpriteFace { get; set; }
        public Point LocationMap;

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;
       
        public string pathToSprites;

        public int Size { get; set; }

        public OrangeMonster(int posX, int posY, int idleFrames, Point locationMap, int size)
        {
            this.posX = posX;
            this.posY = posY;
            this.idleFrames = idleFrames;
            this.pathToSprites = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Orange\Stand\");
            Size = size;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            LocationMap = locationMap;
            SpriteFace = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Orange\Face.png"));
            SpriteForBattle = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Orange\BattleModel.png"));
            Health = 5;
            AttackPower = 1;
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation(Graphics g, int posX, int posY, int size)
        {
            if (currentFrame < currentLimit - 1)
                currentFrame++;
            else currentFrame = 0;

            var currentPathToSprite= new StringBuilder(pathToSprites);
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