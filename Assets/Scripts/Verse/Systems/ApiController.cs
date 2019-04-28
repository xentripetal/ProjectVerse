using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse.API;
using Verse.API.Events;
using Verse.API.Events.EventBus;
using Verse.API.Interfaces;
using Verse.Systems.Visual;

namespace Verse.Systems {
    public class ApiController : MonoBehaviour {
        public static ApiController Instance;
        private RoomController _roomController;
        public bool EditorMode;


        private void Awake() {
            Instance = this;
            RegisterStaticSubscribers();
        }

        private void RegisterStaticSubscribers() {
            var subscribers = GetMethodsWithAttribute<Subscribe>();
            foreach (var subscriber in subscribers) {
                if (subscriber.GetParameters().Length != 1) continue;

                var priority = ((Subscribe) subscriber.GetCustomAttribute(typeof(Subscribe))).priority;
                ((DictEventBus) World.EventBus).Register(subscriber, priority);
            }
        }

        private void Start() {
            _roomController = RoomController.Instance;
            if (!EditorMode) _roomController.ChangeRoom("main", null);
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
            if (EditorMode) return;

            var tile = _roomController.GameObjectToTile(go);
            if (tile != null)
                if (tile.Entity != null)
                    tile.Entity.OnCharacterEnter();
        }

        #endregion

        private void Update() {
            if (!EditorMode) {
                World.EventBus.Post(new FrameUpdateEvent());

                //Todo find a better way to do this
                var tiles = _roomController.CurrentRoom.Tiles.GetTilesWithEntities();
                foreach (var tile in tiles) tile.Entity.OnFrameUpdate();
            }
        }

        private void LateUpdate() {
            if (EditorMode) return;
        }
    }
}