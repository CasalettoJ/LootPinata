﻿using Microsoft.Xna.Framework;
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
        }

        // Entities
        public int EntityCount { get; private set; } = 0;
        public List<Entity> Entities { get; private set; } = new List<Entity>();

        // Component Arrays
        public Dictionary<Type, Dictionary<int, dynamic>> Components = new Dictionary<Type, Dictionary<int, dynamic>>()
        {
            {typeof(Position), new Dictionary<int,dynamic>() },
            {typeof(Label), new Dictionary<int,dynamic>() },
            {typeof(Display), new Dictionary<int,dynamic>() },
            {typeof(Movement), new Dictionary<int,dynamic>() }
        };

        // Manager Properties
        public List<Action> DelayedActions { get; private set; } = new List<Action>();

        public int CreateEntity(params ComponentFlags[] flags)
        {
            this.Entities.Insert(this.EntityCount, new Entity(this.EntityCount, flags));
            return this.EntityCount++;
        }

        public void AddComponent<T>(int entityId, T component)
        {
            Components[component.GetType()].Add(entityId, component);
        }

        public T FindComponent<T>(int entityId)
        {
            if(Components[typeof(T)].ContainsKey(entityId))
            {
                return Components[typeof(T)][entityId];
            }
            return default(T);
        }

        public void DestroyEntity(int entityId)
        {
            this.Entities.RemoveAt(entityId);
            foreach(var item in Components)
            {
                item.Value.Remove(entityId);
            }
            this.EntityCount -= 1;
        }

        public void InvokeDelayedActions()
        {
            foreach (Action action in this.DelayedActions)
            {
                action();
            }
            this.DelayedActions.Clear();
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
