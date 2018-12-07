using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.API.Models;

namespace UI {
    public class UITileData {
        public TileType Type;
        public string name;
        public int PPU;
        public string SpriteLocation;
        public Sprite Sprite;
        public Vector2 PivotPoint;
        public bool HasCollider;
        public bool IsTrigger;
        public bool HasTransparency;
        public List<Vector2> ColliderShape;
        public List<Vector2> TransparencyShape;
        public List<IThingScript> Scripts;

        public static implicit operator UITileData(TileDef value) {
            var data = new UITileData();
            data.Type = TileType.Tile;
            data.name = value.FullName;
            data.PPU = value.SpriteInfo.PixelsPerUnit;
            data.SpriteLocation = value.SpriteInfo.SpritePath;
            data.PivotPoint = value.SpriteInfo.PivotPoint;
            data.Sprite = Utils.InfoToSprite(value.SpriteInfo);
            return data;
        }
        
        public static implicit operator UITileData(ThingDef value) {
            UITileData data = (TileDef) value;
            data.Type = TileType.Thing;
            data.HasCollider = value.IsCollidable;
            data.HasTransparency = value.IsTransparentOnPlayerBehind;
            if (value.SpriteInfo.ColliderShape != null) {
                data.ColliderShape = value.SpriteInfo.ColliderShape.Select(pos => (Vector2) pos).ToList();
            }

            if (value.SpriteInfo.TransparencyShape != null) {
                data.TransparencyShape = value.SpriteInfo.TransparencyShape.Select(pos => (Vector2) pos).ToList();
            }

            return data;
        }

        public static implicit operator UITileData(ScriptableThingDef value) {
            UITileData data = (ThingDef) value;
            data.Type = TileType.ScriptableThing;
            data.IsTrigger = value.IsTrigger;
            data.Scripts = value.Scripts.ToList();
            return data;
        }
        
    }
}