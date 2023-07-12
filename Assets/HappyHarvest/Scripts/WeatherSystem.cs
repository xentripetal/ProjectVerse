using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HappyHarvest
{
    /// <summary>
    /// When a weather is set on this system, it will find all WeatherSystemElement in the scene and enable the one that
    /// match the set weather and disable the ones that doesn't match.
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        [Flags]
        public enum WeatherType
        {
            Sun = 0x1,
            Rain = 0x2,
            Thunder = 0x4
        }

        public WeatherType StartingWeather;

        private WeatherType m_CurrentWeatherType;
        private List<WeatherSystemElement> m_Elements = new List<WeatherSystemElement>();

        private void Awake()
        {
            GameManager.Instance.WeatherSystem = this;
        }

        void Start()
        {
            FindAllElements();
            ChangeWeather(StartingWeather);
        }

        public static void UnregisterElement(WeatherSystemElement element)
        {
#if UNITY_EDITOR
            //in the editor when not running, we find the instance manually. Less efficient but not a problem at edit time
            //allow to be able to previz shadow in editor 
            if (!Application.isPlaying)
            {
                var instance = GameObject.FindObjectOfType<WeatherSystem>();
                if (instance != null)
                {
                    instance.m_Elements.Remove(element);
                }
            }
            else
            {
#endif
                GameManager.Instance?.WeatherSystem?.m_Elements.Remove(element);
#if UNITY_EDITOR
            }
#endif
        }

        public void ChangeWeather(WeatherType newType)
        {
            m_CurrentWeatherType = newType;
            SwitchAllElementsToCurrentWeather();
            UIHandler.UpdateWeatherIcons(newType);
        }

        void FindAllElements()
        {
            //we use FindObject of type as Object can be disabled in the editor (so they don't play all over each other at edit time)
            //and that mean their Awake/Start function won't be called, so we can't self register to this WeatherSystem.
            //This can be costly, but will only ever be called once at scene load, so won't impact gameplay.
            m_Elements = new(GameObject.FindObjectsOfType<WeatherSystemElement>(true));
        }

        void SwitchAllElementsToCurrentWeather()
        {
            foreach (var element in m_Elements)
            {
                element.gameObject.SetActive(element.WeatherType.HasFlag(m_CurrentWeatherType));
            }
        }

#if UNITY_EDITOR
        //This is only needed in editor at edit time to test weather system.
        public void EditorWeatherUpdate()
        {
            m_CurrentWeatherType = StartingWeather;
            FindAllElements();
            SwitchAllElementsToCurrentWeather();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WeatherSystem))]
    public class WeatherSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Updating eather");
                (target as WeatherSystem).EditorWeatherUpdate();
            }
        }
    }
#endif

}