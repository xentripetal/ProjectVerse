using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using UnityEngine;

namespace ProjectVerse.Client {
    public static class ClientManager {
        public static IPAddress IpAddress { get; private set; }
        public static ushort Port { get; private set; }
        public static DarkRiftClient Client { get; private set; }

        private static Dispatcher _dispatcher;

        private static bool IsSafeToStartConnection() {
            if (Client != null) {
                if (Client.ConnectionState == ConnectionState.Connected) {
                    Debug.LogError("Attempted to connect to a new server while already connected.");
                    return false;
                }

                if (Client.ConnectionState == ConnectionState.Connecting) {
                    Debug.LogError("Attempted to start a new connection while another is attempting to connect.");
                    return false;
                }

                if (Client.ConnectionState == ConnectionState.Disconnecting) {
                    Debug.LogError("Attempted to start a new connection while another is still disconnecting.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Temporary call until event bus can support private statics
        /// </summary>
        public static void Start() {
            _dispatcher = new Dispatcher(true);
            Client = new DarkRiftClient();
        }

        /// <summary>
        /// Temporary call until event bus can support private statics
        /// </summary>
        public static void Update() {
            _dispatcher.ExecuteDispatcherTasks();
        }
        
        public static bool Connect(IPAddress ipAddress, ushort port) {
            if (!IsSafeToStartConnection())
                return false;
            
            Client.Connect(ipAddress, port, true);
            if (Client.ConnectionState == ConnectionState.Connected) return true;
            Debug.LogError("Failed to connect to server.");
            return false;
        }

        public static void ConnectInBackground(IPAddress ipAddress, ushort port, DarkRiftClient.ConnectCompleteHandler callback = null) {
            if (!IsSafeToStartConnection())
                return;
            
            Client.ConnectInBackground(ipAddress, port, true, delegate(Exception e) {  
                if (callback != null)
                {
                    _dispatcher.InvokeAsync(() => callback(e));
                }
                
                if (Client.ConnectionState == ConnectionState.Connected)
                    Debug.Log("Connected to " + ipAddress + " on port " + port + ".");
                else
                    Debug.Log("Connection failed to " + ipAddress + " on port " + port + ".");
            });
        }

        public static bool Disconnect() {
            if (Client.ConnectionState == ConnectionState.Disconnected) {
                Debug.LogError("Attempted to disconnect client who is already disconnected.");
                return false;
            }

            if (Client.ConnectionState == ConnectionState.Disconnecting) {
                Debug.LogError("Attempted to disconnect client is currently disconnecting");
                return false;
            }

            return Client.Disconnect();
        }
    }
}