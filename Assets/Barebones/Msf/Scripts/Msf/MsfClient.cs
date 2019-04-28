using Barebones.Networking;

namespace Barebones.MasterServer {
    public class MsfClient : MsfBaseClient {
        public MsfClient(IClientSocket connection) : base(connection) {
            Rooms = new MsfRoomsClient(connection);
            Spawners = new MsfSpawnersClient(connection);
            Matchmaker = new MsfMatchmakerClient(connection);
            Auth = new MsfAuthClient(connection);
            Chat = new MsfChatClient(connection);
            Lobbies = new MsfLobbiesClient(connection);
            Profiles = new MsfProfilesClient(connection);
        }

        public MsfRoomsClient Rooms { get; }

        public MsfSpawnersClient Spawners { get; }

        public MsfMatchmakerClient Matchmaker { get; }

        public MsfAuthClient Auth { get; }

        public MsfChatClient Chat { get; }

        public MsfLobbiesClient Lobbies { get; }

        public MsfProfilesClient Profiles { get; }
    }
}