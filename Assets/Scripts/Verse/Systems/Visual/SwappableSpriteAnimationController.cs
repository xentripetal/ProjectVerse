using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.Utilities;

namespace Verse.Systems.Visual {
    public class SwappableSpriteAnimationController : MonoBehaviour {

        public string[] BodyParts;
        public RuntimeAnimatorController Animator;

        private const string CharacterFolderPath = "Sprites/Character/";

        private string _lastState;

        private List<Dictionary<string, Sprite>> _spriteMappings;
        private List<GameObject> _partRenderers;

        private void Start() {
            _spriteMappings = new List<Dictionary<string, Sprite>>();
            _partRenderers = new List<GameObject>();
            
            for (var i = 0; i < BodyParts.Length; i++) {
                _spriteMappings.Add(LoadSpriteSheet(BodyParts[i]));
                _partRenderers.Add(CreateSpriteObject(i));
            }

            var anim = _partRenderers[0].AddComponent<Animator>();
            anim.runtimeAnimatorController = Animator;
            GetComponent<PlayerController>().Animator = anim;
        }

        private GameObject CreateSpriteObject(int order) {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(0, 0, -.00005f * order);
            return go;
        }

        private Dictionary<string, Sprite> LoadSpriteSheet(string spriteSheet) {
            var sprites = Resources.LoadAll<Sprite>(CharacterFolderPath + spriteSheet);
            return sprites.ToDictionary(x => x.name, x => x);
        }
        
        private void UpdateSprites(string currentState) {
            for (var i = 0; i < BodyParts.Length; i++) {
                try {
                    _partRenderers[i].GetComponent<SpriteRenderer>().sprite = _spriteMappings[i][currentState];
                }
                catch (KeyNotFoundException) {
                    Debug.Log(BodyParts[i] + " does not have state " + currentState);
                }
            }
        }
        
        private void LateUpdate() {
            var currentState = _partRenderers[0].GetComponent<SpriteRenderer>().sprite.name;
            if (currentState != _lastState) {
                UpdateSprites(currentState);
                _lastState = currentState;
            }
        }
    }
}