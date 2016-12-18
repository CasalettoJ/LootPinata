using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata.Engine.Components
{
    public class BaseEntity
    {
        public List<ComponentFlags> Flags;
        public Movement Movement;
        public Position Position;
        public Display Display;
        public Label Label;
    }
}
