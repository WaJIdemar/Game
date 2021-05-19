using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Движение.Entites;

namespace Движение.Controllers
{
    public static class MoveController
    {
        private static Hero player; 

        public static void AddPlayer(Hero entity)
        {
            player = entity;
        }

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            player.pressButtonMove = false;
        }

        public static void OnPress(object sender, KeyEventArgs e)
        {
            var dir = player.Size / 10;
            if (!player.pressButtonMove && !player.isMoving && player.isAlive)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        player.dirY = -dir;
                        player.isMoving = true;
                        player.lastDirY = -1;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.S:
                        player.dirY = dir;
                        player.isMoving = true;
                        player.lastDirY = 1;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.A:
                        player.dirX = -dir;
                        player.lastDirX = -1;
                        player.lastDirY = 0;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.D:
                        player.dirX = dir;
                        player.lastDirX = 1;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.Escape:
                        Application.Exit();
                        break;
                }
                player.pressButtonMove = true;
            }
        }

    }
}
