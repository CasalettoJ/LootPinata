using LootPinata.Engine;
using LootPinata.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootPinata
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LootPinata : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _prevKey;
        private MouseState _prevMouse;
        private Camera _camera;
        private IState _currentState;

        public LootPinata()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this._spriteBatch = new SpriteBatch(GraphicsDevice);
            this.IsMouseVisible = false;
            this.Window.IsBorderless = false;
            this.Window.AllowUserResizing = false;
            this._currentState = new TitleState(Content);
            this._graphics.PreferredBackBufferWidth = 800;
            this._graphics.PreferredBackBufferHeight = 600;
            this._graphics.ApplyChanges();
            this._camera = new Camera(GraphicsDevice.Viewport, GraphicsDevice.Viewport.Bounds.Center.ToVector2(), 0f, 1f);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                this._camera.CurrentMatrix = this._camera.GetMatrix();
                this._camera.CurrentInverseMatrix = this._camera.GetInverseMatrix();
                KeyboardState currentKey = Keyboard.GetState();
                MouseState currentMouse = Mouse.GetState();
                this._currentState = this._currentState.UpdateState(gameTime, this._camera, currentKey, this._prevKey, currentMouse, this._prevMouse);
                this._prevKey = currentKey;
                this._prevMouse = currentMouse;

                if (this._currentState == null)
                {
                    this.Exit();
                }

                base.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Draw Entities
            this._spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: this._camera.CurrentMatrix);
            this._currentState.DrawContent(this._spriteBatch, this._camera);
            this._spriteBatch.End();

            // Draw UI
            this._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            this._currentState.DrawUI(this._spriteBatch, this._camera);
            this._spriteBatch.End();

            // Draw Debug
            //this._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //this._spriteBatch.DrawString(this._debugText, "FPS: " + Math.Round((1 / (decimal)gameTime.ElapsedGameTime.TotalSeconds), 2).ToString(), new Vector2(25, 25), Color.Yellow);
            //this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
