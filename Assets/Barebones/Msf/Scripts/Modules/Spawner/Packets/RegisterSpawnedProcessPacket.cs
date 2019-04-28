using Barebones.Networking;

namespace Barebones.MasterServer {
    public class RegisterSpawnedProcessPacket : SerializablePacket {
        public string SpawnCode;
        public int SpawnId;

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(SpawnId);
            writer.Write(SpawnCode);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            SpawnId = reader.ReadInt32();
            SpawnCode = reader.ReadString();
        }
    }
}