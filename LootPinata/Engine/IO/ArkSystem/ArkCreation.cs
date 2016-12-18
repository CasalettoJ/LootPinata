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
        public static T CreateEntityFromFile<T>(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(fileStream);
            }
        }
    }
}
