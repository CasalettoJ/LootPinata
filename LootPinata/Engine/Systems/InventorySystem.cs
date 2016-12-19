﻿using LootPinata.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Systems
{
    public static class InventorySystem
    {
        #region debug
        public static void GenerateRandomInventoryItemsForEntity(ref ECSContainer ecsContainer, Guid ownerId)
        {
            List<Guid> itemIds = new List<Guid>();
            int numberOfItems = Constants.Random.Next(1, 90);
            for (int i = 0; i <= numberOfItems; i++)
            {
                BaseEntity item = new BaseEntity(ComponentFlags.LABEL, ComponentFlags.DISPLAY);
                item.Label = new Label()
                {
                    Color = Color.Black,
                    Displacement = new Vector2(0, -4),
                    WhenToShow = WhenToShowLabel.NEVER,
                    Origin = Vector2.Zero,
                    Rotation = 0f,
                    Scale = 1f,
                    DistanceRenderBuffer = 200,
                    SpriteEffect = SpriteEffects.None,
                    Text = "item"
                };
                item.Display = new Display()
                {
                    Color = Color.White,
                    Layer = DisplayLayer.FOREGROUND,
                    Opacity = 1f,
                    Origin = new Vector2(Constants.Sprites.ItemGrid / 2, Constants.Sprites.ItemGrid / 2),
                    Rotation = 0f,
                    Scale = 2f,
                    SpriteEffect = SpriteEffects.None,
                    SpriteSheetSize = Constants.Sprites.ItemGrid,
                    SpriteSheetKey = Constants.Sprites.ItemSheetKey,
                    SpriteSheetCol = Constants.Random.Next(1, 22),
                    SpriteSheetRow = Constants.Random.Next(1, 14)
                };
                Guid id = ecsContainer.AddEntity(item);
                itemIds.Add(id);
            }
            BaseEntity ownerAddition = new BaseEntity(ComponentFlags.INVENTORY);
            ownerAddition.Inventory = new Inventory() { EntitiesOwned = itemIds };
            ecsContainer.AppendEntity(ownerAddition, ownerId);
        }

        public static void DropEntityInventory(ref ECSContainer ecsContainer, Guid ownerId)
        {
            Inventory ownerInventory = ecsContainer.Inventories[ownerId];
            Position ownerPosition = ecsContainer.Positions[ownerId];
            foreach(Guid id in ownerInventory.EntitiesOwned)
            {
                BaseEntity itemChange = new BaseEntity(ComponentFlags.POSITION, ComponentFlags.MOVEMENT);
                itemChange.Position = new Position()
                {
                    OriginPosition = ownerPosition.OriginPosition
                };
                int velocity = Constants.Random.Next(50, 250);
                itemChange.Movement = new Movement()
                {
                    BaseVelocity = velocity,
                    MovementType = MovementType.DIRECTED,
                    Velocity = velocity * Constants.Random.Next(2, 6),
                    TargetPosition = ownerPosition.OriginPosition + new Vector2(Constants.Random.Next(-velocity, velocity), Constants.Random.Next(-velocity, velocity))
                };
                ecsContainer.AppendEntity(itemChange, id);
            }
            ownerInventory.EntitiesOwned.Clear();
        }
        #endregion
    }
}
