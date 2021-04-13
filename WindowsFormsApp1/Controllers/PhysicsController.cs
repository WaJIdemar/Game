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
            if (!MapBorders(entity) || !CollisionWithObjects(entity))
            {
                entity.ResetMove();
                return false;
            }
            return true;
        }

        public static bool MapBorders(Entity entity)
        {
            if (entity.LocationMap.X + 1 > MapController.mapWidth && entity.LocationMap.X - 1 < 0 
                && entity.LocationMap.Y + 1 > MapController.mapHeight && entity.LocationMap.Y - 1 < 0)
            {
                return false;
            }

            return true;
        }

        public static bool CollisionWithObjects(Entity entity)
        {
            if (entity.dirX > 0 && MapController.map[entity.LocationMap.Y, entity.LocationMap.X + 1] != 0)
                return false;
            else if (entity.dirX < 0 && MapController.map[entity.LocationMap.Y, entity.LocationMap.X - 1] != 0)
                return false;
            else if (entity.dirY > 0 && MapController.map[entity.LocationMap.Y + 1, entity.LocationMap.X] != 0)
                return false;
            else if (entity.dirY < 0 && MapController.map[entity.LocationMap.Y - 1, entity.LocationMap.X] != 0)
                return false;

            return true;
        }
    }
}
