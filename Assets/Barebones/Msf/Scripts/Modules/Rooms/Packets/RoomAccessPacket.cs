using System.Collections.Generic;
using Barebones.Networking;

namespace Barebones.MasterServer {
    public class RoomAccessPacket : SerializablePacket {
        public Dictionary<string, string> Properties;
        public int RoomId;
        public string RoomIp;
        public int RoomPort;
        public string SceneName = "";
        public string Token;

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(Token);
            writer.Write(RoomIp);
            writer.Write(RoomPort);
            writer.Write(RoomId);
            writer.Write(SceneName);
            writer.Write(Properties);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            Token = reader.ReadString();
            RoomIp = reader.ReadString();
            RoomPort = reader.ReadInt32();
            RoomId = reader.ReadInt32();
            SceneName = reader.ReadString();
            Properties = reader.ReadDictionary();
        }

        public override string ToString() {
            return string.Format("[RoomAccessPacket| PublicAddress: {0}, RoomId: {1}, Token: {2}, Properties: {3}]",
                RoomIp + ":" + RoomPort, RoomId, Token, Properties.ToReadableString());
        }
    }
}