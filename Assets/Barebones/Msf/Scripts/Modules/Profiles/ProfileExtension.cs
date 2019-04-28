using Barebones.Networking;

namespace Barebones.MasterServer {
    public class ProfileExtension {
        public ProfileExtension(ObservableServerProfile profile, IPeer peer) {
            Username = profile.Username;
            Profile = profile;
            Peer = peer;
        }

        public string Username { get; }
        public ObservableServerProfile Profile { get; }
        public IPeer Peer { get; }
    }
}