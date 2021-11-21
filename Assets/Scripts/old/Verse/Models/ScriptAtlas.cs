using System.Collections.Generic;
using Verse.API.Interfaces;

namespace Verse.API.Models {
    public class ScriptAtlas {
        private static Dictionary<string, IThingScript> _scriptAtlas;

        public static void InitializeAtlas() {
            if (_scriptAtlas == null) CreateAtlas();
        }

        public static IThingScript GetScript(string scriptName) {
            InitializeAtlas();
            return _scriptAtlas[scriptName];
        }

        private static void CreateAtlas() {
            _scriptAtlas = new Dictionary<string, IThingScript>();
        }
    }
}