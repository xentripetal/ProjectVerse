using System.Collections.Generic;
using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Information about a member of the lobby
    /// </summary>
    public class LobbyMemberData : SerializablePacket {
        public bool IsReady;
        public Dictionary<string, string> Properties;
        public string Team;
        public string Username;

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.WriteDictionary(Properties);
            writer.Write(IsReady);
            writer.Write(Username);
            writer.Write(Team);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            Properties = reader.ReadDictionary();
            IsReady = reader.ReadBoolean();
            Username = reader.ReadString();
            Team = reader.ReadString();
        }
    }
}