namespace Verse.API.Models {
    public class SpriteInfo {
        /// <value>The file path of the image resource for the Tile. It can contain directories.</value>
        public readonly string SpritePath;

        /// <value>The pixels per unit of the tile. Should normally be 32.</value>
        public readonly int PixelsPerUnit;

        /// <value>The position of the pivot. Should normally be the bottom left corner (0,0).</value>
        public readonly PlayerPosition PivotPoint;

        /// <value>A list of points for the collider shape. Can be null</value>
        public readonly PlayerPosition[] ColliderShape;

        /// <value>A list of points for detecting when the player is behind the defined shape. Can be null</value>
        public readonly PlayerPosition[] TransparencyShape;

        public SpriteInfo(string spritePath, int pixelsPerUnit, PlayerPosition pivotPoint, PlayerPosition[] colliderShape,
            PlayerPosition[] transparencyShape) {
            SpritePath = spritePath;
            PixelsPerUnit = pixelsPerUnit;
            PivotPoint = pivotPoint;
            ColliderShape = colliderShape;
            TransparencyShape = transparencyShape;
        }
    }
}