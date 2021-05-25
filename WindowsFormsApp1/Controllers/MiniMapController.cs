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
        public const int mapHeight = 21;
        public static HashSet<char> stop = new HashSet<char>(new char[] { 'w', 'W' });
        public const int mapWidth = 21;
        public static int cellSize = 30;
        public static char[,] map = new char[mapHeight, mapWidth];
        public static Image spriteSheet;
        public static Bitmap mapImage;
        public static Hero hero;

        public static void Init(Hero player)
        {
            map = MapController.GetMap();
            map[player.LocationMap.Y, player.LocationMap.X] = 'p';
            hero = player;
            spriteSheet =
               new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"Sprites\Forest.png"));
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

            var points = MapController.FindFieldView(hero).OrderBy(p => p.X).ThenBy(p => p.Y).ToHashSet();
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
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, cellSize * hero.visibility * 2 - cellSize, cellSize * hero.visibility * 2 - cellSize));
            for (int i = deltaX; i <= rightBord; i++)
            {
                for (int j = 0; j <= downBord; j++)
                {
                    if (points.Contains(new Point(i, j)))
                    {
                        switch (map[j, i])
                        {
                            case 'p':
                                var pathToSprite = hero.pathSpriteSheetRigth + @"0.png";
                                var image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(),
                                    pathToSprite));
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                                g.DrawImage(image, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)));
                                break;

                            case 'w':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'K':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 170, 0, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'B':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'W':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 170, 75, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'M':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'C':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'm':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case 'T':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                                break;

                            case '0':
                                g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                                break;
                        }
                    }
                    else
                    {
                        g.DrawImage(spriteSheet, new Rectangle(new Point((i - deltaX) * cellSize, (j - deltaY) * cellSize), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                    }
                }
            }
            mapImage.Save(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
           .Parent.Parent.Parent.FullName.ToString(), @"Map\MiniMap.png"), ImageFormat.Png);
        }
    }
}
