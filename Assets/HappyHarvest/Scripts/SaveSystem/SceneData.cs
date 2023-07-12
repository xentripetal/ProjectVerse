using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyHarvest
{
    /// <summary>
    /// Required in every scene. This define the unique name of the scene, used in the save system to identify the scene
    /// This mean the scene can be moved, renamed or its build id changed and saves won't break.
    /// </summary>
    public class SceneData : MonoBehaviour
    {
        public string UniqueSceneName;
        
        private void OnEnable()
        {
            GameManager.Instance.LoadedSceneData = this;
        }

        private void OnDisable()
        {
            if(GameManager.Instance?.LoadedSceneData == this)
                GameManager.Instance.LoadedSceneData = null;
        }
    }
}