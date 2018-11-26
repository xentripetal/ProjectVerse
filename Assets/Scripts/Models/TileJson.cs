using System;

[Serializable]
public class TileJson {
    public string name;
    public int X;
    public int Y;
    public IThingData[] datasets;
}

[Serializable]
public class TileMap {
    public TileJson[] Tiles;
}