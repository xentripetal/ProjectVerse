using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace HappyHarvest
{
    /// <summary>
    /// Add this to the CinemachineVirtualCamera in your scene that is the main gameplay camera. When a scene load, the
    /// player will set itself as target of the camera with that script 
    /// </summary>
    [DefaultExecutionOrder(100)]
    public class CameraSetter : MonoBehaviour
    {
        private void Awake()
        {
            var cam = GetComponent<CinemachineVirtualCamera>();
            GameManager.Instance.MainCamera = cam;
        }
    }
}