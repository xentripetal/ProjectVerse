using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif


namespace HappyHarvest
{
    /// <summary>
    /// This class will blend between multiple Light2D shaped across the day. This allow to have moving Shadow, used for
    /// example by the house or barn shadow.
    /// </summary>
    [DefaultExecutionOrder(999)]
    [ExecuteInEditMode]
    public class LightInterpolator : MonoBehaviour
    {
        [Serializable]
        public class LightFrame
        {
            public Light2D ReferenceLight;
            public float NormalizedTime;
        }
        
        [Tooltip("The light of which the shape will be changed according to the defined frames")]
        public Light2D TargetLight;
        public LightFrame[] LightFrames;

        private void OnEnable()
        {
            DayCycleHandler.RegisterLightBlender(this);
        }

        private void OnDisable()
        {
            DayCycleHandler.UnregisterLightBlender(this);
        }

        public void SetRatio(float t)
        {
            if(LightFrames.Length == 0)
                return;
            
            int startFrame = 0;

            while (startFrame < LightFrames.Length - 1 && LightFrames[startFrame + 1].NormalizedTime < t)
            {
                startFrame += 1;
            }

            if (startFrame == LightFrames.Length - 1)
            {
                //the last frame is the "start frame" so there is no frame to interpolate TO, so we just use the last settings
                Interpolate(LightFrames[startFrame].ReferenceLight, LightFrames[startFrame].ReferenceLight, 0.0f);
            }
            else
            {
                float frameLength = LightFrames[startFrame + 1].NormalizedTime - LightFrames[startFrame].NormalizedTime;
                float frameValue = t - LightFrames[startFrame].NormalizedTime;
                float normalizedFrame = frameValue / frameLength;
                
                Interpolate(LightFrames[startFrame].ReferenceLight, LightFrames[startFrame + 1].ReferenceLight, normalizedFrame);
            }
        }

        void Interpolate(Light2D start, Light2D end, float t)
        {
            TargetLight.color = Color.Lerp(start.color, end.color, t);
            TargetLight.intensity = Mathf.Lerp(start.intensity, end.intensity, t);

            var startPath = start.shapePath;
            var endPath = end.shapePath;

            var newPath = new Vector3[startPath.Length];
            
            for (int i = 0; i < startPath.Length; ++i)
            {
                newPath[i] = Vector3.Lerp(startPath[i], endPath[i], t);
            }
            
            TargetLight.SetShapePath(newPath);
        }

        public void DisableAllLights()
        {
            foreach (var frame in LightFrames)
            {
                frame.ReferenceLight?.gameObject.SetActive(false);
            }
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(LightInterpolator))]
    public class LightInterpolatorEditor : Editor
    {
        private LightInterpolator m_Interpolator;
        private DayCycleHandler m_DayCycleHandler;

        private Slider m_PreviewSlider;
        
        private void OnEnable()
        {
            m_Interpolator = target as LightInterpolator;
            m_Interpolator.DisableAllLights();
            m_Interpolator.SetRatio(0);

            m_DayCycleHandler = GameObject.FindObjectOfType<DayCycleHandler>();
            if (m_DayCycleHandler != null)
            {
                m_DayCycleHandler.UpdateLight(0.0f);
            }
            
            EditorApplication.update += SceneUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= SceneUpdate;
        }

        void SceneUpdate()
        {
            if (m_PreviewSlider == null ||
                m_Interpolator == null ||
                m_DayCycleHandler == null)
                return;
            
            m_Interpolator.SetRatio(m_PreviewSlider.value);
            m_DayCycleHandler.UpdateLight(m_PreviewSlider.value);
            
            SceneView.RepaintAll();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            CustomUI(root);
            
            m_PreviewSlider = new Slider("Preview time 00:00 (0)", 0.0f, 1.0f);
            m_PreviewSlider.RegisterValueChangedCallback(evt =>
            {
                m_Interpolator.SetRatio(m_PreviewSlider.value);
                if (m_DayCycleHandler != null)
                {
                    m_PreviewSlider.label = $"Preview time {GameManager.GetTimeAsString(m_PreviewSlider.value)} ({m_PreviewSlider.value})";
                    m_DayCycleHandler.UpdateLight(m_PreviewSlider.value);
                }
            });
            
            root.Add(m_PreviewSlider);
            
            return root;
        }

        void CustomUI(VisualElement root)
        {
            var targetLight = serializedObject.FindProperty(nameof(LightInterpolator.TargetLight));
            var frames = serializedObject.FindProperty(nameof(LightInterpolator.LightFrames));
            
            var list = new ListView();

            list.showFoldoutHeader = false;
            list.showBoundCollectionSize = false;
            list.reorderable = true;
            list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            list.reorderMode = ListViewReorderMode.Animated;
            list.showBorder = true;
            list.headerTitle = "Title";
            list.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            list.selectionType = SelectionType.Single;
            list.showAddRemoveFooter = true;

            list.style.flexGrow = 1.0f;

            
            list.Bind(serializedObject);
            list.bindingPath = frames.propertyPath;

            list.makeItem += () =>
            {
                var foldout = new Foldout();
                var refLight = new PropertyField()
                {
                    name = "ReferenceLightEntry"
                };
                var timeEntry = new PropertyField()
                {
                    name = "TimeEntry"
                };

                foldout.Add(refLight);
                foldout.Add(timeEntry);
                
                return foldout;
            };
            
            list.bindItem += (element, i) =>
            {
                var entry = frames.GetArrayElementAtIndex(i);
                
                (element as Foldout).text = entry.displayName;

                var refLight = entry.FindPropertyRelative(nameof(LightInterpolator.LightFrame.ReferenceLight));
            
                element.Q<PropertyField>("ReferenceLightEntry")
                    .BindProperty(refLight);
                element.Q<PropertyField>("TimeEntry")
                    .BindProperty(entry.FindPropertyRelative(nameof(LightInterpolator.LightFrame.NormalizedTime)));
            };

            list.unbindItem += (element, i) =>
            {
                if (element.userData != null)
                {
                    DestroyImmediate(element.userData as Editor);
                }
            };

            list.selectionChanged += objs =>
            {
                if(!objs.Any())
                    return;
                
                var first = objs.First() as SerializedProperty;
                m_PreviewSlider.value = first.FindPropertyRelative(nameof(LightInterpolator.LightFrame.NormalizedTime)).floatValue;
            };

            list.itemsChosen += objects =>
            {
                var first = objects.First() as SerializedProperty;

                var target = first.FindPropertyRelative(nameof(LightInterpolator.LightFrame.ReferenceLight))
                    .objectReferenceValue as Light2D;

                if(target != null) target.gameObject.SetActive(true);
                
                Selection.activeObject = target;
            };
            
            
            root.Add(list);
        }
    }
#endif
    
}
