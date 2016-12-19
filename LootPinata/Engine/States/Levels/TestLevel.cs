using LootPinata.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LootPinata.Engine.Systems;
using LootPinata.Engine.Menus.States;
using LootPinata.Engine.IO.Settings;
using LootPinata.Engine.States.Menus;
using System.Xml.Serialization;
using System.IO;
using LootPinata.Engine.IO.ArkSystem;

namespace LootPinata.Engine.States.Levels
{
    public class TestLevel : IState
    {
        private Dictionary<string, Texture2D> _spriteSheets = new Dictionary<string, Texture2D>();
        private SpriteFont _labelFont;
        private ECSContainer _components;
        private ContentManager _content;

        public TestLevel(ContentManager content, Camera camera)
        {
            _content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            _labelFont = _content.Load<SpriteFont>(Constants.Fonts.TelegramaSmall);
            _spriteSheets.Add(Constants.Sprites.MonsterSheetKey, _content.Load<Texture2D>(Constants.Sprites.MonsterSheet));
            _spriteSheets.Add(Constants.Sprites.ItemSheetKey, _content.Load<Texture2D>(Constants.Sprites.ItemSheet));
            _spriteSheets.Add(Constants.Sprites.TileSheetKey, _content.Load<Texture2D>(Constants.Sprites.TileSheet));
            this._components = new ECSContainer();

            #region Debug Creation
            Guid playerId = ArkCreation.SpawnEntityWithOverrides(Constants.Ark.Monsters.Player, ref this._components, new BaseEntity(ComponentFlags.POSITION) {Position= new Position() { OriginPosition = new Vector2(0, 0) } });
            Guid testId = ArkCreation.SpawnEntityWithOverrides(Constants.Ark.Monsters.TestNpc, ref this._components, new BaseEntity(ComponentFlags.POSITION) { Position = new Position() { OriginPosition = new Vector2(0, 0) } });
            InventorySystem.GenerateRandomInventoryItemsForEntity(ref this._components, testId);
            camera.TargetEntity = playerId;
            #endregion
        }

        public void DrawContent(SpriteBatch spriteBatch, Camera camera)
        {
            Guid playerId = this._components.Entities.Where(c => c.HasComponents(ComponentFlags.IS_PLAYER)).FirstOrDefault().Id;
            // Draw Sprites
            this._components.Entities.ForEach((c) => 
            {
                if (c.HasDrawableSprite())
                {
                    DisplaySystem.DisplayEntity(spriteBatch, camera, this._components.Displays[c.Id], this._components.Positions[c.Id], this._spriteSheets[this._components.Displays[c.Id].SpriteSheetKey]);
                }
                if (c.HasDrawableLabel())
                {
                    DisplaySystem.DisplayLabel(spriteBatch, camera, this._components.Displays[c.Id], this._components.Labels[c.Id], this._components.Positions[c.Id], _labelFont, this._components.Positions[playerId], this._components.Displays[playerId]);
                }
            });
        }

        public IState UpdateState(ref GameSettings gameSettings, GameTime gameTime, Camera camera, KeyboardState currentKey, KeyboardState prevKey, MouseState currentMouse, MouseState prevMouse)
        {
            Guid playerId = this._components.Entities.Where(c => c.HasComponents(ComponentFlags.IS_PLAYER)).FirstOrDefault().Id;
            // Level input
            if (currentKey.IsKeyDown(Keys.Escape) && prevKey.IsKeyUp(Keys.Escape))
            {
                return new PauseState(this._content, this);
            }

            if (currentKey.IsKeyDown(Keys.F) && prevKey.IsKeyUp(Keys.F))
            {
                this._components.DelayedActions.Add(new Action(() =>
                {
                    Guid testId = ArkCreation.SpawnEntityWithOverrides(Constants.Ark.Monsters.TestNpc, ref this._components, new BaseEntity(ComponentFlags.POSITION) { Position = new Position() { OriginPosition = this._components.Positions[playerId].OriginPosition } });
                    InventorySystem.GenerateRandomInventoryItemsForEntity(ref this._components, testId);
                }));
            }
            if (currentKey.IsKeyDown(Keys.R) && prevKey.IsKeyUp(Keys.R))
            {
                this._components.DelayedActions.Add(new Action(() =>
                {
                    Guid id = this._components.Entities.Where(x => x.HasDrawableSprite() && !x.HasComponents(ComponentFlags.IS_PLAYER) && x.HasComponents(ComponentFlags.INVENTORY)).First().Id;
                    InventorySystem.DropEntityInventory(ref this._components, id);
                    this._components.DestroyEntity(id);
                }));
            }

            // Camera Updates
            CameraSystem.ControlCamera(currentKey, prevKey, camera, gameTime);
            CameraSystem.PanCamera(camera, gameTime);

            // Entity Movement Updates
            this._components.Entities.ForEach(c =>
            {
                if (c.IsMovable())
                {
                    switch (this._components.Movements[c.Id].MovementType)
                    {
                        case MovementType.AI:
                            //AI Movement System Call
                            break;
                        case MovementType.INPUT:
                            MovementSystem.InputMovement(currentKey, prevKey, gameTime, this._components.Positions[c.Id], this._components.Movements[c.Id]);
                            break;
                        case MovementType.DIRECTED:
                            MovementSystem.UpdateMovingEntities(this._components.Movements[c.Id], this._components.Positions[c.Id], gameTime, ref this._components, c);
                            break;
                    }
                }
            });

            // Entity Information Updates
            // Collision
            CollisionSystem.CheckForCollisions(ref this._components);

            // Set up for next frame
            CameraSystem.UpdateCameraTarget(this._components, camera);
            CollisionSystem.ResetCollisions(ref this._components);
            this._components.InvokeDelayedActions();

            return this;
        }

        public void DrawUI(SpriteBatch spriteBatch, Camera camera)
        {
            //Nothing here yet.
        }
    }
}
