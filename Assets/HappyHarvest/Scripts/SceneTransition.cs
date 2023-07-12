using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HappyHarvest
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneTransition : MonoBehaviour
    {
        public int TargetSceneBuildIndex;
        public int TargetSpawnIndex;

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameManager.Instance.MoveTo(TargetSceneBuildIndex, TargetSpawnIndex);
        }
    }
}