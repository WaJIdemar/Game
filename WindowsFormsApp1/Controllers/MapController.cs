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
        public const int mapWidth = 21;
        public static int cellSize = 31;
        public static int[,] map = new int[mapHeight, mapWidth];
        public static Image spriteSheet;

        public static void Init()
        {
            map = GetMap();
            spriteSheet =
               new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Forest.png"));
        }

        public static int[,] GetMap()
        {
            //return new int[,]
            //{
            //    {1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 2},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 3, 8, 4, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {3, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 4},
            //};
            //return new int[,]
            //{
            //    {1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 2},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
            //    {3, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 4},
            //};
            return new int[,]
            {
                {1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 2},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 1, 5, 0, 3, 2, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 3, 5, 0, 1, 4, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 3, 8, 4, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7},
                {3, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 4},
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

                        case 1:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 2:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 3:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 4:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 5:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 96, 20, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 6:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 120, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 7:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 8:
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize, i * cellSize), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 0:
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
