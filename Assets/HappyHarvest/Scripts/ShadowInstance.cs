using System;
using System.Collections;
using System.Collections.Generic;
using HappyHarvest;
using UnityEngine;


namespace HappyHarvest
{
//execute late to be sure the manager are instantiated 
    [DefaultExecutionOrder(999)]
    [ExecuteInEditMode]
    public class ShadowInstance : MonoBehaviour
    {
        [Range(0, 10f)] public float BaseLength = 1f;

        private void OnEnable()
        {
            DayCycleHandler.RegisterShadow(this);
        }

        private void OnDisable()
        {
            DayCycleHandler.UnregisterShadow(this);
        }
    }
}