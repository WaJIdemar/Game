using System;
using System.Collections.Generic;
using System.Text;
using Движение.Entites;

namespace Движение.Controllers
{
    public static class PhysicsController
    {
        public static bool IsCollide(Hero entity, HashSet<char> colObj)
        {  
            if (MapBorders(entity) && CollisionWithObjects(entity, colObj))
            {
                return true;
            }
            entity.ResetMove();
            return false;
        }

        public static bool MapBorders(Hero entity)
        {
            if ((entity.dirX > 0 && entity.LocationMap.X + 1 < MapController.mapWidth) || (entity.LocationMap.X - 1 >= 0 
                && entity.dirX < 0) || (entity.LocationMap.Y + 1 < MapController.mapHeight && entity.dirY > 0)
                || (entity.LocationMap.Y - 1 >= 0 && entity.dirY < 0))
            {
                return true;
            }

            return false;
        }

        public static bool CollisionWithObjects(Hero entity, HashSet<char> obj)
        {
            if (entity.dirX > 0 && obj.Contains(MapController.map[entity.LocationMap.Y, entity.LocationMap.X + 1]))
                return false;
            else if (entity.dirX < 0 && obj.Contains(MapController.map[entity.LocationMap.Y, entity.LocationMap.X - 1]))
                return false;
            else if (entity.dirY > 0 && obj.Contains(MapController.map[entity.LocationMap.Y + 1, entity.LocationMap.X]))
                return false;
            else if (entity.dirY < 0 && obj.Contains(MapController.map[entity.LocationMap.Y - 1, entity.LocationMap.X]))
                return false;

            return true;
        }
    }
}