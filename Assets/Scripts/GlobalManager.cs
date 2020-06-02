using System;
using System.Net;
using DarkRift;
using DarkRift.Client.Unity;
using DarkRift.Server;
using DarkRift.Server.Unity;
using UnityEngine;

namespace ProjectVerse {
    [RequireComponent(typeof(UnityClient))]
    [RequireComponent(typeof(XmlUnityServer))]
    public class GlobalManager : MonoBehaviour {
        public enum ClientServerType {CLIENT, SERVER, CLIENTSERVER}
        public static GlobalManager Instance;

        [Header("Variables")] public ClientServerType clientType;
        public string IpAdress;
        public int Port;

        [Header("References")]
        public UnityClient Client;

        public XmlUnityServer XmlServer;
        private DarkRiftServer _server;

        void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start() {
            if (clientType == ClientServerType.SERVER || clientType == ClientServerType.CLIENTSERVER) {
                StartServer();
            }

            if (clientType == ClientServerType.CLIENT || clientType == ClientServerType.CLIENTSERVER) {
                StartClient();
            }
        }

        private void StartClient() {
            Client.ConnectInBackground(IPAddress.Parse(IpAdress), Port, true, ConnectCallback);
        }

        private void StartServer() {
            XmlServer.Create();
            _server = XmlServer.Server;
            _server.ClientManager.ClientConnected += OnClientConnect;
            _server.ClientManager.ClientDisconnected += OnClientDisconnect;
        }

        private void OnDestroy() {
            _server.ClientManager.ClientConnected -= OnClientConnect;
            _server.ClientManager.ClientDisconnected -= OnClientDisconnect;
        }

        void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e) {
            e.Client.MessageReceived -= OnMessage;
        }

        void OnClientConnect(object sender, ClientConnectedEventArgs e) {
            Debug.Log("Client Connected");
            e.Client.MessageReceived += OnMessage;
        }

        private void OnMessage(object sender, MessageReceivedEventArgs e) {
            DarkRiftReader reader = e.GetMessage().GetReader();
            var message = reader.ReadString();
            reader.Dispose();
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(message);
            var new_message = Message.Create(0, writer);
            writer.Dispose();
            foreach (var client in _server.ClientManager.GetAllClients()) {
                client.SendMessage(new_message, SendMode.Reliable);
            }
        }

        private void ConnectCallback(Exception exception) {
            if (Client.ConnectionState == ConnectionState.Connected) {
                Debug.Log("Connected!");
            }
            else {
                Debug.LogError("Failed to Connect");
            }
        }
    }
}