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
            int playerId = ArkCreation.SpawnEntityWithOverrides(Constants.Ark.Monsters.Player, ref this._components, new BaseEntity(ComponentFlags.POSITION) {Position= new Position() { OriginPosition = new Vector2(0, 0) } });
            ArkCreation.SpawnEntityWithOverrides(Constants.Ark.Monsters.TestNpc, ref this._components, new BaseEntity(ComponentFlags.POSITION) { Position = new Position() { OriginPosition = new Vector2(0, 0) } });
            camera.TargetEntity = this._components.Entities[playerId].Id;
            #endregion
        }

        public void DrawContent(SpriteBatch spriteBatch, Camera camera)
        {
            int playerId = this._components.Entities.Where(c => c.HasComponents(ComponentFlags.IS_PLAYER)).FirstOrDefault().Id;
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
            // Level input
            if (currentKey.IsKeyDown(Keys.Escape) && prevKey.IsKeyUp(Keys.Escape))
            {
                return new PauseState(this._content, this);
            }

            if (currentKey.IsKeyDown(Keys.F) && prevKey.IsKeyUp(Keys.F))
            {
                for(int i = 0; i < 1001; i++)
                {
                    int playerId = ArkCreation.CreateEntityFromFile(Constants.Ark.Monsters.Player, ref this._components);
                    this._components.Entities.Where(x => x.Id == playerId).First().AddComponentFlags(ComponentFlags.POSITION);
                    this._components.Positions.Add(playerId, new Position() { OriginPosition = new Vector2(0, 16)});
                }
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
                    }
                }
            });

            // Entity Information Updates

            // Set up for next frame
            CameraSystem.UpdateCameraTarget(this._components, camera);

            return this;
        }

        public void DrawUI(SpriteBatch spriteBatch, Camera camera)
        {
            //Nothing here yet.
        }
    }
}
