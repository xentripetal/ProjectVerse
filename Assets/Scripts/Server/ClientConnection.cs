using DarkRift;
using DarkRift.Server;
using ProjectVerse.Shared.SerializedData;

namespace Server {
    [System.Serializable]
    public class ClientConnection {

        public string Name;
        public IClient Client;
        
        public ClientConnection(IClient client, LoginRequestData data) {
            Client = client;
            Name = data.Name;
        }
        
    }
}