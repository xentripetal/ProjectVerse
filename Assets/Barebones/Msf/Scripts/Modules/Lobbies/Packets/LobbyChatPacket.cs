using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     A lobby chat message
    /// </summary>
    public class LobbyChatPacket : SerializablePacket {
        public bool IsError;
        public string Message = "";
        public string Sender = "";

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(Sender);
            writer.Write(Message);
            writer.Write(IsError);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            Sender = reader.ReadString();
            Message = reader.ReadString();
            IsError = reader.ReadBoolean();
        }
    }
}