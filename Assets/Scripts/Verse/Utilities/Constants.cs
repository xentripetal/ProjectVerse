using UnityEngine;

namespace Verse.Utilities {
    public static class Constants {
        public const float ZPositionMultiplier = .01f;
        public const float ZPositionOffset = -10f;
        public static string ContentFolder = Application.dataPath + "/Resources/";
        public static string RoomsFolder = ContentFolder + "Rooms/";
        public static string SpritesFolder = ContentFolder + "Sprites/";
        public static string DefsFolder = ContentFolder + "Defs/";
    }
}