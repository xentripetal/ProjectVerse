using UnityEngine;

namespace Verse.API.Models {
    /// <summary>
    /// Tiles are the core component to all physical objects in the Verse. A standard tile is simply a
    /// terrain/ground asset that the player walks on top of.
    /// </summary>
    public class TileDef {
        public string Name { get; protected set; }
        public ModPackage ModPackage { get; protected set; }
        public Vector2Int[] OccupiedPositions { get; protected set; }
        public Sprite Sprite { get; protected set; }
        public bool CanBuildOn { get; protected set; }
        public bool HasCollision { get; protected set; }
        public bool HasTransparencyOnPlayerBehind { get; protected set; }
        public TileEntity TileEntityDefault { get; protected set; }

        public TileDef(string name, ModPackage modPackage, Vector2Int[] occupiedPositions, Sprite sprite,
            bool canBuildOn, bool hasCollision, bool hasTransparencyOnPlayerBehind, TileEntity entityType) {
            Name = name;
            ModPackage = modPackage;
            OccupiedPositions = occupiedPositions;
            Sprite = sprite;
            CanBuildOn = canBuildOn;
            HasCollision = hasCollision;
            HasTransparencyOnPlayerBehind = hasTransparencyOnPlayerBehind;
            TileEntityDefault = entityType;
        }
    }
}