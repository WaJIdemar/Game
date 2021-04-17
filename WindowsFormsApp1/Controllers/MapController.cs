using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

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

        public static void Init()
        {
            map = GetMap();
            spriteSheet =
               new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Forest.png"));
            spriteChest =
                new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Chest.png"));
        }

        public static char[,] GetMap()
        {
            return new char[,]
            {
                { '0', '0', '0', 'w', 'w', 'w', 'w', '0', 'w', '0', '0', '0', 'w', 'w', '0', 'w', '0', '0', '0', 'w', '0'},
                { '0', '0', '0', '0', 'w', 'w', 'w', '0', '0', '0', 'w', '0', 'w', 'w', '0', '0', '0', 'w', '0', 'w', '0'},
                { '0', '0', '0', '0', '0', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', '0', '0'},
                { 'w', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', '0', 'w', 'w'},
                { 'w', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', 'w', 'w', 'w', '0', '0', '0'},
                { 'w', 'w', 'w', '0', '0', 'w', 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0'},
                { 'w', 'w', 'w', 'w', '0', 'w', 'C', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', '0'},
                { '0', 'w', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', 'w', 'w', 'w', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0'},
                { '0', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},
                { '0', 'w', 'w', 'w', '0', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', 'w', '0', '0', '0', '0', 'w', '0'},
                { '0', '0', 'w', 'w', '0', '0', 'w', '0', 'w', 'w', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0'},
                { 'w', '0', '0', 'w', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},
                { 'w', 'w', '0', '0', '0', '0', '0', '0', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w'},
                { 'w', '0', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', '0', '0', '0', '0', '0', '0', 'w'},
                { 'w', '0', '0', '0', '0', '0', 'w', '0', '0', '0', '0', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', '0', 'w'},
                { 'w', '0', 'w', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', '0', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w'},
                { 'w', '0', '0', '0', 'w', '0', 'w', '0', '0', '0', 'w', 'w', '0', 'w', '0', 'w', '0', 'w', 'w', '0', '0'},
                { 'w', 'w', '0', '0', 'w', '0', 'w', '0', 'w', '0', '0', 'w', '0', 'w', '0', '0', '0', '0', 'w', 'w', '0'},
                { 'w', 'w', 'w', '0', '0', '0', '0', '0', 'w', 'w', '0', '0', '0', 'w', 'w', '0', 'w', '0', '0', '0', '0'},
            };
        }

        public static void DrawMap(Graphics g)
        {
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

                        case 'M':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 20, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'C':
                            g.DrawImage(spriteChest, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 0, 0, 200, 129, GraphicsUnit.Pixel);
                            break;

                        case 'm':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'T':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case '0':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;
                    }
                }
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