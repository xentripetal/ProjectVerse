using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HappyHarvest
{
    /// <summary>
    /// This just load the next scene. This is used by the Loaded scene that just ensure that the GameManager is
    /// created in a build.  
    /// </summary>
    public class Loader : MonoBehaviour
    {
        public int TargetScene = 3;
        
        private void Start()
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
