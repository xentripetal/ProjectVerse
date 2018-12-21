using System;
using System.Linq;
using Newtonsoft.Json;
using Verse.API.Scripting;
using Verse.Models;
using Verse.Utilities;

namespace Verse.API.Models {
    /// <summary>
    /// Tiles are the core component to all physical objects in the Verse. A standard tile is simply a
    /// terrain/ground asset that the player walks on top of.
    /// </summary>
    public class TileDef {
        /// <value>The combined name of the package and the tile name. E.g. core.barrel.</value>
        public string FullName { get; protected set; }

        /// <value>The tile name. E.g. 'barrel'.</value>
        [JsonIgnore] public readonly string Name;

        /// <value>
        /// The providing package for the Tile. For example, the Object 'core.staticobjects.deco.barrel'
        /// will return 'core.staticobjects.deco'
        /// </value>
        [JsonIgnore] public readonly string Package;

        /// <value>
        /// The top level package for the Tile. For example, all non-modded content will have the Provider 'core'.
        /// </value>
        [JsonIgnore] public readonly string Provider;

        /// <value>Information on the Tiles sprite</value>
        public SpriteInfo SpriteInfo { get; protected set; }

        [JsonConstructor]
        public TileDef(String fullName, SpriteInfo spriteInfo) {
            FullName = fullName;
            var splitFullName = fullName.Split('.');
            Name = splitFullName.Last();
            Package = String.Join(".", splitFullName.DropLast().ToArray());
            Provider = splitFullName.First();
            SpriteInfo = spriteInfo;
        }
    }

    /// <summary>
    /// ThingDefs are definitions are static objects in the world. They exist in 2.5 dimensions as the
    /// player can be in front of or behind them.
    /// </summary>
    public class ThingDef : TileDef {
        /// <value>Is true if the object has collisions enabled.</value>
        public readonly bool IsCollidable;

        /// <value>Is true if the object will go transparent when the player is behind it.</value>
        public readonly bool IsTransparentOnPlayerBehind;

        [JsonConstructor]
        public ThingDef(String fullName, SpriteInfo spriteInfo, bool isCollidable, bool isTransparentOnPlayerBehind) :
            base(fullName, spriteInfo) {
            IsCollidable = isCollidable;
            IsTransparentOnPlayerBehind = isTransparentOnPlayerBehind;
        }
    }

    public class ScriptableThingDef : ThingDef {
        /// <value>
        /// True if the collider should act as a trigger.
        /// Must be true if using a script inheriting from ITrigger. IsCollidable must also be true
        /// </value>
        public readonly bool IsTrigger;

        /// <value>the list of scripts for the object.</value>
        [JsonIgnore] public readonly IThingScript[] Scripts;

        /// <value>List of scripts by script name only.</value>
        public readonly string[] ScriptNames;

        [JsonConstructor]
        public ScriptableThingDef(String fullName, SpriteInfo spriteInfo, bool isCollidable,
            bool isTransparentOnPlayerBehind, bool isTrigger,
            string[] scriptNames) : base(fullName, spriteInfo, isCollidable, isTransparentOnPlayerBehind) {
            IsTrigger = isTrigger;
            ScriptNames = scriptNames;
            Scripts = scriptNames.Select(scriptName => ScriptAtlas.GetScript(scriptName)).ToArray();
        }
    }
}