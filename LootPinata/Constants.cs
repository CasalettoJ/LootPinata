using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootPinata
{
    public static class Constants
    {
        public static class Fonts
        {
            public static readonly string FontFolder = @"Fonts/";
            public static readonly string TelegramaSmall = FontFolder + "TelegramaSmall";
        }

        public static class Sprites
        {
            public static readonly string SpriteFolder = @"Sprites/";
            public static readonly string Placeholder = SpriteFolder + "placeholder";
            public static readonly string Shieldbun = SpriteFolder + "shieldbunsprite";
        }

        public static class IO
        {
            public static class GameSettings
            {
                public const string SettingsDirectory = @"Settings/";
                public const string CurrentSettings = SettingsDirectory + "Config.xml";
                public const string DefaultGameSettings = SettingsDirectory + "DefaultConfig.xml";
            }
        }
    }
}
