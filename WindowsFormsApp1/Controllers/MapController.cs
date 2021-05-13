﻿using System;
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
                { '0', '0', 'w', 'w', '0', '0', 'w', '0', 'w', 'w', '0', 'w', 'w', '0', 'w', 'w', 'w', 'w', 'w', 'w', '0'},
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

        public static void DrawMap(Graphics g, Point delta, int width, int height)
        {
            drawMonster = monsters.First;
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    switch (map[i, j])
                    {

                        case 'w':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 96, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'K':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 170, 0, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'B':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 96, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'W':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 170, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'M':
                            drawMonster.Value.posX = j * cellSize - delta.X - width / 24;
                            drawMonster.Value.posY = i * cellSize - delta.Y - height / 5;
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            drawMonster.Value.PlayAnimation(g, drawMonster.Value.posX, drawMonster.Value.posY, drawMonster.Value.size);
                            drawMonster = drawMonster.Next;
                            break;

                        case 'C':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            g.DrawImage(spriteChest, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 0, 0, 200, 129, GraphicsUnit.Pixel);
                            break;

                        case 'm':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 170, 30, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case 'T':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 120, 75, 20, 20, GraphicsUnit.Pixel);
                            break;

                        case '0':
                            g.DrawImage(spriteSheet, new Rectangle(new Point(j * cellSize - delta.X - width / 24, i * cellSize - delta.Y - height / 5), new Size(cellSize, cellSize)), 0, 0, 20, 20, GraphicsUnit.Pixel);
                            break;
                    }
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