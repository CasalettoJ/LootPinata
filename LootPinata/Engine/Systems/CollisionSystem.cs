using LootPinata.ArkContent.Dungeons;
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
        public static void CheckForCollisions(ref ECSContainer ecsContainer, DungeonTile[,] grid)
        {

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
