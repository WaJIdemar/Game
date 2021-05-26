using System.Drawing;
using System.IO;
using System.Text;
using Движение.Controllers;

namespace Движение.Entites
{
    public class Hero : ICharacter
    {
        public bool isAlive { get; private set; }
        public int AttackPower { get; set; }
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
        public Image SpriteForBattle { get; set; }
        public Image SpriteFace { get; set; }
        public bool pressButtonMove;

        public int currentAnimation;
        public int currentFrame;
        public int currentLimit;

        public int idleFrames;
        public int runFrames;

        public string pathSpriteSheetLeft;
        public string pathSpriteSheetRigth;
        public string pathSpriteSheetUp;
        public string pathSpriteSheetDown;
        public string pathSpriteSheetStand;

        public int Size { get; set; }
        public int posX { get; set ; }
        public int posY { get; set ; }

        public Hero(int posX, int posY, int idleFrames, int runFrames, int size)
        {
            this.posX = posX;
            this.posY = posY;
            this.runFrames = runFrames;
            this.idleFrames = idleFrames;
            delta = new Point(0, 0);
            Size = size;
            currentAnimation = 0;
            currentFrame = 0;
            currentLimit = idleFrames;
            pressButtonMove = false;
            LocationMap = new Point(MapController.mapWidth / 2, MapController.mapHeight / 2);
            isInBattle = false;
            Health = 25;
            isAlive = true;
            pathSpriteSheetRigth = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Move Right\");
            pathSpriteSheetLeft = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Move Left\");
            pathSpriteSheetUp = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Move Up\");
            pathSpriteSheetDown = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Move Down\");
            pathSpriteSheetStand = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Stand\");
            SpriteFace = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\Face.png"));
            SpriteForBattle = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Main character\BattleModel.png"));
            AttackPower = 2;
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
                else if (MapController.map[LocationMap.Y, LocationMap.X] == 'M' ||
                    MapController.map[LocationMap.Y, LocationMap.X] == 'm')
                {
                    whoInBattle = MapController.monsters[(LocationMap.X, LocationMap.Y)];
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
            SetAnimationConfiguration(0);
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
            }
        }

        public void PlayAnimation(Graphics g, int posX, int posY, int size)
        {
            var pathToSprite = new StringBuilder();
            if (currentFrame < currentLimit - 1)
                currentFrame++;
            else currentFrame = 0;
            if (dirX > 0)
            {
                pathToSprite.Append(pathSpriteSheetRigth + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
            else if (dirX < 0)
            {
                pathToSprite.Append(pathSpriteSheetLeft + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
            else if (dirY > 0)
            {
                pathToSprite.Append(pathSpriteSheetDown + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
            else if (dirY < 0)
            {
                pathToSprite.Append(pathSpriteSheetUp + currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
            else if (dirX == dirY && dirX == 0)
            {
                pathToSprite.Append(pathSpriteSheetStand+ currentFrame.ToString() + ".png");
                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                pathToSprite.ToString()));
                g.DrawImage(image, new Rectangle(new Point(posX, posY), new Size(size, size)));
            }
        }
    }
}
