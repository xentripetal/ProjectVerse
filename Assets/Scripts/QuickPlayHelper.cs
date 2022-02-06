using System;
using System.Collections;
using Mirror.Examples.MultipleAdditiveScenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse {
    public class QuickPlayHelper : MonoBehaviour {
        public static bool Activated;
        public string RoomName;
        public void Start() {
            if (Activated) {
                return;
            }

            Activated = true;
            if (SceneManager.GetActiveScene().Equals(gameObject.scene)) {
                if (RoomName == null) {
                    RoomName = gameObject.scene.name;
                }
                DontDestroyOnLoad(gameObject);
                StartCoroutine(LoadGameScene());
            }
        }

        IEnumerator LoadGameScene() {
            yield return SceneManager.LoadSceneAsync("Scenes/Game", new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            var vnm = GameObject.FindWithTag("NetworkManager").GetComponent<VerseNetworkManager>();
            vnm.defaultScene = RoomName;
            vnm.StartHost();
        }
    }
    
}