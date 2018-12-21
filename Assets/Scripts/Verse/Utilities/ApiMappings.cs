using System;
using UnityEngine;
using Verse.API.Models;

namespace Verse.Utilities {
    public static class ApiMappings {
        public static Vector2 Vector2FromTilePosition(Position value) {
            return new Vector2(value.x, value.y);
        }

        public static Position Vector2ToTilePosition(Vector2 value) {
            return new Position(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
        }

        public static Vector3 Vector3FromTilePosition(Position value) {
            return new Vector3(value.x, value.y, 0);
        }

        public static Position Vector3ToTilePosition(Vector3 value) {
            return new Position(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
        }
        
        public static Vector2 Vector2FromPosition(PlayerPosition value) {
            return new Vector2(value.x, value.y);
        }

        public static PlayerPosition Vector2ToPosition(Vector2 value) {
            return new PlayerPosition(value.x, value.y);
        }

        public static Vector3 Vector3FromPosition(PlayerPosition value) {
            return new Vector3(value.x, value.y, 0);
        }

        public static PlayerPosition Vector3ToPosition(Vector3 value) {
            return new PlayerPosition(value.x, value.y);
        }
        
        public static Sprite InfoToSprite(SpriteInfo info) {
            Texture2D image = Resources.Load<Texture2D>(info.SpritePath);
            Rect rect = new Rect(0, 0, image.width, image.height);
            Sprite sprite = Sprite.Create(image, rect, Vector2FromPosition(info.PivotPoint), info.PixelsPerUnit, uint.MinValue,
                SpriteMeshType.Tight);

            return sprite;
        }
    }
}