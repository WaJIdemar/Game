﻿using System.Drawing;
using Движение.Controllers;

namespace Движение.Entites
{
    public class Entity
    {
        public int posX;
        public int posY;

        public Point LocationMap;
        public Point delta;
        public int dirX;
        public int dirY;
        public int lastDirX = 0;
        private int counter= 0;
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
            delta = new Point(0, 0);
            size = 30;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            pressButtonMove = false;
            LocationMap = new Point(MapController.mapWidth / 2, MapController.mapHeight / 2);
        }

        public void Move()
        {
            delta.X += dirX;
            delta.Y += dirY;
            counter += 3;
            if (counter == 30)
            {
                if (dirX > 0)
                    LocationMap.X++;
                else if (dirX < 0)
                    LocationMap.X--;
                else if (dirY > 0)
                    LocationMap.Y++;
                else if (dirY < 0)
                    LocationMap.Y--;
                if (MapController.map[LocationMap.Y, LocationMap.X] == 'C')
                {
                    MapController.map[LocationMap.Y, LocationMap.X] = '0';
                }

                ResetMove();
            }
        }

        public void ResetMove()
        {
            dirY = 0;
            dirX = 0;
            counter = 0;
            isMoving = false;
        }

        public void PlayAnimation(Graphics g)
        {
            if (currentFrame < currentLimit - 1)
                currentFrame++;
            else currentFrame = 0;
            if ((currentAnimation == 0 || currentAnimation == 1) && (dirX > 0 
                || lastDirX > 0))
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
