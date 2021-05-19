﻿using System.Drawing;
using System.IO;
using System.Text;
using Движение.Controllers;

namespace Движение.Entites
{
    public class Hero : ICharacter
    {
        public int posX;
        public int posY;
        public bool isAlive { get; private set; }
        public int Health { get; set; }
        public Point LocationMap;
        public Point delta;
        public int dirX;
        public int dirY;
        public int lastDirX = 0;
        public int lastDirY = 0;
        private int counter = 0;
        public bool isMoving;
        public int visibility = 3;
        public ICharacter whoInBattle = null;
        public bool isInBattle;
        public Image nowSprite { get; set; }
        public bool pressButtonMove;

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;
        public int runFrames;
        public int attackFrames;
        public int deathFrames;

        public string pathSpriteSheetLeft;
        public string pathSpriteSheetRigth;

        public int Size { get; set; }

        public Hero(int posX, int posY, int idleFrames, int runFrames, int attackFrames, int deathFrames,
            string spriteSheetLeft, string spriteSheetRigth, int size)
        {
            this.posX = posX;
            this.posY = posY;
            this.runFrames = runFrames;
            this.idleFrames = idleFrames;
            this.attackFrames = attackFrames;
            this.deathFrames = deathFrames;
            this.pathSpriteSheetLeft = spriteSheetLeft;
            this.pathSpriteSheetRigth = spriteSheetRigth;
            delta = new Point(0, 0);
            this.Size = size;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            pressButtonMove = false;
            LocationMap = new Point(MapController.mapWidth / 2, MapController.mapHeight / 2);
            isInBattle = false;
            Health = 25;
            isAlive = true;
        }

        public void Move()
        {
            delta.X += dirX;
            delta.Y += dirY;
            counter += 5;
            if (counter == 50)
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
                else if (MapController.map[LocationMap.Y, LocationMap.X] == 'M')
                {
                    whoInBattle = MapController.characters[LocationMap.Y, LocationMap.X];
                    if (Health > 0)
                    {
                        isInBattle = true;
                    }
                    else
                        isAlive = false;
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

        public void PlayAnimation(Graphics g, int posX, int posY, int size)
        {
            var pathToSprite = new StringBuilder();
            var state = new StringBuilder();
            if (currentFrame < currentLimit - 1)
                currentFrame++;
            else currentFrame = 0;
            if (currentAnimation == 0)
                state.Append(@"Stand\");
            else if (currentAnimation == 1)
                state.Append(@"Run\");
            if ((currentAnimation == 0 || currentAnimation == 1) && (dirX > 0
                || lastDirX > 0))
            {
                pathToSprite.Append(pathSpriteSheetRigth + state + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                nowSprite = image;
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
            else
            {
                pathToSprite.Append(pathSpriteSheetLeft + state + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                nowSprite = image;
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
        }
    }
}
