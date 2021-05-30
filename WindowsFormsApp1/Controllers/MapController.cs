using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Движение.Entites;
using Движение.Models;

namespace Движение.Controllers
{
    public static class MapController
    {
        public const int mapHeight = 23;
        public static HashSet<char> stop = new HashSet<char>(new char[] { 'w', 'W' });
        public const int mapWidth = 23;
        public static int cellSize;
        public static char[,] map = new char[mapHeight, mapWidth];
        public static Image spriteSheet;
        public static Image spriteChest;
        public static Dictionary<(int, int), ICharacter> monsters;
        public static Image background = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Background.jpg"));
        public static Image pictureMap;
        public static Hero Hero;
        public static Image spriteGround;
        public static Image spriteWall;
        public static Image spriteWater;
        public static Image spriteKnight;

        public static void Init(Hero hero)
        {
            map = GetMap();
            Hero = hero;
            cellSize = hero.Size;
            spriteSheet =
               new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Forest.png"));
            spriteGround = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Object in Map\0.png"));
            spriteWall = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Object in Map\w.png"));
            spriteChest = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Object in Map\C.png"));
            spriteWater = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Object in Map\Water.png"));
            spriteKnight = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Object in Map\K.png"));
            CreateEntity();
        }

        /* 
         * w = wall;
         * W = Water;
         * M = Monster;
         * C = Chest;
         * m = mimic
         * K = knight
         * B = boss
         * 0 = Ground;
        */

        public static char[,] GetMap()
        {
            var t = new char[,]
            {
                { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'},
                { 'w', 'B', 'B', 'K', 'w', 'w', 'w', 'w', 'C', 'w', '0', '0', '0', 'w', 'w', 'C', 'w', '0', 'C', '0', 'w', '0', 'w'},
                { 'w', 'B', 'B', '0', 'K', 'w', 'w', 'w', '0', 'M', '0', 'w', '0', 'w', 'w', '0', 'M', '0', 'w', '0', 'w', '0', 'w'},
                { 'w', 'K', '0', '0', '0', 'K', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', '0', '0', 'w'},
                { 'w', 'w', 'K', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', '0', 'w', 'w', 'w'},
                { 'w', 'w', 'w', 'K', '0', '0', '0', '0', '0', '0', 'M', '0', '0', 'w', '0', 'w', 'w', 'w', 'w', '0', '0', '0', 'w'},
                { 'w', 'w', 'w', 'w', '0', '0', 'w', 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0', 'w'},
                { 'w', 'w', 'w', 'w', 'w', '0', 'w', 'C', '0', 'w', 'w', 'w', '0', 'w', 'M', 'w', '0', 'w', 'w', 'w', 'w', 'M', 'w'},
                { 'w', 'C', 'w', 'w', '0', '0', '0', '0', 'M', '0', '0', '0', 'M', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0', 'w'},
                { 'w', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'C', 'w', 'w', 'w', 'w', 'w', 'w', '0', 'w'},
                { 'w', 'M', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', 'W', 'W', 'W', 'W', 'w', '0', 'w'},
                { 'w', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', 'M', '0', '0', '0', '0', 'w'},
                { 'w', '0', 'w', 'w', 'w', '0', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', 'W', 'W', 'W', 'W', 'w', '0', 'w'},
                { 'w', '0', '0', 'w', 'w', '0', '0', 'w', '0', 'w', 'w', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', 'w'},
                { 'w', 'w', '0', '0', 'w', 'w', 'M', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'w'},
                { 'w', 'w', 'w', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'M', 'w', 'w', 'w', 'w', 'w'},
                { 'w', 'w', '0', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', 'w', 'w'},
                { 'w', 'w', 'M', '0', '0', '0', '0', 'w', '0', '0', '0', 'M', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', '0', 'w', 'w'},
                { 'w', 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', '0', '0', 'w', '0', 'w', '0', 'M', '0', '0', 'w', 'w'},
                { 'w', 'w', '0', '0', 'm', 'w', '0', 'w', '0', '0', '0', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', '0', '0', 'w'},
                { 'w', 'w', 'w', 'C', '0', 'w', '0', 'w', '0', 'w', 'C', '0', 'w', '0', 'w', '0', 'M', '0', '0', 'w', 'w', '0', 'w'},
                { 'w', 'w', 'w', 'w', '0', '0', 'M', '0', '0', 'w', 'w', '0', '0', 'M', 'w', 'w', 'C', 'w', '0', '0', '0', 'M', 'w'},
                { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'}
            };
            return t;
        }

        public static void DrawMap(Graphics g, Hero player)
        {
            foreach (var point in FindFieldView(player))
            {
                switch (map[point.Y, point.X])
                {
                    case 'w':
                        g.DrawImage(spriteWall, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        break;

                    case 'K':
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        g.DrawImage(spriteKnight, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        break;

                    case 'B':
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        if (monsters.ContainsKey((point.X, point.Y)))
                        {
                            if (map[point.Y +1, point.X +1] == 'B' && map[point.Y, point.X +1] == 'B'
                                && map[point.Y + 1, point.X] == 'B')
                            {
                                monsters[(point.X, point.Y)].posX = point.X * cellSize - player.delta.X;
                                monsters[(point.X, point.Y)].posY = point.Y * cellSize - player.delta.Y;
                                monsters[(point.X, point.Y)].PlayAnimation(g, monsters[(point.X, point.Y)].posX, monsters[(point.X, point.Y)].posY, monsters[(point.X, point.Y)].Size);
                            }
                        }
                        break;

                    case 'W':
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        g.DrawImage(spriteWater, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        break;

                    case 'M':
                        monsters[(point.X, point.Y)].posX = point.X * cellSize - player.delta.X;
                        monsters[(point.X, point.Y)].posY = point.Y * cellSize - player.delta.Y;
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        monsters[(point.X, point.Y)].PlayAnimation(g, monsters[(point.X, point.Y)].posX, monsters[(point.X, point.Y)].posY, monsters[(point.X, point.Y)].Size);
                        break;

                    case 'C':
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        g.DrawImage(spriteChest, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        break;

                    case 'm':
                        monsters[(point.X, point.Y)].posX = point.X * cellSize - player.delta.X;
                        monsters[(point.X, point.Y)].posY = point.Y * cellSize - player.delta.Y;
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        monsters[(point.X, point.Y)].PlayAnimation(g, monsters[(point.X, point.Y)].posX, monsters[(point.X, point.Y)].posY, monsters[(point.X, point.Y)].Size);
                        break;

                    case 'T':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case '0':
                        g.DrawImage(spriteGround, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)));
                        break;
                }
            }
        }

        private static void CreateEntity()
        {
            monsters = new Dictionary<(int, int), ICharacter>();

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (map[i, j] == 'M')
                        monsters.Add((j, i), new OrangeMonster(j * cellSize, i * cellSize, MonsterModels.idleFrames, new Point(j, i), Hero.Size));
                    else if (map[i, j] == 'm')
                        monsters.Add((j, i), new MimicMonster(j * cellSize, i * cellSize, MimicModels.idleFrames, new Point(j, i), Hero.Size));
                    else if (map[i, j] == 'B')
                    {
                        monsters.Add((j, i), new Boss(j * cellSize, i * cellSize, 1, new Point(j, i), Hero.Size * 2));
                    }
                }
            }
        }

        public static IEnumerable<Point> FindFieldView(Hero player)
        {
            var queue = new Queue<Point>();
            var visited = new HashSet<Point>();
            var start = player.LocationMap;
            var visibility = player.visibility;
            queue.Enqueue(start);

            var invisiblePoints = new HashSet<Point>();

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                if (point.X >= mapWidth || point.Y >= mapWidth || point.X < 0 || point.Y < 0
                    || point.X > start.X + visibility || point.Y > start.Y + visibility
                    || point.X < start.X - visibility || point.Y < start.Y - visibility
                    || visited.Contains(point) || invisiblePoints.Contains(point))
                    continue;
                visited.Add(point);
                yield return point;
                if (map[point.Y, point.X] == 'w')
                {
                    invisiblePoints.UnionWith(GetInvisiblePoints(start, point, visibility));
                    continue;
                }
                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                        if (dy != 0 && dx != 0) continue;
                        else
                        {
                            var newPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                            queue.Enqueue(newPoint);
                        }
            }
        }

        public static HashSet<Point> GetInvisiblePoints(Point start, Point wall, int visibility)
        {
            var hashSet = new HashSet<Point>();
            int x = 0;
            int y = 0;
            int yOld = 0;
            if (wall.X < start.X && wall.Y < start.Y)
            {
                if (start.X - visibility >= 0)
                    x = start.X - visibility;
                if (start.Y - visibility >= 0)
                {
                    y = start.Y - visibility;
                    yOld = y;
                }

                for (; x <= wall.X; x++)
                {
                    y = yOld;
                    for (; y <= wall.Y; y++)
                        if (map[y, x] != 'w')
                            hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.X == start.X && wall.Y < start.Y)
            {
                x = wall.X;
                if (start.Y - visibility >= 0)
                    y = start.Y - visibility;
                for (; y <= wall.Y; y++)
                    if (map[y, x] != 'w')
                        hashSet.Add(new Point(x, y));
            }
            else if (wall.X > start.X && wall.Y < start.Y)
            {
                x = wall.X;
                if (start.Y - visibility >= 0)
                {
                    y = start.Y - visibility;
                    yOld = y;
                }
                for (; (x <= start.X + visibility) && (x < mapWidth); x++)
                {
                    y = yOld;
                    for (; y <= wall.Y; y++)
                        if (map[y, x] != 'w')
                            hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.Y == start.Y && wall.X < start.X)
            {
                if (start.X - visibility >= 0)
                    x = start.X - visibility;
                y = wall.Y;
                for (; x <= wall.X; x++)
                    if (map[y, x] != 'w')
                        hashSet.Add(new Point(x, y));
            }
            else if (wall.Y == start.Y && wall.X > start.X)
            {
                x = wall.X;
                y = wall.Y;
                for (; x <= (start.X + visibility) && (x < mapWidth); x++)
                    if (map[y, x] != 'w')
                        hashSet.Add(new Point(x, y));
            }
            else if (wall.X < start.X && wall.Y > start.Y)
            {
                if (start.X - visibility > 0)
                    x = start.X - visibility;
                yOld = wall.Y;
                for (; x <= wall.X; x++)
                {
                    y = yOld;
                    for (; (y <= start.Y + visibility) && (y < mapHeight); y++)
                        if (map[y, x] != 'w')
                            hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.X == start.X && wall.Y > start.Y)
            {
                x = wall.X;
                y = wall.Y;
                for (; (y <= start.Y + visibility) && (y < mapHeight); y++)
                    if (map[y, x] != 'w')
                        hashSet.Add(new Point(x, y));
            }
            else
            {
                x = wall.X;
                yOld = wall.Y;
                for (; (x <= start.X + visibility) && (x < mapWidth); x++)
                {
                    y = yOld;
                    for (; (y <= start.Y + visibility) && (y < mapHeight); y++)
                        if (map[y, x] != 'w')
                            hashSet.Add(new Point(x, y));
                }
            }
            hashSet.Remove(wall);
            return hashSet;
        }

        public static void BackStep()
        {
            if (Hero.lastDirY != 0)
            {
                Hero.LocationMap.Y = Hero.LocationMap.Y - Hero.lastDirY;
                Hero.delta.Y = Hero.delta.Y - cellSize * Hero.lastDirY;
            }
            else
            {
                Hero.LocationMap.X = Hero.LocationMap.X - Hero.lastDirX;
                Hero.delta.X = Hero.delta.X - cellSize * Hero.lastDirX;
            }


        }

        public static int GetWidth()
        {
            return cellSize * (mapWidth) + 15;
        }
        public static int GetHeight()
        {
            return cellSize * (mapHeight + 1) + 10;
        }
    }
}