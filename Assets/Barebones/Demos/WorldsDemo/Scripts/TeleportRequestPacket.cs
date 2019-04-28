using Barebones.Networking;

public class TeleportRequestPacket : SerializablePacket {
    public string Position;
    public string Username;
    public string ZoneName;

    public override void ToBinaryWriter(EndianBinaryWriter writer) {
        writer.Write(Username);
        writer.Write(ZoneName);
        writer.Write(Position);
    }

    public override void FromBinaryReader(EndianBinaryReader reader) {
        Username = reader.ReadString();
        ZoneName = reader.ReadString();
        Position = reader.ReadString();
    }
}