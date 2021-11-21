
using Newtonsoft.Json;
using UnityEngine;
using Verse.Utilities;

namespace Verse.API.Models {
    public class SpriteInfo {
        /// <value>A list of points for the collider shape. Can be null</value>
        public readonly Position[] ColliderShape;

        /// <value>The position of the pivot. Should normally be the bottom left corner (0,0).</value>
        public readonly Position PivotPoint;

        /// <value>The pixels per unit of the tile. Should normally be 32.</value>
        public readonly int PixelsPerUnit;

        
        [JsonIgnore] public readonly Sprite sprite;

        /// <value>The file path of the image resource for the Tile. It can contain directories.</value>
        public readonly string SpritePath;

        /// <value>A list of points for detecting when the player is behind the defined shape. Can be null</value>
        public readonly Position[] TransparencyShape;

        public SpriteInfo(string spritePath, int pixelsPerUnit, Position pivotPoint, Position[] colliderShape,
            Position[] transparencyShape) {
            SpritePath = spritePath;
            PixelsPerUnit = pixelsPerUnit;
            PivotPoint = pivotPoint;
            ColliderShape = colliderShape;
            TransparencyShape = transparencyShape;
            sprite = ApiMappings.InfoToSprite(this);
        }
    }
}