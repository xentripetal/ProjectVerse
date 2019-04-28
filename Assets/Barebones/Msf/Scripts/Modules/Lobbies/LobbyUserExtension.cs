using Barebones.Networking;

namespace Barebones.MasterServer {
    public class LobbyUserExtension {
        public LobbyUserExtension(IPeer peer) {
            Peer = peer;
        }

        public IPeer Peer { get; }

        /// <summary>
        ///     Lobby, to which current peer belongs
        /// </summary>
        public ILobby CurrentLobby { get; set; }
    }
}