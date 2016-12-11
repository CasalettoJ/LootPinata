using LootPinata.Engine.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.States
{
    public interface IState
    {
        IState UpdateState(GameTime gameTime, Camera camera, KeyboardState currentKey, KeyboardState prevKey, MouseState currentMouse, MouseState prevMouse);
        void DrawContent(SpriteBatch spriteBatch, Camera camera);
        void DrawUI(SpriteBatch spriteBatch, Camera camera);
        void SetLevel(ILevel level, Camera camera);
    }
}
