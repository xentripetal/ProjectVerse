using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse {
    public static class RoomManager {
        static Dictionary<Room, Scene> loadedRooms = new();

        /**
         * The room that the player is currently in.
         */
        static Room activeRoom;

        public delegate void OnClientRoomLoaded(Room room, Scene scene);

        public static event OnClientRoomLoaded onRoomLoaded;

        public delegate void OnClientRoomUnloaded(Room room, Scene scene);

        // Gameobjects in the unloaded scene with renderers, audio players, or any side affects are expected to listen to this and disable
        // themselves when the room is unloaded. Due to how networking works, we often will not truly unload the scene 
        // even if the player is no longer in it. Thus we need to mimick it so that the player just sees the scene
        // as unloaded.
        public static event OnClientRoomUnloaded onRoomUnloaded;

        private static Scene? loadedSceneBuffer;

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // TODO might need to move this to a coroutine and wait for buffer to be null
            loadedSceneBuffer = scene;
        }

        /**
		 * Loads the room if not already loaded and moves the given identity to the room.
		 * If this is just the client it will make the room visible and hide any other rooms. Identity should be the
		 * player's identity.
		 * If it is the server or host it will just move the identity to the room. Identity should be the identity of the player that is entering this room.
		 */
        public static IEnumerator ActivateRoom(Room room, NetworkIdentity identity) {
            Scene scene;
            var firstLoad = false;
            Debug.Log("Loading room " + room.Id);
            if (!loadedRooms.TryGetValue(room, out scene)) {
                SceneManager.sceneLoaded += OnSceneLoaded;
                yield return SceneManager.LoadSceneAsync(room.SceneName,
                    new LoadSceneParameters
                        {loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics2D});
                while (!loadedSceneBuffer.HasValue) {
                    yield return null;
                }

                scene = loadedSceneBuffer.Value;
                loadedSceneBuffer = null;
                
                foreach (var go in scene.GetRootGameObjects()) {
                    if (go.CompareTag("RoomLoader")) {
                        go.GetComponent<RoomLoader>().Room = room;
                        break;
                    }
                }
                firstLoad = true;
            }
            Debug.Log("Got Scene for " + room.Id);

            loadedRooms[room] = scene;

            if (identity.isLocalPlayer) {
                var prevRoom = activeRoom;
                activeRoom = room;
                if (prevRoom != null) {
                    onRoomUnloaded?.Invoke(prevRoom, loadedRooms[prevRoom]);
                }

                SceneManager.MoveGameObjectToScene(identity.gameObject, scene);
                onRoomLoaded?.Invoke(room, scene);
            }
            else if (NetworkServer.active) {
                if (room != activeRoom && firstLoad) {
                    onRoomUnloaded?.Invoke(room, scene);
                }

                identity.connectionToClient.Send(new RoomMessage
                    {RoomId = room.Id.ToString(), Operation = RoomOperation.Enter});
                yield return new WaitForEndOfFrame();
                SceneManager.MoveGameObjectToScene(identity.gameObject, scene);
            }
            else {
                throw new InvalidOperationException("RoomManager.ActivateRoom called with no client or server.");
            }
        }

        public static IEnumerator Unload() {
            activeRoom = null;
            // Possible race condition. Not really sure how IEnumerators work.
            foreach (var scene in loadedRooms.Values) {
                yield return SceneManager.UnloadSceneAsync(scene);
            }

            loadedRooms.Clear();

            yield return Resources.UnloadUnusedAssets();
        }
    }
}