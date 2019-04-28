using UnityEngine;
using Verse.API.Models;

namespace Verse.Utilities {
    public static class ApiMappings {
        public static Vector2 Vector2FromTilePosition(TilePosition value) {
            return new Vector2(value.x, value.y);
        }

        public static TilePosition Vector2ToTilePosition(Vector2 value) {
            return new TilePosition(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
        }

        public static Vector3 Vector3FromTilePosition(TilePosition value) {
            return new Vector3(value.x, value.y, 0);
        }

        public static TilePosition Vector3ToTilePosition(Vector3 value) {
            return new TilePosition(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
        }

        public static Vector2 Vector2FromPosition(Position value) {
            return new Vector2(value.x, value.y);
        }

        public static Position Vector2ToPosition(Vector2 value) {
            return new Position(value.x, value.y);
        }

        public static Vector3 Vector3FromPosition(Position value) {
            return new Vector3(value.x, value.y, 0);
        }

        public static Position Vector3ToPosition(Vector3 value) {
            return new Position(value.x, value.y);
        }

        public static Sprite InfoToSprite(SpriteInfo info) {
            var image = Resources.Load<Texture2D>(info.SpritePath);
            var rect = new Rect(0, 0, image.width, image.height);
            var sprite = Sprite.Create(image, rect, Vector2FromPosition(info.PivotPoint), info.PixelsPerUnit,
                uint.MinValue,
                SpriteMeshType.Tight);

            return sprite;
        }
    }
}