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
            List<Entity> collidableEntities = ecsContainer.Entities.Where(x => x.IsCollidable()).ToList();
            //Find the most top-left and most bottom-right collidable entities and split up search in quadrants based on that.
            List<KeyValuePair<Guid, Position>> collidablePositions = ecsContainer.Positions.Where(x => collidableEntities.Any(y => y.Id == x.Key)).Select(x => x).ToList();
            List<KeyValuePair<Guid, Display>> collidableDisplays = ecsContainer.Displays.Where(x => collidableEntities.Any(y => y.Id == x.Key)).Select(x => x).ToList();
            collidablePositions = collidablePositions.OrderBy(x => x.Value.OriginPosition.X).ThenBy(x => x.Value.OriginPosition.Y).ToList();
            Dictionary<int, List<Entity>> buckets = new Dictionary<int, List<Entity>>();
            if (collidablePositions.Count >= 2)
            {
                //https://conkerjo.wordpress.com/2009/06/13/spatial-hashing-implementation-for-fast-2d-collisions/
                Position first = collidablePositions.First().Value;
                Position last = collidablePositions.Last().Value;
                Display firstDiplay = collidableDisplays.Where(x => x.Key == collidablePositions.First().Key).First().Value;
                Display lastDisplay = collidableDisplays.Where(x => x.Key == collidablePositions.Last().Key).First().Value;
                Rectangle collidableArea = new Rectangle((int)first.OriginPosition.X - ((int)(firstDiplay.SpriteSheetSize / 2) * (int)firstDiplay.Scale), 
                    (int)first.OriginPosition.Y - ((int)(firstDiplay.SpriteSheetSize / 2) * (int)firstDiplay.Scale), 
                    ((int)last.OriginPosition.X - (int)last.OriginPosition.X) - ((int)(firstDiplay.SpriteSheetSize / 2) * (int)firstDiplay.Scale), 
                    ((int)last.OriginPosition.Y - (int)first.OriginPosition.Y) -((int)(firstDiplay.SpriteSheetSize / 2) * (int)firstDiplay.Scale));

                int rows = (int)Math.Round((double)collidableArea.Height / Constants.Systems.Collision.MapCellSize, 0, MidpointRounding.AwayFromZero) + 1;
                int cols = (int)Math.Round((double)collidableArea.Width / Constants.Systems.Collision.MapCellSize, 0, MidpointRounding.AwayFromZero) + 1;
                for(int i = 0; i < rows*cols; i++)
                {
                    buckets.Add(i, new List<Entity>());
                }

                //Populate the buckets
                foreach(Entity entity in collidableEntities)
                {
                    Position entityPosition = collidablePositions.Where(x => x.Key == entity.Id).First().Value;
                    Display entityDisplay = ecsContainer.Displays[entity.Id];
                    //Get 4 corners of the thing
                    Vector2 topLeft = new Vector2(entityPosition.OriginPosition.X - ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale), entityPosition.OriginPosition.Y - ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale));
                    Vector2 bottomLeft = new Vector2(entityPosition.OriginPosition.X - ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale), entityPosition.OriginPosition.Y + ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale));
                    Vector2 topRight = new Vector2(entityPosition.OriginPosition.X + ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale), entityPosition.OriginPosition.Y - ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale));
                    Vector2 bottomRight = new Vector2(entityPosition.OriginPosition.X + ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale), entityPosition.OriginPosition.Y + ((entityDisplay.SpriteSheetSize / 2) * entityDisplay.Scale));
                    buckets[CalculateBucketKey(topLeft, collidableArea)].Add(entity);
                    buckets[CalculateBucketKey(bottomLeft, collidableArea)].Add(entity);
                    buckets[CalculateBucketKey(topRight, collidableArea)].Add(entity);
                    buckets[CalculateBucketKey(bottomRight, collidableArea)].Add(entity);
                }

                for (int i = 0; i < buckets.Count; i++)
                {
                    buckets[i] = buckets[i].Distinct().ToList();
                }

                // BREAKPOINT HERE
            }
        }

        private static int CalculateBucketKey(Vector2 point, Rectangle collidableArea)
        {
           return (int)(
                   (Math.Floor(Math.Abs(point.X) / Constants.Systems.Collision.MapCellSize)) +
                   (Math.Floor(Math.Abs(point.Y) / Constants.Systems.Collision.MapCellSize)) *
                   collidableArea.Width );
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
