using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Verse;

namespace Mirror.Examples.MultipleAdditiveScenes {
	public class VerseNetworkManager : NetworkManager {
		public static VerseNetworkManager Instance;
		
		[Scene] public string roomScene = "Room";

		// This is set true after server loads all subscene instances
		private Dictionary<string, Scene> loadedRooms = new Dictionary<string, Scene>();

		public override void Awake() {
			if (Instance != null) {
				throw new InvalidOperationException("Multiple instances of VerseNetworkManager are not allowed");
            }
			base.Awake();
			VerseNetworkManager.Instance = this;
		}

		#region Server System Callbacks

		/// <summary>
		/// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
		/// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
		/// </summary>
		/// <param name="conn">Connection from client.</param>
		public override void OnServerAddPlayer(NetworkConnection conn) {
			// TODO : find out what room the player should be joining
			StartCoroutine(OnServerAddPlayerDelayed(conn, "room1"));
		}

		IEnumerator LoadRoom(string roomName) {
			if (loadedRooms.ContainsKey(roomName)) yield break;
			var loader = SceneManager.LoadSceneAsync(roomScene,
				new LoadSceneParameters
					{loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics2D});
			yield return loader;
			Scene room = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
			loadedRooms.Add(roomName, room);
			foreach (var go in room.GetRootGameObjects()) {
				if (go.CompareTag("RoomLoader")) {
					go.GetComponent<RoomLoader>().Room = roomName;
					break;
				}
			}
		}

		// This delay is mostly for the host player that loads too fast for the
		// server to have subscenes async loaded from OnStartServer ahead of it.
		IEnumerator OnServerAddPlayerDelayed(NetworkConnection conn, string roomName) {
			// wait for server to async load all subscenes for game instances
			yield return LoadRoom(roomName);

			// Send Scene message to client to additively load the game scene
			conn.Send(new RoomMessage {RoomName = roomName, Operation = RoomOperation.Enter});
			//conn.Send(new SceneMessage {sceneOperation = SceneOperation.LoadAdditive, sceneName = roomScene});

			// Wait for end of frame before adding the player to ensure Scene Message goes first
			yield return new WaitForEndOfFrame();

			base.OnServerAddPlayer(conn);


			// Do this only on server, not on clients
			// This is what allows the NetworkSceneChecker on player and scene objects
			// to isolate matches per scene instance on server.
			SceneManager.MoveGameObjectToScene(conn.identity.gameObject, loadedRooms[roomName]);
		}

		public IEnumerator ServerMovePlayerToRoom(NetworkIdentity client, string room) {
			yield return LoadRoom(room);
			client.connectionToClient.Send(new RoomMessage(){Operation = RoomOperation.Enter, RoomName = room});
			yield return new WaitForEndOfFrame();
			SceneManager.MoveGameObjectToScene(client.gameObject, loadedRooms[room]);
		}

		#endregion

		#region Start & Stop Callbacks

		public override void OnStartClient() {
			NetworkClient.RegisterHandler<RoomMessage>(ClientLoadRoom, false);
		}

		/// <summary>
		/// This is invoked when a server is started - including when a host is started.
		/// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
		/// </summary>
		public override void OnStartServer() { }

		void ClientLoadRoom(RoomMessage message) {
			if (NetworkClient.isConnected) {
				OnClientSceneInternal(new SceneMessage()
					{sceneName = roomScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true});
				StartCoroutine(OnClientLoadRoom(NetworkClient.connection.identity, message.RoomName, message.Operation));
			}
		}

		IEnumerator OnClientLoadRoom(NetworkIdentity player, string roomName, RoomOperation op) {
			loadingSceneAsync = SceneManager.LoadSceneAsync(roomScene, LoadSceneMode.Additive);
			yield return loadingSceneAsync;
			var newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
			foreach (var go in newScene.GetRootGameObjects()) {
				if (go.CompareTag("RoomLoader")) {
					go.GetComponent<RoomLoader>().Room = roomName;
					break;
				}
			}
			SceneManager.MoveGameObjectToScene(player.gameObject, loadedRooms[roomName]);

			if (SceneManager.sceneCount > 2) {
				yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 2));
			}
		}


		/// <summary>
		/// This is called when a server is stopped - including when a host is stopped.
		/// </summary>
		public override void OnStopServer() {
			StartCoroutine(ServerUnloadSubScenes());
		}

		// Unload the subScenes and unused assets and clear the subScenes list.
		IEnumerator ServerUnloadSubScenes() {
			foreach (var room in loadedRooms.Values)
				yield return SceneManager.UnloadSceneAsync(room);

			loadedRooms.Clear();

			yield return Resources.UnloadUnusedAssets();
		}

		/// <summary>
		/// This is called when a client is stopped.
		/// </summary>
		public override void OnStopClient() {
			// make sure we're not in host mode
			if (mode == NetworkManagerMode.ClientOnly)
				StartCoroutine(ClientUnloadSubScenes());
		}

		// Unload all but the active scene, which is the "container" scene
		IEnumerator ClientUnloadSubScenes() {
			for (int index = 0; index < SceneManager.sceneCount; index++) {
				if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
					yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
			}
		}

		#endregion
	}
}