using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Движение.Entites
{
    public class OrangeMonster : ICharacter
    {
        public int posX;
        public int posY;
        public int Health { get; set; }
        public Point LocationMap;

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;
        public int runFrames;
        public int attackFrames;
        public int deathFrames;

        public string pathToSprites;

        public int size;

        public OrangeMonster(int posX, int posY, int idleFrames, int runFrames, int attackFrames, int deathFrames,
            string pathToSprites)
        {
            this.posX = posX;
            this.posY = posY;
            this.runFrames = runFrames;
            this.idleFrames = idleFrames;
            this.attackFrames = attackFrames;
            this.deathFrames = deathFrames;
            this.pathToSprites = pathToSprites;
            size = 30;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            LocationMap = new Point(9, 2);
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
                case 1:
                    currentLimit = runFrames;
                    break;
                case 2:
                    currentLimit = attackFrames;
                    break;
                case 4:
                    currentLimit = deathFrames;
                    break;
            }
        }
    }
}