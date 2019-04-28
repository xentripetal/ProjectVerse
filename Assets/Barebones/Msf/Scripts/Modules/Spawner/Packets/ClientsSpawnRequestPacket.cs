using System.Collections.Generic;
using Barebones.Networking;

namespace Barebones.MasterServer {
    public class ClientsSpawnRequestPacket : SerializablePacket {
        public Dictionary<string, string> Options;
        public string Region = "";

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(Region);
            writer.Write(Options);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            Region = reader.ReadString();
            Options = reader.ReadDictionary();
        }
    }
}