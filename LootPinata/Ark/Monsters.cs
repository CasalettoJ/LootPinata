using LootPinata.Engine.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LootPinata.Ark.Monsters
{
    public class Player
    {
        public List<ComponentFlags> Flags { get; set; } = new List<ComponentFlags>();
        public Display Display;
        public Movement Movement;
    }
}
