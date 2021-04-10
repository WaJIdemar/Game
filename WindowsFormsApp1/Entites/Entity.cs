using System.Drawing;

namespace Движение.Entites
{
    public class Entity
    {
        public int posX;
        public int posY;

        public int dirX;
        public int dirY;
        private int lastDirX = 0;
        private int delta = 0;
        public bool isMoving;

        public bool pressButtonMove;

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;
        public int runFrames;
        public int attackFrames;
        public int deathFrames;

        public Image spriteSheetLeft;
        public Image spriteSheetRigth;

        public int size;

        public Entity(int posX, int posY, int idleFrames, int runFrames, int attackFrames, int deathFrames,
            Image spriteSheetLeft, Image spriteSheetRigth)
        {
            this.posX = posX;
            this.posY = posY;
            this.runFrames = runFrames;
            this.idleFrames = idleFrames;
            this.attackFrames = attackFrames;
            this.deathFrames = deathFrames;
            this.spriteSheetLeft = spriteSheetLeft;
            this.spriteSheetRigth = spriteSheetRigth;
            size = 30;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            pressButtonMove = false;
        }

        public void Move()
        {
            posY += dirY;
            posX += dirX;
            delta += 3;
            if (delta == 30)
                ResetMove();
        }

        public void ResetMove()
        {
            lastDirX = dirX;
            dirY = 0;
            dirX = 0;
            delta = 0;
            isMoving = false;
        }

        public void PlayAnimation(Graphics g)
        {
            if (currentFrame < currentLimit - 1)
                currentFrame++;
            else currentFrame = 0;
            if (dirX > 0 && currentAnimation == 1 || currentAnimation == 0 && lastDirX > 0)
                g.DrawImage(spriteSheetRigth, new Rectangle(new Point(posX, posY), new Size(size, size)),
                    32 * currentFrame, 32 * currentAnimation, size, size, GraphicsUnit.Pixel);
            else
                g.DrawImage(spriteSheetLeft, new Rectangle(new Point(posX, posY), new Size(size, size)),
                    32 * currentFrame, 32 * currentAnimation, size, size, GraphicsUnit.Pixel); 
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
