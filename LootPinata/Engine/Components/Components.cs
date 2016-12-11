using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Components
{
    #region Component Boilerplate
    public enum ComponentFlags : int
    {
        IS_PLAYER = 0,
        POSITION = 1,
        LABEL = 2,
        DISPLAY = 3,
        MOVEMENT = 4
    }

    public class ECSContainer
    {
        public ECSContainer()
        {
            this.EntityCount = 0;
            this.Entities = new List<Entity>();
            this.Positions = new Dictionary<int, Position>();
            this.Labels = new Dictionary<int, Label>();
            this.Displays = new Dictionary<int, Display>();
            this.Movements = new Dictionary<int, Movement>();
        }

        // Entities
        public int EntityCount { get; private set; }
        public List<Entity> Entities { get; private set; }

        // Component Arrays
        public Dictionary<int,Position> Positions { get; private set; }
        public Dictionary<int, Label> Labels { get; private set; }
        public Dictionary<int, Display> Displays { get; private set; }
        public Dictionary<int, Movement> Movements { get; private set; }

        public int CreateEntity(params ComponentFlags[] flags)
        {
            this.Entities.Insert(this.EntityCount, new Entity(this.EntityCount, flags));
            return this.EntityCount++;
        }

        public void DestroyEntity(int entityId)
        {
            this.Entities.RemoveAt(entityId);
            this.Positions.Remove(entityId);
            this.Labels.Remove(entityId);
            this.Displays.Remove(entityId);
            this.Movements.Remove(entityId);
            this.EntityCount -= 1;
        }


    }
    #endregion

    #region Components
    public class Position
    {
        public Vector2 OriginPosition { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
    }

    public enum WhenToShowLabel
    {
        ALWAYS,
        PLAYER_CLOSE,
        PLAYER_FAR,
        NEVER
    }

    public class Label
    {
        public string Text;
        public float Scale;
        public Vector2 Origin;
        public Vector2 Displacement;
        public Color Color;
        public SpriteEffects SpriteEffect;
        public float Rotation;
        public WhenToShowLabel WhenToShow;
        public int DistanceRenderBuffer;
    }

    public enum DisplayLayer
    {
        BACKGROUND,
        FLOOR,
        FOREGROUND,
        SUPER
    }

    public class Display
    {
        public Rectangle SpriteSource;
        public Color Color;
        public float Scale;
        public Vector2 Origin;
        public SpriteEffects SpriteEffect;
        public float Rotation;
        public float Opacity;
        public DisplayLayer Layer;
        public int LayerDepth;
    }

    public enum MovementType
    {
        NONE,
        INPUT,
        AI
    }

    public class Movement
    {
        public double BaseVelocity;
        public double Velocity;
        public MovementType MovementType;
    } 
    #endregion
}
