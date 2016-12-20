using LootPinata.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Systems
{
    public static class CollisionSystem
    {
        public static void CheckForCollisions(ref ECSContainer ecsContainer)
        {
            //IEnumerable<Entity> collidableEntities = ecsContainer.Entities.Where(x => x.IsCollidable());
            //IEnumerable<KeyValuePair<Guid, Position>> positions = ecsContainer.Positions.Where(x => collidableEntities.Any(y => x.Key == y.Id));
            //positions.OrderBy(x => x.Value.OriginPosition.X).ThenBy(x => x.Value.OriginPosition.Y);
            //if(positions.Count() > 2)
            //{
            //    ecsContainer.Displays[positions.First().Key].Color = Color.Black;
            //    ecsContainer.Displays[positions.First().Key].Scale = 5f;
            //    ecsContainer.Displays[positions.Last().Key].Color = Color.Black;
            //    ecsContainer.Displays[positions.Last().Key].Scale = 5f;
            //}
        }

        public static void ResetCollisions(ref ECSContainer ecsContainer)
        {
            foreach(Entity entity in ecsContainer.Entities.Where(x => x.IsCollidable()))
            {
                ecsContainer.Collisions[entity.Id].CollidedEntities.Clear();
            }
        }
    }
}
