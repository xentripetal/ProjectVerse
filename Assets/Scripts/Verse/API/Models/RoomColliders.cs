using System.Collections.Generic;

namespace Verse.API.Models {
    public class BoxColliderInfo {
        public Position Position;
        public Position Size;
    }

    public class RoomColliders {
        public List<BoxColliderInfo> BoxColliders;
        public List<Position> EdgePoints;

        public RoomColliders(List<Position> edgePoints, List<BoxColliderInfo> boxColliders) {
            EdgePoints = edgePoints;
            BoxColliders = boxColliders;
        }
    }
}