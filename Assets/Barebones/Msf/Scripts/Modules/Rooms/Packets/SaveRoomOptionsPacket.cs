using Barebones.Networking;

namespace Barebones.MasterServer {
    public class SaveRoomOptionsPacket : SerializablePacket {
        public RoomOptions Options;
        public int RoomId;

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(RoomId);
            writer.Write(Options);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            RoomId = reader.ReadInt32();
            Options = reader.ReadPacket(new RoomOptions());
        }
    }
}