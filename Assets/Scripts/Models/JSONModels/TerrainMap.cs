using System;
using System.Collections.Generic;
using API.Models;
using Newtonsoft.Json;

public class SerializableTile{
    public string Definition;
    public Position Position;
}

public class BoxCollider {
    public Position Position;
    public Position Size;
}

public class Colliders {
    public IList<Position> EdgePoints;
    public IList<BoxCollider> BoxColliders;

    public Colliders(IList<Position> edgePoints, IList<BoxCollider> boxColliders) {
        EdgePoints = edgePoints;
        BoxColliders = boxColliders;
    }
}

[JsonObject(MemberSerialization.OptIn)]
public class TerrainMap {
    public Colliders Colliders;
    public IList<SerializableTile> Tiles;
    
    public TerrainMap(Colliders colliders, IList<SerializableTile> tiles) {
        Colliders = colliders;
        Tiles = tiles;
    }
}