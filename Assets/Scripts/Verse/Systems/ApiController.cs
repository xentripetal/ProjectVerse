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

        private TileObjectEntity GetScriptableThingFromGameObject(GameObject go) {
            return _roomController.GetScriptableThingFromGameObject(go);
        }

        private T[] GetScriptsImplementingInterface<T>(TileObjectEntity tileObjectEntity) {
            if (tileObjectEntity.Datasets == null) {
                return new T[] { };
            }

            return GetScriptsImplementingInterface<T>(tileObjectEntity.Definition.Scripts);
        }

        private T[] GetScriptsImplementingInterface<T>(IThingScript[] scripts) {
            return scripts.OfType<T>().ToArray();
        }

        private IThingData GetDatasetOfType(TileObjectEntity tileObjectEntity, Type dataType) {
            foreach (IThingData thingData in tileObjectEntity.Datasets) {
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
            if (EditorMode) {
                return;
            }

            var thing = GetScriptableThingFromGameObject(go);
            //todo Cache results
            ITrigger[] triggerScripts = GetScriptsImplementingInterface<ITrigger>(thing);
            foreach (ITrigger trigger in triggerScripts) {
                IThingData dataset = GetDatasetOfType(thing, trigger.DataModel);
                trigger.OnPlayerEnter(dataset);
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

        public void OnTileCreated(Tile tile) { }

        public void OnTileCreatedExclusive(Tile tile) {
            _roomController.TileCreatedExclusive(tile);
        }

        public void OnTileObjectCreated(TileObject tileObject) { }

        public void OnTileObjectCreatedExclusive(TileObject tileObject) {
            _roomController.TileObjectCreatedExclusive(tileObject);
        }

        public void OnTileObjectEntityCreated(TileObjectEntity tileObjectEntity) {
            _roomController.TileObjectEntityCreated(tileObjectEntity);
        }

        public void OnTileDestroy(Tile tile) {
            ((TileProviderInternal) tile.Room.TileProvider).Remove(tile);

            if (EditorMode) {
                FindObjectOfType<SelectedTileController>().TileDestroyed(tile);
            }
        }

        public void OnTileDestroyExclusive(Tile tile) {
            _roomController.TileDestroyExlusive(tile);
        }

        public void OnTileObjectDestroy(TileObject tile) {
            _roomController.TileObjectDestroy(tile);
        }

        public void OnTileObjectDestroyExclusive(TileObject tile) { }

        public void OnTileObjectEntityDestroy(TileObjectEntity tile) { }
    }
}