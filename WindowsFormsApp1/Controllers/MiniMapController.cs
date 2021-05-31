using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Linq;
using Движение.Entites;

namespace Движение.Controllers
{
    public static class MiniMapController
    {
        public const int mapHeight = MapController.mapHeight;
        public static HashSet<char> stop = new HashSet<char>(new char[] { 'w', 'W' });
        public const int mapWidth = MapController.mapWidth;
        public static int cellSize = 30;
        public static char[,] map = new char[mapHeight, mapWidth];
        public static Bitmap mapImage;
        public static Hero hero;

        public static void Init(Hero player)
        {
            map = MapController.map;
            map[player.LocationMap.Y, player.LocationMap.X] = 'p';
            hero = player;
            CreateMap();
        }

        //Возвращаем картинку мини мапы
        public static Image GetMiniMap()
        {
            return mapImage;
        }

        private static void CreateMap()
        {
            mapImage = new Bitmap(cellSize * hero.visibility * 2 + cellSize, cellSize * hero.visibility * 2 + cellSize);
            var g = Graphics.FromImage(mapImage);

            var points = MapController.FindFieldView(hero).ToHashSet();
            var deltaX = 0;
            if (hero.LocationMap.X - hero.visibility > 0)
                deltaX = hero.LocationMap.X - hero.visibility;
            var deltaY = 0;
            if (hero.LocationMap.Y - hero.visibility > 0)
                deltaY = hero.LocationMap.Y - hero.visibility;
            var rightBord = mapWidth;
            if (hero.LocationMap.X + hero.visibility < mapWidth)
                rightBord = hero.LocationMap.X + hero.visibility;
            var downBord = mapHeight;
            if (hero.LocationMap.Y + hero.visibility < mapHeight)
                downBord = hero.LocationMap.Y + hero.visibility;
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, cellSize * hero.visibility * 2 + cellSize, cellSize * hero.visibility * 2 + cellSize));
            for (int i = deltaX; i <= rightBord; i++)
            {
                for (int j = 0; j <= downBord; j++)
                {
                    if (points.Contains(new Point(i, j)))
                    {
                        switch (map[j, i])
                        {
                            case 'p':
                                var pathToSprite = hero.pathSpriteSheetStand + @"0.png";
                                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                                    pathToSprite));
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                g.DrawImage(image, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            case 'w':
                                g.DrawImage(MapController.spriteWall, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            case 'K':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                g.DrawImage(MapController.spriteKnight, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            //case 'B':
                            //    g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                            //    break;

                            case 'W':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                g.DrawImage(MapController.spriteWater, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            case 'M':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                MapController.monsters[(i, j)].PlayAnimation(g, (i - deltaX) * cellSize, (j - deltaY) * cellSize, cellSize);
                                break;

                            case 'C':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                g.DrawImage(MapController.spriteChest, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            case 'm':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                MapController.monsters[(i, j)].PlayAnimation(g, (i - deltaX) * cellSize, (j - deltaY) * cellSize, cellSize);
                                break;

                            //case 'T':
                            //    g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                            //    break;

                            case '0':
                                g.DrawImage(MapController.spriteGround, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;
                        }
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Black, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                    }
                }
            }
        }
    }
}
