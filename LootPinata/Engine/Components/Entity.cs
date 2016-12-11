using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Components
{
    public class Entity
    {
        public Entity(int id, params ComponentFlags[] flags)
        {
            this.Id = id;
            this.ComponentFlags = new BitArray(Enum.GetNames(typeof(ComponentFlags)).Length);

            foreach (ComponentFlags flag in flags)
            {
                this.ComponentFlags[(int)flag] = true;
            }
        }

        public int Id { get; }
        public BitArray ComponentFlags { get; }
    }

    // Extension Methods take place of Bit Masking
    public static class EntityExtensions
    {
        public static void AddComponentFlags(this Entity e, params ComponentFlags[] flags)
        {
            foreach (ComponentFlags flag in flags)
            {
                e.ComponentFlags[(int)flag] = true;
            }
        }

        public static void RemoveComponentFlags(this Entity e, params ComponentFlags[] flags)
        {
            foreach (ComponentFlags flag in flags)
            {
                e.ComponentFlags[(int)flag] = false;
            }
        }

        public static bool HasComponents(this Entity e, params ComponentFlags[] flags)
        {
            foreach (ComponentFlags flag in flags)
            {
                if (!e.ComponentFlags[(int)flag])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HasDrawableSprite(this Entity e)
        {
            return (e.ComponentFlags[(int)ComponentFlags.DISPLAY] && e.ComponentFlags[(int)ComponentFlags.POSITION]);
        }

        public static bool HasDrawableLabel(this Entity e)
        {
            return (e.ComponentFlags[(int)ComponentFlags.LABEL] && e.ComponentFlags[(int)ComponentFlags.POSITION]);
        }

        public static bool IsMovable(this Entity e)
        {
            return (e.ComponentFlags[(int)ComponentFlags.MOVEMENT] && e.ComponentFlags[(int)ComponentFlags.POSITION]);
        }
    }
}
