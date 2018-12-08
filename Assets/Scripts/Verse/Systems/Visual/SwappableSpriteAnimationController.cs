using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Verse.Systems.Visual {
    public class SwappableSpriteAnimationController : MonoBehaviour {
        [FormerlySerializedAs("bodySpriteSheetName")] public string BodySpriteSheetName;
        [FormerlySerializedAs("hairSpriteSheetName")] public string HairSpriteSheetName;
        [FormerlySerializedAs("shirtSpriteSheetName")] public string ShirtSpriteSheetName;
        [FormerlySerializedAs("pantsSpriteSheetName")] public string PantsSpriteSheetName;

        [FormerlySerializedAs("bodySpriteRenderer")] public SpriteRenderer BodySpriteRenderer;
        [FormerlySerializedAs("hairSpriteRenderer")] public SpriteRenderer HairSpriteRenderer;
        [FormerlySerializedAs("shirtSpriteRenderer")] public SpriteRenderer ShirtSpriteRenderer;
        [FormerlySerializedAs("pantsSpriteRenderer")] public SpriteRenderer PantsSpriteRenderer;

        private string _loadedBodySpriteSheetName;
        private string _loadedHairSpriteSheetName;
        private string _loadedShirtSpriteSheetName;
        private string _loadedPantsSpriteSheetName;

        private Dictionary<string, Sprite> _bodySpriteSheet;
        private Dictionary<string, Sprite> _hairSpriteSheet;
        private Dictionary<string, Sprite> _shirtSpriteSheet;
        private Dictionary<string, Sprite> _pantsSpriteSheet;

        private void Start() {
            _bodySpriteSheet = LoadSpriteSheet(BodySpriteSheetName);
            _loadedBodySpriteSheetName = BodySpriteSheetName;

            _hairSpriteSheet = LoadSpriteSheet(HairSpriteSheetName);
            _loadedHairSpriteSheetName = HairSpriteSheetName;

            _shirtSpriteSheet = LoadSpriteSheet(ShirtSpriteSheetName);
            _loadedShirtSpriteSheetName = ShirtSpriteSheetName;

            _pantsSpriteSheet = LoadSpriteSheet(PantsSpriteSheetName);
            _loadedPantsSpriteSheetName = PantsSpriteSheetName;
        }

        private Dictionary<string, Sprite> LoadSpriteSheet(string spriteSheet) {
            var sprites = Resources.LoadAll<Sprite>(spriteSheet);
            return sprites.ToDictionary(x => x.name, x => x);
        }

        private void LateUpdate() {
            if (_loadedBodySpriteSheetName != BodySpriteSheetName) {
                _bodySpriteSheet = LoadSpriteSheet(BodySpriteSheetName);
                _loadedBodySpriteSheetName = BodySpriteSheetName;
            }


            if (_loadedHairSpriteSheetName != HairSpriteSheetName) {
                _hairSpriteSheet = LoadSpriteSheet(HairSpriteSheetName);
                _loadedHairSpriteSheetName = HairSpriteSheetName;
            }


            if (_loadedShirtSpriteSheetName != ShirtSpriteSheetName) {
                _shirtSpriteSheet = LoadSpriteSheet(ShirtSpriteSheetName);
                _loadedShirtSpriteSheetName = ShirtSpriteSheetName;
            }


            if (_loadedPantsSpriteSheetName != PantsSpriteSheetName) {
                _pantsSpriteSheet = LoadSpriteSheet(PantsSpriteSheetName);
                _loadedPantsSpriteSheetName = PantsSpriteSheetName;
            }

            string currentSpriteName = BodySpriteRenderer.sprite.name;

            BodySpriteRenderer.sprite = _bodySpriteSheet[currentSpriteName];
            HairSpriteRenderer.sprite = _hairSpriteSheet[currentSpriteName];
            ShirtSpriteRenderer.sprite = _shirtSpriteSheet[currentSpriteName];
            PantsSpriteRenderer.sprite = _pantsSpriteSheet[currentSpriteName];
        }
    }
}