using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse.API.Interfaces;
using Verse.API.Interfaces.Events;
using Verse.API.Models;
using Verse.Systems.Visual;

namespace Verse.Systems {
    public class ApiController : MonoBehaviour {
        private RoomController _roomController;

        public static ApiController Instance;

        private static MethodInfo[] _frameUpdateMethods;
        private static MethodInfo[] _lateFrameUpdateMethods;

        private void Awake() {
            Instance = this;
        }

        void Start() {
            _roomController = RoomController.Instance;
            _frameUpdateMethods = GetMethodsWithAttribute<OnFrameUpdate>();
            _lateFrameUpdateMethods = GetMethodsWithAttribute<OnLateFrameUpdate>();
        }

        #region Utilities#Systems

        private MethodInfo[] GetMethodsWithAttribute<T>() where T : Attribute {
            var type = typeof(T);
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods())
                .Where(m => m.IsStatic && m.GetCustomAttributes(typeof(T), false).Length > 0).ToArray();
        }

        #endregion

        #region Utilities#Things

        private ScriptableTileObject GetScriptableThingFromGameObject(GameObject go) {
            return _roomController.GetScriptableThingFromGameObject(go);
        }

        private T[] GetScriptsImplementingInterface<T>(ScriptableTileObject tileObject) {
            if (tileObject.Datasets == null) {
                return new T[] { };
            }

            return GetScriptsImplementingInterface<T>(tileObject.Definition.Scripts);
        }

        private T[] GetScriptsImplementingInterface<T>(IThingScript[] scripts) {
            return scripts.OfType<T>().ToArray();
        }

        private IThingData GetDatasetOfType(ScriptableTileObject tileObject, Type dataType) {
            foreach (IThingData thingData in tileObject.Datasets) {
                if (dataType.IsInstanceOfType(thingData)) {
                    return thingData;
                }
            }

            Debug.Log("No dataType " + dataType + " was found");
            return null;
        }

        #endregion

        #region ITrigger

        public void OnPlayerEnter(GameObject go) {
            var thing = GetScriptableThingFromGameObject(go);
            //todo Cache results
            ITrigger[] triggerScripts = GetScriptsImplementingInterface<ITrigger>(thing);
            foreach (ITrigger trigger in triggerScripts) {
                IThingData dataset = GetDatasetOfType(thing, trigger.DataModel);
                trigger.OnPlayerEnter(dataset);
            }
        }

        private void Update() {
            foreach (var methodInfo in _frameUpdateMethods) {
                methodInfo.Invoke(null, null);
            }
        }

        private void LateUpdate() {
            foreach (var methodInfo in _lateFrameUpdateMethods) {
                methodInfo.Invoke(null, null);
            }
        }

        #endregion
    }
}