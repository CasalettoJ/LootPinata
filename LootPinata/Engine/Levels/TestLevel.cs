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

namespace LootPinata.Engine.Levels
{
    public class TestLevel : ILevel
    {
        private Texture2D _tileSheet;
        private SpriteFont _labelFont;
        private ECSContainer _components;

        public void DrawContent(SpriteBatch spriteBatch, Camera camera)
        {
            int playerId = this._components.Entities.Where(c => c.HasComponents(ComponentFlags.IS_PLAYER)).FirstOrDefault().Id;
            // Draw Sprites
            this._components.Entities.ForEach((c) => 
            {
                if (c.HasDrawableSprite())
                {
                    DisplaySystem.DisplayEntity(spriteBatch, camera, this._components.Displays[c.Id], this._components.Positions[c.Id], this._tileSheet);
                }
            });

            //Draw Labels
            this._components.Entities.ForEach((c) =>
            {
                if (c.HasDrawableLabel())
                {
                    DisplaySystem.DisplayLabel(spriteBatch, camera, this._components.Displays[c.Id], this._components.Labels[c.Id], this._components.Positions[c.Id], _labelFont, this._components.Positions[playerId], this._components.Displays[playerId]);
                }
            });
        }

        public void LoadLevel(ContentManager content, Camera camera)
        {
            _labelFont = content.Load<SpriteFont>(Constants.Fonts.TelegramaSmall);
            _tileSheet = content.Load<Texture2D>(Constants.Sprites.Shieldbun);
            this._components = new ECSContainer();

            #region Debug Creation
            int playerId = this._components.CreateEntity(ComponentFlags.IS_PLAYER, ComponentFlags.MOVEMENT, ComponentFlags.POSITION, ComponentFlags.DISPLAY);
            this._components.Movements[playerId] = new Movement() { BaseVelocity = 300, Velocity = 300, MovementType = MovementType.INPUT };
            this._components.Positions[playerId] = new Position() { OriginPosition = new Vector2(16, 16), TileHeight = 32, TileWidth = 32 };
            this._components.Displays[playerId] = new Display()
            {
                Color = Color.White,
                Layer = DisplayLayer.FOREGROUND,
                LayerDepth = 0,
                Opacity = 1f,
                Origin = new Vector2(16, 16),
                Rotation = 0f,
                Scale = 1f,
                SpriteEffect = SpriteEffects.None,
                SpriteSource = new Rectangle(0, 0, 49, 49)
            };

            int id = this._components.CreateEntity( ComponentFlags.LABEL, ComponentFlags.POSITION, ComponentFlags.DISPLAY);
            this._components.Positions[id] = new Position() { OriginPosition = new Vector2(16, 16), TileHeight = 32, TileWidth = 32 };
            this._components.Displays[id] = new Display()
            {
                Color = Color.White,
                Layer = DisplayLayer.FOREGROUND,
                LayerDepth = 0,
                Opacity = 1f,
                Origin = new Vector2(16, 16),
                Rotation = 0f,
                Scale = 1f,
                SpriteEffect = SpriteEffects.None,
                SpriteSource = new Rectangle(0, 0, 49, 49)
            };
            this._components.Labels[id] = new Label()
            {
                Color = Color.Black,
                Displacement = new Vector2(0, -5),
                Origin = new Vector2(16, 16),
                DistanceRenderBuffer = 100,
                Rotation = 0f,
                Scale = 1f,
                SpriteEffect = SpriteEffects.None,
                Text = "Pssst, come here kid!",
                WhenToShow = WhenToShowLabel.PLAYER_CLOSE
            };

            id = this._components.CreateEntity(ComponentFlags.LABEL, ComponentFlags.POSITION, ComponentFlags.DISPLAY);
            this._components.Positions[id] = new Position() { OriginPosition = new Vector2(26, 600), TileHeight = 32, TileWidth = 32 };
            this._components.Displays[id] = new Display()
            {
                Color = Color.White,
                Layer = DisplayLayer.FOREGROUND,
                LayerDepth = 0,
                Opacity = 1f,
                Origin = new Vector2(16, 16),
                Rotation = 0f,
                Scale = 3f,
                SpriteEffect = SpriteEffects.None,
                SpriteSource = new Rectangle(0, 0, 49, 49)
            };
            this._components.Labels[id] = new Label()
            {
                Color = Color.Black,
                Displacement = new Vector2(0, -20),
                Origin = new Vector2(16, 16),
                DistanceRenderBuffer = 100,
                Rotation = 25f,
                Scale = 3f,
                SpriteEffect = SpriteEffects.None,
                Text = "Big nyan",
                WhenToShow = WhenToShowLabel.PLAYER_CLOSE
            };

            id = this._components.CreateEntity(ComponentFlags.LABEL, ComponentFlags.POSITION, ComponentFlags.DISPLAY);
            this._components.Positions[id] = new Position() { OriginPosition = new Vector2(500, 10), TileHeight = 32, TileWidth = 32 };
            this._components.Displays[id] = new Display()
            {
                Color = Color.White,
                Layer = DisplayLayer.FOREGROUND,
                LayerDepth = 0,
                Opacity = 1f,
                Origin = new Vector2(16, 16),
                Rotation = 0f,
                Scale = 1f,
                SpriteEffect = SpriteEffects.None,
                SpriteSource = new Rectangle(0, 0, 49, 49)
            };
            this._components.Labels[id] = new Label()
            {
                Color = Color.Black,
                Displacement = new Vector2(0, -5),
                Origin = new Vector2(16, 16),
                DistanceRenderBuffer = 100,
                Rotation = 0f,
                Scale = 1f,
                SpriteEffect = SpriteEffects.None,
                Text = "nyan",
                WhenToShow = WhenToShowLabel.PLAYER_FAR
            };

            camera.TargetEntity = this._components.Entities[playerId].Id;
            #endregion
        }

        public ILevel Update(GameTime gameTime, Camera camera, KeyboardState currentKey, KeyboardState prevKey, MouseState currentMouse, MouseState prevMouse)
        {
            // Level input
            if (currentKey.IsKeyDown(Keys.Escape))
            {
                return null;
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
