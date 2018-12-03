using System.Collections.Generic;
using Newtonsoft.Json;
using Verse.API.Models;

public class BoxColliderInfo {
    public Position Position;
    public Position Size;
}

public class Colliders {
    public IList<Position> EdgePoints;
    public IList<BoxColliderInfo> BoxColliders;

    public Colliders(IList<Position> edgePoints, IList<BoxColliderInfo> boxColliders) {
        EdgePoints = edgePoints;
        BoxColliders = boxColliders;
    }
}

public class TerrainMap {
    public Colliders Colliders;
    public IList<SerializableTile> Tiles;
    
    public TerrainMap(Colliders colliders, IList<SerializableTile> tiles) {
        Colliders = colliders;
        Tiles = tiles;
    }
}