using System;

[Serializable]
public class TileJson {
    public string name;
    public int X;
    public int Y;
}

[Serializable]
public class TileMap {
    public TileJson[] Tiles;
}