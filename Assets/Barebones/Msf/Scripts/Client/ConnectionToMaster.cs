using System.Collections;
using Barebones.Logging;
using Barebones.Networking;
using UnityEngine;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Automatically connects to master server
    /// </summary>
    public class ConnectionToMaster : MonoBehaviour {
        private static ConnectionToMaster _instance;

        private IClientSocket _connection;

        public HelpBox _header = new HelpBox {
            Text = "This script automatically connects to any server. Most likely, " +
                   "you'll use it to connect to Master server",
            Type = HelpBoxType.Info
        };

        [Header("Automation")] [Tooltip("If true, will try to connect on the Start()")]
        public bool ConnectOnStart;

        public BmLogger Logger = Msf.Create.Logger(typeof(ConnectionToMaster).Name);

        [Tooltip("Log level of this script")] public LogLevel LogLevel = LogLevel.Info;

        public float MaxTimeToConnect = 4f;

        [Header("Advanced ")] public float MinTimeToConnect = 0.5f;

        [Tooltip("If true, ip and port will be read from cmd args")]
        public bool ReadMasterServerAddressFromCmd = true;

        [Tooltip("Address to the server")] public string ServerIp = "127.0.0.1";

        [Tooltip("Port of the server")] public int ServerPort = 5000;

        public float TimeToConnect = 0.5f;

        private void Awake() {
            if (_instance != null) {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            Logger.LogLevel = LogLevel;

            // In case this object is not at the root level of hierarchy
            // move it there, so that it won't be destroyed
            if (transform.parent != null)
                transform.SetParent(null, false);

            DontDestroyOnLoad(gameObject);

            if (ReadMasterServerAddressFromCmd) {
                // If master IP is provided via cmd arguments
                if (Msf.Args.IsProvided(Msf.Args.Names.MasterIp))
                    ServerIp = Msf.Args.MasterIp;

                // If master port is provided via cmd arguments
                if (Msf.Args.IsProvided(Msf.Args.Names.MasterPort))
                    ServerPort = Msf.Args.MasterPort;
            }
        }

        public void Start() {
            if (ConnectOnStart) StartCoroutine(StartConnection());
        }

        public virtual IClientSocket GetConnection() {
            return Msf.Connection;
        }

        private IEnumerator StartConnection() {
            // Wait a fraction of a second, in case we're also starting a master server
            yield return new WaitForSeconds(0.2f);

            var connection = GetConnection();

            connection.Connected += Connected;
            connection.Disconnected += Disconnected;

            while (true) {
                // Skip one frame
                yield return null;

                if (connection.IsConnected) yield break;

                // If we got here, we're not connected 
                if (connection.IsConnecting)
                    Logger.Debug("Retrying to connect to server at: " + ServerIp + ":" + ServerPort);
                else
                    Logger.Debug("Connecting to server at: " + ServerIp + ":" + ServerPort);

                connection.Connect(ServerIp, ServerPort);

                // Give a few seconds to try and connect
                yield return new WaitForSeconds(TimeToConnect);

                // If we're still not connected
                if (!connection.IsConnected) TimeToConnect = Mathf.Min(TimeToConnect * 2, MaxTimeToConnect);
            }
        }

        private void Disconnected() {
            TimeToConnect = MinTimeToConnect;
        }

        private void Connected() {
            TimeToConnect = MinTimeToConnect;
            Logger.Info("Connected to: " + ServerIp + ":" + ServerPort);
        }

        private void OnApplicationQuit() {
            var connection = GetConnection();

            if (connection != null)
                connection.Disconnect();
        }
    }
}