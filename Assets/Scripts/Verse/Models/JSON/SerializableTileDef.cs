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

        public TileUnifiedDef ToTileUnifiedDef(ModPackage modPackage) {
            var sprite = Resources.Load<Sprite>(SpriteKey);
            return new TileUnifiedDef(Name, modPackage, OccupiedPositions, sprite, CanBuildOn, HasCollision,
                HasTransparencyOnPlayerBehind, TileEntityDefaults);
        }
    }
}