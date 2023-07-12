using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace HappyHarvest
{
    [DefaultExecutionOrder(1000)]
    public class SpawnPoint : MonoBehaviour
    {
        public int SpawnIndex;

        private void OnEnable()
        {
            GameManager.Instance.RegisterSpawn(this);
        }

        private void OnDisable()
        {
            GameManager.Instance?.UnregisterSpawn(this);
        }

        public void SpawnHere()
        {
            var playerTransform = GameManager.Instance.Player.transform;
            
            playerTransform.position = transform.position;

            if (GameManager.Instance.MainCamera != null)
            {//some scene, like interior, may have fixed camera, so no need to change anything
                GameManager.Instance.MainCamera.Follow = playerTransform;
                GameManager.Instance.MainCamera.LookAt = playerTransform;
                GameManager.Instance.MainCamera.ForceCameraPosition(playerTransform.position, Quaternion.identity);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SpawnPoint[] transitions = GameObject.FindObjectsOfType<SpawnPoint>();
            var local = target as SpawnPoint;
            foreach (var transition in transitions)
            {
                if (transition == local)
                {
                    continue;
                }

                if (transition.SpawnIndex == local.SpawnIndex)
                {
                    EditorGUILayout.HelpBox(
                        $"Spawn Index need to be unique and this Spawn Index is already used by {transition.gameObject.name}",
                        MessageType.Error);
                }
            }
        }
    }
#endif
}