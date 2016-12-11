using LootPinata.Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Systems
{
    public static class DisplaySystem
    {
        public static void DisplayEntity(SpriteBatch spriteBatch, Camera camera, Display displayInfo, Position positionInfo, Texture2D spriteSheet)
        {
            if (positionInfo != null && displayInfo != null && camera.IsInView(GetCameraBounds(positionInfo, camera)))
            {
                spriteBatch.Draw(spriteSheet, positionInfo.OriginPosition, displayInfo.SpriteSource, displayInfo.Color * displayInfo.Opacity, displayInfo.Rotation, displayInfo.Origin, displayInfo.Scale, displayInfo.SpriteEffect, 0f);
            }
        }

        public static void DisplayLabel(SpriteBatch spriteBatch, Camera camera, Display displayInfo, Label labelInfo, Position positionInfo, SpriteFont font, Position playerPosition, Display playerDisplay)
        {
            if (positionInfo != null && displayInfo != null && labelInfo != null && camera.IsInView(GetCameraBounds(positionInfo, camera)))
            {
                int distance = Math.Abs((int)Vector2.Distance(playerPosition.OriginPosition + playerDisplay.Origin, positionInfo.OriginPosition + displayInfo.Origin));
                bool show = false;
                switch (labelInfo.WhenToShow)
                {
                    case WhenToShowLabel.ALWAYS:
                        show = true;
                        break;
                    case WhenToShowLabel.PLAYER_CLOSE:
                        show = distance <= labelInfo.DistanceRenderBuffer;
                        break;
                    case WhenToShowLabel.PLAYER_FAR:
                        show = distance >= labelInfo.DistanceRenderBuffer;
                        break;
                }

                if (show)
                {
                    Vector2 fontSize = font.MeasureString(labelInfo.Text);
                    spriteBatch.DrawString(font, labelInfo.Text, positionInfo.OriginPosition + labelInfo.Displacement - new Vector2(0, fontSize.Y), labelInfo.Color, labelInfo.Rotation, new Vector2(fontSize.X / 2, fontSize.Y / 2), labelInfo.Scale, labelInfo.SpriteEffect, 0f);
                }
            }
        }

        private static Rectangle GetCameraBounds(Position positionInfo, Camera camera)
        {
            Vector2 bottomRight = Vector2.Transform(new Vector2((positionInfo.OriginPosition.X) + positionInfo.TileWidth, (positionInfo.OriginPosition.Y) + positionInfo.TileHeight), camera.CurrentMatrix);
            Vector2 topLeft = Vector2.Transform(new Vector2(positionInfo.OriginPosition.X, positionInfo.OriginPosition.Y), camera.CurrentMatrix);
            Rectangle cameraBounds = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)bottomRight.X - (int)topLeft.X, (int)bottomRight.Y - (int)topLeft.Y);
            return cameraBounds;
        }
    }
}
