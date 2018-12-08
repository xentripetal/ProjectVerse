using System.Collections.Generic;
using Verse.API.Interfaces;
using Verse.API.Models;

namespace Verse.Models.JSON {
    public class SerializableTile {
        public string Definition;
        public Position Position;

        public SerializableTile(string definition, Position position) {
            Definition = definition;
            Position = position;
        }

        static public implicit operator Tile(SerializableTile value) {
            return new Tile(ObjectAtlas.GetTileDef(value.Definition), value.Position);
        }

        static public implicit operator SerializableTile(Tile value) {
            return new SerializableTile(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableThing : SerializableTile {
        public SerializableThing(string definition, Position position) : base(definition, position) { }

        static public implicit operator Thing(SerializableThing value) {
            return new Thing(ObjectAtlas.GetThingDef(value.Definition), value.Position);
        }

        static public implicit operator SerializableThing(Thing value) {
            return new SerializableThing(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableScriptableThing : SerializableThing {
        public IList<IThingData> Datasets;

        public SerializableScriptableThing(string definition, Position position, IList<IThingData> datasets) : base(
            definition, position) {
            Datasets = datasets;
        }

        static public implicit operator ScriptableThing(SerializableScriptableThing value) {
            return new ScriptableThing(ObjectAtlas.GetScriptableThingDef(value.Definition), value.Position,
                value.Datasets);
        }

        static public implicit operator SerializableScriptableThing(ScriptableThing value) {
            return new SerializableScriptableThing(value.Definition.FullName, value.Position, value.Datasets);
        }
    }
}