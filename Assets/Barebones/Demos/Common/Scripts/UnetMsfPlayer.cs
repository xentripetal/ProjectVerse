using Barebones.MasterServer;
using UnityEngine.Networking;

public class UnetMsfPlayer {
    public UnetMsfPlayer(NetworkConnection connection, PeerAccountInfoPacket accountInfo) {
        Connection = connection;
        AccountInfo = accountInfo;
    }

    public NetworkConnection Connection { get; }
    public PeerAccountInfoPacket AccountInfo { get; set; }

    public string Username => AccountInfo.Username;
    public int PeerId => AccountInfo.PeerId;
}