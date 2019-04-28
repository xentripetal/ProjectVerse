using Barebones.Networking;

namespace Barebones.MasterServer {
    public class MsfServer : MsfBaseClient {
        public MsfDbAccessorFactory DbAccessors;

        public MsfServer(IClientSocket connection) : base(connection) {
            DbAccessors = new MsfDbAccessorFactory();
            Rooms = new MsfRoomsServer(connection);
            Spawners = new MsfSpawnersServer(connection);
            Auth = new MsfAuthServer(connection);
            Lobbies = new MsfLobbiesServer(connection);
            Profiles = new MsfProfilesServer(connection);
        }

        public MsfRoomsServer Rooms { get; }
        public MsfSpawnersServer Spawners { get; }

        public MsfAuthServer Auth { get; }
        public MsfLobbiesServer Lobbies { get; }
        public MsfProfilesServer Profiles { get; }
    }
}