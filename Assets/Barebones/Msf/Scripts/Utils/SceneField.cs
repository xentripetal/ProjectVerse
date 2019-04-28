using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Barebones.MasterServer {
    [Serializable]
    public class SceneField {
        [SerializeField] private Object m_SceneAsset;

        [SerializeField] private string m_SceneName = "";

        public string SceneName => m_SceneName;

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField) {
            return sceneField.SceneName;
        }

        public bool IsSet() {
            return !string.IsNullOrEmpty(m_SceneName);
        }
    }
}