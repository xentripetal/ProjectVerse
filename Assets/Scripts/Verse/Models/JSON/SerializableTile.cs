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

    public class SerializableThing : SerializableTile {
        public SerializableThing(string definition, TilePosition position) : base(definition, position) { }

        public SerializableThing() { }

        public TileObject ToTileObject(Room room) {
            return new TileObjectActual(TileAtlas.GetTileObjectDef(Definition), Position, room);
        }

        static public implicit operator SerializableThing(TileObject value) {
            return new SerializableThing(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableScriptableThing : SerializableThing {
        public List<IThingData> Datasets;

        public SerializableScriptableThing(string definition, TilePosition position, List<IThingData> datasets) : base(
            definition, position) {
            Datasets = datasets;
        }

        public SerializableScriptableThing() { }

        public TileObjectEntity ToScriptableTileObject(Room room) {
            return new TileObjectEntityActual(TileAtlas.GetScriptableTileObjectDef(Definition), Position, room,
                Datasets);
        }

        static public implicit operator SerializableScriptableThing(TileObjectEntity value) {
            return new SerializableScriptableThing(value.Definition.FullName, value.Position, value.Datasets);
        }
    }
}