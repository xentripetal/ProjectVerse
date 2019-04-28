using UnityEngine;

namespace Verse.Core.TileLayers {
    public sealed class GroundTileLayer : TileLayer {
        public GroundTileLayer() {
            Name = "Ground";
            SortingOrder = 0;
        }

        public override string Name { get; protected set; }
        public override int SortingOrder { get; protected set; }

        public override Vector3 TilePositionToVisualPosition(Vector2Int pos) {
            return new Vector3(pos.x, pos.y, 0);
        }
    }
}