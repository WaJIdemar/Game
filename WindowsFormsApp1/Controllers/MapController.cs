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
        public const int mapHeight = 21;
        public static HashSet<char> stop = new HashSet<char>(new char[] { 'w', 'W' });
        public const int mapWidth = 21;
        public static int cellSize = 30;
        public static char[,] map = new char[mapHeight, mapWidth];
        public static Image spriteSheet;
        public static Image spriteChest;
        public static LinkedList<OrangeMonster> monsters;
        public static LinkedListNode<OrangeMonster> drawMonster;
        public static Image background = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Background.jpg"));
        public static Image pictureMap;

        public static void Init()
        {
            map = GetMap();
            spriteSheet =
               new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Forest.png"));
            spriteChest =
                new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Chest.png"));
            CreateEntity();
            CreateMap();
        }

        /* 
         * w = wall;
         * W = Water;
         * M = Monster;
         * C = Chest;
         * 0 = Ground;
        */

        public static char[,] GetMap()
        {
            var t = new char[,]
            {
                { '0', '0', '0', 'w', 'w', 'w', 'w', 'C', 'w', '0', '0', '0', 'w', 'w', 'C', 'w', '0', 'C', '0', 'w', '0'},
                { '0', '0', '0', '0', 'w', 'w', 'w', '0', 'M', '0', 'w', '0', 'w', 'w', '0', 'M', '0', 'w', '0', 'w', '0'},
                { '0', '0', '0', '0', '0', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', '0', '0'},
                { 'w', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', '0', 'w', 'w'},
                { 'w', 'w', '0', '0', '0', '0', '0', '0', '0', 'M', '0', '0', 'w', '0', 'w', 'w', 'w', 'w', '0', '0', '0'},
                { 'w', 'w', 'w', '0', '0', 'w', 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0'},
                { 'w', 'w', 'w', 'w', '0', 'w', 'C', '0', 'w', 'w', 'w', '0', 'w', 'M', 'w', '0', 'w', 'w', 'w', 'w', 'M'},
                { 'C', 'w', 'w', '0', '0', '0', '0', 'M', '0', '0', '0', 'M', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'C', 'w', 'w', 'w', 'w', 'w', 'w', '0'},
                { 'M', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', '0', '0', 'M', 'M', 'M', 'M', 'M', '0', '0', '0', '0'},
                { '0', 'w', 'w', 'w', '0', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', 'w', 'w', '0', '0', 'w', '0', 'w', 'w', 'M', 'w', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0'},
                { 'w', '0', '0', 'w', 'w', 'M', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},
                { 'w', 'w', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'M', 'w', 'w', 'w', 'w'},
                { 'w', '0', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', 'w'},
                { 'w', 'M', '0', '0', '0', '0', 'w', '0', '0', '0', 'M', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', '0', 'w'},
                { 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', '0', '0', 'w', '0', 'w', '0', 'M', '0', '0', 'w'},
                { 'w', '0', '0', 'C', 'w', '0', 'w', '0', '0', '0', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', '0', '0'},
                { 'w', 'w', 'C', '0', 'w', '0', 'w', '0', 'w', 'C', '0', 'w', '0', 'w', '0', 'M', '0', '0', 'w', 'w', '0'},
                { 'w', 'w', 'w', '0', '0', 'M', '0', '0', 'w', 'w', '0', '0', 'M', 'w', 'w', 'C', 'w', '0', '0', '0', 'M'},
            };
            return t;
        }

        public static void DrawMap(Graphics g, Hero player)
        {
            drawMonster = monsters.First;

            foreach (var point in FindFieldView(player))
            {
                switch (map[point.Y, point.X])
                {

                    case 'w':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case 'K':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 170, 0, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case 'B':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case 'W':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 170, 75, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case 'M':
                        drawMonster.Value.posX = point.X * cellSize - player.delta.X; 
                        drawMonster.Value.posY = point.Y * cellSize - player.delta.Y;
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                        drawMonster.Value.PlayAnimation(g, drawMonster.Value.posX, drawMonster.Value.posY, drawMonster.Value.size);
                        drawMonster = drawMonster.Next;
                        break;

                    case 'C':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                        g.DrawImage(spriteChest, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 0, 0, 200, 129, GraphicsUnit.Pixel);
                        break;

                    case 'm':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case 'T':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                        break;

                    case '0':
                        g.DrawImage(spriteSheet, new Rectangle(new Point(point.X * cellSize - player.delta.X, point.Y * cellSize - player.delta.Y), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                        break;
                }
            }
        }

        private static void CreateMap()
        {
            var mapImage = new Bitmap(cellSize * mapWidth, cellSize * mapHeight);
            var g = Graphics.FromImage(mapImage);
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    switch (map[i, j])
                    {

                        case 'w':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'K':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'B':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'W':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'm':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'T':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'M':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'C':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case '0':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;
                    }
                }
            }
            mapImage.Save(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
            .Parent.Parent.Parent.FullName.ToString(), @"Map\Map.png"), ImageFormat.Png);
            pictureMap = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
            .Parent.Parent.Parent.FullName.ToString(), @"Map\Map.png"));
        }

        private static void CreateEntity()
        {
            monsters = new LinkedList<OrangeMonster>();
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (map[i, j] == 'M')
                        monsters.AddLast(new OrangeMonster(j * cellSize, i * cellSize, MonsterModels.idleFrames, MonsterModels.runFrames, MonsterModels.deathFrames,
               MonsterModels.deathFrames, Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Monsters\Orange\Stand\"), new Point(i, j)));
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
                    || point.X >= start.X + visibility || point.Y >= start.Y + visibility
                    || point.X <= start.X - visibility || point.Y <= start.Y - visibility
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

        //private static Tuple<Tuple<int, int>, Tuple<int, int>> GetInvisiblePoints(Point start, Point wall)
        //{
        //    if (wall.X < start.X && wall.Y < start.Y)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, -1), new Tuple<int, int>(wall.Y, -1));
        //    else if (wall.X == start.X && wall.Y < start.Y)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, 0), new Tuple<int, int>(wall.Y, -1));
        //    else if (wall.X > start.X && wall.Y < start.Y)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, 1), new Tuple<int, int>(wall.Y, -1));
        //    else if (wall.Y == start.Y && wall.X < start.X)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, -1), new Tuple<int, int>(wall.Y, 0));
        //    else if (wall.Y == start.Y && wall.X > start.X)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, 1), new Tuple<int, int>(wall.Y, 0));
        //    else if (wall.X < start.X && wall.Y > start.Y)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, -1), new Tuple<int, int>(wall.Y, 1));
        //    else if (wall.X == start.X && wall.Y > start.Y)
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, 0), new Tuple<int, int>(wall.Y, 1));
        //    else
        //        return new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(wall.X, 1), new Tuple<int, int>(wall.Y, 1));
        //}

        //private static bool PointVisible(Point point, List<Tuple<Tuple<int, int>, Tuple<int, int>>> walls)
        //{
        //    foreach (var t in walls)
        //    {
        //        if (t.Item1.Item1 < 0)
        //    }
        //}

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
                        hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.X == start.X && wall.Y < start.Y)
            {
                x = wall.X;
                if (start.Y - visibility >= 0)
                    y = start.Y - visibility;
                for (; y <= wall.Y; y++)
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
                        hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.Y == start.Y && wall.X < start.X)
            {
                if (start.X - visibility >= 0)
                    x = start.X - visibility;
                y = wall.Y;
                for (; x <= wall.X; x++)
                    hashSet.Add(new Point(x, y));
            }
            else if (wall.Y == start.Y && wall.X > start.X)
            {
                x = wall.X;
                y = wall.Y;
                for (; x <= (start.X + visibility) && (x < mapWidth); x++)
                    hashSet.Add(new Point(x, y));
            }
            else if (wall.X < start.X && wall.Y > start.Y)
            {
                if (start.X - visibility >= 0)
                    x = start.X - visibility;
                yOld = wall.Y;
                for (; x <= wall.X; x++)
                {
                    y = yOld;
                    for (; (y < start.Y + visibility) && (y < mapHeight); y++)
                        hashSet.Add(new Point(x, y));
                }
            }
            else if (wall.X == start.X && wall.Y > start.Y)
            {
                x = wall.X;
                y = wall.Y;
                for (; (y <= start.Y + visibility) && (y < mapHeight); y++)
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
                        hashSet.Add(new Point(x, y));
                }
            }
            hashSet.Remove(wall);
            return hashSet;
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