using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using HappyHarvest;
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace HappyHarvest
{
    /// <summary>
    /// Allow to define events that will be triggered at given time in the day. An event have a start and end time which
    /// define a range, and functions can be called when going from out of range into the range and from in the range to
    /// out of range
    /// </summary>
    [DefaultExecutionOrder(999)]
    public class DayEventHandler : MonoBehaviour
    {
        [System.Serializable]
        public class DayEvent
        {
            public float StartTime = 0.0f;
            public float EndTime = 1.0f;

            public UnityEvent OnEvents;
            public UnityEvent OffEvent;

            public bool IsInRange(float t)
            {
                return t >= StartTime && t <= EndTime;
            }
        }

        public DayEvent[] Events;

        private void Start()
        {
            GameManager.RegisterEventHandler(this);
        }

        private void OnDisable()
        {
            GameManager.RemoveEventHandler(this);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DayEventHandler.DayEvent))]
    public class DayEvent : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var dayHandler = GameObject.FindObjectOfType<DayCycleHandler>();

            // Create property container element.
            var container = new VisualElement();

            if (dayHandler != null)
            {
                var minProperty = property.FindPropertyRelative(nameof(DayEventHandler.DayEvent.StartTime));
                var maxProperty = property.FindPropertyRelative(nameof(DayEventHandler.DayEvent.EndTime));

                var slider = new MinMaxSlider(
                    $"Day range {GameManager.GetTimeAsString(minProperty.floatValue)} - {GameManager.GetTimeAsString(maxProperty.floatValue)}",
                    minProperty.floatValue, maxProperty.floatValue, 0.0f, 1.0f);

                slider.RegisterValueChangedCallback(evt =>
                {
                    minProperty.floatValue = evt.newValue.x;
                    maxProperty.floatValue = evt.newValue.y;

                    property.serializedObject.ApplyModifiedProperties();

                    slider.label =
                        $"Day range {GameManager.GetTimeAsString(minProperty.floatValue)} - {GameManager.GetTimeAsString(maxProperty.floatValue)}";
                });

                var evtOnProperty = property.FindPropertyRelative(nameof(DayEventHandler.DayEvent.OnEvents));
                var evtOffProperty = property.FindPropertyRelative(nameof(DayEventHandler.DayEvent.OffEvent));

                container.Add(slider);

                container.Add(new PropertyField(evtOnProperty, "On Event"));
                container.Add(new PropertyField(evtOffProperty, "Off Event"));
            }
            else
            {
                container.Add(new Label("There is no DayCycleHanlder in the scene and it is needed"));
            }

            return container;
        }
    }
#endif

}