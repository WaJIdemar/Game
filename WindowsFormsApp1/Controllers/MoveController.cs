using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Движение.Entites;

namespace Движение.Controllers
{
    public static class MoveController
    {
        private static Entity player;

        private const int dir = 3; 

        public static void AddPlayer(Entity entity)
        {
            player = entity;
        }

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            player.pressButtonMove = false;
        }

        public static void OnPress(object sender, KeyEventArgs e)
        {
            if (!player.pressButtonMove && !player.isMoving)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        player.dirY = -dir;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.S:
                        player.dirY = dir;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.A:
                        player.dirX = -dir;
                        player.lastDirX = -1;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                    case Keys.D:
                        player.dirX = dir;
                        player.lastDirX = 1;
                        player.isMoving = true;
                        player.SetAnimationConfiguration(1);
                        break;
                }
                player.pressButtonMove = true;
            }
        }

    }
}
