using LootPinata.Engine.IO.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Menus.States
{
    public interface IState
    {
        IState UpdateState(ref GameSettings gameSettings, GameTime gameTime, Camera camera, KeyboardState currentKey, KeyboardState prevKey, MouseState currentMouse, MouseState prevMouse);
        void DrawContent(SpriteBatch spriteBatch, Camera camera);
        void DrawUI(SpriteBatch spriteBatch, Camera camera);
    }
}
