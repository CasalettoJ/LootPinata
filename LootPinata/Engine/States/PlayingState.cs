using LootPinata.Engine.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.States
{
    public class PlayingState : IState
    {
        private IState _previousState;
        private ILevel _currentLevel;
        private ContentManager _content;

        public PlayingState(ContentManager content, Camera camera, ILevel level, IState previous = null)
        {
            this._previousState = previous;
            this._content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            this.SetLevel(level, camera);
        }

        public void DrawContent(SpriteBatch spriteBatch, Camera camera)
        {
            this._currentLevel.DrawContent(spriteBatch, camera);
        }

        public void SetLevel(ILevel level, Camera camera)
        {
            if (this._content != null && level != null)
            {
                this._content.Unload();
                this._currentLevel = level;
                this._currentLevel.LoadLevel(this._content, camera);
            }
        }

        public IState UpdateState(GameTime gameTime, Camera camera, KeyboardState currentKey, KeyboardState prevKey, MouseState currentMouse, MouseState prevMouse)
        {
            ILevel nextLevel = this._currentLevel;
            nextLevel = this._currentLevel.Update(gameTime, camera, currentKey, prevKey, currentMouse, prevMouse);

            if (nextLevel != this._currentLevel && nextLevel != null)
            {
                this.SetLevel(nextLevel, camera);
            }
            else if (nextLevel == null)
            {
                return _previousState;
            }

            return this;

        }

        public void DrawUI(SpriteBatch spriteBatch, Camera camera)
        {
            this._currentLevel.DrawUI(spriteBatch, camera);
        }
    }
}
