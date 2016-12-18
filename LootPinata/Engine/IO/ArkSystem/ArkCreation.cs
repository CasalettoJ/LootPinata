using LootPinata.Engine.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LootPinata.Engine.IO.ArkSystem
{
    public static class ArkCreation
    {
        public static int CreateEntityFromFile(string xmlFilePath, ref ECSContainer ecsContainer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BaseEntity));
            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                BaseEntity entity = (BaseEntity)serializer.Deserialize(fileStream);
                return ecsContainer.AddEntity(entity);
            }
        }
    }
}
