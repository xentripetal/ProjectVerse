using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Systems.Visual {
    public class SwappableSpriteAnimationController : MonoBehaviour {
        private const string CharacterFolderPath = "Sprites/Character/";

        private string _lastState;
        private List<SpriteRenderer> _partRenderers;

        private List<Dictionary<string, Sprite>> _spriteMappings;
        public RuntimeAnimatorController Animator;

        public string[] BodyParts;

        private void Start() {
            _spriteMappings = new List<Dictionary<string, Sprite>>();
            _partRenderers = new List<SpriteRenderer>();

            for (var i = 0; i < BodyParts.Length; i++) {
                _spriteMappings.Add(LoadSpriteSheet(BodyParts[i]));
                _partRenderers.Add(CreateSpriteObject(i));
            }

            var anim = _partRenderers[0].gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Animator;
            GetComponent<PlayerController>().Animator = anim;
        }

        private SpriteRenderer CreateSpriteObject(int order) {
            var go = new GameObject();
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 5;
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(0, 0, -.00005f * order);
            return sr;
        }

        private Dictionary<string, Sprite> LoadSpriteSheet(string spriteSheet) {
            var sprites = Resources.LoadAll<Sprite>(CharacterFolderPath + spriteSheet);
            return sprites.ToDictionary(x => x.name, x => x);
        }

        private void UpdateSprites(string currentState) {
            for (var i = 0; i < BodyParts.Length; i++)
                try {
                    _partRenderers[i].sprite = _spriteMappings[i][currentState];
                }
                catch (KeyNotFoundException) {
                    Debug.Log(BodyParts[i] + " does not have state " + currentState);
                }
        }

        private void LateUpdate() {
            var currentState = _partRenderers[0].sprite.name;
            if (currentState != _lastState) {
                UpdateSprites(currentState);
                _lastState = currentState;
            }
        }
    }
}