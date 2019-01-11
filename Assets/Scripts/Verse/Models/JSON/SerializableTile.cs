using System.Collections.Generic;
using Verse.API.Interfaces;
using Verse.API.Models;

namespace Verse.API.Models.JSON {
    public class SerializableTile {
        public string Definition;
        public TilePosition Position;

        public SerializableTile(string definition, TilePosition position) {
            Definition = definition;
            Position = position;
        }

        public SerializableTile() { }

        static public implicit operator Tile(SerializableTile value) {
            return new Tile(ObjectAtlas.GetTileDef(value.Definition), value.Position);
        }

        static public implicit operator SerializableTile(Tile value) {
            return new SerializableTile(value.Definition.FullName, value.TilePosition);
        }
    }

    public class SerializableThing : SerializableTile {
        public SerializableThing(string definition, TilePosition position) : base(definition, position) { }

        public SerializableThing() { }

        static public implicit operator TileObject(SerializableThing value) {
            return new TileObject(ObjectAtlas.GetThingDef(value.Definition), value.Position);
        }

        static public implicit operator SerializableThing(TileObject value) {
            return new SerializableThing(value.Definition.FullName, value.TilePosition);
        }
    }

    public class SerializableScriptableThing : SerializableThing {
        public IList<IThingData> Datasets;

        public SerializableScriptableThing(string definition, TilePosition position, IList<IThingData> datasets) : base(
            definition, position) {
            Datasets = datasets;
        }

        public SerializableScriptableThing() { }

        static public implicit operator ScriptableTileObject(SerializableScriptableThing value) {
            return new ScriptableTileObject(ObjectAtlas.GetScriptableThingDef(value.Definition), value.Position,
                value.Datasets);
        }

        static public implicit operator SerializableScriptableThing(ScriptableTileObject value) {
            return new SerializableScriptableThing(value.Definition.FullName, value.TilePosition, value.Datasets);
        }
    }
}