using System;
using System.Collections.Generic;
using System.Text;
using Движение.Entites;

namespace Движение.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(Entity entity)
        {
            if (entity.posX + entity.dirX <= 0 || entity.posX + entity.dirX >= MapController.cellSize * (MapController.mapWidth - 1)
                || entity.posY + entity.dirY <= 0 || entity.posY + entity.dirY >= MapController.cellSize * (MapController.mapHeight - 1))
            {
                entity.isMoving = false;
                entity.delta = 0;
                entity.dirX = 0;
                entity.dirY = 0;
                return false;
            }
            return true;
        }

    }
}
