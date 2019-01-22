using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse.API.Interfaces;
using Verse.API.Interfaces.Events;
using Verse.Systems.Visual;

namespace Verse.Systems {
    public class ApiController : MonoBehaviour {
        public bool EditorMode;
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
            if (!EditorMode) {
                _roomController.ChangeRoom("main", null);
            }
        }

        #region Utilities#Systems

        private MethodInfo[] GetMethodsWithAttribute<T>() where T : Attribute {
            var type = typeof(T);
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods())
                .Where(m => m.IsStatic && m.GetCustomAttributes(typeof(T), false).Length > 0).ToArray();
        }

        #endregion

        #region Utilities#Things

        private T[] GetScriptsImplementingInterface<T>(IThingScript[] scripts) {
            return scripts.OfType<T>().ToArray();
        }

        #endregion

        #region ITrigger

        public void OnPlayerEnter(GameObject go) {
            if (EditorMode) {
                return;
            }
        }

        #endregion

        private void Update() {
            if (EditorMode) {
                return;
            }

            foreach (var methodInfo in _frameUpdateMethods) {
                methodInfo.Invoke(null, null);
            }
        }

        private void LateUpdate() {
            if (EditorMode) {
                return;
            }

            foreach (var methodInfo in _lateFrameUpdateMethods) {
                methodInfo.Invoke(null, null);
            }
        }
    }
}