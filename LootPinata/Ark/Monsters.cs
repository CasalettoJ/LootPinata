using LootPinata.Engine.Components;
using LootPinata.Engine.IO.ArkSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LootPinata.Ark.Monsters
{
    public static class Spawners
    {
        public static int SpawnPlayer(ref ECSContainer ecsContainer, Position position = null)
        {

            int playerId = ArkCreation.CreateEntityFromFile(Constants.Ark.Monsters.Player, ref ecsContainer);
            if (position != null)
            {
                ecsContainer.Entities.Where(x => x.Id == playerId).First().AddComponentFlags(ComponentFlags.POSITION);
                ecsContainer.Positions.Add(playerId, position);
            }
            return playerId;
        }

        public static int SpawnTestNpc(ref ECSContainer ecsContainer, Position position = null)
        {

            int id = ArkCreation.CreateEntityFromFile(Constants.Ark.Monsters.TestNpc, ref ecsContainer);
            if (position != null)
            {
                ecsContainer.Entities.Where(x => x.Id == id).First().AddComponentFlags(ComponentFlags.POSITION);
                ecsContainer.Positions.Add(id, position);
            }
            return id;
        }
    }
}
