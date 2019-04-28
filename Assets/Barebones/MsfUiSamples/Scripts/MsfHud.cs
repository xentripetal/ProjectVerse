using System.Linq;
using Barebones.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Barebones.MasterServer {
    public class MsfHud : MonoBehaviour {
        protected const string HudExpansionPrefKey = "msf.hudExpanded";

        protected IClientSocket Connection;
        public Text ConnectionPermissionsText;
        public Image ConnectionStatusBg;

        [Header("Connection Status")] public Text ConnectionStatusText;

        public GameObject Disclaimer;

        public bool EditorOnly;

        public bool FullViewByDefault = true;

        public bool IsExpanded = true;
        public GameObject MasterConnection;

        [Header("Game objects")] public GameObject MasterRunning;

        public GameObject Rooms;
        public Text RoomsText;
        public GameObject Spawners;

        [Header("Other Text Components")] public Text SpawnersText;

        public Text VersionText;

        private void Awake() {
            if (!Msf.Runtime.IsEditor && EditorOnly) gameObject.SetActive(false);

            Connection = Msf.Connection;

            IsExpanded = PlayerPrefs.GetInt(HudExpansionPrefKey, 1) > 0;
        }

        private void Start() {
            VersionText.text = Msf.Version;

            // Subscribe
            Connection.StatusChanged += OnConnectionStatusChanged;
            MasterServerBehaviour.MasterStarted += OnMasterStatusChanged;
            MasterServerBehaviour.MasterStopped += OnMasterStatusChanged;
            Msf.Server.Rooms.RoomRegistered += OnRoomCountChanged;
            Msf.Server.Rooms.RoomDestroyed += OnRoomCountChanged;
            Msf.Server.Spawners.SpawnerRegistered += OnSpawnersCountChange;

            // Update all views
            UpdateAllViews();
        }

        private void OnMasterStatusChanged(MasterServerBehaviour obj) {
            UpdateRunningMasterView(obj.IsRunning);
        }

        private void OnSpawnersCountChange(SpawnerController obj) {
            UpdateSpawnersView();
        }

        private void OnRoomCountChanged(RoomController obj) {
            UpdateRoomsView();
        }

        private void OnConnectionStatusChanged(ConnectionStatus status) {
            UpdateConnectionStatusView();
            MasterConnection.gameObject.SetActive(IsExpanded);
        }

        public void ToggleFullWindow() {
            IsExpanded = !IsExpanded;
            UpdateAllViews();

            PlayerPrefs.SetInt(HudExpansionPrefKey, IsExpanded ? 1 : 0);
        }

        private void UpdateAllViews() {
            UpdateRunningMasterView(MasterServerBehaviour.IsMasterRunning);
            UpdateConnectionStatusView();
            UpdateRoomsView();
            UpdateSpawnersView();
            Disclaimer.SetActive(IsExpanded);
        }

        private void UpdateConnectionStatusView() {
            switch (Connection.Status) {
                case ConnectionStatus.Connected:
                    ConnectionStatusText.text = "Connected To Master";
                    break;
                case ConnectionStatus.Connecting:
                    ConnectionStatusText.text = "Connecting...";
                    ConnectionPermissionsText.text = "To " + Connection.ConnectionIp + ":" + Connection.ConnectionPort;
                    break;
                case ConnectionStatus.Disconnected:
                    ConnectionStatusText.text = "Not Connected";
                    break;
            }

            if (Connection.Status == ConnectionStatus.Connected)
                ConnectionPermissionsText.text = "Permission Level: " + Msf.Security.CurrentPermissionLevel;

            MasterConnection.SetActive(IsExpanded && Connection.Status != ConnectionStatus.Disconnected);
        }

        private void UpdateRunningMasterView(bool isRunning) {
            MasterRunning.gameObject.SetActive(isRunning && IsExpanded);
        }

        private void UpdateRoomsView() {
            var rooms = Msf.Server.Rooms.GetLocallyCreatedRooms().ToList();
            Rooms.SetActive(rooms.Count > 0 && IsExpanded);
            RoomsText.text = "Created Rooms: " + rooms.Count;
        }

        private void UpdateSpawnersView() {
            var spawners = Msf.Server.Spawners.GetLocallyCreatedSpawners().ToList();
            Spawners.SetActive(spawners.Count > 0 && IsExpanded);
            SpawnersText.text = "Started Spawners: " + spawners.Count;
        }

        private void OnDestroy() {
            // Unsubscribe
            Connection.StatusChanged -= OnConnectionStatusChanged;
            MasterServerBehaviour.MasterStarted -= OnMasterStatusChanged;
            MasterServerBehaviour.MasterStopped -= OnMasterStatusChanged;
            Msf.Server.Rooms.RoomRegistered -= OnRoomCountChanged;
            Msf.Server.Rooms.RoomDestroyed -= OnRoomCountChanged;
            Msf.Server.Spawners.SpawnerRegistered -= OnSpawnersCountChange;
        }
    }
}