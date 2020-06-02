using System;
using System.Net;
using DarkRift.Client;
using DarkRift.Client.Unity;
using DarkRift.Server;
using DarkRift.Server.Unity;
using ProjectVerse.Client;
using ProjectVerse.Server;
using UnityEngine;

namespace ProjectVerse.Shared {
    public class World : MonoBehaviour {
        public static World Instance;
        public bool IsClient { get; protected set; }
        public bool IsServer { get; protected set; }

        public bool IsClientServer => IsClient && IsServer;

        /// <summary>
        /// Temporary bool. will be replaced later with UI logic
        /// </summary>
        public bool AutoStartServer;
        /// <summary>
        /// Temporary bool. will be replaced later with UI logic
        /// </summary>
        public bool AutoStartClient;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("Attempted creating multiple Worlds. Destroying new one.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start() {
            if (AutoStartServer) {
                StartServer("127.0.0.1", 4296);
            }
            if (AutoStartClient) {
                ConnectClient("127.0.0.1", 4296);
            }
        }

        public bool ConnectClient(string ipAddress, ushort port) {
            if (IsClient) {
                Debug.LogError("Attempted to start client connection while already connected");
                return false;
            }

            if (!ClientManager.Connect(IPAddress.Parse(ipAddress), port)) 
                return false;

            IsClient = true;
            return true;
        }

        public bool DisconnectClient() {
            if (!IsClient) {
                Debug.LogError("Attempted to stop client while no client active");
                return false;
            }

            if (!ClientManager.Disconnect())
                return false;

            IsClient = false;
            return true;
        }

        public bool StartServer(string ipAddress, ushort port) {
            if (IsServer) {
                Debug.LogError("Attempted to start server while server already active");
                return false;
            }

            ServerManager.Start(IPAddress.Parse(ipAddress), port);
            return true;
        }

        public bool StopServer() {
            if (!IsServer) {
                Debug.LogError("Attempted to stop server while no server active");
                return false;
            }

            ServerManager.ForceClose();
            return true;
        }
    }
}