using System.Collections.Generic;
using Unity.Mathematics;

namespace Verse.API.Models {
    public class Tile {
        public TileDef Definition { get; }
        public Position Position { get; }

        public Tile(TileDef definition, Position position) {
            Definition = definition;
            Position = position;
        }
    }

    public class Thing : Tile {
        public readonly ThingDef Definition;

        public Thing(ThingDef definition, Position position) : base(definition, position) {
            Definition = definition;
        }
    }

    public class ScriptableThing : Thing {
        public readonly ScriptableThingDef Definition;
        public IList<IThingData> Datasets { get; }

        public ScriptableThing(ScriptableThingDef definition, float2 position, IList<IThingData> datasets) : base(
            definition, position) {
            Definition = definition;
            Datasets = datasets;
        }
    }
}