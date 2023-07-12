using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyHarvest
{
    /// <summary>
    /// Add this to any object that should be enabled only for a specific set of weathers (e.g. VFX of dripping water
    /// should have this with Weather type set to Rain and Thunder)
    /// </summary>
    [DefaultExecutionOrder(999)]
    [ExecuteInEditMode]
    public class WeatherSystemElement : MonoBehaviour
    {
        public WeatherSystem.WeatherType WeatherType;

        private void OnDestroy()
        {
            WeatherSystem.UnregisterElement(this);
        }
    }
}