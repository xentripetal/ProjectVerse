using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Verse;

namespace Mirror.Examples.MultipleAdditiveScenes {
    public class VerseNetworkManager : NetworkManager {
        public static VerseNetworkManager Instance;

        public string defaultScene = "roomA";

        // This is set true after server loads all subscene instances
        private Dictionary<string, Scene> loadedRooms = new Dictionary<string, Scene>();

        public override void Awake() {
            if (Instance != null) {
                throw new InvalidOperationException("Multiple instances of VerseNetworkManager are not allowed");
            }

            base.Awake();
            Instance = this;
        }

        #region Server System Callbacks

        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnection conn) {
            // TODO : find out what room the player should be joining
            base.OnServerAddPlayer(conn);
            StartCoroutine(OnServerAddPlayerDelayed(conn, defaultScene));
        }

        // This delay is mostly for the host player that loads too fast for the
        // server to have subscenes async loaded from OnStartServer ahead of it.
        IEnumerator OnServerAddPlayerDelayed(NetworkConnection conn, string roomName) {
            yield return RoomManager.ActivateRoom(Mocks.FromId(defaultScene), conn.identity);
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
            Debug.Log("Loading client room " + message.RoomId);
            StartCoroutine(RoomManager.ActivateRoom(Mocks.FromId(message.RoomId), NetworkClient.connection.identity));
        }


        /// <summary>
        /// This is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public override void OnStopServer() {
            StartCoroutine(RoomManager.Unload());
        }

        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient() {
            // make sure we're not in host mode
            if (mode == NetworkManagerMode.ClientOnly)
                StartCoroutine(RoomManager.Unload());
        }

        #endregion
    }
}