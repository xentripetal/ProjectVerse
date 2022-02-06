using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Verse {
    /**
     * Will handle any rendering or audio sources for the given object when the room is loaded/unloaded.
     */
    public class ClientVisibilityHandler : MonoBehaviour {
        private Renderer _renderer;
        private bool _rendererState;
        private AudioSource _audioSource;
        private bool _audioSourceState;

        public void Awake() {
            _renderer = GetComponent<Renderer>();
            _audioSource = GetComponent<AudioSource>();
            RoomManager.onRoomUnloaded += OnRoomUnloaded;
            RoomManager.onRoomLoaded += OnRoomLoaded;
            if (_renderer) {
                _rendererState = _renderer.enabled;
            }

            if (_audioSource) {
                _audioSourceState = _audioSource.enabled;
            }
        }

        public void OnDestroy() {
            RoomManager.onRoomUnloaded -= OnRoomUnloaded;
            RoomManager.onRoomLoaded -= OnRoomLoaded;
        }

        void OnRoomUnloaded(Room room, Scene scene) {
            Debug.Log($"Unloading room {room.Name}");
            if (scene != gameObject.scene) {
                return;
            }

            Debug.Log($"Object {gameObject.name} hiding in room {room.Name}");
            if (_renderer != null) {
                _rendererState = _renderer.enabled;
                _renderer.enabled = false;
            }

            if (_audioSource != null) {
                _audioSourceState = _audioSource.enabled;
                _audioSource.enabled = false;
            }
        }

        void OnRoomLoaded(Room room, Scene scene) {
            Debug.Log($"Loading room {room.Name}");
            if (scene != gameObject.scene) {
                return;
            }

            Debug.Log($"Object {gameObject.name} revealing in room {room.Name}");
            if (_renderer != null) {
                _renderer.enabled = _rendererState;
            }

            if (_audioSource != null) {
                _audioSource.enabled = _audioSourceState;
            }
        }
    }
}