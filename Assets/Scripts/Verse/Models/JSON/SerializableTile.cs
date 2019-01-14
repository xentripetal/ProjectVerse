using System.Collections.Generic;
using Verse.API.Interfaces;

namespace Verse.API.Models.JSON {
    public class SerializableTile {
        public string Definition;
        public TilePosition Position;

        public SerializableTile(string definition, TilePosition position) {
            Definition = definition;
            Position = position;
        }

        public SerializableTile() { }

        public Tile ToTile(Room room) {
            return new TileActual(TileAtlas.GetTileDef(Definition), Position, room);
        }

        static public implicit operator SerializableTile(Tile value) {
            return new SerializableTile(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableTileObject : SerializableTile {
        public SerializableTileObject(string definition, TilePosition position) : base(definition, position) { }

        public SerializableTileObject() { }

        public TileObject ToTileObject(Room room) {
            return new TileObjectActual(TileAtlas.GetTileObjectDef(Definition), Position, room);
        }

        static public implicit operator SerializableTileObject(TileObject value) {
            return new SerializableTileObject(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableTileObjectEntity : SerializableTileObject {
        public List<IThingData> Datasets;

        public SerializableTileObjectEntity(string definition, TilePosition position, List<IThingData> datasets) : base(
            definition, position) {
            Datasets = datasets;
        }

        public SerializableTileObjectEntity() { }

        public TileObjectEntity ToScriptableTileObject(Room room) {
            return new TileObjectEntityActual(TileAtlas.GetScriptableTileObjectDef(Definition), Position, room,
                Datasets);
        }

        static public implicit operator SerializableTileObjectEntity(TileObjectEntity value) {
            return new SerializableTileObjectEntity(value.Definition.FullName, value.Position, value.Datasets);
        }
    }
}