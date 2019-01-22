using UnityEngine;

namespace Verse.API.Models.JSON {
    public class SerializableTileDef {
        public string Name;
        public Vector2Int[] OccupiedPositions;
        public string SpriteKey;
        public bool CanBuildOn;
        public bool HasCollision;
        public bool HasTransparencyOnPlayerBehind;
        public TileEntity TileEntityDefaults;

        public TileDef ToTileUnifiedDef(ModPackage modPackage) {
            var sprite = Resources.Load<Sprite>(SpriteKey);
            return new TileDef(Name, modPackage, OccupiedPositions, sprite, CanBuildOn, HasCollision,
                HasTransparencyOnPlayerBehind, TileEntityDefaults);
        }
    }
}