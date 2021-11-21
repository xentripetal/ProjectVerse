using System.IO;
using UnityEngine;

namespace Verse.Utilities {
    public static class Constants {
        public const float ZPositionMultiplier = .01f;
        public const float ZPositionOffset = -10f;
        public static string SpritesFolder = Path.Combine(Application.dataPath, "Resources/Sprites");
    }
}